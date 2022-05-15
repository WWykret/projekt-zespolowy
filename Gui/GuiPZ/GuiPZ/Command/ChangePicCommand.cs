using GuiPZ.MVVM.ViewModel.Main;

namespace GuiPZ.Command;

public class ChangePicCommand : CommandBase
{
    private readonly UserStockViewModel _data;

    public ChangePicCommand(UserStockViewModel data)
    {
        _data = data;
    }
    
    public override void Execute(object? parameter)
    {
        int index = int.Parse((string) parameter);

        _data.PlotIndex = index;
    }
}