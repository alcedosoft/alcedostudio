namespace Alcedosoft.AlcedoStudio;

public partial class GenerateCodeCommand
{
    public async Task GenerateLocalizationAsync(
        ProjectName projectName, FileSystemDirectoryHandle src, IEnumerable<FileSchema> schemas)
    {
        var shared = await src.GetDirectoryHandleAsync(
            $"{projectName.Value}.Domain.Shared", new(){ Create = true});

        var localizationDir = await shared.GetDirectoryHandleAsync(
            "Localization", new(){ Create = true});

        localizationDir = await localizationDir.GetDirectoryHandleAsync(
            projectName.PascalSubName, new() { Create = true });

        var zh_HansFile = await localizationDir.GetFileHandleAsync(
            "zh-Hans.json", new(){ Create = true });

        var zh_HansContent = this.Generatezh_Hans(projectName, schemas);

        await this.WriteTextAsync(zh_HansFile, zh_HansContent);
    }

    private string Generatezh_Hans(ProjectName projectName, IEnumerable<FileSchema> schemas)
    {
        var texts = new StringBuilder();

        _ = texts.AppendLine(@$"    ""New"": ""新增"",");
        _ = texts.AppendLine(@$"    ""Edit"": ""编辑"",");
        _ = texts.AppendLine(@$"    ""Delete"": ""删除"",");
        _ = texts.AppendLine(@$"    ""Cancel"": ""取消"",");
        _ = texts.AppendLine(@$"    ""Menu:Home"": ""首页"",");

        _ = texts.AppendLine(@$"    ""Menu:{projectName.PascalSubName}"": ""{projectName.PascalSubName}"",");
        _ = texts.AppendLine(@$"    ""Permission:{projectName.PascalSubName}"": ""{projectName.PascalSubName}"",");

        foreach (var schema in schemas)
        {
            var schemaName = new SchemaName(schema.Name);

            _ = texts.AppendLine(@$"    ""{schemaName.PluralPascalName}"": ""{schema.Description}"",");
            _ = texts.AppendLine(@$"    ""Menu:{schemaName.PluralPascalName}"": ""{schema.Description}"",");

            _ = texts.AppendLine(@$"    ""Permission:{schemaName.PluralPascalName}"": ""{schema.Description}"",");
            _ = texts.AppendLine(@$"    ""Permission:{schemaName.PluralPascalName}.Create"": ""新增{schema.DisplayName}"",");
            _ = texts.AppendLine(@$"    ""Permission:{schemaName.PluralPascalName}.Update"": ""修改{schema.DisplayName}"",");
            _ = texts.AppendLine(@$"    ""Permission:{schemaName.PluralPascalName}.Delete"": ""删除{schema.DisplayName}"",");

            foreach (var item in schema.Items)
            {
                _ = texts.AppendLine(@$"    ""{schemaName.PascalName}:{item.Name}"": ""{item.DisplayName}"",");
            }
        }

        return $@"{{
  ""culture"": ""zh-Hans"",
  ""texts"": {{
{texts}
    ""Welcome"": ""欢迎"",
    ""LongWelcomeMessage"": ""欢迎来到该应用程序. 这是一个基于ABP框架的启动项目. 有关更多信息, 请访问 abp.io.""
  }}
}}";
    }
}
