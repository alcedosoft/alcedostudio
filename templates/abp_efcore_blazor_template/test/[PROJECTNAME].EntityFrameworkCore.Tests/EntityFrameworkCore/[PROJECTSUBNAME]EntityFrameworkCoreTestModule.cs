using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Sqlite;
using Volo.Abp.Modularity;

namespace [PROJECTNAME];

[DependsOn(
    typeof([PROJECTSUBNAME]EntityFrameworkCoreModule),
    typeof([PROJECTSUBNAME]TestBaseModule),
    typeof(AbpEntityFrameworkCoreSqliteModule)
    )]
public class [PROJECTSUBNAME]EntityFrameworkCoreTestModule : AbpModule
{
    private SqliteConnection _sqliteConnection;

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        ConfigureInMemorySqlite(context.Services);
    }

    private void ConfigureInMemorySqlite(IServiceCollection services)
    {
        _sqliteConnection = CreateDatabaseAndGetConnection();

        _ = services.Configure<AbpDbContextOptions>(options =>
        {
            options.Configure(context =>
            {
                _ = context.DbContextOptions.UseSqlite(_sqliteConnection);
            });
        });
    }

    public override void OnApplicationShutdown(ApplicationShutdownContext context)
    {
        _sqliteConnection.Dispose();
    }

    private static SqliteConnection CreateDatabaseAndGetConnection()
    {
        var connection = new SqliteConnection("Data Source=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<[PROJECTSUBNAME]DbContext>()
            .UseSqlite(connection)
            .Options;

        using (var context = new [PROJECTSUBNAME]DbContext(options))
        {
            context.GetService<IRelationalDatabaseCreator>().CreateTables();
        }

        return connection;
    }
}
