using System;
using System.Collections.Generic;

namespace GuiPZ.MVVM.Model;

public class Profile
{
    public Profile(string Name, string Img, List<String> TrackedCompanies)
    {
        this.Name = Name;
        this.Img = new Uri(Img);
        this.TrackedCompanies = TrackedCompanies;
    }

    public Profile()
    {
        Name = "";
        Img = null;
        TrackedCompanies = new List<String>();
    }

    public string Name { get; set; }

    public List<String> TrackedCompanies { get; set; }
    public Uri Img { get; set; }
}