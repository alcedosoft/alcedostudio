namespace Alcedosoft.AlcedoStudio;

public class OpenProjectCommand : Command
{
    private readonly Workspace _workspace;
    private readonly SchemaHandler _schemaHandler;

    public OpenProjectCommand(Workspace workspace)
    {
        _workspace = workspace;
        _schemaHandler = new(workspace);
    }

    public override async void Execute(object? parameter)
    {
        var options = new DirectoryPickerOptionsStartInWellKnownDirectory
        {
            StartIn = WellKnownDirectory.Desktop,
        };

        _workspace.DirectoryHandle = await _workspace.FileSystemService.ShowDirectoryPickerAsync(options);

        var state = await _workspace.DirectoryHandle
            .RequestPermission(new() { Mode = FileSystemPermissionMode.ReadWrite });

        if (state is PermissionState.Granted)
        {
            try
            {
                _workspace.IsLoading = true;

                _workspace.StateHasChanged();

                foreach (var schema in await _schemaHandler.GetSchemasAsync())
                {
                    _ = _workspace.Schemas.Add(schema);
                }

                await ResolveSolution(_workspace.DirectoryHandle, _workspace.FileSystemItems);

                _ = _workspace.Snackbar.Add("Project Opened", Severity.Success);
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

    public async Task LoadDirectory(FileSystemDirectoryHandle directory)
    {
        _workspace.Schemas.Clear();
        _workspace.FileSystemItems.Clear();

        foreach (var schema in await _schemaHandler.GetSchemasAsync())
        {
            _ = _workspace.Schemas.Add(schema);
        }

        await ResolveSolution(directory, _workspace.FileSystemItems);
    }

    private async Task ResolveSolution(FileSystemDirectoryHandle directory, HashSet<FileSystemItem> items)
    {
        var handles = await directory.ValuesAsync();

        foreach (var handle in handles)
        {
            FileSystemItem? item;

            if (handle.Kind == FileSystemHandleKind.File)
            {
                var file = await directory.GetFileHandleAsync(handle.Name);

                item = new FileSystemFileItem(file);
            }
            else
            {
                if (handle.Name is ".vs" or ".vscode" or "obj" or "bin" or "node_modules")
                {
                    continue;
                }

                var subDirectory = await directory.GetDirectoryHandleAsync(handle.Name);

                item = new FileSystemFolderItem(subDirectory);

                await ResolveSolution(subDirectory, item.Items);
            }

            _ = items.Add(item);
        }
    }
}
