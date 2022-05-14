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
    
    public Profile CurrentProfile { get; set; }

    public ObservableCollection<Company> CompaniesToAdd { get; set; }
    
    public ObservableCollection<Company> TrackedCompanies { get; set; }

    public DataContainer()
    {
        Profiles = new();
        Companies = new();
        Avatars = new();
    }

    public void SetCompaniesToAdd(Profile profile)
    {
        CompaniesToAdd = new();
        
        foreach (var company in Companies)
        {
            if (!profile.TrackedCompanies.Contains(company.Name))
            {
                CompaniesToAdd.Add(company);
            }
        }
    }

    public void SetTrackedCompanies(Profile profile)
    {
        TrackedCompanies = new();
        
        foreach (var company in Companies)
        {
            if (profile.TrackedCompanies.Contains(company.Name))
            {
                TrackedCompanies.Add(company);
            }
        }
    }
}