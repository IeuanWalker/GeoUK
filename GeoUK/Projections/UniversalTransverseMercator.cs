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
		private const double TO_LAT = 0; //is this for Britain? These will need to be calculated based on the lat and long being transformed.
		private const double N_NH = 0;
		private const double N_SH = 10000000;

		public UniversalTransverseMercator(double degreesLatitude, double degreesLongitude)
			: base(SCALE_FACTOR, E, CalculateOriginNorthing(degreesLatitude), TO_LAT, CalculateLongitudeOrigin(degreesLongitude))
		{
		}

		private static double CalculateOriginNorthing(double degreesLatitude) => degreesLatitude < 0 ? N_SH : N_NH;

		private static double CalculateLongitudeOrigin(double degreesLongitude)
		{
			double longOrigin;
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