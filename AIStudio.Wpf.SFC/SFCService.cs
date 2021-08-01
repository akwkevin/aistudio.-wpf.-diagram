using AIStudio.Wpf.SFC.ViewModels;
using System;
using System.Collections.Generic;
using Util.DiagramDesigner;

namespace AIStudio.Wpf.SFC
{
    public static class SFCService
    {
        public static List<LinkPoint> LinkPoint { get; set; }

        static SFCService()
        {
            LinkPoint = new List<LinkPoint>();

            LinkPoint.Add(new LinkPoint { Id = Guid.NewGuid(), Name = "S0", Despcription = "启动按钮", Value = 0 });
            LinkPoint.Add(new LinkPoint { Id = Guid.NewGuid(), Name = "K1_DI", Despcription = "阀门1输入", Value = 0 });
            LinkPoint.Add(new LinkPoint { Id = Guid.NewGuid(), Name = "K2_DI", Despcription = "阀门2输入", Value = 0 });
            LinkPoint.Add(new LinkPoint { Id = Guid.NewGuid(), Name = "K3_DI", Despcription = "阀门3输入", Value = 0 });
            LinkPoint.Add(new LinkPoint { Id = Guid.NewGuid(), Name = "K4_DI", Despcription = "阀门4输入", Value = 0 });
            LinkPoint.Add(new LinkPoint { Id = Guid.NewGuid(), Name = "K1_DO", Despcription = "阀门1反馈", Value = 0 });
            LinkPoint.Add(new LinkPoint { Id = Guid.NewGuid(), Name = "K2_DO", Despcription = "阀门2反馈", Value = 0 });
            LinkPoint.Add(new LinkPoint { Id = Guid.NewGuid(), Name = "K3_DO", Despcription = "阀门3反馈", Value = 0 });
            LinkPoint.Add(new LinkPoint { Id = Guid.NewGuid(), Name = "K4_DO", Despcription = "阀门4反馈", Value = 0 });
            LinkPoint.Add(new LinkPoint { Id = Guid.NewGuid(), Name = "T1", Despcription = "容器1液位", Value = 100, });
            LinkPoint.Add(new LinkPoint { Id = Guid.NewGuid(), Name = "T2", Despcription = "容器2液位", Value = 0 });
            LinkPoint.Add(new LinkPoint { Id = Guid.NewGuid(), Name = "T3", Despcription = "容器3液位", Value = 20 });
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="json"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static void InitData(List<SFCNode> oASteps, List<ConnectorViewModel> connectors)
        {
          
        }
    }
}
