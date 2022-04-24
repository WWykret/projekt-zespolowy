using System;
using GuiPZ.MVVM.ViewModel;

namespace GuiPZ.Navigation;

public abstract class NavigationBase
{
    public event Action CurrentViewModelChanged;

    private ViewModelBase _currentViewModel;
    
    public ViewModelBase CurrentViewModel
    {
        get => _currentViewModel;
        set
        {
            _currentViewModel = value;
            OnCurrentViewModelChanged();
        }
    }
    private void OnCurrentViewModelChanged()
    {
        CurrentViewModelChanged?.Invoke();
    }
}