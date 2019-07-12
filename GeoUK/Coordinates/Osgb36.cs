using System;

namespace GeoUK.Coordinates
{
    /// <summary>
    /// This immutable class, derived from EastingNorthingCoordinates, provides a convenient means
    /// to represent OSGB36 eastings and northings.
    /// </summary>
    /// <remarks>
    /// Eastings and northings are represented in British National Grid and Height is specifgied
    /// in meters based on the geoid datum returned by the RegionGeoidDatum property.
    /// </remarks>
    public class Osgb36 : EastingNorthing
    {
        private Osgb36GeoidDatum _datum = Osgb36GeoidDatum.OutsideModelBoundary;

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoUK.Coordinates.Osgb36Cordinates"/> class.
        /// </summary>
        /// <param name="easting">Easting.</param>
        /// <param name="northing">Northing.</param>
        public Osgb36(double easting, double northing)
            : base(easting, northing, 0)
        {
            _datum = Osgb36GeoidDatum.NewlynUkMainland;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoUK.Coordinates.Osgb36Cordinates"/> class.
        /// </summary>
        /// <param name="eastingNorthingCoordinates">Easting northing coordinates.</param>
        public Osgb36(EastingNorthing eastingNorthingCoordinates)
            : base(eastingNorthingCoordinates.Easting, eastingNorthingCoordinates.Northing, eastingNorthingCoordinates.Height)
        {
            _datum = Osgb36GeoidDatum.NewlynUkMainland;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoUK.Coordinates.Osgb36Cordinates"/> class.
        /// </summary>
        /// <param name="eastingNorthingCoordinates">Easting northing coordinates.</param>
        /// <param name="datum">Datum.</param>
        public Osgb36(EastingNorthing eastingNorthingCoordinates, Osgb36GeoidDatum datum)
            : base(eastingNorthingCoordinates.Easting, eastingNorthingCoordinates.Northing, eastingNorthingCoordinates.Height)
        {
            _datum = datum;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoUK.Coordinates.Osgb36Cordinates"/> class.
        /// </summary>
        /// <param name="easting">Easting.</param>
        /// <param name="northing">Northing.</param>
        /// <param name="height">Height.</param>
        /// <param name="datum">Datum.</param>
        public Osgb36(double easting, double northing, double height, Osgb36GeoidDatum datum)
            : base(easting, northing, height)
        {
            _datum = datum;
        }

        /// <summary>
        /// Returns the Local Geoid datum in use. other property values should be
        /// considered invalid if this property is set to OutsideModelBoundary.
        /// </summary>
        public Osgb36GeoidDatum RegionGeoidDatum
        {
            get { return _datum; }
        }

        public string MapReference
        {
            get
            {
                /*
				10km (2-figure) Grid Reference: SO84 = 380000 Easting 240000 Northing
				1km (4-figure) Grid Reference: NS2468 = 224000 Easting 668000 Northing
				100m (6-figure) Grid Reference: TL123456 = 512300 Easting 245600 Northing
			*/
                var easting = this.Easting;
                var northing = this.Northing;

                var bngSquare = GetBngSquare(easting, northing);

                //get the number of complete 500k squares
                var indexNorthing = (int)Math.Floor(northing / 500000);
                var indexEasting = (int)Math.Floor(easting / 500000);

                //reduce E and N by the number of 500k squares
                northing = northing - indexNorthing * 500000;
                easting = easting - indexEasting * 500000;

                //reduce by the number of 100k squares within the 500k square.
                indexNorthing = (int)Math.Floor(northing) / 100000;
                indexEasting = (int)Math.Floor(easting) / 100000;

                northing = northing - indexNorthing * 100000;
                easting = easting - indexEasting * 100000;

                northing = Math.Round(northing / 100);
                easting = Math.Round(easting / 100);
                return string.Format("{0}{1}{2}", bngSquare, Math.Round(easting).ToString("000"), Math.Round(northing).ToString("000"));
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
            var result = string.Empty;

            //test for our upper and lower limits
            if (easting >= 0 && easting < 700000 && northing >= 0 && northing < 1300000)
            {
                var firstChar = new char[6] { 'S', 'N', 'H', 'T', 'O', 'J' };
                var secondChar = new char[25] { 'V', 'Q', 'L', 'F', 'A', 'W', 'R', 'M', 'G', 'B', 'X', 'S', 'N', 'H', 'C', 'Y', 'T', 'O', 'J', 'D', 'Z', 'U', 'P', 'K', 'E' };

                //calculate the first letter
                var indexNorthing = (int)Math.Floor(northing / 500000);
                var indexEasting = (int)Math.Floor(easting / 500000);

                //get the first char
                var chr1 = firstChar[(indexEasting * 3) + indexNorthing];

                //to get the second letter we subtract the number of 500km sectors calculated above
                indexNorthing = (int)Math.Floor((northing - (indexNorthing * 500000)) / 100000);
                indexEasting = (int)Math.Floor((easting - (indexEasting * 500000)) / 100000);

                //get the second char
                var chr2 = secondChar[(indexEasting * 5) + indexNorthing];

                result = string.Format("{0}{1}", chr1, chr2);
            }
            return result;
        }
    }
}