using System;
using System.Collections.Generic;
using System.Text;
using AbpStudy.Localization;
using Volo.Abp.Application.Services;

namespace AbpStudy
{
    /* Inherit your application services from this class.
     */
    public abstract class AbpStudyAppService : ApplicationService
    {
        protected AbpStudyAppService()
        {
            LocalizationResource = typeof(AbpStudyResource);
        }
    }
}
