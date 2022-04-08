using GuiPZ.Navigation;

namespace GuiPZ.MVVM.ViewModel;

public class MainViewModel : ViewModelBase
{
    private readonly MainNav _mainNav;

    public ViewModelBase CurrentViewModel => _mainNav.CurrentViewModel;

    public MainViewModel()
    {
        _mainNav = new MainNav();
        _mainNav.CurrentViewModel = new LoginViewModel(_mainNav);
        _mainNav.CurrentViewModelChanged += OnCurrentViewModelChanged;
    }
    
    private void OnCurrentViewModelChanged()
    {
        OnPropertyChanged(nameof(CurrentViewModel));
    }
}