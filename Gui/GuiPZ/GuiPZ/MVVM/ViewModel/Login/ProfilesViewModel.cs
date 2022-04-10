using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using GuiPZ.Command;
using GuiPZ.Container;
using GuiPZ.MVVM.Model;
using GuiPZ.MVVM.ViewModel.Main;
using GuiPZ.Navigation;

namespace GuiPZ.MVVM.ViewModel.Login;

public class ProfilesViewModel : ViewModelBase
{
    public ICommand NavMainCommand { get; }
    public ICommand RegistrationViewCommand { get; }
    
    public ICommand DeleteProfileCommand { get; }

    public ObservableCollection<Profile> Profiles => DataContainer.Profiles;



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

    public ProfilesViewModel(ContextNavigation mainNav, ContextNavigation loginNav)
    {
        NavMainCommand = new NavCommand<HomeViewModel>(mainNav,() => new HomeViewModel(mainNav, SelectedProfile));
        
        RegistrationViewCommand =
            new NavCommand<RegistrationViewModel>(loginNav, () => new RegistrationViewModel(mainNav, loginNav));

        DeleteProfileCommand = new DeleteProfileCommand(this);
        
        SelectedProfile = Profiles.First();
        
    }
    
}