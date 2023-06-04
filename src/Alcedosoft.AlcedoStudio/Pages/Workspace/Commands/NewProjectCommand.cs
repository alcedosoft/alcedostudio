namespace Alcedosoft.AlcedoStudio;

public class NewProjectCommand : Command
{
    public const string PROJECTNAME = "[PROJECTNAME]";
    public const string PROJECTSUBNAME = "[PROJECTSUBNAME]";
    public const string REPLACE = "__REPLACE__";

    private readonly Workspace _workspace;
    private readonly OpenProjectCommand _openCommand;

    public NewProjectCommand(Workspace workspace)
    {
        _workspace = workspace;
        _openCommand = new OpenProjectCommand(workspace);
    }

    public override async void Execute(object? parameter)
    {
        var dialog = _workspace.DialogService.Show<NewProjectDialog>("Create New Project");

        if (await dialog.Result is { Cancelled: false } result && result.Data is NewProjectViewModel viewModel)
        {
            var state = await viewModel.DirectoryHandle
                .RequestPermission(new() { Mode = FileSystemPermissionMode.ReadWrite });

            if (state is PermissionState.Granted)
            {
                try
                {
                    _workspace.IsLoading = true;

                    _workspace.DirectoryHandle = viewModel.DirectoryHandle;

                    _workspace.StateHasChanged();

                    await this.ExecuteAsync(viewModel);

                    await _openCommand.LoadDirectory(viewModel.DirectoryHandle);

                    _ = _workspace.Snackbar.Add("Project Created", Severity.Success);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                finally
                {
                    _workspace.IsLoading = false;

                    _workspace.StateHasChanged();
                }
            }
        }

        base.Execute(parameter);
    }

    public async Task ExecuteAsync(NewProjectViewModel viewModel)
    {
        var templateStream = await _workspace.HttpClient.GetStreamAsync(viewModel.TemplatePath);

        var templateArchive = new ZipArchive(templateStream, ZipArchiveMode.Read, true);

        var directoryMappings = new Dictionary<string, FileSystemDirectoryHandle>();

        var replaceFiles = new HashSet<string>(templateArchive.Entries.Count);

        var replace = templateArchive.GetEntry(REPLACE);

        if (replace is not null)
        {
            var reader = new StreamReader(replace.Open());

            while (await reader.ReadLineAsync() is string item)
            {
                _ = replaceFiles.Add(item);
            }
        }

        foreach (var entry in templateArchive.Entries)
        {
            if (entry.Name is REPLACE)
            {
                continue;
            }

            var subname = viewModel.ProjectName
                .Split('.', StringSplitOptions.RemoveEmptyEntries)
                .LastOrDefault() ?? viewModel.ProjectName;

            var name = entry.Name
                .Replace(PROJECTNAME, viewModel.ProjectName)
                .Replace(PROJECTSUBNAME, subname);
            var fullName = entry.FullName
                .Replace(PROJECTNAME, viewModel.ProjectName)
                .Replace(PROJECTSUBNAME, subname);

            var isFile = !String.IsNullOrEmpty(name);

            var names = fullName.Split("/", StringSplitOptions.RemoveEmptyEntries);

            var directoryNames = isFile ? names[..^1] : names;

            var parentPath = String.Empty;

            var parentDirectory = viewModel.DirectoryHandle;

            foreach (var directoryName in directoryNames)
            {
                var currentPath = $"{parentPath}{directoryName}/";

                if (!directoryMappings.TryGetValue(currentPath, out var currentDirectory))
                {
                    currentDirectory = await parentDirectory
                        .GetDirectoryHandleAsync(directoryName, new() { Create = true });

                    directoryMappings[currentPath] = currentDirectory;
                }

                parentPath = currentPath;
                parentDirectory = currentDirectory;
            }

            if (isFile)
            {
                var file = await parentDirectory.GetFileHandleAsync(name, new() { Create = true });

                var writer = await file.CreateWritableAsync(new() { KeepExistingData = false });

                if (replaceFiles.Contains(entry.FullName))
                {
                    var reader = new StreamReader(entry.Open());

                    var content = await reader.ReadToEndAsync();

                    content = content
                        .Replace(PROJECTNAME, viewModel.ProjectName)
                        .Replace(PROJECTSUBNAME, subname);

                    await writer.WriteAsync(content);
                }
                else
                {
                    await entry.Open().CopyToAsync(writer);
                }

                await writer.CloseAsync();
            }
        }
    }
}
