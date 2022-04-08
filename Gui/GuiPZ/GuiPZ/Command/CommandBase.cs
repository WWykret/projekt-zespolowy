using System;
using System.Windows.Input;

namespace GuiPZ.Command;

public abstract  class CommandBase : ICommand
{
    public virtual bool CanExecute(object? parameter) => true;
    
    public abstract void Execute(object? parameter);

    public event EventHandler? CanExecuteChanged;

    protected void OnCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}