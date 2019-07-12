using System.Globalization;

namespace GeoUK.Projections
{
    /// <summary>
    /// This immutable class, derived from Projection, represents the Universal Transverse Mercator projection.
    /// The class handles all medidian calculations internally.
    /// </summary>
    public class UniversalTransverseMercator : Projection
    {
        private const double SCALE_FACTOR = 0.9996;
        private const double E = 500000;
        private const double TO_LAT = 0; //is this for britain? These will need to be calculated based on the lat and long being transfformed.
        private const double N_NH = 0;
        private const double N_SH = 10000000;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="degreesLatitude"></param>
        /// <param name="degreesLongitude"></param>
		public UniversalTransverseMercator(double degreesLatitude, double degreesLongitude)
            : base(SCALE_FACTOR, E, CalculateOriginNorthing(degreesLatitude, degreesLongitude), TO_LAT, CalculateLongitudeOrigin(degreesLatitude, degreesLongitude))
        {
        }

        private static double CalculateOriginNorthing(double degreesLatitude, double degreesLongitude)
        {
            double falseNorthing = 0.0;
            //sort out false northing
            if (degreesLatitude < 0)
            {
                falseNorthing = N_SH;
            }
            else
            {
                falseNorthing = N_NH;
            }
            return falseNorthing;
        }

        private static double CalculateLongitudeOrigin(double degreesLatitude, double degreesLongitude)
        {
            double longOrigin = 0.0;

            if (degreesLongitude < 0)
            {
                if (degreesLongitude > -6)
                {
                    longOrigin = -3;
                }
                else
                {
                    longOrigin = Div(degreesLongitude, 6.0);
                    longOrigin = (System.Convert.ToInt32(longOrigin, CultureInfo.InvariantCulture)) * 6 - 3;
                }
            }
            else
            {
                if (degreesLongitude < 6)
                {
                    longOrigin = 3.0;
                }
                else
                {
                    longOrigin = Div(degreesLongitude, 6);
                    longOrigin = (System.Convert.ToInt32(longOrigin, CultureInfo.InvariantCulture)) * 6 + 3;
                }
            }
            return longOrigin;
        }
    }
}