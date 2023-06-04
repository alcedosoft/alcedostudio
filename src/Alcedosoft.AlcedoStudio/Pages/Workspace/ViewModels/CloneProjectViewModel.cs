namespace Alcedosoft.AlcedoStudio;

public class CloneProjectViewModel
{
    public string ProjectName { get; set; } = String.Empty;

    public string RepositoryUrl { get; set; } = String.Empty;

    public string ProjectDirectory { get; set; } = String.Empty;

    public FileSystemDirectoryHandle DirectoryHandle { get; set; } = null!;
}
