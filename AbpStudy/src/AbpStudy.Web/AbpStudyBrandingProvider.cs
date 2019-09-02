using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Components;
using Volo.Abp.DependencyInjection;

namespace AbpStudy.Web
{
    [Dependency(ReplaceServices = true)]
    public class AbpStudyBrandingProvider : DefaultBrandingProvider
    {
        public override string AppName => "AbpStudy";
    }
}
