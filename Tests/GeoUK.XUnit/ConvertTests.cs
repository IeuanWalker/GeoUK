using FluentAssertions;
using GeoUK.Coordinates;
using GeoUK.Ellipsoids;
using GeoUK.Projections;
using Xunit;

namespace GeoUK.XUnit
{
    public class ConvertTests
    {
        [Fact]
        public void ToCartesian_With_Easting_Northing_Returns_Expected_Coordinates()
        {
            const double easting = 319267D;
            const double northing = 175189D;

            Cartesian cartesian = Convert.ToCartesian(new Airy1830(), new BritishNationalGrid(), new EastingNorthing(easting, northing));

            cartesian.X.Should().Be(3974861.5006269566D);
            cartesian.Y.Should().Be(-219615.34764596159D);
            cartesian.Z.Should().Be(4965879.5924725151D);
        }

        [Fact]
        public void ToCartesian_With_Latitude_Longitude_Returns_Expected_Coordinates()
        {
            const double latitude = 51.469886D;
            const double longitude = -3.1636964D;

            LatitudeLongitude latLong = new LatitudeLongitude(latitude, longitude);

            Cartesian cartesian = Convert.ToCartesian(new Wgs84(), latLong);

            cartesian.X.Should().Be(3975202.3191297553D);
            cartesian.Y.Should().Be(-219721.77255870664D);
            cartesian.Z.Should().Be(4966276.08905928D);
        }

        [Fact]
        public void ToLatitudeLongitude_Returns_Expected_Coordinates()
        {
            Cartesian wgsCartesian = new Cartesian(3975233.3492949815D, -219723.39313066352D, 4966314.9848013865D);

            LatitudeLongitude latitudeLongitude = Convert.ToLatitudeLongitude(new Wgs84(), wgsCartesian);

            latitudeLongitude.Latitude.Should().Be(51.469885297942383D);
            latitudeLongitude.Longitude.Should().Be(-3.163695041217216D);
        }

        [Fact]
        public void ToEastingNorthing_Returns_Expected_Coordinates()
        {
            Cartesian bngCartesian = new Cartesian(3974830.4765356039D, -219613.73035729351D, 4965840.70534001D);

            EastingNorthing bngEN = Convert.ToEastingNorthing(new Airy1830(), new BritishNationalGrid(), bngCartesian);

            bngEN.Easting.Should().Be(319266.904605355113D);
            bngEN.Northing.Should().Be(175189.07967857248D);
        }
    }
}