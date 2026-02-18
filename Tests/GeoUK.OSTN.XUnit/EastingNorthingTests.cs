using GeoUK.Coordinates;
using System;
using Xunit;

namespace GeoUK.OSTN.XUnit
{
    public class EastingNorthingTests
    {
        [Fact]
        public void DistanceTo_NullArgument_ThrowsArgumentNullException()
        {
            var point = new EastingNorthing(100, 200);
            Assert.Throws<ArgumentNullException>(() => point.DistanceTo(null));
        }

        [Fact]
        public void DistanceTo_SamePoint_ReturnsZero()
        {
            var point = new EastingNorthing(530000, 180000);
            Assert.Equal(0.0, point.DistanceTo(point));
        }

        [Fact]
        public void DistanceTo_PureEastingDifference_ReturnsCorrectDistance()
        {
            var from = new EastingNorthing(530000, 180000);
            var to = new EastingNorthing(531000, 180000);
            Assert.Equal(1000.0, from.DistanceTo(to), precision: 6);
        }

        [Fact]
        public void DistanceTo_PureNorthingDifference_ReturnsCorrectDistance()
        {
            var from = new EastingNorthing(530000, 180000);
            var to = new EastingNorthing(530000, 181000);
            Assert.Equal(1000.0, from.DistanceTo(to), precision: 6);
        }

        [Fact]
        public void DistanceTo_DiagonalDifference_ReturnsEuclideanDistance()
        {
            // 3-4-5 right triangle scaled to metres
            var from = new EastingNorthing(530000, 180000);
            var to = new EastingNorthing(530300, 180400);
            Assert.Equal(500.0, from.DistanceTo(to), precision: 6);
        }

        [Fact]
        public void DistanceTo_IsSymmetric()
        {
            var a = new EastingNorthing(530000, 180000);
            var b = new EastingNorthing(531500, 182000);
            Assert.Equal(a.DistanceTo(b), b.DistanceTo(a), precision: 10);
        }

        [Fact]
        public void DistanceTo_DifferentHeights_IgnoresHeightComponent()
        {
            var from = new EastingNorthing(530000, 180000, 0);
            var to = new EastingNorthing(531000, 180000, 999);
            // Height should not affect the 2D result
            Assert.Equal(1000.0, from.DistanceTo(to), precision: 6);
        }

        [Fact]
        public void DistanceTo_ReturnsDouble_NotRoundedToInteger()
        {
            var from = new EastingNorthing(0, 0);
            var to = new EastingNorthing(1, 1); // √2 ≈ 1.41421...
            double result = from.DistanceTo(to);
            Assert.NotEqual(Math.Round(result), result);
        }
    }
}
