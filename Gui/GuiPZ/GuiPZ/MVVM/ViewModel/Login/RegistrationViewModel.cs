using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using GuiPZ.Command;
using GuiPZ.Container;
using GuiPZ.MVVM.Model;
using GuiPZ.Navigation;
using GuiPZ.MVVM.View.Login;

namespace GuiPZ.MVVM.ViewModel.Login;

public class RegistrationViewModel : ViewModelBase
{
    public ICommand ProfilesViewCommand { get; }

    public ICommand AddProfileCommand { get; }

    public List<Avatar> Avatars => DataContainer.Avatars;

    private Avatar _selectedAvatar;
    
    public Avatar SelectedAvatar
    {
        get => _selectedAvatar;
        set
        {
            _selectedAvatar = value;
            OnPropertyChanged(nameof(SelectedAvatar));
            
            CurrentProfile = new Profile()
            {
                Name = CurrentProfile.Name,
                Img = value.Img
            };
        }
    }

    public RegistrationViewModel(ContextNavigation mainNav, ContextNavigation loginNav)
    {
        ProfilesViewCommand = new NavCommand<ProfilesViewModel>(loginNav, () => new ProfilesViewModel(mainNav, loginNav));

        AddProfileCommand =
            new AddProfileCommand<ProfilesViewModel>(loginNav, () => new ProfilesViewModel(mainNav, loginNav), this);
        
        
        CurrentProfile = new Profile()
        {
            Name = null,
            Img = null
        };

        ProfileName = "NewProfile";
        
        
        
        SelectedAvatar = Avatars.First();
        
        
    }

    private string _profileName;

    public string ProfileName
    {
        get => _profileName;
        set
        {
            _profileName = value;
            OnPropertyChanged(nameof(ProfileName));

            CurrentProfile = new Profile()
            {
                Name = value,
                Img = CurrentProfile.Img
            };
        }
    }




    private Profile _currentProfile;

    public Profile CurrentProfile
    {
        get => _currentProfile;
        set
        {
            _currentProfile = value;
            OnPropertyChanged(nameof(CurrentProfile));
        }
    }
}