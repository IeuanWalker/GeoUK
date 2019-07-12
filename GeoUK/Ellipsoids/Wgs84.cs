using System;

namespace GeoUK.Ellipsoids
{
    /// <summary>
	/// This immutable class, derived from Ellipsoid, represents an WGS84 ellipsoid and is provided for convienience.
    /// </summary>
    public class Wgs84 : Ellipsoid
    {
        //WGS constants
        private const double C_SEMI_MAJOR_AXIS = 6378137;             //a
        private const double C_SEMI_MINOR_AXIS = 6356752.3141;         //b
        /// <summary>
        /// Constructor.
        /// </summary>
        public Wgs84()
            : base(C_SEMI_MAJOR_AXIS, C_SEMI_MINOR_AXIS)
        {
        }
    }
}

