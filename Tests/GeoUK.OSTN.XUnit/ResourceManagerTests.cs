using FluentAssertions;
using Xunit;

namespace GeoUK.OSTN.XUnit
{
    public class ResourceManagerTests
    {
        [Fact]
        public void PreloadResources_IsSuccessful()
        {
            Transform.PreloadResources();
            ResourceManager.GetOstnData(OstnVersionEnum.OSTN02).Should().NotBeEmpty();
            ResourceManager.GetOstnData(OstnVersionEnum.OSTN15).Should().NotBeEmpty();
        }
    }
}
