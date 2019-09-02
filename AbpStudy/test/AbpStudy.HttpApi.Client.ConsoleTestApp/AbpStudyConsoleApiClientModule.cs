using Volo.Abp.Http.Client.IdentityModel;
using Volo.Abp.Modularity;

namespace AbpStudy.HttpApi.Client.ConsoleTestApp
{
    [DependsOn(
        typeof(AbpStudyHttpApiClientModule),
        typeof(AbpHttpClientIdentityModelModule)
        )]
    public class AbpStudyConsoleApiClientModule : AbpModule
    {
        
    }
}
