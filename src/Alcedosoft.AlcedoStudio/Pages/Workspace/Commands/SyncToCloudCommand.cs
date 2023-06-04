namespace Alcedosoft.AlcedoStudio;

public class SyncToCloudCommand : Command
{
    private readonly Workspace _workspace;

    public SyncToCloudCommand(Workspace workspace)
    {
        _workspace = workspace;
    }

    public override async void Execute(object? parameter)
    {
        var dialog = _workspace.DialogService.Show<SyncToCloudDialog>("Sync To Cloud");

        if (await dialog.Result is { Cancelled: false } result && result.Data is SyncToCloudViewModel viewModel)
        {
            Console.WriteLine(viewModel);
        }

        base.Execute(parameter);
    }
}
