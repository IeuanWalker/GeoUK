using GeoUK.Ellipsoids;
using GeoUK.Projections;

namespace GeoUK.Coordinates
{
    /// <summary>
    /// This immutable class represents a set of latitude/longitude/ellipsoidal height coordinates.
    /// </summary>
    public class LatitudeLongitude
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="degreesLatitude"></param>
        /// <param name="degreesLongitude"></param>
        public LatitudeLongitude(double degreesLatitude, double degreesLongitude)
        {
            Latitude = degreesLatitude;
            Longitude = degreesLongitude;
            EllipsoidalHeight = 0.0;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="degreesLatitude"></param>
        /// <param name="degreesLongitude"></param>
        /// <param name="ellipsoidalHeight"></param>
        public LatitudeLongitude(double degreesLatitude, double degreesLongitude, double ellipsoidalHeight)
        {
            Latitude = degreesLatitude;
            Longitude = degreesLongitude;
            EllipsoidalHeight = ellipsoidalHeight;
        }

        /// <summary>
        /// Returns latitude in degrees.
        /// </summary>
        public double Latitude { get; }

        /// <summary>
        /// Returns longitude in degrees.
        /// </summary>
        public double Longitude { get; }

        /// <summary>
        /// returns ellipsoidal height in meters.
        /// </summary>
        public double EllipsoidalHeight { get; }

        /// <summary>
        /// Creates a <see cref="LatitudeLongitude"/> object from easting and northing coordinates.
        /// </summary>
        /// <param name="easting">The easting coordinate in meters.</param>
        /// <param name="northing">The northing coordinate in meters.</param>
        /// <returns>A <see cref="LatitudeLongitude"/> object representing the converted coordinates.</returns>
        public static LatitudeLongitude FromEastingNorthing(double easting, double northing)
        {
            // Convert to Cartesian
            Cartesian cartesian = Convert.ToCartesian(new Airy1830(),
                    new BritishNationalGrid(),
                    new EastingNorthing(easting, northing));

            //ETRS89 is effectively WGS84
            Cartesian wgsCartesian = Transform.Osgb36ToEtrs89(cartesian);

            return Convert.ToLatitudeLongitude(new Wgs84(), wgsCartesian);
        }
    }
}