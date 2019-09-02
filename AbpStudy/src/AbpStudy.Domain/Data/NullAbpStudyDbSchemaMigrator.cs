using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace AbpStudy.Data
{
    /* This is used if database provider does't define
     * IAbpStudyDbSchemaMigrator implementation.
     */
    public class NullAbpStudyDbSchemaMigrator : IAbpStudyDbSchemaMigrator, ITransientDependency
    {
        public Task MigrateAsync()
        {
            return Task.CompletedTask;
        }
    }
}