using FluentAssertions;
using GeoUK.Coordinates;
using Xunit;

namespace GeoUK.XUnit
{
    public class TransformTests
    {
        [Fact]
        public void Osgb36ToEtrs89_Returns_Expected_Coordinates()
        {
            Cartesian cartesian = new Cartesian(3974861.5006269566D, -219615.34764596159D, 4965879.5924725151D);
            Cartesian wgsCartesian = Transform.Osgb36ToEtrs89(cartesian);
            wgsCartesian.X.Should().Be(3975233.3492949815D);
            wgsCartesian.Y.Should().Be(-219723.39313066352D);
            wgsCartesian.Z.Should().Be(4966314.9848013865D);
        }

        [Fact]
        public void Etrs89ToOsgb36_Returns_Expected_Coordinates()
        {
            Cartesian cartesian = new Cartesian(3975202.3191297553D, -219721.77255870664D, 4966276.08905928D);
            Cartesian bngCartesian = Transform.Etrs89ToOsgb36(cartesian);
            bngCartesian.X.Should().Be(3974830.4765356039D);
            bngCartesian.Y.Should().Be(-219613.73035729351D);
            bngCartesian.Z.Should().Be(4965840.70534001D);
        }
    }
}