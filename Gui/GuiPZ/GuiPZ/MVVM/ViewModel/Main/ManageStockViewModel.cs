using System.Collections.ObjectModel;
using System.Windows.Input;
using GuiPZ.Command;
using GuiPZ.Communicator.Client;
using GuiPZ.Container;
using GuiPZ.MVVM.Model;

namespace GuiPZ.MVVM.ViewModel.Main;

public class ManageStockViewModel : ViewModelBase
{
    public DataContainer _dataContainer;
    public DataExchanger _dataExchanger;
    
    public ICommand TrackCompanyCommand { get; }
    
    public ICommand UntrackCompanyCommand { get; }

    public ObservableCollection<Company> CompaniesToAdd => _dataContainer.CompaniesToAdd;
    
    public ObservableCollection<Company> TrackedCompanies => _dataContainer.TrackedCompanies;
    
    public ManageStockViewModel(DataContainer dataContainer, DataExchanger dataExchanger)
    {
        _dataContainer = dataContainer;
        _dataExchanger = dataExchanger;

        TrackCompanyCommand = new TrackCompanyCommand(this);
        UntrackCompanyCommand = new UntrackCompanyCommand(this);
    }
}