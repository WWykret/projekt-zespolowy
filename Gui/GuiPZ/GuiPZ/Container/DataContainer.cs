using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using GuiPZ.MVVM.Model;

namespace GuiPZ.Container;

public class DataContainer
{
    public ObservableCollection<Profile> Profiles { get; set; }
    public ObservableCollection<Company> Companies { get; set; }
    public List<Avatar> Avatars { get; set; }

    public DataContainer()
    {
        Profiles = new();
        Companies = new();
        Avatars = Directory.GetFiles("./Data/Images/Avatars")
            .Select(x => new Avatar()
                {Img = new Uri(string.Concat("pack://application:,,,/GuiPZ;component", x.AsSpan(1)))})
            .ToList();
    }

    public void AddProfile(Profile profile)
    {
        Profiles.Add(profile);
    }
}