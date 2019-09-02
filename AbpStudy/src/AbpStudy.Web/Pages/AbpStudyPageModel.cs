using AbpStudy.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace AbpStudy.Web.Pages
{
    /* Inherit your PageModel classes from this class.
     */
    public abstract class AbpStudyPageModel : AbpPageModel
    {
        protected AbpStudyPageModel()
        {
            LocalizationResourceType = typeof(AbpStudyResource);
        }
    }
}