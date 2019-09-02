using AbpStudy.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace AbpStudy.Controllers
{
    /* Inherit your controllers from this class.
     */
    public abstract class AbpStudyController : AbpController
    {
        protected AbpStudyController()
        {
            LocalizationResource = typeof(AbpStudyResource);
        }
    }
}