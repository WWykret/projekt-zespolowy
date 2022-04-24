using System;
using GuiPZ.MVVM.ViewModel;
using GuiPZ.Navigation;

namespace GuiPZ.Command;

public class NavCommand<T> : CommandBase where T : ViewModelBase
{
    private readonly ContextNavigation _contextNavigation;
    private readonly Func<T> _createViewModel;

    public NavCommand(ContextNavigation contextNavigation, Func<T> createViewModel)
    {
        _contextNavigation = contextNavigation;
        _createViewModel = createViewModel;
    }

    public override void Execute(object? parameter)
    {
        _contextNavigation.CurrentViewModel = _createViewModel();
    }
}