using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using GuiPZ.Command;
using GuiPZ.Communicator.Client;
using GuiPZ.Container;
using GuiPZ.MVVM.Model;
using GuiPZ.Navigation;
using GuiPZ.MVVM.View.Login;

namespace GuiPZ.MVVM.ViewModel.Login;

public class RegistrationViewModel : ViewModelBase
{
    public DataContainer _dataContainer;
    private DataExchanger _dataExchanger;
    
    public void AddProfile(Profile profile) => _dataExchanger.CreateNewProfile(profile);
    public ICommand ProfilesViewCommand { get; }

    public ICommand AddProfileCommand { get; }

    public List<Avatar> Avatars => _dataContainer.Avatars;

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

    public RegistrationViewModel(ContextNavigation mainNav, ContextNavigation loginNav, DataContainer dataContainer, DataExchanger dataExchanger)
    {
        _dataContainer = dataContainer;
        _dataExchanger = dataExchanger;

        ProfilesViewCommand = new NavCommand<ProfilesViewModel>(loginNav, () => new ProfilesViewModel(mainNav, loginNav, _dataContainer, _dataExchanger));

        AddProfileCommand =
            new AddProfileCommand<ProfilesViewModel>(loginNav, () => new ProfilesViewModel(mainNav, loginNav, _dataContainer, _dataExchanger), this);
        
        
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
            OnPropertyChanged(nameof(TextColour));
        }
    }

    public string TextColour
    {
        get
        {
            if (_dataContainer.Profiles.Select(p => p.Name).Contains(_profileName))
            {
                return "Red";
            }

            return "White";
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