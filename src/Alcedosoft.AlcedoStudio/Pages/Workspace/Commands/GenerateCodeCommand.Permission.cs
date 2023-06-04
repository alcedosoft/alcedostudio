namespace Alcedosoft.AlcedoStudio;

public partial class GenerateCodeCommand : Command
{
    public async Task GeneratePermissionAsync(
        ProjectName projectName, FileSystemDirectoryHandle src, IEnumerable<FileSchema> schemas)
    {
        var contracts = await src.GetDirectoryHandleAsync(
            $"{projectName.Value}.Application.Contracts", new(){ Create = true});

        var permissionDir = await contracts.GetDirectoryHandleAsync(
            "Permissions", new(){ Create = true});

        var constantFile = await permissionDir.GetFileHandleAsync(
            $"{projectName.PascalSubName}Permissions.cs", new(){ Create = true });
        var definitionFile = await permissionDir.GetFileHandleAsync(
            $"{projectName.PascalSubName}PermissionDefinitionProvider.cs", new(){ Create = true });

        var constantContent = GenerateConstant(projectName, schemas);
        var definitionContent = GenerateDefinition(projectName, schemas);

        await WriteTextAsync(constantFile, constantContent);
        await WriteTextAsync(definitionFile, definitionContent);
    }

    private string GenerateConstant(ProjectName projectName, IEnumerable<FileSchema> schemas)
    {
        var permissions = new StringBuilder();

        foreach (var schema in schemas)
        {
            var schemaName = new SchemaName(schema.Name);

            _ = permissions.AppendLine($@"
    public static class {schemaName.PluralPascalName}
    {{
        public const string Default = GroupName + "".{schemaName.PluralPascalName}"";
        public const string Create = Default + "".Create"";
        public const string Update = Default + "".Update"";
        public const string Delete = Default + "".Delete"";
    }}");
        }

        return $@"namespace {projectName.Value};

public static class {projectName.PascalSubName}Permissions
{{
    public const string GroupName = ""{projectName.PascalSubName}"";

    //Add your own permission names. Example:
    //public const string MyPermission1 = GroupName + "".MyPermission1"";

{permissions}
}}
";
    }

    private string GenerateDefinition(ProjectName projectName, IEnumerable<FileSchema> schemas)
    {
        var permissions = new StringBuilder();

        foreach (var schema in schemas)
        {
            var schemaName = new SchemaName(schema.Name);

            _ = permissions.AppendLine($@"
        _ = {projectName.CamelSubName}Group
            .AddPermission({projectName.PascalSubName}Permissions.{schemaName.PluralPascalName}.Default, L(""Permission:{schemaName.PluralPascalName}""))
            .AddChild({projectName.PascalSubName}Permissions.{schemaName.PluralPascalName}.Create, L(""Permission:{schemaName.PluralPascalName}.Create""))
            .AddChild({projectName.PascalSubName}Permissions.{schemaName.PluralPascalName}.Update, L(""Permission:{schemaName.PluralPascalName}.Update""))
            .AddChild({projectName.PascalSubName}Permissions.{schemaName.PluralPascalName}.Delete, L(""Permission:{schemaName.PluralPascalName}.Delete""));");
        }

        return $@"namespace {projectName.Value};

public class {projectName.PascalSubName}PermissionDefinitionProvider : PermissionDefinitionProvider
{{
    public override void Define(IPermissionDefinitionContext context)
    {{
        var {projectName.CamelSubName}Group = context.AddGroup({projectName.PascalSubName}Permissions.GroupName, L(""Permission:{projectName.PascalSubName}""));

        //Define your own permissions here. Example:
        //{projectName.CamelSubName}Group.AddPermission({projectName.PascalSubName}Permissions.MyPermission1, L(""Permission:MyPermission1""));

{permissions}
    }}

    private static LocalizableString L(string name)
    {{
        return LocalizableString.Create<{projectName.PascalSubName}Resource>(name);
    }}
}}
";
    }
}
