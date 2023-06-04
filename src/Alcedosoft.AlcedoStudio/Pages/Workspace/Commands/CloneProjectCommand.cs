namespace Alcedosoft.AlcedoStudio;

public class CloneProjectCommand : Command
{
    private readonly Workspace _workspace;

    public CloneProjectCommand(Workspace workspace)
    {
        _workspace = workspace;
    }

    public override async void Execute(object? parameter)
    {
        var dialog = _workspace.DialogService.Show<CloneProjectDialog>("Clone Project");

        if (await dialog.Result is { Cancelled: false } result && result.Data is CloneProjectViewModel viewModel)
        {
            Console.WriteLine(viewModel);
        }

        base.Execute(parameter);
    }
}
