namespace Alcedosoft.AlcedoStudio;

public partial class GenerateCodeCommand
{
    public async Task GenerateAdapterAsync(
        ProjectName projectName, FileSystemDirectoryHandle src, IEnumerable<FileSchema> schemas)
    {
        var blazor = await src.GetDirectoryHandleAsync(
            $"{projectName.Value}.Blazor", new(){ Create = true});
        var application = await src.GetDirectoryHandleAsync(
            $"{projectName.Value}.Application", new(){ Create = true});

        var pageFile = await blazor.GetFileHandleAsync(
            $"{projectName.PascalSubName}BlazorAutoMapperProfile.cs", new(){ Create = true });
        var serviceFile = await application.GetFileHandleAsync(
            $"{projectName.PascalSubName}ApplicationAutoMapperProfile.cs", new(){ Create = true });

        var pageContent = this.GeneratePageAutoMapper(projectName, schemas);
        var serviceContent = this.GenerateServiceAutoMapper(projectName, schemas);

        await this.WriteTextAsync(pageFile, pageContent);
        await this.WriteTextAsync(serviceFile, serviceContent);
    }

    private string GenerateServiceAutoMapper(ProjectName projectName, IEnumerable<FileSchema> schemas)
    {
        var maps = new StringBuilder();

        foreach (var schema in schemas)
        {
            var schemaName = new SchemaName(schema.Name);

            _ = maps.AppendLine($@"
        _ = CreateMap<{schemaName.PascalName}, {schemaName.PascalName}QueryDto>();
        _ = CreateMap<{schemaName.PascalName}CreateDto, {schemaName.PascalName}>();
        _ = CreateMap<{schemaName.PascalName}UpdateDto, {schemaName.PascalName}>();");
        }

        return $@"namespace {projectName.Value};

public class {projectName.PascalSubName}ApplicationAutoMapperProfile : Profile
{{
    public {projectName.PascalSubName}ApplicationAutoMapperProfile()
    {{
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */
{maps}
    }}
}}
";
    }

    private string GeneratePageAutoMapper(ProjectName projectName, IEnumerable<FileSchema> schemas)
    {
        var maps = new StringBuilder();

        foreach (var schema in schemas)
        {
            var schemaName = new SchemaName(schema.Name);

            _ = maps.AppendLine($@"
        _ = CreateMap<{schemaName.PascalName}QueryDto, {schemaName.PascalName}UpdateDto>();");
        }

        return $@"namespace {projectName.Value};

public class {projectName.PascalSubName}BlazorAutoMapperProfile : Profile
{{
    public {projectName.PascalSubName}BlazorAutoMapperProfile()
    {{
        //Define your AutoMapper configuration here for the Blazor project.
{maps}
    }}
}}
";
    }
}
