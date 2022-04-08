using System.Windows.Input;
using GuiPZ.Command;
using GuiPZ.Navigation;

namespace GuiPZ.MVVM.ViewModel;

public class ProfilesViewModel : ViewModelBase
{
    public ICommand NavMainCommand { get; }
    public ICommand RegistrationViewCommand { get; }

    public ProfilesViewModel(MainNav mainNav, LoginNav loginNav)
    {
        NavMainCommand = new NavMainCommand<HomeViewModel>(mainNav,() => new HomeViewModel(mainNav));
        
        RegistrationViewCommand =
            new NavLoginCommand<RegistrationViewModel>(loginNav, () => new RegistrationViewModel(mainNav, loginNav));
    }
}