using GeoUK.Coordinates;
using GeoUK.Ellipsoids;
using GeoUK.Projections;
using System;
using System.Globalization;

namespace GeoUK
{
    /// <summary>
    /// This class performs various generic conversions between coordinate systems and units of measure.
    /// This class does not perform transformations.
    /// </summary>
    public static class Convert
    {
        /// <summary>
        /// Method to convert from Easting Northing coordinates to Latitude Longitude coordinates.
        /// </summary>
        /// <returns>The latitude longitude.</returns>
        /// <param name = "ellipsoid"></param>
        /// <param name="projection">Projection.</param>
        /// <param name="coordinates">Coordinates.</param>
        public static LatitudeLongitude ToLatitudeLongitude(Ellipsoid ellipsoid, Projections.Projection projection, EastingNorthing coordinates)
        {
            double M;
            var N = coordinates.Northing;
            var E = coordinates.Easting;

            //from OS Guide
            //constants needed are a Semi-Major Axis , b , e2 , N0 , E0 , F0 ,φ0 , and λ0
            double a = ellipsoid.SemiMajorAxis;
            double b = ellipsoid.SemiMinorAxis;
            double e2 = ellipsoid.EccentricitySquared;
            double F0 = projection.ScaleFactor;
            double lat0 = ToRadians(projection.TrueOriginLatitude);
            double lon0 = ToRadians(projection.TrueOriginLongitude);
            double E0 = projection.TrueOriginEasting;
            double N0 = projection.TrueOriginNorthing;

            /*
			Console.WriteLine ("a: {0}", a);
			Console.WriteLine ("b: {0}", b);
			Console.WriteLine ("e2: {0}", e2);
			Console.WriteLine ("F0: {0}", F0);
			Console.WriteLine ("lat0 (deg): {0}", projection.TrueOriginLatitude);
			Console.WriteLine ("lon0 (deg): {0}", projection.TrueOriginLongitude);
			Console.WriteLine ("lat0 (rad): {0}", lat0);
			Console.WriteLine ("lon0 (rad): {0}", lon0);
			Console.WriteLine ("E0: {0}", E0);
			Console.WriteLine ("N0: {0}", N0);
			Console.WriteLine ("N: {0}", N);
			Console.WriteLine ("E: {0}", E);
			*/

            var lat = ((N - N0) / (a * F0)) + lat0;

            //double n = (a - b) / (a + b);
            //check for error and reiterate as required
            int loopCount = 0;
            do
            {
                M = CalculateM(lat, lat0, a, b, F0);

                lat += ((N - N0 - M) / (a * F0));

                /*
				Console.WriteLine ("\nlat #{0}: {1}", loopCount + 1, lat);
				Console.WriteLine ("M #{0}: {1}", loopCount + 1, M);
				Console.WriteLine ("N - N0 - M #{0}: {1}\n", loopCount + 1, N - N0 - M);
				*/

                loopCount++;
            } while (!IsNearlyZero(N - N0 - M, 1e-16) & loopCount < 10);

            double v = a * F0 * Math.Pow((1 - e2 * Math.Pow(Math.Sin(lat), 2)), -0.5);
            double p = a * F0 * (1 - e2) * Math.Pow((1 - e2 * Math.Pow(Math.Sin(lat), 2)), -1.5);
            double n2 = (v / p) - 1;

            var VII = Math.Tan(lat) / (2 * p * v);

            var VIIIa = Math.Tan(lat) / (24 * p * Math.Pow(v, 3));
            var VIIIb = 5 + (3 * Math.Pow(Math.Tan(lat), 2)) + n2 - 9 * (Math.Pow(Math.Tan(lat), 2)) * n2;
            var VIII = VIIIa * VIIIb;

            var IXa = Math.Tan(lat) / (720 * p * Math.Pow(v, 5));
            var IXb = 61 + (90 * Math.Pow(Math.Tan(lat), 2)) + (45 * Math.Pow(Math.Tan(lat), 4));
            var IX = IXa * IXb;

            var X = MathEx.Secant(lat) / v;

            var XIa = MathEx.Secant(lat) / (6 * Math.Pow(v, 3));
            var XIb = v / p + (2 * Math.Pow(Math.Tan(lat), 2));
            var XI = XIa * XIb;

            var XIIa = MathEx.Secant(lat) / (120 * Math.Pow(v, 5));
            var XIIb = 5 + (28 * Math.Pow(Math.Tan(lat), 2)) + (24 * Math.Pow(Math.Tan(lat), 4));
            var XII = XIIa * XIIb;

            var XIIAa = MathEx.Secant(lat) / (5040 * Math.Pow(v, 7));
            var XIIAb = 61 + (662 * Math.Pow(Math.Tan(lat), 2)) + (1320 * Math.Pow(Math.Tan(lat), 4)) + (720 * Math.Pow(Math.Tan(lat), 6));
            var XIIA = XIIAa * XIIAb;

            lat = lat - VII * Math.Pow(E - E0, 2) + VIII * Math.Pow(E - E0, 4) - IX * Math.Pow(E - E0, 6);
            var lon = lon0 + X * (E - E0) - XI * Math.Pow(E - E0, 3) + XII * Math.Pow(E - E0, 5) - XIIA * Math.Pow(E - E0, 7);

            /*
			Console.WriteLine ("v: {0}", v);
			Console.WriteLine ("p: {0}", p);
			Console.WriteLine ("n2: {0}", n2);
			Console.WriteLine ("VII: {0}", VII);
			Console.WriteLine ("VIII: {0}", VIII);
			Console.WriteLine ("IX: {0}", IX);
			Console.WriteLine ("X: {0}", X);
			Console.WriteLine ("XI: {0}", XI);
			Console.WriteLine ("XII: {0}", XII);
			Console.WriteLine ("XIIA: {0}", XIIA);
			Console.WriteLine ("lat (final): {0}", lat);
			Console.WriteLine ("lon: {0}", lon);
			*/

            return new LatitudeLongitude(ToDegrees(lat), ToDegrees(lon));
        }

        private static bool IsNearlyZero(double x, double tolerance)
        {
            if (x < 0)
            {
                x = x * -1;
            }

            return (x < 0 && x > tolerance * -1) || (x >= 0 && x < tolerance);
        }

        private static double CalculateM(double latitude, double latitudeOrigin, double semiMajorAxis, double semiMinorAxis, double scaleFactor)
        {
            double n = (semiMajorAxis - semiMinorAxis) / (semiMajorAxis + semiMinorAxis);

            double Ma = (1 + n + ((5.0 / 4.0) * Math.Pow(n, 2)) + ((5.0 / 4.0) * Math.Pow(n, 3))) * (latitude - latitudeOrigin);
            double Mb = ((3 * n) + (3 * Math.Pow(n, 2)) + ((21.0 / 8.0) * Math.Pow(n, 3))) * Math.Sin(latitude - latitudeOrigin) * Math.Cos(latitude + latitudeOrigin);
            double Mc = (((15.0 / 8.0) * Math.Pow(n, 2)) + (15.0 / 8.0) * Math.Pow(n, 3)) * Math.Sin(2 * (latitude - latitudeOrigin)) * Math.Cos(2 * (latitude + latitudeOrigin));
            double Md = (35.0 / 24.0 * Math.Pow(n, 3)) * Math.Sin(3 * (latitude - latitudeOrigin)) * Math.Cos(3 * (latitude + latitudeOrigin));
            double M = semiMinorAxis * scaleFactor * (Ma - Mb + Mc - Md);

            return M;
        }

        /// <summary>
        /// Converts decimal degrees to radians.
        /// </summary>
        /// <param name="degrees"></param>
        /// <returns></returns>
        public static double ToRadians(double degrees)
        {
            return degrees * (Math.PI / 180);
        }

        /// <summary>
        /// Converts radians to decimal degrees.
        /// </summary>
        /// <param name="radians"></param>
        /// <returns></returns>
        public static double ToDegrees(double radians)
        {
            return radians * (180 / Math.PI);
        }

        /// <summary>
        /// Converts cartesian coordinates to grid eastings and northings for any Transverse Mercator map projection, including the Ordnance Survey National Grid.
        /// Ellipsoid height is ignored.
        /// </summary>
        /// <remarks>
        /// When converting OSGB36 coordinates between (easting, northing) and (latitude, longitude),
        /// use the Airy 1830 ellipsoid. When converting ETRS89 coordinates between (easting, northing) and
        /// (latitude, longitude), use the GRS80 ellipsoid. Use the same National Grid projection
        /// constants for both ETRS89 and OSGB36 coordinates.
        /// </remarks>
        public static EastingNorthing ToEastingNorthing(Ellipsoid ellipsoid, Projections.Projection projection, Cartesian coordinates)
        {
            LatitudeLongitude coords = ToLatitudeLongitude(ellipsoid, coordinates);
            return ToEastingNorthing(ellipsoid, projection, coords);
        }

        /// <summary>
        /// Converts latitude and longitude to grid eastings and northings for any Transverse Mercator map projection, including the Ordnance Survey National Grid.
        /// Ellipsoid height is ignored.
        /// </summary>
        /// <remarks>
        /// When converting OSGB36 coordinates between (easting, northing) and (latitude, longitude),
        /// use the Airy 1830 ellipsoid. When converting ETRS89 coordinates between (easting, northing) and
        /// (latitude, longitude), use the GRS80 ellipsoid. Use the same National Grid projection
        /// constants for both ETRS89 and OSGB36 coordinates.
        /// </remarks>
        /// <param name="ellipsoid"></param>
        /// <param name="projection"></param>
        /// <param name="coordinates"></param>
        /// <returns></returns>
        public static EastingNorthing ToEastingNorthing(Ellipsoid ellipsoid, Projections.Projection projection, LatitudeLongitude coordinates)
        {
            double lat = ToRadians(coordinates.Latitude);
            double lon = ToRadians(coordinates.Longitude);

            //OS Document Transformation and OSGM02 User Guide, Appendix B.
            //B1
            double a = ellipsoid.SemiMajorAxis;
            double b = ellipsoid.SemiMinorAxis;
            double e2 = ellipsoid.EccentricitySquared;
            double F0 = projection.ScaleFactor;
            double lat0 = ToRadians(projection.TrueOriginLatitude);
            double lon0 = ToRadians(projection.TrueOriginLongitude);
            double E0 = projection.TrueOriginEasting;
            double N0 = projection.TrueOriginNorthing;

            //B2
            //double n = (a - b) / (a + b);

            //B3
            double v = a * F0 * Math.Pow((1 - e2 * Math.Pow(Math.Sin(lat), 2)), -0.5);

            //B4
            double p = a * F0 * (1 - e2) * Math.Pow((1 - e2 * Math.Pow(Math.Sin(lat), 2)), -1.5);

            //B5
            double n2 = v / p - 1;

            //B6
            var M = CalculateM(lat, lat0, a, b, F0);

            //			double Ma = (1 + n + ((5.0 / 4.0) * Math.Pow (n, 2)) + ((5.0 / 4.0) * Math.Pow (n, 3))) * (lat - lat0);
            //			double Mb = ((3 * n) + (3 * Math.Pow (n, 2)) + ((21.0 / 8.0) * Math.Pow (n, 3))) * Math.Sin (lat - lat0) * Math.Cos (lat + lat0);
            //			double Mc = (((15.0 / 8.0) * Math.Pow (n, 2)) + (15.0 / 8.0) * Math.Pow (n, 3)) * Math.Sin (2 * (lat - lat0)) * Math.Cos (2 * (lat + lat0));
            //			double Md = (35.0 / 24.0 * Math.Pow (n, 3)) * Math.Sin (3 * (lat - lat0)) * Math.Cos (3 * (lat + lat0));
            //			double M = b * F0 * (Ma - Mb + Mc - Md);

            double I = M + N0;
            double II = (v / 2) * Math.Sin(lat) * Math.Cos(lat);
            double III = (v / 24) * Math.Sin(lat) * Math.Pow(Math.Cos(lat), 3) * (5 - Math.Pow(Math.Tan(lat), 2) + 9 * n2);
            double IIIA = (v / 720) * Math.Sin(lat) * Math.Pow(Math.Cos(lat), 5) * (61 - 58 * Math.Pow(Math.Tan(lat), 2) + Math.Pow(Math.Tan(lat), 4));
            double IV = v * Math.Cos(lat);
            double V = (v / 6) * Math.Pow(Math.Cos(lat), 3) * ((v / p) - Math.Pow(Math.Tan(lat), 2));
            double VI = (v / 120) * Math.Pow(Math.Cos(lat), 5) * (5 - 18 * Math.Pow(Math.Tan(lat), 2) + Math.Pow(Math.Tan(lat), 4) + 14 * n2 - 58 * Math.Pow(Math.Tan(lat), 2) * n2);

            //B7
            double N = I + (II * Math.Pow((lon - lon0), 2)) + (III * Math.Pow((lon - lon0), 4)) + (IIIA * Math.Pow((lon - lon0), 6));

            //B8
            double E = E0 + (IV * (lon - lon0)) + (V * Math.Pow((lon - lon0), 3)) + (VI * Math.Pow((lon - lon0), 5));

            return new EastingNorthing(E, N, coordinates.ElipsoidalHeight); //height is still with respect to the ellipsoid
        }

        public static Cartesian ToCartesian(Ellipsoid ellipsoid, Projection projection, EastingNorthing coordinates)
        {
            var latLongCoordinates = ToLatitudeLongitude(
                ellipsoid,
                projection,
                coordinates);

            return ToCartesian(ellipsoid, latLongCoordinates);
        }

        /// <summary>
        /// Converts latitude, longitude and elipsoidal height coordinates to cartesian coordinates using the same ellipsoid.
        /// Please note this is not a transformation between ellipsoids.
        /// </summary>
        /// <param name="ellipsoid"></param>
        /// <param name="coordinates"></param>
        /// <returns></returns>
        public static Cartesian ToCartesian(Ellipsoid ellipsoid, LatitudeLongitude coordinates)
        {
            double lat = ToRadians(coordinates.Latitude);
            double lon = ToRadians(coordinates.Longitude);
            double height = coordinates.ElipsoidalHeight;

            double e2 = ellipsoid.EccentricitySquared;
            double a = ellipsoid.SemiMajorAxis;

            double v = a / Math.Sqrt(1 - (e2 * Math.Pow(Math.Sin(lat), 2)));

            double x = (v + height) * Math.Cos(lat) * Math.Cos(lon);
            double y = (v + height) * Math.Cos(lat) * Math.Sin(lon);
            double z = ((1 - e2) * v + height) * Math.Sin(lat);

            return new Cartesian(x, y, z);
        }

        /// <summary>
        /// Converts cartesian coordinates to latitude, longitude and elipsoidal height using the same ellipsoid.
        /// Please note this is not a transformation between ellipsoids.
        /// </summary>
        /// <param name="ellipsoid"></param>
        /// <param name="coordinates"></param>
        /// <returns></returns>
        public static LatitudeLongitude ToLatitudeLongitude(Ellipsoid ellipsoid, Cartesian coordinates)
        {
            double e2 = ellipsoid.EccentricitySquared;
            double a = ellipsoid.SemiMajorAxis;
            double p = Math.Sqrt(Math.Pow(coordinates.X, 2) + Math.Pow(coordinates.Y, 2));
            double lon = Math.Atan(coordinates.Y / coordinates.X);

            //have a first stab
            double v = 0.0;
            double lat = Math.Atan(coordinates.Z / (p * (1 - e2)));

            //iterate a few times 3 is enough but 10 to be safe
            for (int iterations = 0; iterations < 10; iterations++)
            {
                v = a / Math.Sqrt(1 - (e2 * Math.Pow(Math.Sin(lat), 2)));
                lat = Math.Atan((coordinates.Z + e2 * v * Math.Sin(lat)) / p);
            }
            double height = (p / Math.Cos(lat)) - v;

            return new LatitudeLongitude(ToDegrees(lat), ToDegrees(lon), height);
        }

        /// <summary>
        /// Converts degrees minutes and seconds to decimal degrees.
        /// </summary>
        /// <param name="degrees"></param>
        /// <param name="minutes"></param>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static double ToDecimelDegrees(int degrees, int minutes, double seconds)
        {
            //determine seconds as minutes
            double m = minutes + (seconds / 60.0);
            return ToDecimelDegrees(degrees, m);
        }

        /// <summary>
        /// Converts degrees and decimel minutes to decimal degrees.
        /// </summary>
        /// <param name="degrees"></param>
        /// <param name="minutes"></param>
        /// <returns></returns>
        public static double ToDecimelDegrees(int degrees, double minutes)
        {
            //determine minutes as derees
            return degrees + (minutes / 60.0);
        }

        private static double Div(double value, double divisor)
        {
            double dblResult = 0;
            bool blnNegative = false;

            //make the division
            dblResult = value / divisor;

            //do all calculations on positive numbers
            if (dblResult < 0)
            {
                blnNegative = true;
                dblResult = dblResult * -1;
            }

            //see if there is any remainder
            if (dblResult % 1 > 0)
            {
                dblResult = Math.Ceiling(dblResult) - 1;
            }
            else
            {
                dblResult = System.Convert.ToInt32(dblResult, CultureInfo.InvariantCulture);
            }

            if (blnNegative)
            {
                dblResult = Negate(dblResult);
            }

            return dblResult;
        }

        /// <summary>
        /// Helper funtion to reverse the sign of a value. Helps code to be more readable.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static double Negate(double value)
        {
            return value * -1.0;
        }
    }
}