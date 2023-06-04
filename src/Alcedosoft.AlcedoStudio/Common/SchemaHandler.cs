namespace Alcedosoft.AlcedoStudio;

public class SchemaHandler
{
    public const string EXTENSION = ".schema";
    public const string SCHEMADIR = ".alcedostudio";

    private readonly Workspace _workspace;

    public SchemaHandler(Workspace workspace)
    {
        _workspace = workspace;
    }

    public async Task SaveAsync(FileSchema schema)
    {
        if (schema.Handle is null && _workspace.DirectoryHandle is not null)
        {
            var schemaDirectory = await _workspace.DirectoryHandle
                .GetDirectoryHandleAsync(SCHEMADIR, new() { Create = true });

            if (schemaDirectory is not null)
            {
                var file = await schemaDirectory.GetFileHandleAsync(
                    $"{schema.Name}{EXTENSION}", new() { Create = true });

                schema.Handle = file;
            }
        }

        if (schema.Handle is not null)
        {
            var writer = await schema.Handle
                .CreateWritableAsync(new() { KeepExistingData = false });

            await writer.WriteAsync(JsonSerializer.Serialize(schema, new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
            }));

            await writer.CloseAsync();
        }
    }

    public async Task DeleteAsync(FileSchema schema)
    {
        if (_workspace.DirectoryHandle is not null)
        {
            var schemaDirectory = await _workspace.DirectoryHandle
                .GetDirectoryHandleAsync(SCHEMADIR, new() { Create = true });

            if (schemaDirectory is not null)
            {
                await schemaDirectory.RemoveEntryAsync($"{schema.Name}{EXTENSION}");
            }
        }
    }

    public async Task<FileSchema[]> GetSchemasAsync()
    {
        var schemas = new List<FileSchema>();

        if (_workspace.DirectoryHandle is not null)
        {
            var schemaDirectory = await _workspace.DirectoryHandle
                .GetDirectoryHandleAsync(SCHEMADIR, new() { Create = true });

            foreach (var handle in await schemaDirectory.ValuesAsync())
            {
                if (handle.Kind is FileSystemHandleKind.File && handle.Name.EndsWith(EXTENSION))
                {
                    var fileHandle = await schemaDirectory.GetFileHandleAsync(handle.Name);

                    var reader = await fileHandle.GetFileAsync();

                    var schemaText = await reader.TextAsync();

                    var schema = JsonSerializer.Deserialize<FileSchema>(schemaText);

                    if (schema is not null)
                    {
                        schema.Handle = fileHandle;

                        schemas.Add(schema);
                    }
                }
            }
        }

        return schemas.ToArray();
    }
}
