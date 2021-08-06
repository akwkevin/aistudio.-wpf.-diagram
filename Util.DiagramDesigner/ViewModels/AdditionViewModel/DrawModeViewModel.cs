using System;
using System.Collections.Generic;
using System.Text;

namespace Util.DiagramDesigner
{
    public class DrawModeViewModel : BindableBase, IDrawModeViewModel
    {
        public DrawMode GetDrawMode()
        {
            if (CursorDrawModeSelected)
            {
                return CursorDrawMode;
            }
            else if (VectorLineDrawModeSelected)
            {
                return VectorLineDrawMode;
            }
            else if (ShapeDrawModeSelected)
            {
                return ShapeDrawMode;
            }
            else if (TextDrawModeSelected)
            {
                return TextDrawMode;
            }

            return DrawMode.Normal;
        }

        public void ResetDrawMode()
        {
            CursorDrawModeSelected = true;
            CursorDrawMode = DrawMode.Normal;
        }

        public void SetDrawMode(DrawMode drawMode)
        {
            CursorDrawMode = drawMode;
        }

        private bool _cursordrawModeSelected = true;
        public bool CursorDrawModeSelected
        {
            get
            {
                return _cursordrawModeSelected;
            }
            set
            {
                SetProperty(ref _cursordrawModeSelected, value);
            }
        }

        private bool _vectorLineDrawModeSelected;
        public bool VectorLineDrawModeSelected
        {
            get
            {
                return _vectorLineDrawModeSelected;
            }
            set
            {
                SetProperty(ref _vectorLineDrawModeSelected, value);
            }
        }

        private bool _shapeDrawModeSelected;
        public bool ShapeDrawModeSelected
        {
            get
            {
                return _shapeDrawModeSelected;
            }
            set
            {
                SetProperty(ref _shapeDrawModeSelected, value);
            }
        }

        private bool _textDrawModeSelected;
        public bool TextDrawModeSelected
        {
            get
            {
                return _textDrawModeSelected;
            }
            set
            {
                SetProperty(ref _textDrawModeSelected, value);
            }
        }

        private DrawMode _cursordrawMode = DrawMode.Normal;
        public DrawMode CursorDrawMode
        {
            get
            {
                return _cursordrawMode;
            }
            set
            {
                SetProperty(ref _cursordrawMode, value);
                CursorDrawModeSelected = true;
            }
        }

        private DrawMode _vectorLineDrawMode = DrawMode.CornerConnectingLine;
        public DrawMode VectorLineDrawMode
        {
            get
            {
                return _vectorLineDrawMode;
            }
            set
            {
                SetProperty(ref _vectorLineDrawMode, value);
                VectorLineDrawModeSelected = true;
            }
        }

        private DrawMode _shapeDrawMode = DrawMode.Rectangle;
        public DrawMode ShapeDrawMode
        {
            get
            {
                return _shapeDrawMode;
            }
            set
            {
                SetProperty(ref _shapeDrawMode, value);
                ShapeDrawModeSelected = true;
            }
        }

        private DrawMode _textDrawMode = DrawMode.Text;
        public DrawMode TextDrawMode
        {
            get
            {
                return _textDrawMode;
            }
            set
            {
                SetProperty(ref _textDrawMode, value);
                TextDrawModeSelected = true;
            }
        }

        private CursorMode _cursorMode;
        public CursorMode CursorMode
        {
            get
            {
                return _cursorMode;
            }
            set
            {
                SetProperty(ref _cursorMode, value);
            }
        }

    }
}
