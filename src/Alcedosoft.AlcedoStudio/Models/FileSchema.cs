namespace Alcedosoft.AlcedoStudio;

public class FileSchema
{
    public string Name { get; set; } = String.Empty;
    public string DisplayName { get; set; } = String.Empty;
    public string Description { get; set; } = String.Empty;
    public List<FileSchemaItem> Items { get; set; } = new();

    [JsonIgnore]
    public bool IsExpanded { get; set; }
    [JsonIgnore]
    public string Icon { get; init; } = Icons.Filled.Schema;
    [JsonIgnore]
    public Color IconColor { get; set; } = Color.Default;

    [JsonIgnore]
    public FileSystemFileHandle? Handle { get; set; }
}
