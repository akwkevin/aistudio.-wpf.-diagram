using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace Util.DiagramDesigner
{
    public class StrokeDashArray
    {
        public static readonly List<DoubleCollection> Dash = new List<DoubleCollection>
        {
            new DoubleCollection() { 1, 0 },
            new DoubleCollection() { 8, 4 },
            new DoubleCollection() { 1, 4 },
            new DoubleCollection() { 8, 4, 2, 4 },
            new DoubleCollection() { 8, 4, 2, 4, 2, 4 },
            new DoubleCollection() { 8, 4, 8, 4, 2, 4 },
            new DoubleCollection() { 18, 4, 4, 4 },
            new DoubleCollection() { 18, 4, 4, 4, 4, 4 },
            new DoubleCollection() { 4, 4 },
            new DoubleCollection() { 2, 2 },
            new DoubleCollection() { 5, 2, 2, 2 },
            new DoubleCollection() { 5, 2, 2, 2, 2, 2 },
            new DoubleCollection() { 10, 4, 4, 4 },
            new DoubleCollection() { 10, 4, 4, 4, 4, 4 },
            new DoubleCollection() { 16, 8 },
            new DoubleCollection() { 2, 8 },
            new DoubleCollection() { 16, 8, 2, 8 },
            new DoubleCollection() { 16, 8, 2, 8, 2, 8 },
            new DoubleCollection() { 16, 8, 16, 8, 2, 8 },
            new DoubleCollection() { 40, 8, 8, 8 },
            new DoubleCollection() { 40, 8, 8, 8, 8, 8 },
            new DoubleCollection() { 4, 2 },
        };
    }
}
