using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace Util.DiagramDesigner
{
    public interface IFontViewModel
    {
        string FontFamily { get; set; }
        Color FontColor { get; set; }
        double FontSize { get; set; }  
        Color TextEffectColor { get; set; }
        Color HighlightColor { get; set; }
        FontCase FontCase { get; set; }
        FontWeight FontWeight { get; set; }
        FontStyle FontStyle { get; set; }    
        FontStretch FontStretch { get; set; }
        bool Underline { get; set; }
        bool Strikethrough { get; set; }
        bool OverLine { get; set; }
        HorizontalAlignment HorizontalAlignment { get; set; }
        VerticalAlignment VerticalAlignment { get; set; }
        double LineHeight { get; set; }
        event PropertyChangedEventHandler PropertyChanged;

    }
}
