using GuiPZ.MVVM.ViewModel.Login;
using GuiPZ.Navigation;

namespace GuiPZ.MVVM.ViewModel;

public class MainViewModel : ViewModelBase
{
    private readonly ContextNavigation _mainNav;

    public ViewModelBase CurrentViewModel => _mainNav.CurrentViewModel;

    public MainViewModel()
    {
        _mainNav = new ContextNavigation();
        _mainNav.CurrentViewModel = new LoginViewModel(_mainNav);
        _mainNav.CurrentViewModelChanged += OnCurrentViewModelChanged;
    }
    
    private void OnCurrentViewModelChanged()
    {
        OnPropertyChanged(nameof(CurrentViewModel));
    }
}