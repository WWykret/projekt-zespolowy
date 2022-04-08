using System;
using GuiPZ.MVVM.ViewModel;
using GuiPZ.Navigation;

namespace GuiPZ.Command;

public class NavStockCommand<T> : CommandBase where T : ViewModelBase
{
    private readonly StockNav _stockNav;
    private readonly Func<T> _createViewModel;

    public NavStockCommand(StockNav stockNav, Func<T> createViewModel)
    {
        _stockNav = stockNav;
        _createViewModel = createViewModel;
    }

    public override void Execute(object? parameter)
    {
        _stockNav.CurrentViewModel = _createViewModel();
    }
}