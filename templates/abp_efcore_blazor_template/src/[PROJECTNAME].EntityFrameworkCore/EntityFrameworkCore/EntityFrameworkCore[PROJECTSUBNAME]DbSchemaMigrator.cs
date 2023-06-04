namespace [PROJECTNAME];

public class EntityFrameworkCore[PROJECTSUBNAME]DbSchemaMigrator
    : I[PROJECTSUBNAME]DbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCore[PROJECTSUBNAME]DbSchemaMigrator(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolving the [PROJECTSUBNAME]DbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<[PROJECTSUBNAME]DbContext>()
            .Database.MigrateAsync();
    }
}
