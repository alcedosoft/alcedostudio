namespace Alcedosoft.AlcedoStudio;

public partial class GenerateCodeCommand : Command
{
    public async Task GenerateServiceAsync(
        ProjectName projectName, FileSystemDirectoryHandle src, FileSchema schema)
    {
        var schemaName  = new SchemaName(schema.Name);

        var contracts = await src.GetDirectoryHandleAsync(
            $"{projectName.Value}.Application.Contracts", new(){ Create = true});
        var application = await src.GetDirectoryHandleAsync(
            $"{projectName.Value}.Application", new(){ Create = true});

        var interfaceDir = await contracts.GetDirectoryHandleAsync(
            schemaName.PluralPascalName, new(){ Create = true});
        var implementDir = await application.GetDirectoryHandleAsync(
            schemaName.PluralPascalName, new(){ Create = true});

        var interfaceFile = await interfaceDir.GetFileHandleAsync(
            $"I{schemaName.PascalName}AppService.cs", new(){ Create = true });
        var implementFile = await implementDir.GetFileHandleAsync(
            $"{schemaName.PascalName}AppService.cs", new(){ Create = true });

        var interfaceContent = this.GenerateInterface(projectName, schema);
        var implementContent = this.GenerateImplement(projectName, schema);

        await this.WriteTextAsync(interfaceFile, interfaceContent);
        await this.WriteTextAsync(implementFile, implementContent);
    }

    private string GenerateInterface(ProjectName projectName, FileSchema schema)
    {
        var schemaName = new SchemaName(schema.Name);

        return $@"namespace {projectName.Value};

public interface I{schemaName.PascalName}AppService : ICrudAppService<
    {schemaName.PascalName}QueryDto, {schemaName.PascalName}QueryDto, Guid, PagedAndSortedResultRequestDto, {schemaName.PascalName}CreateDto, {schemaName.PascalName}UpdateDto>
{{

}}
";
    }

    private string GenerateImplement(ProjectName projectName, FileSchema schema)
    {
        var schemaName = new SchemaName(schema.Name);

        return $@"namespace {projectName.Value};

public class {schemaName.PascalName}AppService : CrudAppService<
    {schemaName.PascalName}, {schemaName.PascalName}QueryDto, {schemaName.PascalName}QueryDto, Guid, PagedAndSortedResultRequestDto, {schemaName.PascalName}CreateDto, {schemaName.PascalName}UpdateDto>
    , I{schemaName.PascalName}AppService
{{
    public {schemaName.PascalName}AppService(IRepository<{schemaName.PascalName}, Guid> repository) : base(repository)
    {{
        GetPolicyName = {projectName.PascalSubName}Permissions.{schemaName.PluralPascalName}.Default;
        GetListPolicyName = {projectName.PascalSubName}Permissions.{schemaName.PluralPascalName}.Default;
        CreatePolicyName = {projectName.PascalSubName}Permissions.{schemaName.PluralPascalName}.Create;
        UpdatePolicyName = {projectName.PascalSubName}Permissions.{schemaName.PluralPascalName}.Update;
        DeletePolicyName = {projectName.PascalSubName}Permissions.{schemaName.PluralPascalName}.Delete;
    }}
}}
";
    }
}
