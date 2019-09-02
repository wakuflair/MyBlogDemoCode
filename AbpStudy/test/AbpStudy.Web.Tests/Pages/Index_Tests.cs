using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace AbpStudy.Pages
{
    public class Index_Tests : AbpStudyWebTestBase
    {
        [Fact]
        public async Task Welcome_Page()
        {
            var response = await GetResponseAsStringAsync("/");
            response.ShouldNotBeNull();
        }
    }
}
