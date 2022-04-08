using System.Windows.Input;
using GuiPZ.Command;
using GuiPZ.Navigation;

namespace GuiPZ.MVVM.ViewModel;

public class HomeViewModel : ViewModelBase
{
    public ICommand NavMainCommand { get; }

    public HomeViewModel(MainNav mainNav)
    {
        NavMainCommand = new NavMainCommand<LoginViewModel>(mainNav, () => new LoginViewModel(mainNav));
        
        _stockNav = new StockNav();
        _stockNav.CurrentViewModel = new PredictionsViewModel();
        _stockNav.CurrentViewModelChanged += OnCurrentViewModelChanged;

        PredictionsViewCommand = new NavStockCommand<PredictionsViewModel>(_stockNav, () => new PredictionsViewModel());
        UserViewCommand = new NavStockCommand<UserStockViewModel>(_stockNav, () => new UserStockViewModel());
        ManageViewCommand = new NavStockCommand<ManageStockViewModel>(_stockNav, () => new ManageStockViewModel());
    }

    public ICommand PredictionsViewCommand { get; }
    public ICommand UserViewCommand { get; }
    public ICommand ManageViewCommand { get; }

    private readonly StockNav _stockNav;
    
    public ViewModelBase CurrentViewModel => _stockNav.CurrentViewModel;
    
    private void OnCurrentViewModelChanged()
    {
        OnPropertyChanged(nameof(CurrentViewModel));
    }
}