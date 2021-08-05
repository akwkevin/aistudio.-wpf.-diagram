using System;
using System.Collections.Generic;
using System.Text;

namespace Util.DiagramDesigner
{
    public interface IDrawModeViewModel
    {
        DrawMode GetDrawMode();
        void SetDrawMode(DrawMode drawMode);

        void ResetDrawMode();

        CursorMode CursorMode { get; set; }
        DrawMode VectorLineDrawMode { get; set; }
    }
}
