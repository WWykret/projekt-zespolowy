using System;
using System.Collections.Generic;

namespace GuiPZ.MVVM.Model;

public class Company
{
    public Company(string Name, string Code, string Link)
    {
        this.Name = Name;
        this.Code = Code;
        this.Link = new Uri(Link);
        this.Img = null;
    }

    public Company(string Name, string Code, Uri Link)
    {
        this.Name = Name;
        this.Code = Code;
        this.Link = Link;
        this.Img = null;
    }

    public Company()
    {
        Name = "";
        Code = "";
        Link = null;
        Img = null;
    }

    public string Name { get; set; }
    public string Code { get; set; }
    public Uri Link { get; set; }
    public List<Int32> Img { get; set; }
}