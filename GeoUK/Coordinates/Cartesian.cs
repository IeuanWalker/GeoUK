namespace GeoUK.Coordinates
{
    /// <summary>
    /// This immutable class represents a set of cartesian coordinates.
    /// </summary>
    public class Cartesian
    {

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public Cartesian(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Returns the X axis parameter.
        /// </summary>
        public double X { get; }

        /// <summary>
        /// Returns the Y axis parameter.
        /// </summary>
        public double Y { get; }

        /// <summary>
        /// Returns the Z axis parameter.
        /// </summary>
        public double Z { get; }
    }
}