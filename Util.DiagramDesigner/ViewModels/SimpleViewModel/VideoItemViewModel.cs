using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Util.DiagramDesigner
{
    public class VideoItemViewModel : MediaItemViewModel
    {
        protected override string Filter { get; set; } = "视频|*.wmv;*.asf;*.asx;*.rm;*.rmvb;*.mp4;*.3gp;*.mov;*.m4v;*.avi;*.dat;*.mkv;*.flv;*.vob";

        public VideoItemViewModel() : base()
        {
        }

        public VideoItemViewModel(IDiagramViewModel parent, MediaDesignerItem designer) : base(parent, designer)
        {

        }               
    }
}
