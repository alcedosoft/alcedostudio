namespace Alcedosoft.AlcedoStudio;

public class FileSystemItem
{
    public FileSystemItem(FileSystemFileHandle file)
    {
        this.Name = file.Name;
        this.Extension = Path.GetExtension(file.Name);

        this.Icon = this.Extension switch
        {
            ".cs" => Icons.Custom.FileFormats.FileCode,
            ".sln" => Icons.Custom.Brands.MicrosoftVisualStudio,
            ".csproj" => Icons.Custom.Brands.MicrosoftVisualStudio,
            _ => Icons.Custom.FileFormats.FileDocument,
        };

        this.Handle = file;
    }

    public FileSystemItem(FileSystemDirectoryHandle directory)
    {
        this.Name = directory.Name;
        this.Extension = Path.GetExtension(directory.Name);

        this.Icon = Icons.Filled.Folder;

        this.Handle = directory;
    }

    public string Icon { get; set; }
    public string Name { get; set; }
    public string Extension { get; set; }

    public bool IsExpanded { get; set; }
    public FileSystemHandle Handle { get; set; }
    public HashSet<FileSystemItem> Items { get; set; } = new();
}
