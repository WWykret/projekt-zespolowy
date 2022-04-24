using System;

namespace GuiPZ.MVVM.Model;

public interface ICloseWindow
{
    Action Close { get; set; }
}