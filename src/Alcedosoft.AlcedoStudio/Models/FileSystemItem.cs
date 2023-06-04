namespace Alcedosoft.AlcedoStudio;

public abstract class FileSystemItem
{
    public string Name { get; init; } = String.Empty;
    public string Extension { get; init; } = String.Empty;
    public virtual string Icon { get; init; } = String.Empty;
    public virtual Color IconColor { get; set; } = Color.Default;

    public bool IsExpanded { get; set; }
    public HashSet<FileSystemItem> Items { get; init; } = new();
}
