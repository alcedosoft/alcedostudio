namespace Alcedosoft.AlcedoStudio;

public abstract class Command : ICommand
{
    public event EventHandler? CanExecuteChanged;

    public virtual bool CanExecute(object? parameter)
    {
        return true;
    }

    public virtual void Execute(object? parameter)
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
