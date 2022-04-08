using System.Collections.Generic;
using System.Windows.Input;
using GuiPZ.Command;
using GuiPZ.Navigation;

namespace GuiPZ.MVVM.ViewModel.Login;

public class RegistrationViewModel : ViewModelBase
{
    public ICommand ProfilesViewCommand { get; }
    
    public List<string> DataLol { get; set; }

    public RegistrationViewModel(ContextNavigation mainNav, ContextNavigation loginNav)
    {
        ProfilesViewCommand = new NavCommand<ProfilesViewModel>(loginNav, () => new ProfilesViewModel(mainNav, loginNav));

        DataLol = new List<string>()
        {
            "Joe",
            "Xina"
        };
    }
}