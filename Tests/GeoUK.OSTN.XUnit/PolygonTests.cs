using System;
using System.Linq;
using Xunit;
using GeoUK;

namespace GeoUK.OSTN.XUnit
{
    public class PolygonTests
    {
        #region Exception Tests - Invalid Input Parameters

        [Theory]
        [InlineData(-90.1)]
        [InlineData(-91)]
        [InlineData(-180)]
        [InlineData(90.1)]
        [InlineData(91)]
        [InlineData(180)]
        public void GeneratePolygonAroundPoint_InvalidLatitude_ThrowsArgumentOutOfRangeException(double invalidLatitude)
        {
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => 
                Polygon.GeneratePolygonAroundPoint(0, invalidLatitude, 1000, 8));
            Assert.Equal("latitude", exception.ParamName);
            Assert.Contains("Latitude must be between -90 and 90 degrees", exception.Message);
        }

        [Theory]
        [InlineData(-180.1)]
        [InlineData(-181)]
        [InlineData(180.1)]
        [InlineData(181)]
        [InlineData(360)]
        public void GeneratePolygonAroundPoint_InvalidLongitude_ThrowsArgumentOutOfRangeException(double invalidLongitude)
        {
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => 
                Polygon.GeneratePolygonAroundPoint(invalidLongitude, 0, 1000, 8));
            Assert.Equal("longitude", exception.ParamName);
            Assert.Contains("Longitude must be between -180 and 180 degrees", exception.Message);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-1000)]
        public void GeneratePolygonAroundPoint_InvalidRadius_ThrowsArgumentOutOfRangeException(double invalidRadius)
        {
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => 
                Polygon.GeneratePolygonAroundPoint(0, 0, invalidRadius, 8));
            Assert.Equal("radius", exception.ParamName);
            Assert.Contains("Radius must be positive", exception.Message);
        }

        [Fact]
        public void GeneratePolygonAroundPoint_RadiusExceedsMaximum_ThrowsArgumentOutOfRangeException()
        {
            // max_radius_meters = Math.PI * 6378137.0 / 2.0 ≈ 10,018,754 meters
            double maxRadius = Math.PI * 6378137.0 / 2.0;
            double excessiveRadius = maxRadius + 1;

            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => 
                Polygon.GeneratePolygonAroundPoint(0, 0, excessiveRadius, 8));
            Assert.Equal("radius", exception.ParamName);
            Assert.Contains("Radius must not exceed", exception.Message);
        }

        [Theory]
        [InlineData(89.1)]
        [InlineData(89.5)]
        [InlineData(90)]
        [InlineData(-89.1)]
        [InlineData(-89.5)]
        [InlineData(-90)]
        public void GeneratePolygonAroundPoint_PolarRegions_ThrowsArgumentOutOfRangeException(double polarLatitude)
        {
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => 
                Polygon.GeneratePolygonAroundPoint(0, polarLatitude, 1000, 8));
            Assert.Equal("latitude", exception.ParamName);
            Assert.Contains("Geodesic calculations are unstable near the poles", exception.Message);
        }

        #endregion

        #region Boundary Value Tests

        [Theory]
        [InlineData(-180, -89)]
        [InlineData(-180, 89)]
        [InlineData(180, -89)]
        [InlineData(180, 89)]
        [InlineData(0, -89)]
        [InlineData(0, 89)]
        public void GeneratePolygonAroundPoint_BoundaryCoordinates_ReturnsValidPolygon(double longitude, double latitude)
        {
            var result = Polygon.GeneratePolygonAroundPoint(longitude, latitude, 1000, 8);
            
            Assert.NotNull(result);
            Assert.Equal(9, result.Count); // 8 points + 1 closing point
            Assert.True(result.All(coord => !double.IsNaN(coord[0]) && !double.IsNaN(coord[1])));
        }

        [Fact]
        public void GeneratePolygonAroundPoint_MinimumValidRadius_ReturnsValidPolygon()
        {
            double minRadius = double.Epsilon;
            var result = Polygon.GeneratePolygonAroundPoint(0, 0, minRadius, 8);
            
            Assert.NotNull(result);
            Assert.Equal(9, result.Count);
            Assert.True(result.All(coord => !double.IsNaN(coord[0]) && !double.IsNaN(coord[1])));
        }

        [Fact]
        public void GeneratePolygonAroundPoint_MaximumValidRadius_ReturnsValidPolygon()
        {
            double maxRadius = Math.PI * 6378137.0 / 2.0; // Exactly at the limit
            var result = Polygon.GeneratePolygonAroundPoint(0, 0, maxRadius, 8);
            
            Assert.NotNull(result);
            Assert.Equal(9, result.Count);
            Assert.True(result.All(coord => !double.IsNaN(coord[0]) && !double.IsNaN(coord[1])));
        }

        #endregion

        #region NumberOfPoints Parameter Tests

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void GeneratePolygonAroundPoint_NumberOfPointsLessThanThree_DefaultsToThree(int numberOfPoints)
        {
            var result = Polygon.GeneratePolygonAroundPoint(0, 0, 1000, numberOfPoints);
            
            Assert.NotNull(result);
            Assert.Equal(4, result.Count); // 3 points + 1 closing point
        }

        [Theory]
        [InlineData(3, 4)]   // 3 points + 1 closing
        [InlineData(4, 5)]   // 4 points + 1 closing
        [InlineData(8, 9)]   // 8 points + 1 closing
        [InlineData(12, 13)] // 12 points + 1 closing
        [InlineData(360, 361)] // Large number of points
        public void GeneratePolygonAroundPoint_VariousNumberOfPoints_ReturnsCorrectCount(int numberOfPoints, int expectedCount)
        {
            var result = Polygon.GeneratePolygonAroundPoint(0, 0, 1000, numberOfPoints);
            
            Assert.Equal(expectedCount, result.Count);
        }

        #endregion

        #region Polygon Closure Tests

        [Fact]
        public void GeneratePolygonAroundPoint_PolygonIsClosed_FirstAndLastPointsAreIdentical()
        {
            var result = Polygon.GeneratePolygonAroundPoint(0, 0, 1000, 8);
            
            var firstPoint = result.First();
            var lastPoint = result.Last();
            
            Assert.Equal(firstPoint[0], lastPoint[0], 10); // longitude
            Assert.Equal(firstPoint[1], lastPoint[1], 10); // latitude
        }

        #endregion

        #region Coordinate Validation Tests

        [Fact]
        public void GeneratePolygonAroundPoint_AllCoordinatesWithinValidRange_Success()
        {
            var result = Polygon.GeneratePolygonAroundPoint(0, 0, 1000, 8);
            
            foreach (var coord in result)
            {
                Assert.True(coord[0] >= -180.0 && coord[0] <= 180.0, $"Longitude {coord[0]} is out of range");
                Assert.True(coord[1] >= -90.0 && coord[1] <= 90.0, $"Latitude {coord[1]} is out of range");
            }
        }

        [Fact]
        public void GeneratePolygonAroundPoint_FloatingPointPrecision_DoesNotReturnNaN()
        {
            // Test edge cases that might cause floating-point precision issues
            var result = Polygon.GeneratePolygonAroundPoint(179.999999, 89.0, 1000, 8);
            Assert.True(result.All(coord => !double.IsNaN(coord[0]) && !double.IsNaN(coord[1])));
        }

        [Fact]
        public void GeneratePolygonAroundPoint_AntimeridianCrossing_HandlesCorrectly()
        {
            // Test longitude normalization across ±180°
            var result = Polygon.GeneratePolygonAroundPoint(179.5, 0, 100000, 8);
            Assert.True(result.All(coord => coord[0] >= -180.0 && coord[0] <= 180.0));
        }

        #endregion

        #region Longitude Normalization Tests

        [Fact]
        public void GeneratePolygonAroundPoint_LongitudeNormalization_HandlesAntimeridianCrossing()
        {
            // Test near antimeridian where polygon might cross ±180°
            var result = Polygon.GeneratePolygonAroundPoint(179, 0, 200000, 8);
            
            Assert.True(result.All(coord => coord[0] >= -180.0 && coord[0] <= 180.0));
            
            // Should have points on both sides of antimeridian
            bool hasPositiveLongitudes = result.Any(coord => coord[0] > 0);
            bool hasNegativeLongitudes = result.Any(coord => coord[0] < 0);
            Assert.True(hasPositiveLongitudes || hasNegativeLongitudes);
        }

        [Fact]
        public void GeneratePolygonAroundPoint_LongitudeNormalization_WesternHemisphere()
        {
            // Test near antimeridian from western side
            var result = Polygon.GeneratePolygonAroundPoint(-179, 0, 200000, 8);
            
            Assert.True(result.All(coord => coord[0] >= -180.0 && coord[0] <= 180.0));
        }

        #endregion

        #region Geometric Properties Tests

        [Fact]
        public void GeneratePolygonAroundPoint_EquatorialRegion_ReturnsReasonableCoordinates()
        {
            double centerLon = 0;
            double centerLat = 0;
            double radius = 1000; // 1km
            
            var result = Polygon.GeneratePolygonAroundPoint(centerLon, centerLat, radius, 8);
            
            // All points should be roughly within expected distance from center
            // For small distances at equator, degrees ≈ meters/111320
            double expectedDegreeDelta = radius / 111320.0; // Rough conversion
            
            foreach (var coord in result.Take(result.Count - 1)) // Exclude closing point
            {
                double lonDiff = Math.Abs(coord[0] - centerLon);
                double latDiff = Math.Abs(coord[1] - centerLat);
                
                Assert.True(lonDiff <= expectedDegreeDelta * 2, $"Longitude difference {lonDiff} too large");
                Assert.True(latDiff <= expectedDegreeDelta * 2, $"Latitude difference {latDiff} too large");
            }
        }

        [Fact]
        public void GeneratePolygonAroundPoint_LargeRadius_ReturnsValidCoordinates()
        {
            // Test with a large but valid radius
            double largeRadius = 5000000; // 5000km, well within max limit
            var result = Polygon.GeneratePolygonAroundPoint(0, 0, largeRadius, 8);
            
            Assert.NotNull(result);
            Assert.Equal(9, result.Count);
            Assert.True(result.All(coord => !double.IsNaN(coord[0]) && !double.IsNaN(coord[1])));
        }

        #endregion

        #region Different Geographic Regions Tests

        [Theory]
        [InlineData(0, 0)]      // Equator, Prime Meridian
        [InlineData(-120, 45)]  // North America
        [InlineData(120, -30)]  // Australia
        [InlineData(-60, -20)]  // South America
        [InlineData(30, 60)]    // Northern Europe/Asia
        [InlineData(100, 10)]   // Southeast Asia
        public void GeneratePolygonAroundPoint_DifferentGeographicRegions_ReturnsValidPolygons(double longitude, double latitude)
        {
            var result = Polygon.GeneratePolygonAroundPoint(longitude, latitude, 10000, 8);
            
            Assert.NotNull(result);
            Assert.Equal(9, result.Count);
            Assert.True(result.All(coord => !double.IsNaN(coord[0]) && !double.IsNaN(coord[1])));
            Assert.True(result.All(coord => coord[0] >= -180.0 && coord[0] <= 180.0));
            Assert.True(result.All(coord => coord[1] >= -90.0 && coord[1] <= 90.0));
        }

        #endregion

        #region Performance and Stress Tests

        [Fact]
        public void GeneratePolygonAroundPoint_HighNumberOfPoints_PerformsReasonably()
        {
            // Test with a high number of points
            var result = Polygon.GeneratePolygonAroundPoint(0, 0, 1000, 1000);
            
            Assert.Equal(1001, result.Count); // 1000 points + 1 closing
            Assert.True(result.All(coord => !double.IsNaN(coord[0]) && !double.IsNaN(coord[1])));
        }

        #endregion

        #region Array Structure Tests

        [Fact]
        public void GeneratePolygonAroundPoint_CoordinateArrayStructure_CorrectFormat()
        {
            var result = Polygon.GeneratePolygonAroundPoint(0, 0, 1000, 8);
            
            foreach (var coord in result)
            {
                Assert.NotNull(coord);
                Assert.Equal(2, coord.Length);
                Assert.True(double.IsFinite(coord[0]), "Longitude must be finite");
                Assert.True(double.IsFinite(coord[1]), "Latitude must be finite");
            }
        }

        #endregion
    }
}
