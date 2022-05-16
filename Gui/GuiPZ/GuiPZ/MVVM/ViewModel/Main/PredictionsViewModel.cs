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

    //public ObservableCollection<Company> TrackedCompanies => _dataContainer.TrackedCompanies;
    
    public ObservableCollection<Company> VeryBad { get; set; }
    
    public ObservableCollection<Company> Bad { get; set; }
    
    public ObservableCollection<Company> Neutral { get; set; }
    
    public ObservableCollection<Company> Good { get; set; }
    
    public ObservableCollection<Company> VeryGood { get; set; }
    
    public ObservableCollection<Company> Unloaded { get; set; }
    public PredictionsViewModel(DataContainer dataContainer, DataExchanger dataExchanger)
    {
        _dataContainer = dataContainer;
        _dataExchanger = dataExchanger;
        
        RefreshData();

        _dataExchanger.DataLoaded += RefreshData;
    }

    private void RefreshData()
    {
        VeryBad = new();
        Bad = new();
        Neutral = new();
        Good = new();
        VeryGood = new();
        Unloaded = new();
        
        foreach (var company in _dataContainer.TrackedCompanies)
        {
            if (company.Img == null)
            {
                Unloaded.Add(company);
            }
            else
            {
                switch(company.Prediction)
                {
                    case <= -0.05F:
                        VeryBad.Add(company);
                        break;
                    case > -0.05F and <= -0.005F:
                        Bad.Add(company);
                        break;
                    case > -0.005F and <= 0.005F:
                        Neutral.Add(company);
                        break;
                    case > 0.005F and <= 0.05F:
                        Neutral.Add(company);
                        break;
                    default:
                        VeryGood.Add(company);
                        break;
                }
            }
            OnPropertyChanged(nameof(Unloaded));
            OnPropertyChanged(nameof(VeryBad));
            OnPropertyChanged(nameof(Bad));
            OnPropertyChanged(nameof(Neutral));
            OnPropertyChanged(nameof(Good));
            OnPropertyChanged(nameof(VeryGood));
        }
    }
}