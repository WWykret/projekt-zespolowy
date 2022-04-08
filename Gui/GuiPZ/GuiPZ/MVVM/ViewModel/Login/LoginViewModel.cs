using System.Windows.Input;
using GuiPZ.Command;
using GuiPZ.Navigation;

namespace GuiPZ.MVVM.ViewModel.Login;

public class LoginViewModel : ViewModelBase
{
    public LoginViewModel(ContextNavigation mainNav)
    {
        _loginNav = new ContextNavigation();
        _loginNav.CurrentViewModel = new ProfilesViewModel(mainNav, _loginNav);
        _loginNav.CurrentViewModelChanged += OnCurrentViewModelChanged;
    }
    
    private readonly ContextNavigation _loginNav;
    
    public ViewModelBase CurrentViewModel => _loginNav.CurrentViewModel;
    
    private void OnCurrentViewModelChanged()
    {
        OnPropertyChanged(nameof(CurrentViewModel));
    }
}