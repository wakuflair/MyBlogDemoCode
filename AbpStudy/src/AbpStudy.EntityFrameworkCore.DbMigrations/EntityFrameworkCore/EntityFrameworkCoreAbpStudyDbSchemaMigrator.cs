using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AbpStudy.Data;
using Volo.Abp.DependencyInjection;

namespace AbpStudy.EntityFrameworkCore
{
    [Dependency(ReplaceServices = true)]
    public class EntityFrameworkCoreAbpStudyDbSchemaMigrator 
        : IAbpStudyDbSchemaMigrator, ITransientDependency
    {
        private readonly AbpStudyMigrationsDbContext _dbContext;

        public EntityFrameworkCoreAbpStudyDbSchemaMigrator(AbpStudyMigrationsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task MigrateAsync()
        {
            await _dbContext.Database.MigrateAsync();
        }
    }
}