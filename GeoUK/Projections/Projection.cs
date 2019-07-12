using System;
using System.Globalization;

namespace GeoUK.Projections
{
    /// <summary>
    /// This immutable class represents a generic projection and is used as a base type for other specific projections.
    /// </summary>
    public class Projection
    {
        private double _trueOriginLatitude = 0.0;
        private double _trueOriginLongitude = 0.0;
        private double _scaleFactor = 0.0;
        private double _trueOriginEasting = 0.0;
        private double _trueOriginNorthing = 0.0;

        /// <summary>
        /// Constructor.
        /// </summary>
        public Projection(double scaleFactor, double trueOriginEasting, double trueOriginNorthing, double trueOriginLatitude, double trueOriginLongitude)
        {
            _scaleFactor = scaleFactor;
            _trueOriginEasting = trueOriginEasting;
            _trueOriginNorthing = trueOriginNorthing;
            _trueOriginLatitude = trueOriginLatitude;
            _trueOriginLongitude = trueOriginLongitude;
        }

        /// <summary>
        /// Returns the scale factor.
        /// </summary>
        public double ScaleFactor
        {
            get { return _scaleFactor; }
        }

        /// <summary>
        /// Returns the Easting coordinate of the true origin.
        /// </summary>
        public double TrueOriginEasting
        {
            get { return _trueOriginEasting; }
        }

        /// <summary>
        /// Returns the Northing coordinate of the true origin.
        /// </summary>
        public double TrueOriginNorthing
        {
            get { return _trueOriginNorthing; }
        }

        /// <summary>
        /// Returns the Latitude coordinate of the true origin for the southern hemisphere.
        /// </summary>
        public double TrueOriginLatitude
        {
            get { return _trueOriginLatitude; }
        }

        /// <summary>
        /// Returns the Longitude coordinate of the true origin for the southern hemisphere.
        /// </summary>
        public double TrueOriginLongitude
        {
            get { return _trueOriginLongitude; }
        }

        //public static double DegreesToRadians(double degrees)
        //{
        //    return degrees * (Math.PI / 180);
        //}
        //public static double RadiansToDegrees(double radians)
        //{
        //    return radians * (180 / Math.PI);
        //}
        /// <summary>
        /// Returns the integer portion of a division operation.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="divisor"></param>
        /// <returns></returns>
        protected static double Div(double value, double divisor)
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
                dblResult = dblResult * -1.0;
            }

            return dblResult;
        }
    }
}