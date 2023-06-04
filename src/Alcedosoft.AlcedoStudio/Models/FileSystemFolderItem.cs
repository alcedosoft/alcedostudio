namespace Alcedosoft.AlcedoStudio;

public class FileSystemFolderItem : FileSystemItem
{
    public FileSystemFolderItem(FileSystemDirectoryHandle directory)
    {
        Name = directory.Name;
        Extension = Path.GetExtension(directory.Name);

        Handle = directory;
    }

    public override string Icon => IsExpanded
        ? Icons.Custom.Uncategorized.FolderOpen
        : Icons.Custom.Uncategorized.Folder;

    public FileSystemDirectoryHandle Handle { get; }
}
