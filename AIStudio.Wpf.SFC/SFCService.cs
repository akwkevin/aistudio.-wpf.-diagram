using AIStudio.Wpf.SFC.ViewModels;
using org.mariuszgromada.math.mxparser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using Util.DiagramDesigner;

namespace AIStudio.Wpf.SFC
{
    public static class SFCService
    {
        public static List<LinkPoint> LinkPoint { get; set; }

        static SFCService()
        {
            LinkPoint = new List<LinkPoint>();

            LinkPoint.Add(new LinkPoint { Id = Guid.Parse("9943AC3B-5E19-4AF4-8CB0-26998FC31545"), Name = "S0", Despcription = "启动按钮", Value = 0 });
            LinkPoint.Add(new LinkPoint { Id = Guid.Parse("F60007E3-913B-43C4-B80B-9624B4BD95AC"), Name = "K1_DI", Despcription = "阀门1输入", Value = 0 });
            LinkPoint.Add(new LinkPoint { Id = Guid.Parse("8B2CBA04-A9C8-4FF0-BE95-27546292FB90"), Name = "K2_DI", Despcription = "阀门2输入", Value = 0 });
            LinkPoint.Add(new LinkPoint { Id = Guid.Parse("F213B720-ADC2-41C9-82B7-D77B56B99CB7"), Name = "K3_DI", Despcription = "阀门3输入", Value = 0 });
            LinkPoint.Add(new LinkPoint { Id = Guid.Parse("48BB7F62-A476-4A68-9DB4-5BD4201C1C26"), Name = "K4_DI", Despcription = "阀门4输入", Value = 0 });
            LinkPoint.Add(new LinkPoint { Id = Guid.Parse("5E52ACA8-8DB4-4D39-A3BF-73EE3A551253"), Name = "K1_DO", Despcription = "阀门1反馈", Value = 0 });
            LinkPoint.Add(new LinkPoint { Id = Guid.Parse("5B8C53FF-A4CA-40BA-9F36-5C228D9D0C5A"), Name = "K2_DO", Despcription = "阀门2反馈", Value = 0 });
            LinkPoint.Add(new LinkPoint { Id = Guid.Parse("29496600-74CF-4AB0-88D1-4C5BFAD4C405"), Name = "K3_DO", Despcription = "阀门3反馈", Value = 0 });
            LinkPoint.Add(new LinkPoint { Id = Guid.Parse("15733895-96E5-4F90-9BC2-3C7FF0A2B048"), Name = "K4_DO", Despcription = "阀门4反馈", Value = 0 });
            LinkPoint.Add(new LinkPoint { Id = Guid.Parse("BD593101-C759-4077-A3F2-F86AA2FC6DF8"), Name = "T1", Despcription = "容器1液位", Value = 100, });
            LinkPoint.Add(new LinkPoint { Id = Guid.Parse("2BCBD2C9-9BCE-4F10-B115-088DEE1F4E08"), Name = "T2", Despcription = "容器2液位", Value = 0 });
            LinkPoint.Add(new LinkPoint { Id = Guid.Parse("DBD10A3C-7E2E-4DAE-9CB0-A2132DC469DF"), Name = "T3", Despcription = "容器3液位", Value = 20 });
        }

        /// <summary>
        /// 流程数据
        /// </summary>
        public static Dictionary<IDiagramViewModel, List<SFCNode>> SFCNodes { get; set; } = new Dictionary<IDiagramViewModel, List<SFCNode>>();

        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="json"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static void InitData(List<SFCNode> nodes, List<ConnectorViewModel> connectors, IDiagramViewModel viewModel)
        {
            var start = nodes.FirstOrDefault(p => p.Kind == SFCNodeKinds.Start);
            if (start == null)
                throw new Exception("没有开始节点");

            foreach (var edge in connectors)
            {
                var source = nodes.FirstOrDefault(p => p.BottomConnector == edge.SourceConnectorInfo || p.LeftConnector == edge.SourceConnectorInfo || p.RightConnector == edge.SourceConnectorInfo || p.TopConnector == edge.SourceConnectorInfo);
                if (source != null)
                {
                    source.NextNode.Add((edge.SinkConnectorInfo as FullyCreatedConnectorInfo).DataItem as SFCNode);
                    ((edge.SinkConnectorInfo as FullyCreatedConnectorInfo).DataItem as SFCNode).PreNode.Add(source);
                }
            }

            SFCNodes.Add(viewModel, nodes);
            ResetStatus(viewModel);
        }

        private static void ResetStatus(IDiagramViewModel viewModel)
        {
            var nodes = SFCNodes[viewModel];
            var start = nodes.FirstOrDefault(p => p.Kind == SFCNodeKinds.Start);

            SetStatus(start, 1);
            nodes.ForEach(p =>
            {
                if (p != start)
                {
                    SetStatus(p, 0);
                }
            });
        }

        public static void Execute(IDiagramViewModel viewModel)
        {
            //仅T2进行计算
            var tank = SFCNodes[viewModel].OfType<Simulate_TankViewModel>().FirstOrDefault(p => p.Text == "T2");
            tank.Execute();

            var startbtn = SFCNodes[viewModel].OfType<Simulate_StartViewModel>().FirstOrDefault();
            if (startbtn != null && startbtn.LinkPoint.Value == 0)//停止
            {
                ResetStatus(viewModel);
            }
            else//启动
            {
                var currentNodes = SFCNodes[viewModel].Where(p => p.Status == 1);
                foreach (var node in currentNodes)
                {
                    if (node.NextNode.OfType<SFCConditionNode>().Count() > 1)//选择分支
                    {
                        foreach (var next in node.NextNode)
                        {
                            if (next is SFCConditionNode nextconditionNode)
                            {
                                List<Argument> args = new List<Argument>();
                                for (int i = 0; i < nextconditionNode.LinkPoint.Count; i++)
                                {
                                    Argument x = new Argument($"p{i}", nextconditionNode.LinkPoint[i].Value);
                                    args.Add(x);
                                }
                                Expression e = new Expression(nextconditionNode.Expression, args.ToArray());
                                var result = e.calculate();
                                if (result == 0)
                                {
                                    continue;
                                }

                                SetStatus(node, 100);
                                SetStatus(next, 1);
                                break;
                            }
                        }
                    }
                    else
                    {
                        if (node is SFCConditionNode conditionNode)
                        {
                            List<Argument> args = new List<Argument>();
                            for (int i = 0; i < conditionNode.LinkPoint.Count; i++)
                            {
                                Argument x = new Argument($"p{i}", conditionNode.LinkPoint[i].Value);
                                args.Add(x);
                            }
                            Expression e = new Expression(conditionNode.Expression, args.ToArray());
                            var result = e.calculate();
                            if (result == 0)
                            {
                                continue;
                            }
                        }
                        else if (node is SFCActionNode actionNode)
                        {
                            if (actionNode.LinkPoint != null)
                            {
                                Expression e = new Expression(actionNode.Expression);
                                actionNode.LinkPoint.Value = e.calculate();
                                SetStatus(actionNode, 100);
                            }
                        }
                        else if (node is SFCCOEndNode cOEndNode)
                        {
                            //并行结束节点需要前面节点都完成
                            if (cOEndNode.PreNode.Any(p => p.Status != 100))
                            {
                                continue;
                            }
                        }


                        SetStatus(node, 100);
                        foreach (var next in node.NextNode)
                        {
                            if (next is SFCStartNode startNode)
                            {
                                ResetStatus(viewModel);
                            }
                            else
                            {
                                SetStatus(next, 1);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 设置颜色
        /// </summary>
        /// <param name="flowNode"></param>
        /// <param name="status"></param>
        /// <param name="remark"></param>
        public static void SetStatus(SFCNode node, int status)
        {
            node.Status = status;
            switch (status)
            {
                case 100:
                    node.ColorViewModel.FillColor.Color = Colors.Gray;
                    break;
                case 0:
                    node.ColorViewModel.FillColor.Color = Colors.Blue;
                    break;
                case 1:
                    node.ColorViewModel.FillColor.Color = Colors.Green;
                    break;
            }
        }


        public static string SerializeLinkPoint(List<LinkPoint> points)
        {
            return string.Join(",", points.Select(p => p.Id));
        }

        public static List<LinkPoint> DeserializeLinkPoint(string point)
        {
            List<LinkPoint> links = new List<LinkPoint>();
            string[] pieces = point.Split(new char[] { ',' });
            foreach (var piece in pieces)
            {
                links.Add(LinkPoint.FirstOrDefault(p => p.Id.ToString() == piece));
            }
            return links;
        }

        public static void DisposeData(IDiagramViewModel viewModel)
        {
            SFCNodes.Remove(viewModel);
        }
    }
}
