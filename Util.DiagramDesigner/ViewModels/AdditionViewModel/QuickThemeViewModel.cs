using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace Util.DiagramDesigner
{
    public class QuickThemeViewModel : BindableBase, IQuickThemeViewModel
    {
        public QuickTheme[] QuickThemes { get; } = new QuickTheme[] {
            new QuickTheme(){ ColorViewModel= new ColorViewModel(){LineColor =  new ColorObject() { Color = Color.FromArgb(0xFF,0x50,0x9D,0x73)}, FillColor =  new ColorObject() { Color = Colors.Transparent}, LineWidth = 1}, FontViewModel = new FontViewModel(){ FontColor = Colors.Black } },
            new QuickTheme(){ ColorViewModel= new ColorViewModel(){LineColor =  new ColorObject() { Color = Color.FromArgb(0xFF,0x51,0x76,0xAD)}, FillColor =  new ColorObject() { Color = Colors.Transparent}, LineWidth = 1}, FontViewModel = new FontViewModel(){ FontColor = Colors.Black } },
            new QuickTheme(){ ColorViewModel= new ColorViewModel(){LineColor =  new ColorObject() { Color = Color.FromArgb(0xFF,0x83,0x83,0x83)}, FillColor =  new ColorObject() { Color = Colors.Transparent}, LineWidth = 1}, FontViewModel = new FontViewModel(){ FontColor = Colors.Black } },
            new QuickTheme(){ ColorViewModel= new ColorViewModel(){LineColor =  new ColorObject() { Color = Color.FromArgb(0xFF,0x50,0x91,0xBA)}, FillColor =  new ColorObject() { Color = Colors.Transparent}, LineWidth = 1}, FontViewModel = new FontViewModel(){ FontColor = Colors.Black } },
            new QuickTheme(){ ColorViewModel= new ColorViewModel(){LineColor =  new ColorObject() { Color = Color.FromArgb(0xFF,0xC5,0x9C,0x50)}, FillColor =  new ColorObject() { Color = Colors.Transparent}, LineWidth = 1}, FontViewModel = new FontViewModel(){ FontColor = Colors.Black } },
            new QuickTheme(){ ColorViewModel= new ColorViewModel(){LineColor =  new ColorObject() { Color = Color.FromArgb(0xFF,0x7A,0xA6,0x59)}, FillColor =  new ColorObject() { Color = Colors.Transparent}, LineWidth = 1}, FontViewModel = new FontViewModel(){ FontColor = Colors.Black } },
            new QuickTheme(){ ColorViewModel= new ColorViewModel(){LineColor =  new ColorObject() { Color = Color.FromArgb(0xFF,0xC3,0x78,0x55)}, FillColor =  new ColorObject() { Color = Colors.Transparent}, LineWidth = 1}, FontViewModel = new FontViewModel(){ FontColor = Colors.Black } },
            new QuickTheme(){ ColorViewModel= new ColorViewModel(){LineColor =  new ColorObject() { Color = Color.FromArgb(0xFF,0xF5,0xC4,0xC5)}, FillColor =  new ColorObject() { Color = Colors.Transparent }, LineWidth = 1}, FontViewModel = new FontViewModel(){ FontColor = Colors.Black } },
                                                                                                           
            new QuickTheme(){ ColorViewModel= new ColorViewModel(){LineColor =  new ColorObject() { Color = Color.FromArgb(0xFF,0x50,0x9D,0x73)}, FillColor =  new ColorObject() { Color = Color.FromArgb(0xFF,0xE9,0xF4,0xED)}, LineWidth = 1}, FontViewModel = new FontViewModel(){ FontColor = Colors.Black } },
            new QuickTheme(){ ColorViewModel= new ColorViewModel(){LineColor =  new ColorObject() { Color = Color.FromArgb(0xFF,0x51,0x76,0xAD)}, FillColor =  new ColorObject() { Color = Color.FromArgb(0xFF,0xE9,0xED,0xF8)}, LineWidth = 1}, FontViewModel = new FontViewModel(){ FontColor = Colors.Black } },
            new QuickTheme(){ ColorViewModel= new ColorViewModel(){LineColor =  new ColorObject() { Color = Color.FromArgb(0xFF,0x83,0x83,0x83)}, FillColor =  new ColorObject() { Color = Color.FromArgb(0xFF,0xEF,0xEF,0xEF)}, LineWidth = 1}, FontViewModel = new FontViewModel(){ FontColor = Colors.Black } },
            new QuickTheme(){ ColorViewModel= new ColorViewModel(){LineColor =  new ColorObject() { Color = Color.FromArgb(0xFF,0x50,0x91,0xBA)}, FillColor =  new ColorObject() { Color = Color.FromArgb(0xFF,0xE7,0xF2,0xFC)}, LineWidth = 1}, FontViewModel = new FontViewModel(){ FontColor = Colors.Black } },
            new QuickTheme(){ ColorViewModel= new ColorViewModel(){LineColor =  new ColorObject() { Color = Color.FromArgb(0xFF,0xC5,0x9C,0x50)}, FillColor =  new ColorObject() { Color = Color.FromArgb(0xFF,0xFF,0xF4,0xE8)}, LineWidth = 1}, FontViewModel = new FontViewModel(){ FontColor = Colors.Black } },
            new QuickTheme(){ ColorViewModel= new ColorViewModel(){LineColor =  new ColorObject() { Color = Color.FromArgb(0xFF,0x7A,0xA6,0x59)}, FillColor =  new ColorObject() { Color = Color.FromArgb(0xFF,0xED,0xF6,0xEA)}, LineWidth = 1}, FontViewModel = new FontViewModel(){ FontColor = Colors.Black } },
            new QuickTheme(){ ColorViewModel= new ColorViewModel(){LineColor =  new ColorObject() { Color = Color.FromArgb(0xFF,0xC3,0x78,0x55)}, FillColor =  new ColorObject() { Color = Color.FromArgb(0xFF,0xFF,0xED,0xE9)}, LineWidth = 1}, FontViewModel = new FontViewModel(){ FontColor = Colors.Black } },
            new QuickTheme(){ ColorViewModel= new ColorViewModel(){LineColor =  new ColorObject() { Color = Color.FromArgb(0xFF,0xF5,0xC4,0xC5)}, FillColor =  new ColorObject() { Color = Color.FromArgb(0xFF,0xFA,0xE9,0xE9) }, LineWidth = 1}, FontViewModel = new FontViewModel(){ FontColor = Colors.Black } },
                                                                                                    
            new QuickTheme(){ ColorViewModel= new ColorViewModel(){LineColor =  new ColorObject() { Color = Color.FromArgb(0xFF,0xE9,0xF4,0xED)}, FillColor =  new ColorObject() { Color = Color.FromArgb(0xFF,0xE9,0xF4,0xED)}, LineWidth = 1}, FontViewModel = new FontViewModel(){ FontColor = Colors.Black } },
            new QuickTheme(){ ColorViewModel= new ColorViewModel(){LineColor =  new ColorObject() { Color = Color.FromArgb(0xFF,0xE9,0xED,0xF8)}, FillColor =  new ColorObject() { Color = Color.FromArgb(0xFF,0xE9,0xED,0xF8)}, LineWidth = 1}, FontViewModel = new FontViewModel(){ FontColor = Colors.Black } },
            new QuickTheme(){ ColorViewModel= new ColorViewModel(){LineColor =  new ColorObject() { Color = Color.FromArgb(0xFF,0xEF,0xEF,0xEF)}, FillColor =  new ColorObject() { Color = Color.FromArgb(0xFF,0xEF,0xEF,0xEF)}, LineWidth = 1}, FontViewModel = new FontViewModel(){ FontColor = Colors.Black } },
            new QuickTheme(){ ColorViewModel= new ColorViewModel(){LineColor =  new ColorObject() { Color = Color.FromArgb(0xFF,0xE7,0xF2,0xFC)}, FillColor =  new ColorObject() { Color = Color.FromArgb(0xFF,0xE7,0xF2,0xFC)}, LineWidth = 1}, FontViewModel = new FontViewModel(){ FontColor = Colors.Black } },
            new QuickTheme(){ ColorViewModel= new ColorViewModel(){LineColor =  new ColorObject() { Color = Color.FromArgb(0xFF,0xFF,0xF4,0xE8)}, FillColor =  new ColorObject() { Color = Color.FromArgb(0xFF,0xFF,0xF4,0xE8)}, LineWidth = 1}, FontViewModel = new FontViewModel(){ FontColor = Colors.Black } },
            new QuickTheme(){ ColorViewModel= new ColorViewModel(){LineColor =  new ColorObject() { Color = Color.FromArgb(0xFF,0xED,0xF6,0xEA)}, FillColor =  new ColorObject() { Color = Color.FromArgb(0xFF,0xED,0xF6,0xEA)}, LineWidth = 1}, FontViewModel = new FontViewModel(){ FontColor = Colors.Black } },
            new QuickTheme(){ ColorViewModel= new ColorViewModel(){LineColor =  new ColorObject() { Color = Color.FromArgb(0xFF,0xFF,0xED,0xE9)}, FillColor =  new ColorObject() { Color = Color.FromArgb(0xFF,0xFF,0xED,0xE9)}, LineWidth = 1}, FontViewModel = new FontViewModel(){ FontColor = Colors.Black } },
            new QuickTheme(){ ColorViewModel= new ColorViewModel(){LineColor =  new ColorObject() { Color = Color.FromArgb(0xFF,0xFA,0xE9,0xE9)}, FillColor =  new ColorObject() { Color = Color.FromArgb(0xFF,0xFA,0xE9,0xE9) }, LineWidth = 1}, FontViewModel = new FontViewModel(){ FontColor = Colors.Black } },
                                                                                                           
            new QuickTheme(){ ColorViewModel= new ColorViewModel(){LineColor =  new ColorObject() { Color = Color.FromArgb(0xFF,0x50,0x9D,0x73)}, FillColor =  new ColorObject() { Color = Color.FromArgb(0xFF,0xE9,0xF4,0xED)}, LineWidth = 2}, FontViewModel = new FontViewModel(){ FontColor = Colors.Black } },
            new QuickTheme(){ ColorViewModel= new ColorViewModel(){LineColor =  new ColorObject() { Color = Color.FromArgb(0xFF,0x51,0x76,0xAD)}, FillColor =  new ColorObject() { Color = Color.FromArgb(0xFF,0xE9,0xED,0xF8)}, LineWidth = 2}, FontViewModel = new FontViewModel(){ FontColor = Colors.Black } },
            new QuickTheme(){ ColorViewModel= new ColorViewModel(){LineColor =  new ColorObject() { Color = Color.FromArgb(0xFF,0x83,0x83,0x83)}, FillColor =  new ColorObject() { Color = Color.FromArgb(0xFF,0xEF,0xEF,0xEF)}, LineWidth = 2}, FontViewModel = new FontViewModel(){ FontColor = Colors.Black } },
            new QuickTheme(){ ColorViewModel= new ColorViewModel(){LineColor =  new ColorObject() { Color = Color.FromArgb(0xFF,0x50,0x91,0xBA)}, FillColor =  new ColorObject() { Color = Color.FromArgb(0xFF,0xE7,0xF2,0xFC)}, LineWidth = 2}, FontViewModel = new FontViewModel(){ FontColor = Colors.Black } },
            new QuickTheme(){ ColorViewModel= new ColorViewModel(){LineColor =  new ColorObject() { Color = Color.FromArgb(0xFF,0xC5,0x9C,0x50)}, FillColor =  new ColorObject() { Color = Color.FromArgb(0xFF,0xFF,0xF4,0xE8)}, LineWidth = 2}, FontViewModel = new FontViewModel(){ FontColor = Colors.Black } },
            new QuickTheme(){ ColorViewModel= new ColorViewModel(){LineColor =  new ColorObject() { Color = Color.FromArgb(0xFF,0x7A,0xA6,0x59)}, FillColor =  new ColorObject() { Color = Color.FromArgb(0xFF,0xED,0xF6,0xEA)}, LineWidth = 2}, FontViewModel = new FontViewModel(){ FontColor = Colors.Black } },
            new QuickTheme(){ ColorViewModel= new ColorViewModel(){LineColor =  new ColorObject() { Color = Color.FromArgb(0xFF,0xC3,0x78,0x55)}, FillColor =  new ColorObject() { Color = Color.FromArgb(0xFF,0xFF,0xED,0xE9)}, LineWidth = 2}, FontViewModel = new FontViewModel(){ FontColor = Colors.Black } },
            new QuickTheme(){ ColorViewModel= new ColorViewModel(){LineColor =  new ColorObject() { Color = Color.FromArgb(0xFF,0xF5,0xC4,0xC5)}, FillColor =  new ColorObject() { Color = Color.FromArgb(0xFF,0xFA,0xE9,0xE9)}, LineWidth = 2}, FontViewModel = new FontViewModel(){ FontColor = Colors.Black } },
                                                                                                               
            new QuickTheme(){ ColorViewModel= new ColorViewModel(){LineColor =  new ColorObject() { Color = Color.FromArgb(0xFF,0x1B,0x5C,0x3B)}, FillColor =  new ColorObject() { Color = Color.FromArgb(0xFF,0x1B,0x5C,0x3B)}, LineWidth = 1}, FontViewModel = new FontViewModel(){ FontColor = Colors.White } },
            new QuickTheme(){ ColorViewModel= new ColorViewModel(){LineColor =  new ColorObject() { Color = Color.FromArgb(0xFF,0x20,0x3D,0x68)}, FillColor =  new ColorObject() { Color = Color.FromArgb(0xFF,0x20,0x3D,0x68)}, LineWidth = 1}, FontViewModel = new FontViewModel(){ FontColor = Colors.White } },
            new QuickTheme(){ ColorViewModel= new ColorViewModel(){LineColor =  new ColorObject() { Color = Color.FromArgb(0xFF,0x47,0x47,0x47)}, FillColor =  new ColorObject() { Color = Color.FromArgb(0xFF,0x47,0x47,0x47)}, LineWidth = 1}, FontViewModel = new FontViewModel(){ FontColor = Colors.White } },
            new QuickTheme(){ ColorViewModel= new ColorViewModel(){LineColor =  new ColorObject() { Color = Color.FromArgb(0xFF,0x00,0x52,0x73)}, FillColor =  new ColorObject() { Color = Color.FromArgb(0xFF,0x00,0x52,0x73)}, LineWidth = 1}, FontViewModel = new FontViewModel(){ FontColor = Colors.White } },
            new QuickTheme(){ ColorViewModel= new ColorViewModel(){LineColor =  new ColorObject() { Color = Color.FromArgb(0xFF,0x7C,0x5B,0x0E)}, FillColor =  new ColorObject() { Color = Color.FromArgb(0xFF,0x7C,0x5B,0x0E)}, LineWidth = 1}, FontViewModel = new FontViewModel(){ FontColor = Colors.White } },
            new QuickTheme(){ ColorViewModel= new ColorViewModel(){LineColor =  new ColorObject() { Color = Color.FromArgb(0xFF,0x40,0x63,0x26)}, FillColor =  new ColorObject() { Color = Color.FromArgb(0xFF,0x40,0x63,0x26)}, LineWidth = 1}, FontViewModel = new FontViewModel(){ FontColor = Colors.White } },
            new QuickTheme(){ ColorViewModel= new ColorViewModel(){LineColor =  new ColorObject() { Color = Color.FromArgb(0xFF,0x7B,0x3F,0x23)}, FillColor =  new ColorObject() { Color = Color.FromArgb(0xFF,0x7B,0x3F,0x23)}, LineWidth = 1}, FontViewModel = new FontViewModel(){ FontColor = Colors.White } },
            new QuickTheme(){ ColorViewModel= new ColorViewModel(){LineColor =  new ColorObject() { Color = Color.FromArgb(0xFF,0x6E,0x1D,0x1E) }, FillColor =  new ColorObject() { Color = Color.FromArgb(0xFF,0x6E,0x1D,0x1E) }, LineWidth = 1}, FontViewModel = new FontViewModel(){ FontColor = Colors.White } },
        };


        private QuickTheme _quickTheme;
        public QuickTheme QuickTheme
        {
            get
            {
                return _quickTheme;
            }
            set
            {
                SetProperty(ref _quickTheme, value);
            }
        }
    }
    public class QuickTheme
    {
        public IColorViewModel ColorViewModel { get; set; }
        public IFontViewModel FontViewModel { get; set; }

        /// <summary>
        /// Gets or sets text
        /// </summary>
        public string Text { get; set; } = "Abc";

        /// <summary>
        /// Gets or sets group name
        /// </summary>
        public string Group { get; set; }
    }
}
