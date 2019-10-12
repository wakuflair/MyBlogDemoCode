using Shouldly;
using System.Threading.Tasks;
using Volo.Abp.Identity;
using Volo.Abp.SettingManagement;
using Xunit;

namespace AbpStudy.Samples
{
    /* This is just an example test class.
     * Normally, you don't test code of the modules you are using
     * (like IIdentityUserAppService here).
     * Only test your own application services.
     */
    public class SampleAppServiceTests : AbpStudyApplicationTestBase
    {
        private readonly IIdentityUserAppService _userAppService;
        private ISettingManager _settingManager;

        public SampleAppServiceTests()
        {
            _userAppService = GetRequiredService<IIdentityUserAppService>();
            _settingManager = GetRequiredService<ISettingManager>();
        }

        [Fact]
        public async Task Initial_Data_Should_Contain_Admin_User()
        {
            //Act
            var result = await _userAppService.GetListAsync(new GetIdentityUsersInput());

            //Assert
            result.TotalCount.ShouldBeGreaterThan(0);
            result.Items.ShouldContain(u => u.UserName == "admin");
        }
    }
}
