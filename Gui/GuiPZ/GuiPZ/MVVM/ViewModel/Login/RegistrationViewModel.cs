using System.Collections.Generic;
using System.Windows.Input;
using GuiPZ.Command;
using GuiPZ.Navigation;

namespace GuiPZ.MVVM.ViewModel;

public class RegistrationViewModel : ViewModelBase
{
    public ICommand ProfilesViewCommand { get; }
    
    public List<string> DataLol { get; set; }

    public RegistrationViewModel(MainNav mainNav, LoginNav loginNav)
    {
        ProfilesViewCommand = new NavLoginCommand<ProfilesViewModel>(loginNav, () => new ProfilesViewModel(mainNav, loginNav));

        DataLol = new List<string>()
        {
            "Joe",
            "Xina"
        };
    }
}