using System;
using GeoUK.Coordinates;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GeoUK.Ellipsoids;
using GeoUK.Projections;

namespace GeoUK.OSTN
{
	public static class Transform
	{
	    /// <summary>
	    /// Loads the OSTN data into memory.
	    /// </summary>
	    /// <param name="ostnVersion">If not provided, it will load both OSTN02 and OSTN15 data.</param>
	    public static void PreloadResources(OstnVersionEnum? ostnVersion = null)
	    {
            ResourceManager.LoadResources(ostnVersion);
	    }

	    /// <summary>
        /// Performs an ETRS89 to OSGB36/ODN datum transformation. Accuracy is approximately 10 centimeters. 
        /// Whilst very accurate this method is much slower than the Helmert transformation.
        /// </summary>
        public static Osgb36 Etrs89ToOsgb(LatitudeLongitude coordinates, OstnVersionEnum ostnVersion = OstnVersionEnum.OSTN15)
		{
			var enCoordinates = Convert.ToEastingNorthing (new Grs80 (), new BritishNationalGrid (), coordinates);
			return Etrs89ToOsgb (enCoordinates, coordinates.ElipsoidalHeight);
		}

		private static Osgb36 Etrs89ToOsgb(EastingNorthing coordinates, double ellipsoidHeight, OstnVersionEnum ostnVersion = OstnVersionEnum.OSTN15)
		{
			var shifts = GetShifts (coordinates, ellipsoidHeight, ostnVersion);

			var easting = coordinates.Easting + shifts.Se;
			var northing = coordinates.Northing + shifts.Sn;
			var height = ellipsoidHeight - shifts.Sg;

			return new Osgb36(easting, northing, height, shifts.GeoidDatum);

		}

		/// <summary>
		/// Performs an OSGB36/ODN to ETRS89 datum transformation. Accuracy is approximately 10 centimeters. 
		/// Whilst very accurate this method is much slower than the Helmert transformation.
		/// </summary>
		public static LatitudeLongitude OsgbToEtrs89 (Osgb36 coordinates, OstnVersionEnum ostnVersion = OstnVersionEnum.OSTN15)
		{
			//calculate shifts from OSGB36 point
			double errorN = double.MaxValue;
			double errorE = double.MaxValue;
			EastingNorthing enCoordinates = null;

			var shiftsA = GetShifts (coordinates, coordinates.Height, ostnVersion);

			//0.0001 error meters
			int iter = 0;
			while ((errorN > 0.0001 || errorE > 0.0001) && iter <10) {

				enCoordinates = new EastingNorthing (coordinates.Easting - shiftsA.Se, coordinates.Northing - shiftsA.Sn);
				var shiftsB = GetShifts (enCoordinates, coordinates.Height, ostnVersion);

				errorE = Math.Abs (shiftsA.Se - shiftsB.Se);
				errorN = Math.Abs (shiftsA.Sn - shiftsB.Sn);

				shiftsA = shiftsB;
				iter++;

			}

			return Convert.ToLatitudeLongitude(new Wgs84(), new BritishNationalGrid(), enCoordinates);

		}

		private static Shifts GetShifts (EastingNorthing coordinates, double ellipsoidHeight, OstnVersionEnum ostnVersion)
		{
			//See OS Document: Transformations and OSGM02/OSGM15 user guide chapter 3
		    var ostnData = GetOstnData(ostnVersion);

			List<int> recordNumbers = new List<int> ();
			var records = new OstnDataRecord[4];

			//determine record numbers
			int eastIndex = (int)(coordinates.Easting / 1000.0);
			int northIndex = (int)(coordinates.Northing / 1000.0);

			double x0 = eastIndex * 1000;
			double y0 = northIndex * 1000;

			//work out the four records
			recordNumbers.Add (CalculateRecordNumber (eastIndex, northIndex));
			recordNumbers.Add (CalculateRecordNumber (eastIndex + 1, northIndex));
			recordNumbers.Add (CalculateRecordNumber (eastIndex + 1, northIndex + 1));
			recordNumbers.Add (CalculateRecordNumber (eastIndex, northIndex + 1));

            // Get the corresponding reccords from the data dictionary
		    for (int index = 0; index < 4; index++)
		    {
		        records[index] = ostnData[recordNumbers[index]];
		    }

            //populate the properties
            var se0 = System.Convert.ToDouble (records[0].ETRS89_OSGB36_EShift);
			var se1 = System.Convert.ToDouble (records[1].ETRS89_OSGB36_EShift);
			var se2 = System.Convert.ToDouble (records[2].ETRS89_OSGB36_EShift);
			var se3 = System.Convert.ToDouble (records[3].ETRS89_OSGB36_EShift);

			var sn0 = System.Convert.ToDouble (records[0].ETRS89_OSGB36_NShift);
			var sn1 = System.Convert.ToDouble (records[1].ETRS89_OSGB36_NShift);
			var sn2 = System.Convert.ToDouble (records[2].ETRS89_OSGB36_NShift);
			var sn3 = System.Convert.ToDouble (records[3].ETRS89_OSGB36_NShift);

			var sg0 = System.Convert.ToDouble (records[0].ETRS89_ODN_HeightShift);
			var sg1 = System.Convert.ToDouble (records[1].ETRS89_ODN_HeightShift);
			var sg2 = System.Convert.ToDouble (records[2].ETRS89_ODN_HeightShift);
			var sg3 = System.Convert.ToDouble (records[3].ETRS89_ODN_HeightShift);

			var dx = coordinates.Easting - x0;
			var dy = coordinates.Northing - y0;

			var t = dx / 1000.0;
			var u = dy / 1000.0;

			var shifts = new Shifts ();

			shifts.Se = (1 - t) * (1 - u) * se0 + t * (1 - u) * se1 + t * u * se2 + (1 - t) * u * se3;
			shifts.Sn = (1 - t) * (1 - u) * sn0 + t * (1 - u) * sn1 + t * u * sn2 + (1 - t) * u * sn3;
			shifts.Sg = (1 - t) * (1 - u) * sg0 + t * (1 - u) * sg1 + t * u * sg2 + (1 - t) * u * sg3;

			shifts.GeoidDatum = (Osgb36GeoidDatum)System.Convert.ToInt32 (records[0].Height_Datum_Flag);

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

