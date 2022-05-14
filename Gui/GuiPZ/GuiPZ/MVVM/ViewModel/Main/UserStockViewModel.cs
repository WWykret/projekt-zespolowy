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
    
    private Company _selectedCompany;

    public Company SelectedCompany
    {
        get => _selectedCompany;
        set
        {
            _selectedCompany = value;
            OnPropertyChanged(nameof(SelectedCompany));
        }
    }
    
    public BitmapImage Plot { get; set; }
    
    public UserStockViewModel(DataContainer dataContainer, DataExchanger dataExchanger)
    {
        _dataContainer = dataContainer;
        _dataExchanger = dataExchanger;

        if (TrackedCompanies.Count > 0)
        {
            SelectedCompany = TrackedCompanies[0];
            
            Plot = new BitmapImage(
                new Uri("pack://application:,,,/GuiPz;component/Data/Images/Assets/Placeholder.png"));
            
            if (SelectedCompany.Img != null && SelectedCompany.Img[0] != null)
            {
                var temp = new List<BitmapImage>();
                for (int i = 0; i < 4; i++)
                {
                    temp.Add(new BitmapImage(new Uri("pack://application:,,,/GuiPz;component/Data/Images/Assets/Placeholder.png")));
                }

                //SelectedCompany.Img = temp;

                
            }
                
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