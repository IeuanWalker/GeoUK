using GeoUK.Coordinates;
using System;

namespace GeoUK
{
    /// <summary>
    /// This class performs transformations between datums.
    /// </summary>
    public static class Transform
    {
        /// <summary>
        /// Performs an ETRS89 to OSGB36 datum transformation. Accuracy is approximately 5 meters in all directions.
        /// For this method, ERTS89, ITRS2000 and WGS84 datums can be considered the same.
        /// </summary>
        /// <param name="coordinates">Cartesian Coordinates to be transformed.</param>
        /// <remarks>
        /// This method uses a Helmert transformation to determine the OSGB36 coordinates.
        /// Whilst only accurate to 5 meters in all directions, it is extremely fast.
        /// </remarks>
        public static Cartesian Etrs89ToOsgb36(Cartesian coordinates)
        {
            //tX (m)        tY (m)      tZ (m)      s (ppm)     rX (sec)        rY (sec)        rZ (sec)
            //-446.448      125.157   - 542.060     20.4894   - 0.1502          - 0.247       - 0.8421

            //set up the parameters
            double tx = -446.448;
            double ty = 125.157;
            double tz = -542.060;
            double s = 20.4894;
            double rx = ToRadians(ToDecimalDegrees(0, 0, -0.1502));
            double ry = ToRadians(ToDecimalDegrees(0, 0, -0.247));
            double rz = ToRadians(ToDecimalDegrees(0, 0, -0.8421));

            Cartesian result = HelmertTransformation(coordinates, tx, ty, tz, rx, ry, rz, s);

            return result;
        }

        /// <summary>
        /// Performs an OSGB36 to ETRS89 datum transformation. Accuracy is approximately 5 meters in all directions.
        /// For this method, ERTS89, ITRS2000 and WGS84 datums can be considered the same.
        /// </summary>
        /// <param name="coordinates">Cartesian Coordinates to be transformed.</param>
        /// <returns></returns>
        public static Cartesian Osgb36ToEtrs89(Cartesian coordinates)
        {
            //(BUT CHANGE SIGNS OF EACH PARAMETER FOR REVERSE)
            //tX (m)        tY (m)      tZ (m)      s (ppm)     rX (sec)        rY (sec)        rZ (sec)
            //-446.448      125.157   - 542.060     20.4894   - 0.1502          - 0.247       - 0.8421

            double tx = 446.448;
            double ty = -125.157;
            double tz = 542.060;
            double s = -20.4894;
            double rx = ToRadians(ToDecimalDegrees(0, 0, 0.1502));
            double ry = ToRadians(ToDecimalDegrees(0, 0, 0.247));
            double rz = ToRadians(ToDecimalDegrees(0, 0, 0.8421));

            return HelmertTransformation(coordinates, tx, ty, tz, rx, ry, rz, s);
        }

        /*
		 *  //(BUT CHANGE SIGNS OF EACH PARAMETER FOR REVERSE)
			tX (m)   tY (m)   tZ (m)   s (ppm)  rX (sec) rY (sec) rZ (sec)
			-446.448 +125.157 -542.060 +20.4894 -0.1502  -0.2470  -0.8421

		 */

        /// <summary>
        /// Performs an ETRS89 to ITRS2000 datum transformation.
        /// </summary>
        /// <param name="coordinates">Cartesian Coordinates to be transformed.</param>
        /// <param name="epochYear">Refers to the year the data specified in coordinates was gathered.</param>
        /// <returns></returns>
        public static Cartesian Etrs89ToItrs2000(Cartesian coordinates, int epochYear)
        {
            //tX (m)        tY (m)      tZ (m)      s (ppm)     rX (sec)        rY (sec)        rZ (sec)
            //0.054         0.051      - 0.048      0           0.000081 dt     0.00049 dt     - 0.000792 dt

            //dt represents shift in years since time of survey to when ETRS89 was determined

            int dt = epochYear - 1989;
            //set up the parameters
            double tx = Negate(0.054);
            double ty = Negate(0.051);
            double tz = Negate(-0.048);
            double s = 0;
            double rx = Negate(ToRadians(ToDecimalDegrees(0, 0, 0.000081) * dt));
            double ry = Negate(ToRadians(ToDecimalDegrees(0, 0, 0.00049) * dt));
            double rz = Negate(ToRadians(ToDecimalDegrees(0, 0, -0.000792) * dt));

            return HelmertTransformation(coordinates, tx, ty, tz, rx, ry, rz, s);
        }

        /// <summary>
        /// Performs an ITRS2000 to ETRS89 datum transformation.
        /// </summary>
        /// <param name="coordinates">Cartesian Coordinates to be transformed.</param>
        /// <param name="epochYear">Refers to the year the data specified in coordinates was gathered.</param>
        /// <returns></returns>
        public static Cartesian Itrs2000ToEtrs89(Cartesian coordinates, int epochYear)
        {
            //tX (m)        tY (m)      tZ (m)      s (ppm)     rX (sec)        rY (sec)        rZ (sec)
            //0.054         0.051      - 0.048      0           0.000081 dt     0.00049 dt     - 0.000792 dt

            //dt represents shift in years since time of survey to when ETRS89 was determined

            int dt = epochYear - 1989;
            //set up the parameters
            double tx = 0.054;
            double ty = 0.051;
            double tz = -0.048;
            double s = 0;
            double rx = ToRadians(ToDecimalDegrees(0, 0, 0.000081) * dt);
            double ry = ToRadians(ToDecimalDegrees(0, 0, 0.00049) * dt);
            double rz = ToRadians(ToDecimalDegrees(0, 0, -0.000792) * dt);

            return HelmertTransformation(coordinates, tx, ty, tz, rx, ry, rz, s);
        }

        /// <summary>
        /// Performs an ITRS94/96/97 to ETRS89 datum transformation.
        /// </summary>
        /// <param name="coordinates">Cartesian Coordinates to be transformed.</param>
        /// <param name="epochYear">Refers to the year the data specified in coordinates was gathered.</param>
        /// <returns></returns>
        public static Cartesian Itrs97ToEtrs89(Cartesian coordinates, int epochYear)
        {
            //ITRS94/96/97 to ETRS89 datum transformation
            //tX (m)        tY (m)      tZ (m)      s (ppm)     rX (sec)        rY (sec)        rZ (sec)
            //0.041         0.041      - 0.049      0           0.00020 dt      0.00050 dt     - 0.00065 dt

            //dt represents shift in years since time of survey to when ETRS89 was determined

            int dt = epochYear - 1989;
            //set up the parameters
            double tx = 0.041;
            double ty = 0.041;
            double tz = -0.049;
            double s = 0;
            double rx = ToRadians(ToDecimalDegrees(0, 0, 0.00020) * dt);
            double ry = ToRadians(ToDecimalDegrees(0, 0, 0.00050) * dt);
            double rz = ToRadians(ToDecimalDegrees(0, 0, -0.00065) * dt);

            return HelmertTransformation(coordinates, tx, ty, tz, rx, ry, rz, s);
        }

        /// <summary>
        /// Performs an ETRS89 to ITRS94/96/97 datum transformation.
        /// </summary>
        /// <param name="coordinates">Cartesian Coordinates to be transformed.</param>
        /// <param name="epochYear">Refers to the year the data specified in coordinates was gathered.</param>
        /// <returns></returns>
        public static Cartesian Etrs89ToItrs97(Cartesian coordinates, int epochYear)
        {
            //ITRS94/96/97 to ETRS89 datum transformation (BUT CHANGE SIGNS OF EACH PARAMETER FOR REVERSE)
            //tX (m)        tY (m)      tZ (m)      s (ppm)     rX (sec)        rY (sec)        rZ (sec)
            //0.041         0.041      - 0.049      0           0.00020 dt      0.00050 dt     - 0.00065 dt

            //dt represents shift in years since time of survey to when ETRS89 was determined

            int dt = epochYear - 1989;
            //set up the parameters
            double tx = Negate(0.041);
            double ty = Negate(0.041);
            double tz = Negate(-0.049);
            double s = 0;
            double rx = Negate(ToRadians(ToDecimalDegrees(0, 0, 0.00020) * dt));
            double ry = Negate(ToRadians(ToDecimalDegrees(0, 0, 0.00050) * dt));
            double rz = Negate(ToRadians(ToDecimalDegrees(0, 0, -0.00065) * dt));

            return HelmertTransformation(coordinates, tx, ty, tz, rx, ry, rz, s);
        }

        private static double ToDecimalDegrees(int degrees, int minutes, double seconds)
        {
            //determine seconds as minutes
            double m = minutes + (seconds / 60.0);
            return ToDecimalDegrees(degrees, m);
        }

        // determine minutes as degrees
        private static double ToDecimalDegrees(int degrees, double minutes) => degrees + (minutes / 60.0);

        private static double ToRadians(double degrees) => degrees * (Math.PI / 180.0);

        /// <summary>
        /// This seven parameter method can be used to transform coordinates between datums.
        /// </summary>
        /// <remarks>
        /// This method assumes that the rotation
        /// parameters are ‘small’. Rotation parameters between geodetic cartesian systems are usually less than 5
        /// seconds of arc, because the axes are conventionally aligned to the Greenwich Meridian and the Pole.
        /// Do not use this formula for larger angles.
        /// </remarks>
        /// <param name="coordinates"></param>
        /// <param name="translationX"></param>
        /// <param name="translationY"></param>
        /// <param name="translationZ"></param>
        /// <param name="rotationX"></param>
        /// <param name="rotationY"></param>
        /// <param name="rotationZ"></param>
        /// <param name="scaleFactorPpm"></param>
        /// <returns></returns>
        public static Cartesian HelmertTransformation(Cartesian coordinates, double translationX, double translationY, double translationZ, double rotationX, double rotationY, double rotationZ, double scaleFactorPpm)
        {
            //to compute this helmert translation we have to multiply XYZa by R and add to T
            //
            //    : X :B  : TX :   : 1+s  -rz   ry  :  : X :A
            //    :   :   :    :   :                :  :   :
            //    : Y : = : TY : + :  rz   1+s  -rx :. : Y :
            //    :   :   :    :   :                :  :   :
            //    : Z :   : TZ :   : -ry    rx  1+s :  : Z :

            //scale factor passed in as a parts/million measure
            double scaleFactor = scaleFactorPpm / 1000000.0;

            //create initial matrixes  (cols, rows)
            double[] XYZa = new double[3]; //initial xyz
            XYZa[0] = coordinates.X;
            XYZa[1] = coordinates.Y;
            XYZa[2] = coordinates.Z;

            //populate main matrix 'R'
            double[,] R = new double[3, 3];
            //top row
            R[0, 0] = 1 + scaleFactor; R[1, 0] = -rotationZ; R[2, 0] = rotationY;
            //second row
            R[0, 1] = rotationZ; R[1, 1] = 1 + scaleFactor; R[2, 1] = -rotationX;
            //third row
            R[0, 2] = -rotationY; R[1, 2] = rotationX; R[2, 2] = 1 + scaleFactor;

            //populate matrix 'T'
            double[] T = new double[3];
            T[0] = translationX; T[1] = translationY; T[2] = translationZ;

            //intermediate result of the multiplication goes here
            double[] temp = new double[3];

            //final result goes here
            //double[,] XYZb = new double[1, 3]; //result

            //start with the multiplication of matrix XYZa and R
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    temp[row] = temp[row] + (R[col, row] * XYZa[col]);
                }
            }

            //adding to T whilst creating the cartesian coordinates
            return new Cartesian(T[0] + temp[0], T[1] + temp[1], T[2] + temp[2]);
        }

        /// <summary>
        /// Helper function to reverse the sign of a value. Helps code to be more readable.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static double Negate(double value) => value * -1.0;
    }
}