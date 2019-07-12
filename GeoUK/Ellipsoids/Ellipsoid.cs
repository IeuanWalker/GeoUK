using System;
using System.Collections.Generic;
using System.Text;

namespace GeoUK.Ellipsoids
{
    /// <summary>
    /// This immutable class represents a generic ellipsoid and is used as a base type for other specific ellipsoids.
    /// </summary>
    public class Ellipsoid
    {

        private double _semiMajorAxis = 0.0;
        private double _semiMinorAxis = 0.0;
        private double _eccentricity = 0.0;
        private double _eccentricitySquared = 0.0;
        
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="semiMajorAxis"></param>
        /// <param name="semiMinorAxis"></param>
        public Ellipsoid(double semiMajorAxis, double semiMinorAxis)
        {
            _semiMinorAxis = semiMinorAxis;
            _semiMajorAxis = semiMajorAxis;
            _eccentricitySquared = (Math.Pow(semiMajorAxis, 2) - Math.Pow(semiMinorAxis, 2)) / Math.Pow(semiMajorAxis, 2);
            _eccentricity = Math.Sqrt(_eccentricitySquared);
        }
        /// <summary>
        /// Calculates the Radius of curvature for a given latitude.
        /// </summary>
        /// <param name="degreesLatitude"></param>
        /// <returns></returns>
        public double GetRadiusOfCurvatureInPV(double degreesLatitude)
        {
            double dblRadians = DegreesToRadians(degreesLatitude);

            //this is the working VB example
            //GetRadiusOfCurvatureInPV = C_SEMI_MAJOR_AXIS / (1 - C_ECCENTRICITY_SQUARED * Sin(Latitude) ^ 2) ^ 0.5

            //GetRadiusOfCurvatureInPV = 6392142.007676
            //return (C_SEMI_MAJOR_AXIS / Math.Pow((1 - m_EccentricitySquared * Math.Pow(Math.Sin(dblRadians),2)), 0.5));
            return _semiMajorAxis / Math.Pow((1 - _eccentricitySquared * Math.Pow(Math.Sin(dblRadians), 2)), 0.5);
        }
        /// <summary>
        /// Returns the semi-major axis of the ellipsoid.
        /// </summary>
        public double SemiMajorAxis
        {
            get
            {
                return _semiMajorAxis;
            }
        }
        /// <summary>
        /// Returns the semi-major axis of the ellipsoid.
        /// </summary>
        public double SemiMinorAxis
        {
            get
            {
                return _semiMinorAxis;
            }
        }
        /// <summary>
        /// returns the eccentricity of the ellipsoid.
        /// </summary>
        public double Eccentricity
        {
            get
            {
                return _eccentricity;
            }
        }
        /// <summary>
        /// returns the eccentricity squared of the ellipsoid.
        /// </summary>
        public double EccentricitySquared
        {
            get
            {
                return _eccentricitySquared;
            }
        }
        /// <summary>
        /// returns the second eccentricity squared of the ellipsoid.
        /// </summary>
        public double SecondEccentricitySquared
        {
            get
            {
                return (Math.Pow(_semiMajorAxis, 2) - Math.Pow(_semiMinorAxis, 2)) / Math.Pow(_semiMinorAxis, 2);
            }
        }
        /// <summary>
        /// Returns radians for a given value of degrees.
        /// </summary>
        /// <param name="degrees"></param>
        /// <returns></returns>
        protected double DegreesToRadians(double degrees)
        {
            return degrees * (Math.PI / 180);
        }
        /// <summary>
        /// Returns degrees for a given value of radians.
        /// </summary>
        /// <param name="radians"></param>
        /// <returns></returns>
        protected double RadiansToDegrees(double radians)
        {
            return radians * (180 / Math.PI);
        }
    }
}
