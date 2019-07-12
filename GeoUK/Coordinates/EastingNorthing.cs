using System;
using System.Collections.Generic;
using System.Text;

namespace GeoUK.Coordinates
{
    /// <summary>
    /// this immutable class represents a set of easting/northing parameters. For convenience the class also
    /// includes a parameter for height.
    /// </summary>
    public class EastingNorthing
    {
        double _easting;
        double _northing;
        double _height;
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="easting"></param>
        /// <param name="northing"></param>
        public EastingNorthing(double easting, double northing)
        {
            _easting = easting;
            _northing = northing;
            _height = 0;
        }
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="easting"></param>
        /// <param name="northing"></param>
        /// <param name="height"></param>
        public EastingNorthing(double easting, double northing, double height)
        {
            _easting = easting;
            _northing = northing;
            _height = height;
        }
        /// <summary>
        /// Retruns the easting parameter.
        /// </summary>
        public double Easting
        {
            get { return _easting; }
        }
        /// <summary>
        /// returns the northing parameter.
        /// </summary>
        public double Northing
        {
            get { return _northing; }
        }
        /// <summary>
        /// Returns the height parameter.
        /// </summary>
        public double Height
        {
            get { return _height; }
        }
    }
}
