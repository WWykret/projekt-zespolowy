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
        
        CurrentProfile = new Profile()
        {
            Name = "Joe",
            Img = new Uri("pack://application:,,,/GuiPz;component/Data/Images/Avatars/Nfd.png")
        };

        Texto = "Joelgh";
        
        
        
        SelectedAvatar = Avatars.First();
    }

    private string _texto;

    public string Texto
    {
        get => _texto;
        set
        {
            _texto = value;
            OnPropertyChanged(nameof(Texto));

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