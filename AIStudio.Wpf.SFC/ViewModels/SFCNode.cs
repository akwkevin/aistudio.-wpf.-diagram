using AIStudio.Wpf.BaseDiagram;
using AIStudio.Wpf.BaseDiagram.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Util.DiagramDesigner;

namespace AIStudio.Wpf.SFC.ViewModels
{
    public class SFCNode : DesignerItemViewModelBase
    {
        protected IUIVisualizerService visualiserService;

        public SFCNode(SFCNodeKinds kind) : base()
        {
            Kind = kind;
            ItemWidth = 80;
            ItemHeight = 40;

        }

        public SFCNode(IDiagramViewModel parent, DesignerItemBase designer) : base(parent, designer)
        {

        }

        protected override void Init()
        {
            base.Init();

            ShowRotate = false;
            ShowText = true;
            IsReadOnlyText = true;

            visualiserService = ApplicationServicesProvider.Instance.Provider.VisualizerService;
        }

        protected override void LoadDesignerItemViewModel(IDiagramViewModel parent, SelectableDesignerItemBase designerbase)
        {
            base.LoadDesignerItemViewModel(parent, designerbase);

        }

        [Browsable(false)]
        public SFCNodeKinds Kind { get; set; }
    }
}
