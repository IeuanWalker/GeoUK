using GeoUK.Coordinates;
using System;
using Xunit;

namespace GeoUK.OSTN.XUnit
{
    public class LatitudeLongitudeTests
    {

        [Fact]
        public void DistanceTo_NullArgument_ThrowsArgumentNullException()
        {
            // Arrange
            LatitudeLongitude point = new(51.5, -0.1);

            // Act + Assert
            Assert.Throws<ArgumentNullException>(() => point.DistanceTo(null));
        }

        [Fact]
        public void DistanceTo_SamePoint_ReturnsZero()
        {
            // Arrange
            LatitudeLongitude point = new(51.5074, -0.1278);
            double expected = 0.0;

            // Act
            double result = point.DistanceTo(point);

            // Assert
            Assert.Equal(expected, result, 10);
        }

        [Fact]
        public void DistanceTo_PureLatitudeDifference_ReturnsCorrectDistance()
        {
            // Arrange
            LatitudeLongitude from = new(51.5, 0.0);
            LatitudeLongitude to = new(52.5, 0.0);
            double expected = 111194.92664455874;

            // Act
            double result = from.DistanceTo(to);

            // Assert
            Assert.Equal(expected, result, 10);
        }

        [Fact]
        public void DistanceTo_PureLongitudeDifference_ReturnsCorrectDistance()
        {
            // Arrange
            LatitudeLongitude from = new(51.5, 0.0);
            LatitudeLongitude to = new(51.5, 1.0);
            double expected = 69219.931246301843;

            // Act
            double result = from.DistanceTo(to);

            // Assert
            Assert.Equal(expected, result, 10);
        }

        [Fact]
        public void DistanceTo_DiagonalDifference_ReturnsCorrectDistance()
        {
            // Arrange
            LatitudeLongitude from = new(51.5, 0.0);
            LatitudeLongitude to = new(52.0, 1.0);
            double expected = 88486.093369970331;

            // Act
            double result = from.DistanceTo(to);

            // Assert
            Assert.Equal(expected, result, 10);
        }

        [Fact]
        public void DistanceTo_IsSymmetric()
        {
            // Arrange
            LatitudeLongitude a = new(51.5074, -0.1278);
            LatitudeLongitude b = new(48.8566, 2.3522);

            // Act
            var resultA = a.DistanceTo(b);
            var resultB = b.DistanceTo(a);

            // Assert
            Assert.Equal(resultA, resultB, 10);
        }

        [Fact]
        public void DistanceTo_DifferentHeights_IncludesHeightInDistance()
        {
            // Arrange
            LatitudeLongitude from = new(51.5, 0.0, 0);
            LatitudeLongitude to = new(52.5, 0.0, 10000);
            double expected = 111643.681914781;

            // Act
            double result = from.DistanceTo(to);

            // Assert
            Assert.Equal(expected, result, 10);
        }

        [Theory]
        [InlineData(51.474821, -3.1710057, 51.515495, -3.0540461, 9274.3887835215028)]
        [InlineData(51.482661, -3.1818924, 51.469638, -3.1631993, 1942.4251992397194)]
        [InlineData(51.478255, -3.1827145, 51.484971, -3.1784236, 803.72817130550686)]
        [InlineData(51.474830, -3.1709915, 51.466634, -3.1642457, 1024.1510680061178)]
        public void DistanceTo_AdHocTests_CorrectValues(double fromLatitude, double fromLongitude, double toLatitude, double toLongitude, double expected)
        {
            // Arrange
            LatitudeLongitude from = new(fromLatitude, fromLongitude);
            LatitudeLongitude to = new(toLatitude, toLongitude);

            // Act
            double result = from.DistanceTo(to);

            // Assert
            Assert.Equal(expected, result, 10);
        }
    }
}
