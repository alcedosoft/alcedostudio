namespace Alcedosoft.AlcedoStudio;

public class EditSchemaCommand : Command
{
    private readonly Workspace _workspace;
    private readonly SchemaHandler _handler;

    public EditSchemaCommand(Workspace workspace)
    {
        _workspace = workspace;
        _handler = new(workspace);
    }

    public override async void Execute(object? parameter)
    {
        if (_workspace.SelectedSchema is not null && _workspace.DirectoryHandle is not null)
        {
            var parameters = new DialogParameters { { "Schema", _workspace.SelectedSchema } };

            var dialog = _workspace.DialogService.Show<EditSchemaDialog>("Eidt Schema", parameters);

            if (await dialog.Result is { Cancelled: false } result && result.Data is FileSchema schema)
            {
                await _handler.SaveAsync(schema);

                _workspace.SelectedSchema = schema;

                _workspace.StateHasChanged();

                _ = _workspace.Snackbar.Add("Schema Updated", Severity.Success);
            }
        }

        base.Execute(parameter);
    }
}
