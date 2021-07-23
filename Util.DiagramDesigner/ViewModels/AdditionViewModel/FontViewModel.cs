using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

namespace Util.DiagramDesigner
{
    public class FontViewModel : BindableBase, IFontViewModel
    {
        #region 界面使用
        static FontViewModel()
        {
            var systemFontFamilies = new List<FontFamily>();
            foreach (FontFamily _f in Fonts.SystemFontFamilies)
            {
                LanguageSpecificStringDictionary _fontDic = _f.FamilyNames;
                if (_fontDic.ContainsKey(XmlLanguage.GetLanguage("zh-cn")))
                {
                    string _fontName = null;
                    if (_fontDic.TryGetValue(XmlLanguage.GetLanguage("zh-cn"), out _fontName))
                    {
                        systemFontFamilies.Add(new FontFamily(_fontName));
                    }
                }
                else
                {
                    string _fontName = null;
                    if (_fontDic.TryGetValue(XmlLanguage.GetLanguage("en-us"), out _fontName))
                    {
                        systemFontFamilies.Add(new FontFamily(_fontName));
                    }
                }
            }

            FontFamilys = systemFontFamilies.Select(fontFamily => fontFamily.ToString()).ToArray();
        }
        public static string[] FontFamilys { get; }
        public static double[] FontSizes { get; } = new double[] { 7, 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 28, 32, 36, 48, 72 };
        public static FontCase[] FontCases { get; } = new FontCase[] { FontCase.None, FontCase.Upper, FontCase.Lower };
        public static Color[] FontColors { get; } = new Color[] { Colors.Red, Colors.Green, Colors.Blue, Colors.White, Colors.Black, Colors.Purple };

        public ICommand GrowFontCommand
        {
            get
            {
                return new SimpleCommand(para =>
                {
                    if (FontSize < 72)
                        FontSize++;
                });
            }
        }

        public ICommand ShrinkFontCommand
        {
            get
            {
                return new SimpleCommand(para =>
                {
                    if (FontSize > 1)
                        FontSize--;
                });
            }
        }

        public ICommand ClearFormattingCommand
        {
            get
            {
                return new SimpleCommand(para =>
                {
                    FontFamily = "Arial";
                    FontSize = 12;
                    FontColor = Colors.Black;
                    FontWeight = FontWeights.Regular;
                    FontStyle = FontStyles.Normal;
                    FontStretch = FontStretches.Normal;
                    Underline = false;
                    Strikethrough = false;
                    OverLine = false;
                    TextEffectColor = Colors.Transparent;
                    HighlightColor = Colors.Transparent;
                    FontCase = FontCase.None;
                    HorizontalAlignment = HorizontalAlignment.Center;
                    VerticalAlignment = VerticalAlignment.Center;
                    LineHeight = 0;
                });
            }
        }

        public ICommand TextEffectColorCommand
        {
            get
            {
                return new SimpleCommand(para =>
                {
                    TextEffectColor = (Color)para;
                });
            }
        }

        public ICommand HighlightColorCommand
        {
            get
            {
                return new SimpleCommand(para =>
                {
                    HighlightColor = (Color)para;
                });
            }
        }

        public ICommand FontColorCommand
        {
            get
            {
                return new SimpleCommand(para =>
                {
                    FontColor = (Color)para;
                });
            }
        }

        private HorizontalVerticalAlignment _horizontalVerticalAlignment = HorizontalVerticalAlignment.CenterAlignCenter;
        public HorizontalVerticalAlignment HorizontalVerticalAlignment
        {
            get
            {
                return _horizontalVerticalAlignment;
            }
            set
            {
                if (SetProperty(ref _horizontalVerticalAlignment, value))
                {
                    _horizontalVerticalAlignment = value;
                    switch (value)
                    {
                        case HorizontalVerticalAlignment.TopAlignLeft:
                            HorizontalAlignment = HorizontalAlignment.Left;
                            VerticalAlignment = VerticalAlignment.Top;
                            break;
                        case HorizontalVerticalAlignment.TopAlignCenter:
                            HorizontalAlignment = HorizontalAlignment.Center;
                            VerticalAlignment = VerticalAlignment.Top;
                            break;
                        case HorizontalVerticalAlignment.TopAlignRight:
                            HorizontalAlignment = HorizontalAlignment.Right;
                            VerticalAlignment = VerticalAlignment.Top;
                            break;
                        case HorizontalVerticalAlignment.TopAlignJustify:
                            HorizontalAlignment = HorizontalAlignment.Stretch;
                            VerticalAlignment = VerticalAlignment.Top;
                            break;

                        case HorizontalVerticalAlignment.CenterAlignLeft:
                            HorizontalAlignment = HorizontalAlignment.Left;
                            VerticalAlignment = VerticalAlignment.Center;
                            break;
                        case HorizontalVerticalAlignment.CenterAlignCenter:
                            HorizontalAlignment = HorizontalAlignment.Center;
                            VerticalAlignment = VerticalAlignment.Center;
                            break;
                        case HorizontalVerticalAlignment.CenterAlignRight:
                            HorizontalAlignment = HorizontalAlignment.Right;
                            VerticalAlignment = VerticalAlignment.Center;
                            break;
                        case HorizontalVerticalAlignment.CenterAlignJustify:
                            HorizontalAlignment = HorizontalAlignment.Stretch;
                            VerticalAlignment = VerticalAlignment.Center;
                            break;

                        case HorizontalVerticalAlignment.BottomAlignLeft:
                            HorizontalAlignment = HorizontalAlignment.Left;
                            VerticalAlignment = VerticalAlignment.Bottom;
                            break;
                        case HorizontalVerticalAlignment.BottomAlignCenter:
                            HorizontalAlignment = HorizontalAlignment.Center;
                            VerticalAlignment = VerticalAlignment.Bottom;
                            break;
                        case HorizontalVerticalAlignment.BottomAlignRight:
                            HorizontalAlignment = HorizontalAlignment.Right;
                            VerticalAlignment = VerticalAlignment.Bottom;
                            break;
                        case HorizontalVerticalAlignment.BottomAlignJustify:
                            HorizontalAlignment = HorizontalAlignment.Stretch;
                            VerticalAlignment = VerticalAlignment.Bottom;
                            break;
                    }
                }
            }
        }
        #endregion

        private string _fontFamily = "Arial";
        public string FontFamily
        {
            get
            {
                return _fontFamily;
            }
            set
            {
                if (!SetProperty(ref _fontFamily, value))
                {
                    RaisePropertyChanged(nameof(FontFamily));
                }
            }
        }

        private double _fontSize = 12;
        public double FontSize
        {
            get
            {
                return _fontSize;
            }
            set
            {
                if (!SetProperty(ref _fontSize, value))
                {
                    RaisePropertyChanged(nameof(FontSize));
                }

            }
        }

        private Color _fontColor = Colors.Black;
        public Color FontColor
        {
            get
            {
                return _fontColor;
            }
            set
            {
                if (!SetProperty(ref _fontColor, value))
                {
                    RaisePropertyChanged(nameof(FontColor));
                }
            }
        }

        private FontWeight _fontWeight = FontWeights.Regular;

        public FontWeight FontWeight
        {
            get
            {
                return _fontWeight;
            }
            set
            {
                if (!SetProperty(ref _fontWeight, value))
                {
                    RaisePropertyChanged(nameof(FontWeight));
                }
            }
        }

        private FontStyle _fontStyle = FontStyles.Normal;
        public FontStyle FontStyle
        {
            get
            {
                return _fontStyle;
            }
            set
            {
                if (!SetProperty(ref _fontStyle, value))
                {
                    RaisePropertyChanged(nameof(FontStyle));
                }
            }
        }

        private FontStretch _fontStretch = FontStretches.Normal;

        public FontStretch FontStretch
        {
            get
            {
                return _fontStretch;
            }
            set
            {
                if (!SetProperty(ref _fontStretch, value))
                {
                    RaisePropertyChanged(nameof(FontStretch));
                }
            }
        }

        private bool _underline;
        public bool Underline
        {
            get
            {
                return _underline;
            }
            set
            {
                if (SetProperty(ref _underline, value))
                {
                    RaisePropertyChanged(nameof(TextDecorations));
                }
                else
                {
                    RaisePropertyChanged(nameof(Underline));
                }
            }
        }

        private bool _strikethrough;
        public bool Strikethrough
        {
            get
            {
                return _strikethrough;
            }
            set
            {
                if (SetProperty(ref _strikethrough, value))
                {
                    RaisePropertyChanged(nameof(TextDecorations));
                }
                else
                {
                    RaisePropertyChanged(nameof(Strikethrough));
                }
            }
        }

        private bool _overLine;
        public bool OverLine
        {
            get
            {
                return _overLine;
            }
            set
            {
                if (SetProperty(ref _overLine, value))
                {
                    RaisePropertyChanged(nameof(TextDecorations));
                }
                else
                {
                    RaisePropertyChanged(nameof(OverLine));
                }
            }
        }

        private TextDecorationCollection _textDecorations;

        public TextDecorationCollection TextDecorations
        {
            get
            {
                _textDecorations = new TextDecorationCollection();
                if (Underline)
                {
                    _textDecorations.Add(System.Windows.TextDecorations.Underline);
                }
                if (Strikethrough)
                {
                    _textDecorations.Add(System.Windows.TextDecorations.Strikethrough);
                }
                if (OverLine)
                {
                    _textDecorations.Add(System.Windows.TextDecorations.OverLine);
                }
                return _textDecorations;
            }
            //set
            //{               
            //    NotifyChanged(nameof(TextDecorations));
            //}
        }

        private Color _textEffectColor = Colors.Transparent;
        public Color TextEffectColor
        {
            get { return _textEffectColor; }

            set
            {
                if (!SetProperty(ref _textEffectColor, value))
                {
                    RaisePropertyChanged(nameof(TextEffectColor));
                }
            }
        }

        private Color _highlightColor = Colors.Transparent;
        public Color HighlightColor
        {
            get { return _highlightColor; }

            set
            {
                if (!SetProperty(ref _highlightColor, value))
                {
                    RaisePropertyChanged(nameof(HighlightColor));
                }
            }
        }

        private FontCase _fontCase = FontCase.None;
        public FontCase FontCase
        {
            get
            {
                return _fontCase;
            }
            set
            {
                if (!SetProperty(ref _fontCase, value))
                {
                    RaisePropertyChanged(nameof(FontCase));
                }
            }
        }

        private HorizontalAlignment _horizontalAlignment = HorizontalAlignment.Center;
        public HorizontalAlignment HorizontalAlignment
        {
            get
            {
                return _horizontalAlignment;
            }
            set
            {
                if (!SetProperty(ref _horizontalAlignment, value))
                {
                    RaisePropertyChanged(nameof(HorizontalAlignment));
                }
            }
        }

        private VerticalAlignment _verticalAlignment = VerticalAlignment.Center;
        public VerticalAlignment VerticalAlignment
        {
            get
            {
                return _verticalAlignment;
            }
            set
            {
                if (!SetProperty(ref _verticalAlignment, value))
                {
                    RaisePropertyChanged(nameof(VerticalAlignment));
                }
            }
        }

        private double _lineHeight;
        public double LineHeight
        {
            get
            {
                return _lineHeight;
            }
            set
            {
                if (!SetProperty(ref _lineHeight, value))
                {
                    RaisePropertyChanged(nameof(LineHeight));
                }
            }
        }

    }
}
