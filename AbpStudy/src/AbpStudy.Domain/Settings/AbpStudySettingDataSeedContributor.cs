using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Localization;
using Volo.Abp.SettingManagement;
using Volo.Abp.Settings;

namespace AbpStudy.Settings
{
    public class AbpStudySettingDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IGuidGenerator _guidGenerator;
        private readonly ISettingRepository _settingRepository;

        public AbpStudySettingDataSeedContributor(IGuidGenerator guidGenerator, ISettingRepository settingRepository)
        {
            _settingRepository = settingRepository;
            _guidGenerator = guidGenerator;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            await _settingRepository.InsertAsync(new Setting(
                _guidGenerator.Create(),
                LocalizationSettingNames.DefaultLanguage,
                "zh-Hans",
                GlobalSettingValueProvider.ProviderName
            ));
        }
    }
}