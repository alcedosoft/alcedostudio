using System.Text.Json;

namespace Alcedosoft.AlcedoStudio.Pages;

public partial class Project
{
    private bool _loading;

    private MonacoEditor? Editor { get; set; }

    public HashSet<FileSchema> Schemas { get; set; } = new();
    private HashSet<FileSystemItem> FileSystemItems { get; set; } = new();

    [Inject]
    public HttpClient HttpClient { get; set; } = null!;
    [Inject]
    public FileSystemAccessService FileSystemAccessService { get; set; } = null!;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await MonacoEditorBase.SetTheme("vs-dark");
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    private async Task OpenDictionayAsync()
    {
        var options = new DirectoryPickerOptionsStartInWellKnownDirectory
        {
            StartIn = WellKnownDirectory.Downloads
        };

        try
        {
            _loading = true;

            var directory = await FileSystemAccessService.ShowDirectoryPickerAsync(options);

            var state = await directory.RequestPermission(new() { Mode = FileSystemPermissionMode.ReadWrite });

            if (state is PermissionState.Granted)
            {
                var handler = new NewProjectHandler(HttpClient);

                await handler.ExecuteAsync("demo1", "/webapi.template", directory);

                await ResolveSchema(Schemas, directory);

                await ResolveSolution(FileSystemItems, directory);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
        finally
        {
            _loading = false;
        }
    }

    private async Task ResolveSchema(HashSet<FileSchema> schemas, FileSystemDirectoryHandle directory)
    {
        var fileSystems = await directory.ValuesAsync();

        var projectDirectory = fileSystems
            .FirstOrDefault(x => x.Kind is FileSystemHandleKind.Directory);

        if (projectDirectory is not null)
        {
            var projectHandle = await directory.GetDirectoryHandleAsync(projectDirectory.Name);

            var propertiesHandle = await projectHandle.GetDirectoryHandleAsync("Properties", new() { Create = true });

            var schemasHandle = await propertiesHandle.GetDirectoryHandleAsync("Schemas", new() { Create = true });

            foreach (var handle in await schemasHandle.ValuesAsync())
            {
                if (handle.Kind is FileSystemHandleKind.File)
                {
                    var fileHandle = await schemasHandle.GetFileHandleAsync(handle.Name);

                    var reader = await fileHandle.GetFileAsync();

                    string schemaText = await reader.TextAsync();

                    var schema = JsonSerializer.Deserialize<FileSchema>(schemaText);

                    if (schema is not null)
                    {
                        _ = schemas.Add(schema);
                    }
                }
            }
        }
    }

    private async Task ResolveSolution(HashSet<FileSystemItem> items, FileSystemDirectoryHandle directory)
    {
        var handles = await directory.ValuesAsync();

        foreach (var handle in handles)
        {
            FileSystemItem? item;

            if (handle.Kind == FileSystemHandleKind.File)
            {
                var file = await directory.GetFileHandleAsync(handle.Name);

                item = new(file);
            }
            else
            {
                if (handle.Name is ".vs" or ".vscode" or "obj" or "bin" or "node_modules")
                {
                    continue;
                }

                var subDirectory = await directory.GetDirectoryHandleAsync(handle.Name);

                item = new(subDirectory);

                await ResolveSolution(item.Items, subDirectory);
            }

            _ = items.Add(item);
        }
    }

    private Task OnSelectSchemaChanged(FileSchema? item)
    {
        return Editor?.SetValue(JsonSerializer.Serialize(item))
            ?? Task.CompletedTask;
    }

    private async Task OnSelectedItemChanged(FileSystemItem? item)
    {
        if (item?.Handle is FileSystemFileHandle file)
        {
            if (item.Extension is ".cs")
            {
                var reader = await file.GetFileAsync();

                string text = await reader.TextAsync();

                _ = Editor?.SetValue(text);
            }
            else
            {
                _ = Editor?.SetValue($"File type {item.Extension} not supported.");
            }
        }
    }

    private StandaloneEditorConstructionOptions EditorConstructionOptions(MonacoEditor editor)
    {
        return new StandaloneEditorConstructionOptions
        {
            ReadOnly = true,
            Language = "csharp"
        };
    }
}