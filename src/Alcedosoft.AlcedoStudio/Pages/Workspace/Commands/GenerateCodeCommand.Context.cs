namespace Alcedosoft.AlcedoStudio;

public partial class GenerateCodeCommand : Command
{
    public async Task GenerateContextAsync(
        ProjectName projectName, FileSystemDirectoryHandle src, IEnumerable<FileSchema> schemas)
    {
        var domain = await src.GetDirectoryHandleAsync(
            $"{projectName.Value}.Domain", new(){ Create = true});
        var efcore = await src.GetDirectoryHandleAsync(
            $"{projectName.Value}.EntityFrameworkCore", new(){ Create = true});

        var seedDir = domain;
        var contextDir = await efcore.GetDirectoryHandleAsync(
            "EntityFrameworkCore", new(){ Create = true});

        var seedFile = await seedDir.GetFileHandleAsync(
            $"{projectName.PascalSubName}DataSeedContributor.cs", new(){ Create = true });
        var contextFile = await contextDir.GetFileHandleAsync(
            $"{projectName.PascalSubName}DbContext.Entity.cs", new(){ Create = true });

        var seedContent = this.GenerateDataSeed(projectName, schemas);
        var contextContent = this.GenerateDataContext(projectName, schemas);

        await this.WriteTextAsync(seedFile, seedContent);
        await this.WriteTextAsync(contextFile, contextContent);
    }

    private string GenerateDataContext(ProjectName projectName, IEnumerable<FileSchema> schemas)
    {
        var props = new StringBuilder();
        var entities= new StringBuilder();

        foreach (var schema in schemas)
        {
            var schemaName = new SchemaName(schema.Name);

            _ = props.AppendLine($@"    public DbSet<{schemaName.PascalName}> {schemaName.PascalName} {{ get; set; }}");

            _ = entities.AppendLine($@"
        _ = builder.Entity<{schemaName.PascalName}>(b =>
        {{
            b.ConfigureByConvention();

            _ = b.ToTable({projectName.PascalSubName}Consts.DbTablePrefix + ""{schemaName.PluralPascalName}"", {projectName.PascalSubName}Consts.DbSchema);
        }});");
        }

        return $@"namespace {projectName.Value};

public partial class {projectName.PascalSubName}DbContext
{{
{props}
    private void ConfigrationEntity(ModelBuilder builder)
    {{
{entities}
    }}
}}
";
    }

    private string GenerateDataSeed(ProjectName projectName, IEnumerable<FileSchema> schemas)
    {
        var fields = new StringBuilder();
        var parameters = new StringBuilder();
        var assignstatements = new StringBuilder();
        var seedInitStatements = new StringBuilder();

        var count = schemas.Count();

        for (var i = 1; i <= count; i++)
        {
            var schema = schemas.ElementAt(i - 1);
            var schemaName = new SchemaName(schema.Name);

            _ = fields.AppendLine($"    private readonly IRepository<{schemaName.PascalName}, Guid> _{schemaName.CamelName}Repository;");
            _ = assignstatements.AppendLine($"        _{schemaName.CamelName}Repository = {schemaName.CamelName}Repository;");

            if (i == count)
            {
                _ = parameters.AppendLine($"        IRepository<{schemaName.PascalName}, Guid> {schemaName.CamelName}Repository");
            }
            else
            {
                _ = parameters.AppendLine($"        IRepository<{schemaName.PascalName}, Guid> {schemaName.CamelName}Repository,");
            }

            _ = seedInitStatements.AppendLine(@$"
        if (await _{schemaName.CamelName}Repository.GetCountAsync() <= 0)
        {{

        }}");
        }

        return $@"namespace {projectName.Value};

public class {projectName.PascalSubName}DataSeedContributor : IDataSeedContributor, ITransientDependency
{{
{fields}

    public {projectName.PascalSubName}DataSeedContributor(
{parameters})
    {{
{assignstatements}
    }}

    public async Task SeedAsync(DataSeedContext context)
    {{
{seedInitStatements}
    }}
}}
";
    }
}
