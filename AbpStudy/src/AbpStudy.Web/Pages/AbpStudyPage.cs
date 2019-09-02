using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.Razor.Internal;
using AbpStudy.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace AbpStudy.Web.Pages
{
    /* Inherit your UI Pages from this class. To do that, add this line to your Pages (.cshtml files under the Page folder):
     * @inherits AbpStudy.Web.Pages.AbpStudyPage
     */
    public abstract class AbpStudyPage : AbpPage
    {
        [RazorInject]
        public IHtmlLocalizer<AbpStudyResource> L { get; set; }
    }
}
