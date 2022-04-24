using System.Windows.Input;
using GuiPZ.Command;
using GuiPZ.Communicator.Client;
using GuiPZ.Container;
using GuiPZ.Navigation;

namespace GuiPZ.MVVM.ViewModel.Login;

public class LoginViewModel : ViewModelBase
{
    private DataContainer _dataContainer;
    private DataExchanger _dataExchanger;
    public LoginViewModel(ContextNavigation mainNav, DataContainer dataContainer, DataExchanger dataExchanger)
    {
        _dataContainer = dataContainer;
        _dataExchanger = dataExchanger;
        
        _loginNav = new ContextNavigation();
        _loginNav.CurrentViewModel = new ProfilesViewModel(mainNav, _loginNav, _dataContainer, _dataExchanger);
        _loginNav.CurrentViewModelChanged += OnCurrentViewModelChanged;
        
        
    }
    
    private readonly ContextNavigation _loginNav;
    
    public ViewModelBase CurrentViewModel => _loginNav.CurrentViewModel;
    
    private void OnCurrentViewModelChanged()
    {
        OnPropertyChanged(nameof(CurrentViewModel));
    }
}