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
            // Act
            EastingNorthing point = new(100, 200);

            // Assert
            Assert.Throws<ArgumentNullException>(() => point.DistanceTo(null));
        }

        [Fact]
        public void DistanceTo_SamePoint_ReturnsZero()
        {
            // Act
            EastingNorthing point = new(530000, 180000);

            // Assert
            Assert.Equal(0.0, point.DistanceTo(point));
        }

        [Fact]
        public void DistanceTo_PureEastingDifference_ReturnsCorrectDistance()
        {
            // Arrange
            EastingNorthing from = new(530000, 180000);
            EastingNorthing to = new(531000, 180000);
            double expected = 1000.1890620159669;

            // Act
            double result = from.DistanceTo(to);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void DistanceTo_PureNorthingDifference_ReturnsCorrectDistance()
        {
            // Arrange
            EastingNorthing from = new(530000, 180000);
            EastingNorthing to = new(530000, 181000);
            double expected = 1000.1906664587142;

            // Act
            double result = from.DistanceTo(to);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void DistanceTo_DiagonalDifference_ReturnsBngCorrectedDistance()
        {
            // Arrange
            EastingNorthing from = new(530000, 180000);
            EastingNorthing to = new(530300, 180400);
            double expected = 500.0950928860272;

            // Act
            double result = from.DistanceTo(to);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void DistanceTo_IsSymmetric()
        {
            // Arrange
            EastingNorthing a = new(530000, 180000);
            EastingNorthing b = new(531500, 182000);

            // Act
            var resultA = a.DistanceTo(b);
            var resultB = b.DistanceTo(a);

            // Assert
            Assert.Equal(resultA, resultB);
        }

        [Fact]
        public void DistanceTo_DifferentHeights_IncludesHeightInDistance()
        {
            // Arrange
            EastingNorthing from = new(530000, 180000, 0);
            EastingNorthing to = new(531000, 180000, 999);
            double expected = 1413.6403926658222;

            // Act
            double result = from.DistanceTo(to);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
