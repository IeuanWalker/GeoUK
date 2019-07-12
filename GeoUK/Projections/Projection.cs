using System;
using System.Globalization;

namespace GeoUK.Projections
{
    /// <summary>
    /// This immutable class represents a generic projection and is used as a base type for other specific projections.
    /// </summary>
    public class Projection
    {
        public Projection(double scaleFactor, double trueOriginEasting, double trueOriginNorthing, double trueOriginLatitude, double trueOriginLongitude)
        {
            ScaleFactor = scaleFactor;
            TrueOriginEasting = trueOriginEasting;
            TrueOriginNorthing = trueOriginNorthing;
            TrueOriginLatitude = trueOriginLatitude;
            TrueOriginLongitude = trueOriginLongitude;
        }

        /// <summary>
        /// Returns the scale factor.
        /// </summary>
        public double ScaleFactor { get; }

        /// <summary>
        /// Returns the Easting coordinate of the true origin.
        /// </summary>
        public double TrueOriginEasting { get; }

        /// <summary>
        /// Returns the Northing coordinate of the true origin.
        /// </summary>
        public double TrueOriginNorthing { get; }

        /// <summary>
        /// Returns the Latitude coordinate of the true origin for the southern hemisphere.
        /// </summary>
        public double TrueOriginLatitude { get; }

        /// <summary>
        /// Returns the Longitude coordinate of the true origin for the southern hemisphere.
        /// </summary>
        public double TrueOriginLongitude { get; }

        /// <summary>
        /// Returns the integer portion of a division operation.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="divisor"></param>
        /// <returns></returns>
        protected static double Div(double value, double divisor)
        {
            //make the division
            double dblResult = value / divisor;

            //do all calculations on positive numbers
            bool blnNegative = false;
            if (dblResult < 0)
            {
                blnNegative = true;
                dblResult *= -1;
            }

            //see if there is any remainder
            dblResult = dblResult % 1 > 0
                ? Math.Ceiling(dblResult) - 1
                : System.Convert.ToInt32(dblResult, CultureInfo.InvariantCulture);

            if (blnNegative)
            {
                dblResult *= -1.0;
            }

            return dblResult;
        }
    }
}