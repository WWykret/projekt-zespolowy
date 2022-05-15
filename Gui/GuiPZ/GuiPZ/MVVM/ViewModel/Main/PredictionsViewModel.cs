using System.Collections.ObjectModel;
using GuiPZ.Command;
using GuiPZ.Communicator.Client;
using GuiPZ.Container;
using GuiPZ.MVVM.Model;

namespace GuiPZ.MVVM.ViewModel.Main;

public class PredictionsViewModel : ViewModelBase
{
    private DataContainer _dataContainer;
    private DataExchanger _dataExchanger;

    public ObservableCollection<Company> TrackedCompanies => _dataContainer.TrackedCompanies;
    
    public ObservableCollection<Company> VeryBad { get; set; }
    
    public ObservableCollection<Company> Bad { get; set; }
    
    public ObservableCollection<Company> Neutral { get; set; }
    
    public ObservableCollection<Company> Good { get; set; }
    
    public ObservableCollection<Company> VeryGood { get; set; }
    public PredictionsViewModel(DataContainer dataContainer, DataExchanger dataExchanger)
    {
        _dataContainer = dataContainer;
        _dataExchanger = dataExchanger;
        
        
    }
}