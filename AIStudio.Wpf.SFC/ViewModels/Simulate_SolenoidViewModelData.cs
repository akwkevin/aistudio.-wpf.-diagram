using AIStudio.Wpf.BaseDiagram.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Util.DiagramDesigner;

namespace AIStudio.Wpf.SFC.ViewModels
{
    public class Simulate_SolenoidViewModelData : TitleBindableBase
    {
        public Simulate_SolenoidViewModelData(LinkPoint dILinkPoint, LinkPoint dOLinkPoint)
        {
            Title = "阀门";
            DILinkPoint = dILinkPoint;
            DOLinkPoint = dOLinkPoint;
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
                SetProperty(ref _dILinkPoint, value);
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

    }
}
