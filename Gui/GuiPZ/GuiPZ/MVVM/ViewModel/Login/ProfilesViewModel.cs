using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using GuiPZ.Command;
using GuiPZ.Communicator.Client;
using GuiPZ.Container;
using GuiPZ.MVVM.Model;
using GuiPZ.MVVM.ViewModel.Main;
using GuiPZ.Navigation;
using GuiPZ.Utils;

namespace GuiPZ.MVVM.ViewModel.Login;

public class ProfilesViewModel : ViewModelBase
{
    private DataContainer _dataContainer;
    private DataExchanger _dataExchanger;
    
    public ICommand NavMainCommand { get; }
    public ICommand RegistrationViewCommand { get; }
    
    public ICommand DeleteProfileCommand { get; }

    public MTObservableCollection<Profile> Profiles => _dataContainer.Profiles;



    private Profile _selectedProfile;

    public Profile SelectedProfile
    {
        get => _selectedProfile;
        set
        {
            _selectedProfile = value;
            OnPropertyChanged(nameof(SelectedProfile));
        }
    }

    public ProfilesViewModel(ContextNavigation mainNav, ContextNavigation loginNav, DataContainer dataContainer, DataExchanger dataExchanger)
    {
        dataExchanger.DataLoaded += RefreshSelectedProfile;
        
        _dataContainer = dataContainer;
        _dataExchanger = dataExchanger;
        
        NavMainCommand = new NavCommand<ViewModelBase>(mainNav,() =>
        {
            if (_selectedProfile == null || _dataContainer.Companies == null || _dataContainer.Companies.Count == 0)
                return mainNav.CurrentViewModel;
            return new HomeViewModel(mainNav, SelectedProfile, _dataContainer, _dataExchanger);
        });
        
        RegistrationViewCommand =
            new NavCommand<RegistrationViewModel>(loginNav, () => new RegistrationViewModel(mainNav, loginNav, _dataContainer, _dataExchanger));

        DeleteProfileCommand = new DeleteProfileCommand(this);
        
        
        if (Profiles.Count > 0)
            SelectedProfile = Profiles.First();
        
    }

    public void RefreshSelectedProfile()
    {
        if (Profiles.Count > 0)
            SelectedProfile = Profiles.First();
    }

    public void DeleteProfile(Profile profile) => _dataExchanger.DeleteProfile(profile);
}