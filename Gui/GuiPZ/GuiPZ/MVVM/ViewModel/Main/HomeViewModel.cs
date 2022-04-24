using System.Windows.Input;
using GuiPZ.Command;
using GuiPZ.Communicator.Client;
using GuiPZ.Container;
using GuiPZ.MVVM.Model;
using GuiPZ.MVVM.ViewModel.Login;
using GuiPZ.Navigation;

namespace GuiPZ.MVVM.ViewModel.Main;

public class HomeViewModel : ViewModelBase
{
    private DataContainer _dataContainer;
    private DataExchanger _dataExchanger;
    public ICommand NavMainCommand { get; }

    private Profile Profile { get; set; }
    public HomeViewModel(ContextNavigation mainNav, Profile profile, DataContainer dataContainer, DataExchanger dataExchanger)
    {
        _dataContainer = dataContainer;
        _dataExchanger = dataExchanger;
        
        NavMainCommand = new NavCommand<LoginViewModel>(mainNav, () => new LoginViewModel(mainNav, _dataContainer, _dataExchanger));
        
        _stockNav = new ContextNavigation();
        _stockNav.CurrentViewModel = new PredictionsViewModel(_dataContainer, _dataExchanger);
        _stockNav.CurrentViewModelChanged += OnCurrentViewModelChanged;

        PredictionsViewCommand = new NavCommand<PredictionsViewModel>(_stockNav, () => new PredictionsViewModel(_dataContainer, _dataExchanger));
        UserViewCommand = new NavCommand<UserStockViewModel>(_stockNav, () => new UserStockViewModel(_dataContainer, _dataExchanger));
        ManageViewCommand = new NavCommand<ManageStockViewModel>(_stockNav, () => new ManageStockViewModel(_dataContainer, _dataExchanger));

        Profile = profile;
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