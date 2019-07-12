namespace GeoUK.Coordinates
{
    /// <summary>
    /// This immutable class represents a set of latitude/longitude/ellipsoidal height coordinates.
    /// </summary>
    public class LatitudeLongitude

    {
        private double _degreesLatitude = 0.0;
        private double _degreesLongitude = 0.0;
        private double _elipsoidalHeight = 0.0;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="degreesLatitude"></param>
        /// <param name="degreesLongitude"></param>
        public LatitudeLongitude(double degreesLatitude, double degreesLongitude)
        {
            _degreesLatitude = degreesLatitude;
            _degreesLongitude = degreesLongitude;
            _elipsoidalHeight = 0.0;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="degreesLatitude"></param>
        /// <param name="degreesLongitude"></param>
        /// <param name="elipsoidalHeight"></param>
        public LatitudeLongitude(double degreesLatitude, double degreesLongitude, double elipsoidalHeight)
        {
            _degreesLatitude = degreesLatitude;
            _degreesLongitude = degreesLongitude;
            _elipsoidalHeight = elipsoidalHeight;
        }

        /// <summary>
        /// Returns latitude in degrees.
        /// </summary>
		public double Latitude
        {
            get
            {
                return _degreesLatitude;
            }
        }

        /// <summary>
        /// Returns longitude in degrees.
        /// </summary>
		public double Longitude
        {
            get
            {
                return _degreesLongitude;
            }
        }

        /// <summary>
        /// returns elipsoidal height in meters.
        /// </summary>
        public double ElipsoidalHeight
        {
            get
            {
                return _elipsoidalHeight;
            }
        }
    }
}