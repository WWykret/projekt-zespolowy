using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using GuiPZ.Command;
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
            RefreshPlot();
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

    public ICommand ChangePicCommand { get;  }

    public UserStockViewModel(DataContainer dataContainer, DataExchanger dataExchanger)
    {
        _dataContainer = dataContainer;
        _dataExchanger = dataExchanger;

        _dataExchanger.DataLoaded += RefreshPlot;

        _plotIndex = 0;

        ChangePicCommand = new ChangePicCommand(this);

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
    
    [MethodImpl(MethodImplOptions.Synchronized)]
    private void RefreshPlot()
    {
        if (SelectedCompany != null && SelectedCompany.Img != null && SelectedCompany.Img[0] != null)
        {
            Plot = ToImage(SelectedCompany.Img[PlotIndex].ToArray());
            OnPropertyChanged(nameof(Plot));
        }
        else
        {
            var plot = new BitmapImage(
                new Uri("pack://application:,,,/GuiPz;component/Data/Images/Assets/Placeholder.png"));
            plot.Freeze();
            Plot = plot;
            OnPropertyChanged(nameof(Plot));
        }
    }
    
    private static BitmapImage ToImage(byte[] data)
    {
        int w = (int) Math.Sqrt(data.Length / 3);
        int h = (int) Math.Sqrt(data.Length / 3);
        
        Bitmap pic = new Bitmap(w, h, PixelFormat.Format32bppArgb);
    
        for (int x = 0; x < w; x++)
        {
            for (int y = 0; y < h; y++)
            {
                int arrayIndex = 3 * y * w +  3 * x;
                Color c = Color.FromArgb(
                    255,
                    data[arrayIndex],
                    data[arrayIndex + 1],
                    data[arrayIndex + 2]
                );
                pic.SetPixel(x, y, c);
            }
        }

        using (var memory = new MemoryStream())
        {
            pic.Save(memory, ImageFormat.Png);
            memory.Position = 0;

            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = memory;
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.EndInit();
            bitmapImage.Freeze();

            return bitmapImage;
        }
    }
}