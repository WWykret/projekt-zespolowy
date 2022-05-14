using System.Collections.ObjectModel;
using GuiPZ.Communicator.Client;
using GuiPZ.Container;
using GuiPZ.MVVM.Model;

namespace GuiPZ.MVVM.ViewModel.Main;

public class PredictionsViewModel : ViewModelBase
{
    private DataContainer _dataContainer;
    private DataExchanger _dataExchanger;

    public ObservableCollection<Company> TrackedCompanies => _dataContainer.TrackedCompanies;
    public PredictionsViewModel(DataContainer dataContainer, DataExchanger dataExchanger)
    {
        _dataContainer = dataContainer;
        _dataExchanger = dataExchanger;
    }
}