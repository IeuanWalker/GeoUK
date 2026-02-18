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
        /// The distance to another point in metres, including any difference in <see cref="EllipsoidalHeight"/>.
        /// Uses the Haversine formula for great-circle distance on a sphere. When both points have
        /// <see cref="EllipsoidalHeight"/> of zero the result equals the 2D horizontal distance.
        /// </summary>
        /// <param name="toLatitudeLongitude">The target point.</param>
        /// <returns>Distance in metres.</returns>
        public double DistanceTo(LatitudeLongitude toLatitudeLongitude)
        {
            if(toLatitudeLongitude == null)
                throw new System.ArgumentNullException(nameof(toLatitudeLongitude));

            const double earthRadius = 6371000.0; // mean Earth radius in metres

            double lat1 = Latitude * System.Math.PI / 180.0;
            double lat2 = toLatitudeLongitude.Latitude * System.Math.PI / 180.0;
            double deltaLat = (toLatitudeLongitude.Latitude - Latitude) * System.Math.PI / 180.0;
            double deltaLon = (toLatitudeLongitude.Longitude - Longitude) * System.Math.PI / 180.0;

            double a = System.Math.Sin(deltaLat / 2.0) * System.Math.Sin(deltaLat / 2.0) +
                       System.Math.Cos(lat1) * System.Math.Cos(lat2) *
                       System.Math.Sin(deltaLon / 2.0) * System.Math.Sin(deltaLon / 2.0);
            double c = 2.0 * System.Math.Atan2(System.Math.Sqrt(a), System.Math.Sqrt(1.0 - a));
            double horizontal = earthRadius * c;

            double deltaHeight = toLatitudeLongitude.EllipsoidalHeight - EllipsoidalHeight;
            return System.Math.Sqrt(horizontal * horizontal + deltaHeight * deltaHeight);
        }

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

            // ETRS89 is effectively WGS84
            Cartesian wgsCartesian = Transform.Osgb36ToEtrs89(cartesian);

            return Convert.ToLatitudeLongitude(new Wgs84(), wgsCartesian);
        }
    }
}