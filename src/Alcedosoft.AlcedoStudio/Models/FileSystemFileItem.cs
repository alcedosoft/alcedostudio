namespace Alcedosoft.AlcedoStudio;

public class FileSystemFileItem : FileSystemItem
{
    public FileSystemFileItem(FileSystemFileHandle file)
    {
        Name = file.Name;
        Extension = Path.GetExtension(file.Name);
        Handle = file;
    }

    public override string Icon => Extension switch
    {
        ".cs" => Icons.Custom.FileFormats.FileCode,
        ".sln" => Icons.Custom.Brands.MicrosoftVisualStudio,
        ".csproj" => Icons.Custom.Brands.MicrosoftVisualStudio,
        _ => Icons.Custom.FileFormats.FileDocument,
    };

    public FileSystemFileHandle Handle { get; }
}
