using AbpStudy.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace AbpStudy
{
    [DependsOn(
        typeof(AbpStudyEntityFrameworkCoreTestModule)
        )]
    public class AbpStudyDomainTestModule : AbpModule
    {

    }
}