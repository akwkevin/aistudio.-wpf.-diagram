using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Util.DiagramDesigner
{
    public enum PageSizeType
    {

        [Description("Letter,8.5英寸*11英寸")]//612*792
        Letter,
        [Description("Folio,8.5英寸*13英寸")]//612*936
        Folio,
        [Description("Folio,8.5英寸*14英寸")]//612*1008
        Legal,
        [Description("Folio,7.25英寸*10.5英寸")]//522*756
        Executive,
        [Description("Folio,5.5英寸*8.5英寸")]//396*612
        Statement,
        [Description("#10 Envelope,4.125英寸*9.5英寸")]//297*684
        Envelope,
        [Description("Monarch Envelope,3.875英寸*7.5英寸")]//279*540
        MonarchEnvelope,
        [Description("Tabloid,11英寸*17英寸")]//792*1224
        Tabloid,
        [Description("Letter Small,8 1/2英寸*11英寸")]//612*792
        LetterSmall,
        [Description("C Sheet,17英寸*22英寸")]//1224*1584
        CSheet,
        [Description("D Sheet,22英寸*34英寸")]//1584*2448
        DSheet,
        [Description("E Sheet,34英寸*44英寸")]//2448*3168
        ESheet,
        [Description("A3 sheet,297毫米*420毫米")]//842*1191
        A3,
        [Description("A4 sheet,210毫米*297毫米")]//595*842
        A4,
        [Description("A5 sheet,148毫米*210毫米")]//420*595
        A5,
        [Description("B4 sheet,250毫米*354毫米")]//709*1003
        B4,
        [Description("B5 sheet,182毫米*257毫米")]//516*729
        B5,
        [Description("DL Envelope,110毫米*220毫米")]//312*624
        DLEnvelope,
        [Description("C5 Envelope,162毫米*229毫米")]//459*649
        C5Envelope,
        [Description("Quarto,215毫米*275毫米")]//609*780
        Quarto,
        [Description("C6 Quarto,114毫米*162毫米")]//323*459
        C6Quarto,
        [Description("B5 Quarto,176毫米*250毫米")]//499*709
        B5Quarto,
        [Description("Italy Quarto,110毫米*230毫米")]//312*652
        ItalyQuarto,
        [Description("A4 small sheet,210毫米*297毫米")]//595*842
        A4small,
        [Description("German Std Fanfold,8.5英寸*12英寸")]//612*864
        GermanStdFanfold,
        [Description("German Lagal Fanfold,8英寸*13英寸")]//576*936
        GermanLegalFanfold,
        [Description("PRC 16K,146毫米*215毫米")]//414*609
        PRC16K,
        [Description("PRC 32K,97毫米*151毫米")]//275*428
        PRC32K,
        //[Description("Japanese Postcard")]//283*420
        //JapanesePostcard,
        //[Description("Double Japanese Postcard")]//420*566
        //DoubleJapanesePostcard,
        [Description("自定义")]
        Custom,
    }
}
