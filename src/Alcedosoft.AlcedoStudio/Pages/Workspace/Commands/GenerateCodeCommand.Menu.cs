namespace Alcedosoft.AlcedoStudio;

public partial class GenerateCodeCommand
{
    public async Task GenerateMenuAsync(
        ProjectName projectName, FileSystemDirectoryHandle src, IEnumerable<FileSchema> schemas)
    {
        var blazor = await src.GetDirectoryHandleAsync(
            $"{projectName.Value}.Blazor", new(){ Create = true});

        var menuDir = await blazor.GetDirectoryHandleAsync(
            "Menus", new(){ Create = true });

        var constFile = await menuDir.GetFileHandleAsync(
            $"{projectName.PascalSubName}Menus.cs", new(){ Create = true });
        var mainMenuFile = await menuDir.GetFileHandleAsync(
            $"{projectName.PascalSubName}MenuContributor.MainMenu.cs", new(){ Create = true });

        var constContent = GenerateMenuConst(projectName, schemas);
        var mainMenuContent = GenerateMenuMain(projectName, schemas);

        await WriteTextAsync(constFile, constContent);
        await WriteTextAsync(mainMenuFile, mainMenuContent);
    }

    private string GenerateMenuConst(ProjectName projectName, IEnumerable<FileSchema> schemas)
    {
        var consts = new StringBuilder();

        foreach (var schema in schemas)
        {
            var schemaName = new SchemaName(schema.Name);
            _ = consts.AppendLine($@"    public const string {schemaName.PascalName} = Prefix + "".{schemaName.PluralPascalName}"";");
        }

        return $@"namespace {projectName.Value};

public class {projectName.PascalSubName}Menus
{{
    public const string Prefix = ""{projectName.PascalSubName}"";
    public const string Home = Prefix + "".Home"";

    //Add your menu items here...

{consts}
}}
";
    }

    private string GenerateMenuMain(ProjectName projectName, IEnumerable<FileSchema> schemas)
    {
        var subMenus =new StringBuilder();

        foreach (var schema in schemas)
        {
            var schemaName = new SchemaName(schema.Name);

            _ = subMenus.AppendLine($@"
        if (await context.IsGrantedAsync({projectName.PascalSubName}Permissions.{schemaName.PluralPascalName}.Default))
        {{
            _ = {projectName.CamelSubName}Menu.AddItem(
                new ApplicationMenuItem(
                    {projectName.PascalSubName}Menus.{schemaName.PascalName},
                    l[""Menu:{schemaName.PluralPascalName}""],
                    icon: ""fa fa-biking"",
                    url: ""/{schemaName.PluralCamelName}"")
                );
        }}");
        }

        return $@"namespace {projectName.Value};

public partial class {projectName.PascalSubName}MenuContributor : IMenuContributor
{{
    private async Task ConfigureMainMenuAsync(MenuConfigurationContext context)
    {{
        var l = context.GetLocalizer<{projectName.PascalSubName}Resource>();

        context.Menu.Items.Insert(
            0,
            new ApplicationMenuItem(
                {projectName.PascalSubName}Menus.Home,
                l[""Menu:Home""],
                ""/"",
                icon: ""fas fa-home""
            )
        );

        var {projectName.CamelSubName}Menu = new ApplicationMenuItem(
            {projectName.PascalSubName}Menus.Prefix,
            l[""Menu:{projectName.PascalSubName}""],
            icon: ""fa fa-coffee""
        );

        _ = context.Menu.AddItem({projectName.CamelSubName}Menu);
{subMenus}
    }}
}}
";
    }
}
