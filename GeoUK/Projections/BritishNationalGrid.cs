namespace GeoUK.Projections
{
    /// <summary>
    /// This immutable class, derived from Projection, represents the British National Grid projection and is provided for convenience.
    /// </summary>
    public class BritishNationalGrid : Projection
    {
        private const double SCALE_FACTOR = 0.9996012717;
        private const double E = 400000;
        private const double TO_LAT = 49;
        private const double TO_LONG = -2;
        private const double N_NH = -100000;

        public BritishNationalGrid()
            : base(SCALE_FACTOR, E, N_NH, TO_LAT, TO_LONG)
        {
        }
    }
}