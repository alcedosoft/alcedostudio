namespace [PROJECTNAME];

/* This is used if database provider does't define
 * I[PROJECTSUBNAME]DbSchemaMigrator implementation.
 */
public class Null[PROJECTSUBNAME]DbSchemaMigrator : I[PROJECTSUBNAME]DbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
