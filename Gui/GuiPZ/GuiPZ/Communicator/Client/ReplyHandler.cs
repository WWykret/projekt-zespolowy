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

        switch (code)
        {
            case "PRS":
                HandleProfile(rest);
                break;
        }
    }

    private void HandleProfile(String message)
    {
        var raw = JsonSerializer.Deserialize<List<string>>(message);

        if (raw is not null && raw.Count % 2 == 0)
        {
            var names = raw.ToList().Where((c, i) => i % 2 == 0).ToList();
            var images = raw.ToList().Where((c, i) => i % 2 != 0).ToList();

            for (int i = 0; i < names.Count; i++)
            {
                _dataContainer.Profiles.Add(new Profile()
                {
                    Name = names[i],
                    Img = new(images[i])
                });
            }
        }
    }
}