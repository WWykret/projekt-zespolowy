using System;
using System.Linq;
using System.Windows;
using GuiPZ.MVVM.Model;
using GuiPZ.MVVM.ViewModel;
using GuiPZ.MVVM.ViewModel.Login;

namespace GuiPZ.Command;

public class DeleteProfileCommand : CommandBase
{
    private readonly ProfilesViewModel _viewModel;

    public DeleteProfileCommand(ProfilesViewModel viewModel)
    {
        _viewModel = viewModel;
    }
    
    
    public override void Execute(object? parameter)
    {
        var item = (Profile) (parameter as FrameworkElement).DataContext;

        int index = _viewModel.Profiles.IndexOf(item);

        if (_viewModel.Profiles.Count > 1)
        {
            if (_viewModel.SelectedProfile == item)
            {
                _viewModel.DeleteProfile(item);
                _viewModel.SelectedProfile = _viewModel.Profiles.First();
            }
            else
            {
                _viewModel.DeleteProfile(item);
            }
            
        }
            
        


    }
}