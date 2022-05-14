using System.Windows;
using GuiPZ.MVVM.Model;
using GuiPZ.MVVM.ViewModel.Main;

namespace GuiPZ.Command;

public class UntrackCompanyCommand : CommandBase
{
    private readonly ManageStockViewModel _data;

    public UntrackCompanyCommand(ManageStockViewModel data)
    {
        _data = data;
    }
    
    public override void Execute(object? parameter)
    {
        var item = (Company) (parameter as FrameworkElement).DataContext;

        int index = _data._dataContainer.TrackedCompanies.IndexOf(item);

        _data._dataContainer.TrackedCompanies.RemoveAt(index);
        _data._dataExchanger.RemoveTrackedCompany(_data._dataContainer.CurrentProfile, item.Name);
        _data._dataContainer.CompaniesToAdd.Add(item);
    }
}