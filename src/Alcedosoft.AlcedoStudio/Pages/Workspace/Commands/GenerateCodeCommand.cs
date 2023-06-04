namespace Alcedosoft.AlcedoStudio;

public partial class GenerateCodeCommand : Command
{
    private readonly Workspace _workspace;
    private readonly OpenProjectCommand _openCommand;

    public GenerateCodeCommand(Workspace workspace)
    {
        _workspace = workspace;
        _openCommand = new OpenProjectCommand(workspace);
    }

    public override async void Execute(object? parameter)
    {
        if (_workspace.DirectoryHandle is not null)
        {
            var alcedostudio = await _workspace.DirectoryHandle
                .GetDirectoryHandleAsync(".alcedostudio", new() { Create = true });

            var template = await alcedostudio
                .GetFileHandleAsync(".template", new(){ Create = true});

            var reader = await template.GetFileAsync();

            var templateName = await reader.TextAsync();

            if (templateName is "abp_efcore_blazor")
            {
                await this.GenerateAbpEFCoreBlazorAsync(_workspace.DirectoryHandle);
            }
        }

        base.Execute(parameter);
    }

    private async Task GenerateAbpEFCoreBlazorAsync(FileSystemDirectoryHandle solutionDirectory)
    {
        var values = await solutionDirectory.ValuesAsync();

        var solutionFile = values.FirstOrDefault(x => x.Name.EndsWith(".sln"));

        if (solutionFile is null)
        {
            _ = _workspace.Snackbar.Add(".sln file not found", Severity.Warning);

            return;
        }

        var projectName = new ProjectName(Path.GetFileNameWithoutExtension(solutionFile.Name));

        var src = await solutionDirectory.GetDirectoryHandleAsync("src", new(){ Create = true});

        foreach (var schema in _workspace.Schemas)
        {
            await this.GenerateModelAsync(projectName, src, schema);
            await this.GenerateServiceAsync(projectName, src, schema);
            await this.GeneratePageAsync(projectName, src, schema);
        }

        await this.GenerateAdapterAsync(projectName, src, _workspace.Schemas);
        await this.GenerateContextAsync(projectName, src, _workspace.Schemas);
        await this.GeneratePermissionAsync(projectName, src, _workspace.Schemas);
        await this.GenerateMenuAsync(projectName, src, _workspace.Schemas);
        await this.GenerateLocalizationAsync(projectName, src, _workspace.Schemas);

        _ = _workspace.Snackbar.Add("Code Generated", Severity.Success);

        await _openCommand.LoadDirectory(solutionDirectory);
    }

    private async Task WriteTextAsync(FileSystemFileHandle file, string content)
    {
        var writer = await file
            .CreateWritableAsync(new() { KeepExistingData = false });
        await writer.WriteAsync(content);
        await writer.CloseAsync();
    }
}
