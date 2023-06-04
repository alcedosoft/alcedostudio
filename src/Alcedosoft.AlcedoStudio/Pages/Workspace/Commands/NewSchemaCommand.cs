namespace Alcedosoft.AlcedoStudio;

public class NewSchemaCommand : Command
{
    private readonly Workspace _workspace;
    private readonly SchemaHandler _handler;

    public NewSchemaCommand(Workspace workspace)
    {
        _workspace = workspace;
        _handler = new(workspace);
    }

    public override async void Execute(object? parameter)
    {
        if (_workspace.DirectoryHandle is null)
        {
            return;
        }

        var dialog = _workspace.DialogService.Show<NewSchemaDialog>("Create New Schame");

        if (await dialog.Result is { Cancelled: false } reuslt && reuslt.Data is FileSchema schema)
        {
            await _handler.SaveAsync(schema);

            _ = _workspace.Schemas.Add(schema);

            _workspace.SelectedSchema = schema;

            _workspace.StateHasChanged();

            _ = _workspace.Snackbar.Add("Schema Created", Severity.Success);
        }

        base.Execute(parameter);
    }
}
