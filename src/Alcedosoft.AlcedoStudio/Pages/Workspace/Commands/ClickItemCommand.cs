namespace Alcedosoft.AlcedoStudio;

public class ClickItemCommand : Command
{
    private readonly Workspace _worksapce;

    public ClickItemCommand(Workspace workspace)
    {
        _worksapce = workspace;
    }

    public override async void Execute(object? parameter)
    {
        if (parameter is FileSystemFileItem file && _worksapce.ProviewEditor is not null)
        {
            string content;

            if (file.Extension is ".cs")
            {
                var reader = await file.Handle.GetFileAsync();

                content = await reader.TextAsync();
            }
            else
            {
                content = $"File type {file.Extension} not supported.";
            }

            await _worksapce.ProviewEditor.SetValue(content);
        }
        else if (parameter is FileSystemFolderItem)
        {

        }

        base.Execute(parameter);
    }
}
