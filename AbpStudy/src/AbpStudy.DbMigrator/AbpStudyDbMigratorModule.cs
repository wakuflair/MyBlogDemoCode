using AbpStudy.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Modularity;

namespace AbpStudy.DbMigrator
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(AbpStudyEntityFrameworkCoreDbMigrationsModule),
        typeof(AbpStudyApplicationContractsModule)
        )]
    public class AbpStudyDbMigratorModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<BackgroundJobOptions>(options => options.IsJobExecutionEnabled = false);
        }
    }
}
