using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace AbpStudy.EntityFrameworkCore
{
    [DependsOn(
        typeof(AbpStudyEntityFrameworkCoreModule)
        )]
    public class AbpStudyEntityFrameworkCoreDbMigrationsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<AbpStudyMigrationsDbContext>();
        }
    }
}
