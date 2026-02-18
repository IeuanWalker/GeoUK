namespace GeoUK.Coordinates
{
    /// <summary>
    /// this immutable class represents a set of easting/northing parameters. For convenience the class also includes a parameter for height.
    /// </summary>
    public class EastingNorthing
    {
        public EastingNorthing(double easting, double northing)
        {
            Easting = easting;
            Northing = northing;
            Height = 0;
        }

        public EastingNorthing(double easting, double northing, double height)
        {
            Easting = easting;
            Northing = northing;
            Height = height;
        }

        /// <summary>
        /// Returns the easting parameter
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

        /// <summary>
        /// The distance to another point in metres, including any difference in <see cref="Height"/>. A BNG midpoint
        /// scale-factor correction is applied to the horizontal component automatically. When both points have
        /// <see cref="Height"/> of zero the result equals the 2D horizontal distance.
        /// </summary>
        /// <param name="toEastingNorthing">The target point.</param>
        /// <returns>Distance in metres.</returns>
        public double DistanceTo(EastingNorthing toEastingNorthing)
        {
            if(toEastingNorthing == null)
                throw new System.ArgumentNullException(nameof(toEastingNorthing));

            double deltaEasting = toEastingNorthing.Easting - Easting;
            double deltaNorthing = toEastingNorthing.Northing - Northing;
            double projectedHorizontal = System.Math.Sqrt(deltaEasting * deltaEasting + deltaNorthing * deltaNorthing);

            // Correct for BNG Transverse Mercator scale distortion using the midpoint easting.
            // Formula: k ≈ k0 * (1 + ((E − E0)² / (2R²)))  where R = mean Earth radius.
            const double k0 = 0.9996012717; // BNG central meridian scale factor
            const double e0 = 400000.0;     // BNG false easting
            const double r = 6371000.0;    // mean Earth radius in metres
            double midEasting = (Easting + toEastingNorthing.Easting) / 2.0;
            double d = midEasting - e0;
            double horizontal = projectedHorizontal / (k0 * (1.0 + (d * d) / (2.0 * r * r)));

            double deltaHeight = toEastingNorthing.Height - Height;
            return System.Math.Sqrt(horizontal * horizontal + deltaHeight * deltaHeight);
        }
    }
}