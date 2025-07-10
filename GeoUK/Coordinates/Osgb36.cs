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
        /// <summary> Number of Digits in OS Grid Reference</summary>
        public enum OsDigitsType
        {
            /// <summary>2 Digits OS Grid Ref 10 km accuracy</summary>
            OsDigits2,
            /// <summary>4 Digits OS Grid Ref 1 km accuracy</summary>
            OsDigits4,
            /// <summary>6 Digits OS Grid Ref 100 metre accuracy</summary>
            OsDigits6,
            /// <summary>8 Digits OS Grid Ref 10 metre accuracy</summary>
            OsDigits8,
            /// <summary>10 Digits OS Grid Ref 1 metre accuracy</summary>
            OsDigits10,
            /// <summary>12 Digits OS Grid Ref 0.1 metre accuracy</summary>
            OsDigits12
        }

        /// <summary> Number of Digits in OS Grid Reference</summary>
        public static OsDigitsType OsDigits { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Coordinates.Osgb36Cordinates"/> class.
        /// </summary>
        /// <param name="easting">Easting.</param>
        /// <param name="northing">Northing.</param>
        public Osgb36(double easting, double northing)
            : base(easting, northing, 0)
        {
            RegionGeoidDatum = Osgb36GeoidDatum.NewlynUkMainland;
            OsDigits = OsDigitsType.OsDigits6;
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
                switch(OsDigits)
                {                   
                    case OsDigitsType.OsDigits2: //10km(2 - figure) Grid Reference: SO84 = 380000 Easting 240000 Northing
                        northing = Math.Round(northing / 10000);
                        easting = Math.Round(easting / 10000);
                        return $"{bngSquare}{Math.Round(easting):0}{Math.Round(northing):0}";
                    case OsDigitsType.OsDigits4: //1km(4 - figure) Grid Reference: NS2468 = 224000 Easting 668000 Northing
                        northing = Math.Round(northing / 1000);
                        easting = Math.Round(easting / 1000);
                        return $"{bngSquare}{Math.Round(easting):00}{Math.Round(northing):00}";
                    default:
                    case OsDigitsType.OsDigits6: // 100m (6-figure) Grid Reference: TL123456 = 512300 Easting 245600 Northing
                        northing = Math.Round(northing / 100);
                        easting = Math.Round(easting / 100);
                        return $"{bngSquare}{Math.Round(easting):000}{Math.Round(northing):000}";
                    case OsDigitsType.OsDigits8:// 10m (8-figure) Grid Reference
                        northing = Math.Round(northing / 10);
                        easting = Math.Round(easting / 10);
                        return $"{bngSquare}{Math.Round(easting):0000}{Math.Round(northing):0000}";
                    case OsDigitsType.OsDigits10:// 1m (10-figure) Grid Reference
                        northing = Math.Round(northing);
                        easting = Math.Round(easting);
                        return $"{bngSquare}{Math.Round(easting):00000}{Math.Round(northing):00000}";
                    case OsDigitsType.OsDigits12:// 0.1m (12-figure) Grid Reference
                        northing = Math.Round(northing * 10);
                        easting = Math.Round(easting * 10);
                        return $"{bngSquare}{Math.Round(easting):00000}{Math.Round(northing):00000}";
                }
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