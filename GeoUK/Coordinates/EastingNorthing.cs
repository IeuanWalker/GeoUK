namespace GeoUK.Coordinates
{
    /// <summary>
    /// this immutable class represents a set of easting/northing parameters. For convenience the class also
    /// includes a parameter for height.
    /// </summary>
    public class EastingNorthing
    {

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="easting"></param>
        /// <param name="northing"></param>
        public EastingNorthing(double easting, double northing)
        {
            Easting = easting;
            Northing = northing;
            Height = 0;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="easting"></param>
        /// <param name="northing"></param>
        /// <param name="height"></param>
        public EastingNorthing(double easting, double northing, double height)
        {
            Easting = easting;
            Northing = northing;
            Height = height;
        }

        /// <summary>
        /// Returns the easting parameter.
        /// </summary>
        public double Easting { get; }

        /// <summary>
        /// returns the northing parameter.
        /// </summary>
        public double Northing { get; }

        /// <summary>
        /// Returns the height parameter.
        /// </summary>
        public double Height { get; }
    }
}