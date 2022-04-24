using System;
using GuiPZ.Communicator.Client;
using GuiPZ.Container;
using GuiPZ.MVVM.Model;
using GuiPZ.MVVM.ViewModel.Login;
using GuiPZ.Navigation;
using Microsoft.Toolkit.Mvvm.Input;

namespace GuiPZ.MVVM.ViewModel;

public class MainViewModel : ViewModelBase, ICloseWindow
{
    public RelayCommand CloseWindowCommand { get; set; }

    public Action Close { get; set; }
    private void CloseWindow()
    {
        _dataExchanger.SaveData();
        Close.Invoke();
    }

    private readonly ContextNavigation _mainNav;
    private DataContainer _dataContainer;
    private DataExchanger _dataExchanger;

    public ViewModelBase CurrentViewModel => _mainNav.CurrentViewModel;

    public MainViewModel()
    {
        CloseWindowCommand = new RelayCommand(CloseWindow);
        
        _dataContainer = new DataContainer();
        
        _dataExchanger = new DataExchanger(_dataContainer);
        _dataExchanger.InitializeData();

        _mainNav = new ContextNavigation();
        _mainNav.CurrentViewModel = new LoginViewModel(_mainNav, _dataContainer, _dataExchanger);
        _mainNav.CurrentViewModelChanged += OnCurrentViewModelChanged;
    }
    
    private void OnCurrentViewModelChanged()
    {
        OnPropertyChanged(nameof(CurrentViewModel));
    }
}