using GuiPZ.Communicator.Client;
using GuiPZ.Container;

namespace GuiPZ.MVVM.ViewModel.Main;

public class ManageStockViewModel : ViewModelBase
{
    private DataContainer _dataContainer;
    private DataExchanger _dataExchanger;

    public ManageStockViewModel(DataContainer dataContainer, DataExchanger dataExchanger)
    {
        _dataContainer = dataContainer;
        _dataExchanger = dataExchanger;
    }
}