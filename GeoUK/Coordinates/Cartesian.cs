using System;
using System.Collections.Generic;
using System.Text;

namespace GeoUK.Coordinates
{
    /// <summary>
    /// This immutable class represents a set of cartesian coordinates.
    /// </summary>
    public class Cartesian
    {
        private double _x;
        private double _z;
        private double _y;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public Cartesian(double x, double y, double z)
        {
            _x = x;
            _y = y;
            _z = z;
        }
        /// <summary>
        /// Returns the X axis parameter.
        /// </summary>
        public double X
        {
            get { return _x; }
        }
        /// <summary>
        /// Returns the Y axis parameter.
        /// </summary>
        public double Y
        {
            get { return _y; }
        }
        /// <summary>
        /// Returns the Z axis parameter.
        /// </summary>
        public double Z
        {
            get { return _z; }
        }

    }
}
