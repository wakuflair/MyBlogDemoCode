using Volo.Abp.Settings;

namespace AbpStudy.Settings
{
    public class AbpStudySettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            //Define your own settings here. Example:
            //context.Add(new SettingDefinition(AbpStudySettings.MySetting1));
        }
    }
}
