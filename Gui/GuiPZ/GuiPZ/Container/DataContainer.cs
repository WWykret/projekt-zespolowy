using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GuiPZ.MVVM.Model;

namespace GuiPZ.Container;

public class DataContainer
{
    public static List<Profile> Profiles { get; set; }
    
    public static List<Avatar> Avatars { get; set; }

    public static void InitializeContainers()
    {
        Profiles = new()
        {
            new Profile()
            {
                Name = "Joe",
                Img = new Uri("pack://application:,,,/GuiPz;component/Data/Images/Avatars/Nfd.png")
            },
            new Profile()
            {
                Name = "Joel",
                Img = new Uri("pack://application:,,,/GuiPZ;component/Data/Images/Avatars/Bakalar.png")
            }
        };

        Avatars = Directory.GetFiles("./Data/Images/Avatars")
            .Select(x => new Avatar() { Img = new Uri(string.Concat("pack://application:,,,/GuiPZ;component", x.AsSpan(1))) })
            .ToList();
    }

    public static void AddProfile(Profile profile)
    {
        Profiles.Add(profile);
    }
}