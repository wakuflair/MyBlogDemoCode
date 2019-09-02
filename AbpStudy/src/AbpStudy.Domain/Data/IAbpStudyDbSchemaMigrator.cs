using System.Threading.Tasks;

namespace AbpStudy.Data
{
    public interface IAbpStudyDbSchemaMigrator
    {
        Task MigrateAsync();
    }
}
