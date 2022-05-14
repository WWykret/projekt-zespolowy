using System;
using GuiPZ.Container;
using GuiPZ.MVVM.ViewModel;
using GuiPZ.MVVM.ViewModel.Login;
using GuiPZ.Navigation;

namespace GuiPZ.Command;

public class AddProfileCommand<T> : CommandBase where T : ViewModelBase
{
    private readonly RegistrationViewModel _dataSource;
    
    private readonly ContextNavigation _contextNavigation;
    private readonly Func<T> _createViewModel;

    public AddProfileCommand(ContextNavigation contextNavigation, Func<T> createViewModel, RegistrationViewModel dataSource)
    {
        _contextNavigation = contextNavigation;
        _createViewModel = createViewModel;
        
        _dataSource = dataSource;
    }
    
    public override void Execute(object? parameter)
    {
        _dataSource.AddProfile(_dataSource.CurrentProfile);
        
        _contextNavigation.CurrentViewModel = _createViewModel();
    }
}