using System;

namespace GeoUK.OSTN.Tests
{
    // Source: https://scottlilly.com/c-tip-how-to-check-if-two-double-values-are-equal/
    public static class ExtensionMethods
    {
        public static bool IsApproximatelyEqualTo(this double initialValue, double value) => IsApproximatelyEqualTo(initialValue, value, 0.00001);

        // Handle comparisons of floating point values that may not be exactly the same
        public static bool IsApproximatelyEqualTo(this double initialValue, double value, double maximumDifferenceAllowed) => (Math.Abs(initialValue - value) < maximumDifferenceAllowed);
    }
}