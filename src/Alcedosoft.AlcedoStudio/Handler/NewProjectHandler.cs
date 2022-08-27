namespace Alcedosoft.AlcedoStudio;

public class NewProjectHandler
{
    private const string NAME = "[NAME]";
    private const string REPLACE = "__REPLACE__";

    private readonly HttpClient _client;

    public NewProjectHandler(HttpClient client)
    {
        _client = client;
    }

    public async Task ExecuteAsync(
        string projectName, string templatePath, FileSystemDirectoryHandle directory)
    {
        var templateStream = await _client.GetStreamAsync(templatePath);

        var templateArchive = new ZipArchive(templateStream, ZipArchiveMode.Read, true);

        var directoryMappings = new Dictionary<string, FileSystemDirectoryHandle>();

        var replaceFiles = new HashSet<string>(templateArchive.Entries.Count);

        var replace = templateArchive.GetEntry(REPLACE);

        if (replace is not null)
        {
            var reader = new StreamReader(replace.Open());

            while (await reader.ReadLineAsync() is string item)
            {
                replaceFiles.Add(item);
            }
        }

        foreach (var entry in templateArchive.Entries)
        {
            if (entry.Name is REPLACE)
            {
                continue;
            }

            string name = entry.Name.Replace(NAME, projectName);
            string fullName = entry.FullName.Replace(NAME, projectName);

            bool isFile = !String.IsNullOrEmpty(name);

            string[] names = fullName.Split("/", StringSplitOptions.RemoveEmptyEntries);

            string[] directoryNames = isFile ? names[..^1] : names;

            string parentPath = String.Empty;

            var parentDirectory = directory;

            foreach (string directoryName in directoryNames)
            {
                string currentPath = $"{parentPath}{directoryName}/";

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

                    string content = await reader.ReadToEndAsync();

                    content = content.Replace(NAME, projectName);

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
