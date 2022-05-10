using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text.Json;
using GuiPZ.Container;
using GuiPZ.MVVM.Model;

namespace GuiPZ.Communicator.Client;

public class ReplyHandler
{
    private DataContainer _dataContainer;

    public ReplyHandler(DataContainer dataContainer)
    {
        _dataContainer = dataContainer;
    }

    public void Handle(String message)
    {
        var code = message.Substring(0, 3);
        var rest = message.Substring(4);

        if (code.Equals("SND"))
        {
            HandleSend(rest);
        }
    }

    public void HandleSend(String message)
    {
        var code = message.Substring(0, 3);
        var rest = message.Substring(4);

        if (code.Equals("PRS"))
        {
            HandleProfile(rest);
        }
        else if (code.Equals("CMP"))
        {
            Handlecompany(rest);
        }
        else if (code.Equals("IMG"))
        {
            HandleImage(rest);
        }
        //else if (code.Equals("TCP"))
        // {
        //     HandleTrackedcompany(rest);
        // }
    }
    // private void HandleProfile(String message)
    // {
    //     var raw = JsonSerializer.Deserialize<List<string>>(message);
    //
    //     if (raw is not null && raw.Count % 2 == 0)
    //     {
    //         var names = raw.ToList().Where((c, i) => i % 2 == 0).ToList();
    //         var images = raw.ToList().Where((c, i) => i % 2 != 0).ToList();
    //
    //         for (int i = 0; i < names.Count; i++)
    //         {
    //             _dataContainer.Profiles.Add(new Profile()
    //             {
    //                 Name = names[i],
    //                 Img = new(images[i])
    //             });
    //         }
    //     }
    // }

    private void HandleProfile(String message)
    {
        List<Profile> profiles = JsonSerializer.Deserialize<List<Profile>>(message);
        _dataContainer.Profiles.Clear();
        for (int i = 0; i < profiles.Count; i++)
        {
            _dataContainer.Profiles.Add(profiles[i]);
        }
    }

    private void Handlecompany(String message)
    {
        List<Company> companies = JsonSerializer.Deserialize<List<Company>>(message);
        _dataContainer.Companies.Clear();
        for (int i = 0; i < companies.Count; i++)
        {
            _dataContainer.Companies.Add(companies[i]);
        }
    }

    private void HandleImage(String message)
    {
        string company_name = message.Substring(0, message.IndexOf(':'));
        List<Int32> company_img = JsonSerializer.Deserialize<List<Int32>>(message.Substring(message.IndexOf(':') + 1));
        var com = _dataContainer.Companies.First(x => x.Name.Equals(company_name));
        com.Img = company_img;
    }

    // private void HandleTrackedcompany(String message)
    // {
    //     Company company = JsonSerializer.Deserialize<Company>(message);
    //     var com = _dataContainer.Companies.First(x => x.Name.Equals(company.Name));
    //     com.Img=company.Img;
    // }
}