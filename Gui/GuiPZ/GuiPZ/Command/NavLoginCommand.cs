using System;
using GuiPZ.MVVM.ViewModel;
using GuiPZ.Navigation;

namespace GuiPZ.Command;

public class NavLoginCommand<T> : CommandBase where T : ViewModelBase
{
    private readonly LoginNav _loginNav;
    private readonly Func<T> _createViewModel;

    public NavLoginCommand(LoginNav loginNav, Func<T> createViewModel)
    {
        _loginNav = loginNav;
        _createViewModel = createViewModel;
    }

    public override void Execute(object? parameter)
    {
        _loginNav.CurrentViewModel = _createViewModel();
    }
}