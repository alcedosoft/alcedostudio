namespace Alcedosoft.AlcedoStudio;

public class FileSchema
{
    public string Name { get; set; } = String.Empty;
    public string DisplayName { get; set; } = String.Empty;
    public string Description { get; set; } = String.Empty;

    public List<FileSchemaItem> Items { get; set; } = new();
}
