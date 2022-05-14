using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace GuiPZ.MVVM.Model;

public class Company
{
    public Company(string Name, string Code, string Link)
    {
        this.Name = Name;
        this.Code = Code;
        this.Link = new Uri(Link);
        Img = null;
        this.Prediction = 0;
    }

    public Company(string Name, string Code, Uri Link)
    {
        this.Name = Name;
        this.Code = Code;
        this.Link = Link;
        Img = null;
        this.Prediction = 0;
    }

    public Company()
    {
        Name = "";
        Code = "";
        Link = null;
        Img = null;
        
        this.Prediction = 0;
    }

    public string Name { get; set; }
    public string Code { get; set; }
    public Uri Link { get; set; }
    public List<List<byte>>? Img { get; set; }
    
    public float Prediction { get; set; }
}