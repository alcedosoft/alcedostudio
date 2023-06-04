namespace [PROJECTNAME];

/* This class is needed for EF Core console commands
 * (like Add-Migration and Update-Database commands) */
public class [PROJECTSUBNAME]DbContextFactory : IDesignTimeDbContextFactory<[PROJECTSUBNAME]DbContext>
{
    public [PROJECTSUBNAME]DbContext CreateDbContext(string[] args)
    {
        [PROJECTSUBNAME]EFCoreEntityExtensionMappings.Configure();

        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<[PROJECTSUBNAME]DbContext>()
            .UseSqlServer(configuration.GetConnectionString("Default"));

        return new [PROJECTSUBNAME]DbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../[PROJECTNAME].DbMigrator/"))
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
