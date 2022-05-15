using System.Windows;
using GuiPZ.Communicator.Client;
using GuiPZ.MVVM.Model;
using GuiPZ.MVVM.ViewModel.Main;

namespace GuiPZ.Command;

public class TrackCompanyCommand : CommandBase
{
    private readonly ManageStockViewModel _data;

    public TrackCompanyCommand(ManageStockViewModel data)
    {
        _data = data;
    }
    
    public override void Execute(object? parameter)
    {
        var item = (Company) (parameter as FrameworkElement).DataContext;
        
        int index = _data._dataContainer.CompaniesToAdd.IndexOf(item);
        
        _data._dataContainer.CompaniesToAdd.RemoveAt(index);
        _data._dataExchanger.AddNewTrackedCompany(_data._dataContainer.CurrentProfile, item.Name);
        _data._dataContainer.TrackedCompanies.Add(item);
        
        _data._dataExchanger.GetImage(item);
    }
}