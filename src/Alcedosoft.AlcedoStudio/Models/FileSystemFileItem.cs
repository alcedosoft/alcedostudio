namespace Alcedosoft.AlcedoStudio;

public class FileSystemFileItem : FileSystemItem
{
    public FileSystemFileItem(FileSystemFileHandle file)
    {
        this.Name = file.Name;
        this.Extension = Path.GetExtension(file.Name);
        this.Handle = file;
    }

    public override string Icon => this.Extension switch
    {
        ".cs" => Icons.Custom.FileFormats.FileCode,
        ".sln" => Icons.Custom.Brands.MicrosoftVisualStudio,
        ".csproj" => Icons.Custom.Brands.MicrosoftVisualStudio,
        _ => Icons.Custom.FileFormats.FileDocument,
    };

    public FileSystemFileHandle Handle { get; }
}
