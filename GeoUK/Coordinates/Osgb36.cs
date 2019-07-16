using System;

namespace GeoUK.Coordinates
{
    /// <summary>
    /// This immutable class, derived from EastingNorthingCoordinates, provides a convenient means
    /// to represent OSGB36 eastings and northings.
    /// </summary>
    /// <remarks>
    /// Eastings and northings are represented in British National Grid and Height is specified
    /// in meters based on the geoid datum returned by the RegionGeoidDatum property.
    /// </remarks>
    public class Osgb36 : EastingNorthing
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Coordinates.Osgb36Cordinates"/> class.
        /// </summary>
        /// <param name="easting">Easting.</param>
        /// <param name="northing">Northing.</param>
        public Osgb36(double easting, double northing)
            : base(easting, northing, 0)
        {
            RegionGeoidDatum = Osgb36GeoidDatum.NewlynUkMainland;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Coordinates.Osgb36Cordinates"/> class.
        /// </summary>
        /// <param name="eastingNorthingCoordinates">Easting northing coordinates.</param>
        public Osgb36(EastingNorthing eastingNorthingCoordinates)
            : base(eastingNorthingCoordinates.Easting, eastingNorthingCoordinates.Northing, eastingNorthingCoordinates.Height)
        {
            RegionGeoidDatum = Osgb36GeoidDatum.NewlynUkMainland;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Coordinates.Osgb36Cordinates"/> class.
        /// </summary>
        /// <param name="eastingNorthingCoordinates">Easting northing coordinates.</param>
        /// <param name="datum">Datum.</param>
        public Osgb36(EastingNorthing eastingNorthingCoordinates, Osgb36GeoidDatum datum)
            : base(eastingNorthingCoordinates.Easting, eastingNorthingCoordinates.Northing, eastingNorthingCoordinates.Height)
        {
            RegionGeoidDatum = datum;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Coordinates.Osgb36Cordinates"/> class.
        /// </summary>
        /// <param name="easting">Easting.</param>
        /// <param name="northing">Northing.</param>
        /// <param name="height">Height.</param>
        /// <param name="datum">Datum.</param>
        public Osgb36(double easting, double northing, double height, Osgb36GeoidDatum datum)
            : base(easting, northing, height)
        {
            RegionGeoidDatum = datum;
        }

        /// <summary>
        /// Returns the Local Geoid datum in use. other property values should be
        /// considered invalid if this property is set to OutsideModelBoundary.
        /// </summary>
        public Osgb36GeoidDatum RegionGeoidDatum { get; }

        public string MapReference
        {
            get
            {
                /*
				10km (2-figure) Grid Reference: SO84 = 380000 Easting 240000 Northing
				1km (4-figure) Grid Reference: NS2468 = 224000 Easting 668000 Northing
				100m (6-figure) Grid Reference: TL123456 = 512300 Easting 245600 Northing
				*/
                double easting = Easting;
                double northing = Northing;

                string bngSquare = GetBngSquare(easting, northing);

                //get the number of complete 500k squares
                int indexNorthing = (int)Math.Floor(northing / 500000);
                int indexEasting = (int)Math.Floor(easting / 500000);

                //reduce E and N by the number of 500k squares
                northing -= indexNorthing * 500000;
                easting -= indexEasting * 500000;

                //reduce by the number of 100k squares within the 500k square.
                indexNorthing = (int)Math.Floor(northing) / 100000;
                indexEasting = (int)Math.Floor(easting) / 100000;

                northing -= indexNorthing * 100000;
                easting -= indexEasting * 100000;

                northing = Math.Round(northing / 100);
                easting = Math.Round(easting / 100);
                return $"{bngSquare}{Math.Round(easting):000}{Math.Round(northing):000}";
            }
        }

        /// <summary>
        /// Returns the two letter OS code based on easting and northing in metres.
        /// </summary>
        /// <returns>The square with northing.</returns>
        /// <param name="northing">Northing.</param>
        /// <param name="easting">Easting.</param>
        public static string GetBngSquare(double easting, double northing)
        {
            string result = string.Empty;

            //test for our upper and lower limits
            if (easting < 0 || easting > 700000 || northing < 0 || northing > 1300000) return result;

            char[] firstChar = { 'S', 'N', 'H', 'T', 'O', 'J' };
            char[] secondChar = { 'V', 'Q', 'L', 'F', 'A', 'W', 'R', 'M', 'G', 'B', 'X', 'S', 'N', 'H', 'C', 'Y', 'T', 'O', 'J', 'D', 'Z', 'U', 'P', 'K', 'E' };

            //calculate the first letter
            int indexNorthing = (int)Math.Floor(northing / 500000);
            int indexEasting = (int)Math.Floor(easting / 500000);

            //get the first char
            char chr1 = firstChar[(indexEasting * 3) + indexNorthing];

            //to get the second letter we subtract the number of 500km sectors calculated above
            indexNorthing = (int)Math.Floor((northing - (indexNorthing * 500000)) / 100000);
            indexEasting = (int)Math.Floor((easting - (indexEasting * 500000)) / 100000);

            //get the second char
            char chr2 = secondChar[(indexEasting * 5) + indexNorthing];

            result = $"{chr1}{chr2}";
            return result;
        }
    }
}