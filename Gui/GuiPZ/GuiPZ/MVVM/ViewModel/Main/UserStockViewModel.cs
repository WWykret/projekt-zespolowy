using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using GuiPZ.Communicator.Client;
using GuiPZ.Container;
using GuiPZ.MVVM.Model;

namespace GuiPZ.MVVM.ViewModel.Main;

public class UserStockViewModel : ViewModelBase
{
    private DataContainer _dataContainer;
    private DataExchanger _dataExchanger;

    
    public ObservableCollection<Company> TrackedCompanies => _dataContainer.TrackedCompanies;
    
    private Company? _selectedCompany;

    public Company? SelectedCompany
    {
        get => _selectedCompany;
        set
        {
            _selectedCompany = value;
            OnPropertyChanged(nameof(SelectedCompany));
        }
    }
    
    public BitmapImage Plot { get; set; }

    private int _plotIndex;

    public int PlotIndex
    {
        get => _plotIndex;
        set
        {
            _plotIndex = value;
            RefreshPlot();
        }
    }

    public UserStockViewModel(DataContainer dataContainer, DataExchanger dataExchanger)
    {
        _dataContainer = dataContainer;
        _dataExchanger = dataExchanger;

        _dataExchanger.DataLoaded += RefreshPlot;

        _plotIndex = 0;

        if (TrackedCompanies.Count > 0)
        {
            SelectedCompany = TrackedCompanies[0];
            
            if (SelectedCompany.Img != null && SelectedCompany.Img[0] != null)
            {
                Plot = ToImage(SelectedCompany.Img[PlotIndex].ToArray());
            }
            else
            {
                Plot = new BitmapImage(
                    new Uri("pack://application:,,,/GuiPz;component/Data/Images/Assets/Placeholder.png"));
            }
                
        }
        
    }

    private void RefreshPlot()
    {
        if (SelectedCompany != null && SelectedCompany.Img != null)
        {
            Plot = ToImage(SelectedCompany.Img[PlotIndex].ToArray());
            OnPropertyChanged(nameof(Plot));
        }
    }
    
    private BitmapImage ToImage(byte[] array)
    {
        using (var ms = new System.IO.MemoryStream(array))
        {
            var image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad; // here
            image.StreamSource = ms;
            image.EndInit();
            return image;
        }
    }
}