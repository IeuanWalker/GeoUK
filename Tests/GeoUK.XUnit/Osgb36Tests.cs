using FluentAssertions;
using GeoUK.Coordinates;
using Xunit;

namespace GeoUK.XUnit
{
    public class Osgb36Tests
    {
        [Fact]
        public void Osgb36_MapReference_Returns_Expected()
        {
            EastingNorthing eastingNorthing = new EastingNorthing(319267, 175189);
            Osgb36 osgb36EN = new Osgb36(eastingNorthing);
            osgb36EN.MapReference.Should().Be("ST193752");
        }
    }
}
