using System;

namespace GeoUK.Ellipsoids
{
    /// <summary>
    /// This immutable class represents a generic ellipsoid and is used as a base type for other specific ellipsoids.
    /// </summary>
    public class Ellipsoid
    {

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="semiMajorAxis"></param>
        /// <param name="semiMinorAxis"></param>
        public Ellipsoid(double semiMajorAxis, double semiMinorAxis)
        {
            SemiMinorAxis = semiMinorAxis;
            SemiMajorAxis = semiMajorAxis;
            EccentricitySquared = (Math.Pow(semiMajorAxis, 2) - Math.Pow(semiMinorAxis, 2)) / Math.Pow(semiMajorAxis, 2);
            Eccentricity = Math.Sqrt(EccentricitySquared);
        }

        /// <summary>
        /// Calculates the Radius of curvature for a given latitude.
        /// </summary>
        /// <param name="degreesLatitude"></param>
        /// <returns></returns>
        public double GetRadiusOfCurvatureInPV(double degreesLatitude)
        {
            double dblRadians = DegreesToRadians(degreesLatitude);
            return SemiMajorAxis / Math.Pow((1 - EccentricitySquared * Math.Pow(Math.Sin(dblRadians), 2)), 0.5);
        }

        /// <summary>
        /// Returns the semi-major axis of the ellipsoid.
        /// </summary>
        public double SemiMajorAxis { get; }

        /// <summary>
        /// Returns the semi-major axis of the ellipsoid.
        /// </summary>
        public double SemiMinorAxis { get; }

        /// <summary>
        /// returns the eccentricity of the ellipsoid.
        /// </summary>
        public double Eccentricity { get; }

        /// <summary>
        /// returns the eccentricity squared of the ellipsoid.
        /// </summary>
        public double EccentricitySquared { get; }

        /// <summary>
        /// returns the second eccentricity squared of the ellipsoid.
        /// </summary>
        public double SecondEccentricitySquared => (Math.Pow(SemiMajorAxis, 2) - Math.Pow(SemiMinorAxis, 2)) / Math.Pow(SemiMinorAxis, 2);

        /// <summary>
        /// Returns radians for a given value of degrees.
        /// </summary>
        /// <param name="degrees"></param>
        /// <returns></returns>
        protected double DegreesToRadians(double degrees) => degrees * (Math.PI / 180);

        /// <summary>
        /// Returns degrees for a given value of radians.
        /// </summary>
        /// <param name="radians"></param>
        /// <returns></returns>
        protected double RadiansToDegrees(double radians) => radians * (180 / Math.PI);
    }
}