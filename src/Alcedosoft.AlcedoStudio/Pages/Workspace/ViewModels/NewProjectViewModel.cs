namespace Alcedosoft.AlcedoStudio;

public class NewProjectViewModel
{
    public string ProjectName { get; set; } = String.Empty;

    public string TemplatePath { get; set; } = String.Empty;

    public string ProjectDirectory { get; set; } = String.Empty;

    public FileSystemDirectoryHandle DirectoryHandle { get; set; } = null!;
}
