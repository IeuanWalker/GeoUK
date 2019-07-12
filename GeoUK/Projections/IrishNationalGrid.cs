namespace GeoUK.Projections
{
	/// <summary>
	/// This immutable class, derived from Projection, represents the British National Grid projection and is provided for convenience.
	/// </summary>
	public class IrishNationalGrid : Projection
	{
		private const double SCALE_FACTOR = 1.000035;
		private const double E = 200000;
		private const double TO_LAT = 53.5;
		private const double TO_LONG = -8;
		private const double N_NH = 250000;

		public IrishNationalGrid()
			: base(SCALE_FACTOR, E, N_NH, TO_LAT, TO_LONG)
		{
		}
	}
}