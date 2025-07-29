using System;
using System.Collections.Generic;

namespace GeoUK
{
    public static class Polygon
    {
        /// <summary>
        /// WGS84 equatorial radius in meters
        /// </summary>
        const double wgs84_equatorial_radius = 6378137.0;

        /// <summary>
        /// Maximum radius to prevent numerical instability (quarter of Earth's circumference)
        /// </summary>
        const double max_radius_meters = Math.PI * wgs84_equatorial_radius / 2.0;

        /// <summary>
        /// Generates a polygon around a given point.
        /// Each point is 'radius' from the center
        /// </summary>
        /// <param name="longitude">Longitude of the center point</param>
        /// <param name="latitude">Latitude of the center point</param>
        /// <param name="radius">Radius of the polygon in meters</param>
        /// <param name="numberOfPoints">Number of points to make up the polygon (minimum 3)</param>
        /// <returns>List of [longitude, latitude] coordinate arrays forming the polygon</returns>
        public static List<double[]> GeneratePolygonAroundPoint(double longitude, double latitude, double radius, int numberOfPoints)
        {
            if(latitude < -90 || latitude > 90)
            {
                throw new ArgumentOutOfRangeException(nameof(latitude), "Latitude must be between -90 and 90 degrees");
            }

            if(longitude < -180 || longitude > 180)
            {
                throw new ArgumentOutOfRangeException(nameof(longitude), "Longitude must be between -180 and 180 degrees");
            }

            if(radius <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(radius), "Radius must be positive");
            }

            if(radius > max_radius_meters)
            {
                throw new ArgumentOutOfRangeException(nameof(radius), $"Radius must not exceed {max_radius_meters:F0} meters");
            }

            // Handle polar regions - near poles, create a simple circle in projected coordinates
            if(Math.Abs(latitude) > 89.0)
            {
                throw new ArgumentOutOfRangeException(nameof(latitude), "Geodesic calculations are unstable near the poles (latitude > ±89°)");
            }

            if(numberOfPoints < 3)
            {
                numberOfPoints = 3;
            }

            // Pre-allocate with exact capacity (including closing point)
            List<double[]> coordinates = new List<double[]>(numberOfPoints + 1);

            // Pre-calculate common values to avoid repeated calculations
            double angleIncrement = 2.0 * Math.PI / numberOfPoints;
            double latRad = DegreesToRadians(latitude);
            double lonRad = DegreesToRadians(longitude);
            double angularDistance = radius / wgs84_equatorial_radius;

            // Pre-calculate trigonometric values for the center point
            double sinLatRad = Math.Sin(latRad);
            double cosLatRad = Math.Cos(latRad);
            double sinAngularDistance = Math.Sin(angularDistance);
            double cosAngularDistance = Math.Cos(angularDistance);

            for(int i = 0; i < numberOfPoints; i++)
            {
                double angle = angleIncrement * i;

                // Calculate destination latitude 
                double asinArg = sinLatRad * cosAngularDistance + cosLatRad * sinAngularDistance * Math.Cos(angle);
                asinArg = Math.Max(-1.0, Math.Min(1.0, asinArg));
                double lat2Rad = Math.Asin(asinArg);

                // Calculate destination longitude
                double lon2Rad = lonRad + Math.Atan2(
                    Math.Sin(angle) * sinAngularDistance * cosLatRad,
                    cosAngularDistance - sinLatRad * Math.Sin(lat2Rad)
                );

                // Convert to degrees and normalize longitude properly
                double lat2 = RadiansToDegrees(lat2Rad);
                double lon2 = RadiansToDegrees(lon2Rad);

                // Normalize longitude to [-180, 180) range
                lon2 = ((lon2 + 180.0) % 360.0) - 180.0;
                if(lon2 <= -180.0)
                {
                    lon2 += 360.0;
                }

                coordinates.Add(new double[] { lon2, lat2 });
            }

            // Close the polygon by repeating the first point
            coordinates.Add(new double[] { coordinates[0][0], coordinates[0][1] });

            return coordinates;
        }

        /// <summary>
        /// Converts degrees to radians.
        /// </summary>
        static double DegreesToRadians(double degrees) => degrees * Math.PI / 180.0;

        /// <summary>
        /// Converts radians to degrees.
        /// </summary>
        static double RadiansToDegrees(double radians) => radians * 180.0 / Math.PI;
    }
}
