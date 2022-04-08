using System.Windows.Input;
using GuiPZ.Command;
using GuiPZ.Navigation;

namespace GuiPZ.MVVM.ViewModel;

public class LoginViewModel : ViewModelBase
{
    public LoginViewModel(MainNav mainNav)
    {
        _loginNav = new LoginNav();
        _loginNav.CurrentViewModel = new ProfilesViewModel(mainNav, _loginNav);
        _loginNav.CurrentViewModelChanged += OnCurrentViewModelChanged;
    }
    
    private readonly LoginNav _loginNav;
    
    public ViewModelBase CurrentViewModel => _loginNav.CurrentViewModel;
    
    private void OnCurrentViewModelChanged()
    {
        OnPropertyChanged(nameof(CurrentViewModel));
    }
}