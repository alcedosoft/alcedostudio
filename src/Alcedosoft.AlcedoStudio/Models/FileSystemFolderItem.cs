namespace Alcedosoft.AlcedoStudio;

public class FileSystemFolderItem : FileSystemItem
{
    public FileSystemFolderItem(FileSystemDirectoryHandle directory)
    {
        this.Name = directory.Name;
        this.Extension = Path.GetExtension(directory.Name);

        this.Handle = directory;
    }

    public override string Icon => this.IsExpanded
        ? Icons.Custom.Uncategorized.FolderOpen
        : Icons.Custom.Uncategorized.Folder;

    public FileSystemDirectoryHandle Handle { get; }
}
