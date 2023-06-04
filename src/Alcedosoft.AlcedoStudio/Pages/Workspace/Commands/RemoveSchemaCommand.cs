namespace Alcedosoft.AlcedoStudio;

public class RemoveSchemaCommand : Command
{
    private readonly Workspace _workspace;
    private readonly SchemaHandler _handler;

    public RemoveSchemaCommand(Workspace workspace)
    {
        _workspace = workspace;
        _handler = new(workspace);
    }

    public override async void Execute(object? parameter)
    {
        var parameters = new DialogParameters
        {
            { "Message", "Do you really want to delete the schema? This process cannot be undone." },
            { "ButtonText", "Delete" },
            { "ButtonColor", Color.Error }
        };

        var dialog = _workspace.DialogService.Show<ConfirmDialog>("Delete", parameters);

        if (await dialog.Result is { Data: true } && _workspace.SelectedSchema is not null)
        {
            _ = _workspace.Schemas.Remove(_workspace.SelectedSchema);

            _workspace.StateHasChanged();

            await _handler.DeleteAsync(_workspace.SelectedSchema);

            _ = _workspace.Snackbar.Add("Schema Deleted", Severity.Success);
        }

        base.Execute(parameter);
    }
}
