using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Util.DiagramDesigner;

namespace AIStudio.Wpf.SFC.ViewModels
{
    public class Simulate_SolenoidViewModel : SFCNode
    {
        public Simulate_SolenoidViewModel() : base(SFCNodeKinds.Simulate_Solenoid)
        {
            ItemWidth = 32;
            ItemHeight = 32;

            ExecuteAddLeftInput(null);
            ExecuteAddRightOutput(null);
        }

        public Simulate_SolenoidViewModel(IDiagramViewModel parent, DesignerItemBase designer) : base(parent, designer)
        {
        }

        private bool _showText;
        public override bool ShowText
        {
            get
            {
                return false;
            }
            set
            {
                SetProperty(ref _showText, value);
            }
        }

        /// <summary>
        /// 输入
        /// </summary>
        private LinkPoint _dILinkPoint;
        public LinkPoint DILinkPoint
        {
            get
            {
                return _dILinkPoint;
            }
            set
            {
                if (_dILinkPoint != null)
                {
                    _dILinkPoint.PropertyChanged -= _dILinkPoint_PropertyChanged;
                }
                SetProperty(ref _dILinkPoint, value);
                if (_dILinkPoint != null)
                {
                    _dILinkPoint.PropertyChanged += _dILinkPoint_PropertyChanged;
                }
            }
        }

        private void _dILinkPoint_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Value")
            {
                if (DOLinkPoint != null)
                {
                    DOLinkPoint.Value = DILinkPoint.Value;
                }
            }
        }

        /// <summary>
        /// 反馈
        /// </summary>
        private LinkPoint _dOLinkPoint;
        public LinkPoint DOLinkPoint
        {
            get
            {
                return _dOLinkPoint;
            }
            set
            {
                SetProperty(ref _dOLinkPoint, value);
            }
        }

        protected override void ExecuteEditCommand(object parameter)
        {
            Simulate_SolenoidViewModelData data = new Simulate_SolenoidViewModelData(DILinkPoint, DOLinkPoint);
            if (visualiserService.ShowDialog(data) == true)
            {
                this.DILinkPoint = data.DILinkPoint;
                this.DOLinkPoint = data.DOLinkPoint;
            }
        }

    }
}
