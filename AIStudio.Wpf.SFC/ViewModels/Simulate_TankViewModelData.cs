using AIStudio.Wpf.BaseDiagram.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Util.DiagramDesigner;

namespace AIStudio.Wpf.SFC.ViewModels
{
    public class Simulate_TankViewModelData : TitleBindableBase
    {
        public Simulate_TankViewModelData(LinkPoint linkPoint)
        {
            Title = "容器";
            LinkPoint = linkPoint;
        }

        /// <summary>
        /// 液位
        /// </summary>
        private LinkPoint linkPoint;
        public LinkPoint LinkPoint
        {
            get
            {
                return linkPoint;
            }
            set
            {
                SetProperty(ref linkPoint, value);
            }
        }       

    }
}
