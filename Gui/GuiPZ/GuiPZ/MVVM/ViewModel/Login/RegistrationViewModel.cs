using System.Collections.Generic;
using System.Windows.Input;
using GuiPZ.Command;
using GuiPZ.Container;
using GuiPZ.MVVM.Model;
using GuiPZ.Navigation;

namespace GuiPZ.MVVM.ViewModel.Login;

public class RegistrationViewModel : ViewModelBase
{
    public ICommand ProfilesViewCommand { get; }


    public List<Avatar> DataLol => DataContainer.Avatars2;

    public RegistrationViewModel(ContextNavigation mainNav, ContextNavigation loginNav)
    {
        ProfilesViewCommand = new NavCommand<ProfilesViewModel>(loginNav, () => new ProfilesViewModel(mainNav, loginNav));
        DataContainer.SetAvatars();
    }
}