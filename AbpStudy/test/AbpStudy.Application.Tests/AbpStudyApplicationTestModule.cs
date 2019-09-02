using Volo.Abp.Modularity;

namespace AbpStudy
{
    [DependsOn(
        typeof(AbpStudyApplicationModule),
        typeof(AbpStudyDomainTestModule)
        )]
    public class AbpStudyApplicationTestModule : AbpModule
    {

    }
}