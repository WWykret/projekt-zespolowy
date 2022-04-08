using System.Windows.Input;
using GuiPZ.Command;
using GuiPZ.MVVM.ViewModel.Login;
using GuiPZ.Navigation;

namespace GuiPZ.MVVM.ViewModel.Main;

public class HomeViewModel : ViewModelBase
{
    public ICommand NavMainCommand { get; }

    public HomeViewModel(ContextNavigation mainNav)
    {
        NavMainCommand = new NavCommand<LoginViewModel>(mainNav, () => new LoginViewModel(mainNav));
        
        _stockNav = new ContextNavigation();
        _stockNav.CurrentViewModel = new PredictionsViewModel();
        _stockNav.CurrentViewModelChanged += OnCurrentViewModelChanged;

        PredictionsViewCommand = new NavCommand<PredictionsViewModel>(_stockNav, () => new PredictionsViewModel());
        UserViewCommand = new NavCommand<UserStockViewModel>(_stockNav, () => new UserStockViewModel());
        ManageViewCommand = new NavCommand<ManageStockViewModel>(_stockNav, () => new ManageStockViewModel());
    }

    public ICommand PredictionsViewCommand { get; }
    public ICommand UserViewCommand { get; }
    public ICommand ManageViewCommand { get; }

    private readonly ContextNavigation _stockNav;
    
    public ViewModelBase CurrentViewModel => _stockNav.CurrentViewModel;
    
    private void OnCurrentViewModelChanged()
    {
        OnPropertyChanged(nameof(CurrentViewModel));
    }
}