using GeoUK.Coordinates;
using GeoUK.Ellipsoids;
using GeoUK.Projections;
using System;
using System.Collections.Generic;

namespace GeoUK.OSTN
{
	public static class Transform
	{
		/// <summary>
		/// Loads the OSTN data into memory.
		/// </summary>
		/// <param name="ostnVersion">If not provided, it will load both OSTN02 and OSTN15 data.</param>
		public static void PreloadResources(OstnVersionEnum? ostnVersion = null) => ResourceManager.LoadResources(ostnVersion);

		/// <summary>
		/// Performs an ETRS89 to OSGB36/ODN datum transformation. Accuracy is approximately 10 centimeters.
		/// Whilst very accurate this method is much slower than the Helmert transformation.
		/// </summary>
		public static Osgb36 Etrs89ToOsgb(LatitudeLongitude coordinates)
		{
			EastingNorthing enCoordinates = Convert.ToEastingNorthing(new Grs80(), new BritishNationalGrid(), coordinates);
			return Etrs89ToOsgb(enCoordinates, coordinates.EllipsoidalHeight);
		}

		private static Osgb36 Etrs89ToOsgb(EastingNorthing coordinates, double ellipsoidHeight, OstnVersionEnum ostnVersion = OstnVersionEnum.OSTN15)
		{
			Shifts shifts = GetShifts(coordinates, ostnVersion);

			double easting = coordinates.Easting + shifts.Se;
			double northing = coordinates.Northing + shifts.Sn;
			double height = ellipsoidHeight - shifts.Sg;

			return new Osgb36(easting, northing, height, shifts.GeoidDatum);
		}

		/// <summary>
		/// Performs an OSGB36/ODN to ETRS89 datum transformation. Accuracy is approximately 10 centimeters.
		/// Whilst very accurate this method is much slower than the Helmert transformation.
		/// </summary>
		public static LatitudeLongitude OsgbToEtrs89(Osgb36 coordinates, OstnVersionEnum ostnVersion = OstnVersionEnum.OSTN15)
		{
			//calculate shifts from OSGB36 point
			double errorN = double.MaxValue;
			double errorE = double.MaxValue;
			EastingNorthing enCoordinates = null;

			Shifts shiftsA = GetShifts(coordinates, ostnVersion);

			//0.0001 error meters
			int iter = 0;
			while ((errorN > 0.0001 || errorE > 0.0001) && iter < 10)
			{
				enCoordinates = new EastingNorthing(coordinates.Easting - shiftsA.Se, coordinates.Northing - shiftsA.Sn);
				Shifts shiftsB = GetShifts(enCoordinates, ostnVersion);

				errorE = Math.Abs(shiftsA.Se - shiftsB.Se);
				errorN = Math.Abs(shiftsA.Sn - shiftsB.Sn);

				shiftsA = shiftsB;
				iter++;
			}

			return Convert.ToLatitudeLongitude(new Wgs84(), new BritishNationalGrid(), enCoordinates);
		}

		private static Shifts GetShifts(EastingNorthing coordinates, OstnVersionEnum ostnVersion)
		{
			//See OS Document: Transformations and OSGM02/OSGM15 user guide chapter 3
			Dictionary<int, OstnDataRecord> ostnData = GetOstnData(ostnVersion);

			List<int> recordNumbers = new List<int>();
			OstnDataRecord[] records = new OstnDataRecord[4];

			//determine record numbers
			int eastIndex = (int)(coordinates.Easting / 1000.0);
			int northIndex = (int)(coordinates.Northing / 1000.0);

			double x0 = eastIndex * 1000;
			double y0 = northIndex * 1000;

			//work out the four records
			recordNumbers.Add(CalculateRecordNumber(eastIndex, northIndex));
			recordNumbers.Add(CalculateRecordNumber(eastIndex + 1, northIndex));
			recordNumbers.Add(CalculateRecordNumber(eastIndex + 1, northIndex + 1));
			recordNumbers.Add(CalculateRecordNumber(eastIndex, northIndex + 1));

			// Get the corresponding records from the data dictionary
			for (int index = 0; index < 4; index++)
			{
				records[index] = ostnData[recordNumbers[index]];
			}

			//populate the properties
			double se0 = System.Convert.ToDouble(records[0].ETRS89_OSGB36_EShift);
			double se1 = System.Convert.ToDouble(records[1].ETRS89_OSGB36_EShift);
			double se2 = System.Convert.ToDouble(records[2].ETRS89_OSGB36_EShift);
			double se3 = System.Convert.ToDouble(records[3].ETRS89_OSGB36_EShift);

			double sn0 = System.Convert.ToDouble(records[0].ETRS89_OSGB36_NShift);
			double sn1 = System.Convert.ToDouble(records[1].ETRS89_OSGB36_NShift);
			double sn2 = System.Convert.ToDouble(records[2].ETRS89_OSGB36_NShift);
			double sn3 = System.Convert.ToDouble(records[3].ETRS89_OSGB36_NShift);

			double sg0 = System.Convert.ToDouble(records[0].ETRS89_ODN_HeightShift);
			double sg1 = System.Convert.ToDouble(records[1].ETRS89_ODN_HeightShift);
			double sg2 = System.Convert.ToDouble(records[2].ETRS89_ODN_HeightShift);
			double sg3 = System.Convert.ToDouble(records[3].ETRS89_ODN_HeightShift);

			double dx = coordinates.Easting - x0;
			double dy = coordinates.Northing - y0;

			double t = dx / 1000.0;
			double u = dy / 1000.0;

			Shifts shifts = new Shifts
			{
				Se = (1 - t) * (1 - u) * se0 + t * (1 - u) * se1 + t * u * se2 + (1 - t) * u * se3,
				Sn = (1 - t) * (1 - u) * sn0 + t * (1 - u) * sn1 + t * u * sn2 + (1 - t) * u * sn3,
				Sg = (1 - t) * (1 - u) * sg0 + t * (1 - u) * sg1 + t * u * sg2 + (1 - t) * u * sg3,

				GeoidDatum = (Osgb36GeoidDatum)System.Convert.ToInt32(records[0].Height_Datum_Flag)
			};

			return shifts;
		}

		/// <summary>
		/// Calculates a data file record number.
		/// </summary>
		/// <param name="eastIndex"></param>
		/// <param name="northIndex"></param>
		/// <returns></returns>
		private static int CalculateRecordNumber(int eastIndex, int northIndex)
		{
			return eastIndex + (northIndex * 701) + 1;
		}

		/// <summary>
		/// Retrieves the parsed OSTN data from the embedded resource file.
		/// </summary>
		/// <param name="ostnVersion"></param>
		/// <returns></returns>
		private static Dictionary<int, OstnDataRecord> GetOstnData(OstnVersionEnum ostnVersion)
		{
			switch (ostnVersion)
			{
				case OstnVersionEnum.OSTN02:
					return ResourceManager.Ostn02Data;

				case OstnVersionEnum.OSTN15:
					return ResourceManager.Ostn15Data;

				default:
					throw new NotImplementedException();
			}
		}
	}
}