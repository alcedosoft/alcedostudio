namespace Alcedosoft.AlcedoStudio;

public partial class GenerateCodeCommand : Command
{
    public async Task GenerateModelAsync(
        ProjectName projectName, FileSystemDirectoryHandle src, FileSchema schema)
    {
        var schemaName  = new SchemaName(schema.Name);

        var domain = await src.GetDirectoryHandleAsync(
            $"{projectName.Value}.Domain", new(){ Create = true});
        var contracts = await src.GetDirectoryHandleAsync(
            $"{projectName.Value}.Application.Contracts", new(){ Create = true});

        var entityDir = await domain.GetDirectoryHandleAsync(
            schemaName.PluralPascalName, new(){ Create = true});
        var entityDtoDir = await contracts.GetDirectoryHandleAsync(
            schemaName.PluralPascalName, new(){ Create = true});

        var entityFile = await entityDir.GetFileHandleAsync(
            $"{schemaName.PascalName}.cs", new(){ Create = true });
        var queryDtoFile = await entityDtoDir.GetFileHandleAsync(
            $"{schemaName.PascalName}QueryDto.cs", new(){ Create = true });
        var createDtoFile = await entityDtoDir.GetFileHandleAsync(
            $"{schemaName.PascalName}CreateDto.cs", new(){ Create = true });
        var updateDtoFile = await entityDtoDir.GetFileHandleAsync(
            $"{schemaName.PascalName}UpdateDto.cs", new(){ Create = true });

        var entityContent = this.GenerateEntity(projectName, schema);
        var queryDtoContent = this.GenerateQueryDto(projectName, schema);
        var createDtoContent = this.GenerateCreateDto(projectName, schema);
        var updateDtoContent = this.GenerateUpdateDto(projectName, schema);

        await this.WriteTextAsync(entityFile, entityContent);
        await this.WriteTextAsync(queryDtoFile, queryDtoContent);
        await this.WriteTextAsync(createDtoFile, createDtoContent);
        await this.WriteTextAsync(updateDtoFile, updateDtoContent);
    }

    private string GenerateEntity(ProjectName projectName, FileSchema schema)
    {
        var properties = new StringBuilder();

        foreach (var item in schema.Items)
        {
            _ = properties.AppendLine(@$"
    /// <summary>
    /// {item.Description}
    /// </summary>
    public {item.DataType} {item.Name} {{ get; set; }}");
        }

        return $@"namespace {projectName.Value};

public class {schema.Name} : AuditedAggregateRoot<Guid>
{{
{properties}
}}
";
    }

    private string GenerateQueryDto(ProjectName projectName, FileSchema schema)
    {
        var properties = new StringBuilder();

        foreach (var item in schema.Items)
        {
            _ = properties.AppendLine(@$"
    /// <summary>
    /// {item.Description}
    /// </summary>
    [DisplayName(""{item.DisplayName}"")]
    public {item.DataType} {item.Name} {{ get; set; }}");
        }

        return $@"namespace {projectName.Value};

public class {schema.Name}QueryDto : AuditedEntityDto<Guid>
{{
{properties}
}}
";
    }

    private string GenerateCreateDto(ProjectName projectName, FileSchema schema)
    {
        var properties = new StringBuilder();

        foreach (var item in schema.Items)
        {
            _ = properties.AppendLine(@$"
    /// <summary>
    /// {item.Description}
    /// </summary>
    [DisplayName(""{item.DisplayName}"")]
    public {item.DataType} {item.Name} {{ get; set; }}");
        }

        return $@"namespace {projectName.Value};

public class {schema.Name}CreateDto : AuditedEntityDto<Guid>
{{
{properties}
}}
";
    }

    private string GenerateUpdateDto(ProjectName projectName, FileSchema schema)
    {
        var properties = new StringBuilder();

        foreach (var item in schema.Items)
        {
            _ = properties.AppendLine(@$"
    /// <summary>
    /// {item.Description}
    /// </summary>
    [DisplayName(""{item.DisplayName}"")]
    public {item.DataType} {item.Name} {{ get; set; }}");
        }

        return $@"namespace {projectName.Value};

public class {schema.Name}UpdateDto : AuditedEntityDto<Guid>
{{
{properties}
}}
";
    }
}
