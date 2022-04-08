using System;
using GuiPZ.MVVM.ViewModel;
using GuiPZ.Navigation;

namespace GuiPZ.Command;

public class NavMainCommand<T> : CommandBase where T : ViewModelBase
{
    private readonly MainNav _mainNav;
    private readonly Func<T> _createViewModel;

    public NavMainCommand(MainNav mainNav, Func<T> createViewModel)
    {
        _mainNav = mainNav;
        _createViewModel = createViewModel;
    }

    public override void Execute(object? parameter)
    {
        _mainNav.CurrentViewModel = _createViewModel();
    }
}