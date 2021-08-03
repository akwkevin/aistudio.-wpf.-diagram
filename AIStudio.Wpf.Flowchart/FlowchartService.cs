using AIStudio.Wpf.Flowchart.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Util.DiagramDesigner;
using Expression = org.mariuszgromada.math.mxparser.Expression;

namespace AIStudio.Wpf.Flowchart
{
    public static class FlowchartService
    {
        static FlowchartService()
        {
            Users = new List<SelectOption>()
            {
                new SelectOption(){ value = "操作员1",text = "操作员1" },
                new SelectOption(){ value = "操作员2",text = "操作员2" },
                new SelectOption(){ value = "Admin",text = "Admin" },
            };

            Roles = new List<SelectOption>()
            {
                new SelectOption(){ value = "操作员",text = "操作员" },
                new SelectOption(){ value = "管理员",text = "管理员" },
            };
        }

        private static List<SelectOption> _users;
        public static List<SelectOption> Users
        {
            get { return _users; }
            set
            {
                _users = value;
            }
        }

        private static List<SelectOption> _roles;
        public static List<SelectOption> Roles
        {
            get { return _roles; }
            set
            {
                _roles = value;
            }
        }


        /// <summary>
        /// 流程数据
        /// </summary>
        public static Dictionary<IDiagramViewModel, List<FlowNode>> FlowNodes { get; set; } = new Dictionary<IDiagramViewModel, List<FlowNode>>();


        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="json"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static void InitData(List<FlowNode> oASteps, List<ConnectorViewModel> connectors, IDiagramViewModel viewModel)
        {
            foreach (var edge in connectors)
            {
                var source = oASteps.FirstOrDefault(p => p.BottomConnector == edge.SourceConnectorInfo || p.LeftConnector == edge.SourceConnectorInfo || p.RightConnector == edge.SourceConnectorInfo);
                if (source != null)
                {
                    if (source.Kind == NodeKinds.Decide)
                    {
                        source.SelectNextStep.Add((edge.SinkConnectorInfo as FullyCreatedConnectorInfo).DataItem.Id.ToString(), "data.Flag" + edge.Text);
                    }
                    else if (source.Kind == NodeKinds.COBegin)
                    {
                        source.SelectNextStep.Add((edge.SinkConnectorInfo as FullyCreatedConnectorInfo).DataItem.Id.ToString(), "True");
                    }
                    else
                    {
                        source.NextStepId = (edge.SinkConnectorInfo as FullyCreatedConnectorInfo).DataItem.Id.ToString();
                    }
                }
            }

            foreach (var step in oASteps)
            {
                //恢复指向上一个节点
                if (!string.IsNullOrEmpty(step.NextStepId))
                {
                    var nextstep = oASteps.FirstOrDefault(p => p.Id.ToString() == step.NextStepId);
                    if (nextstep == null)
                        throw new Exception(string.Format("流程异常，无法找到{0}的下一个流程节点{1}", step.Id, step.NextStepId));
                    if (nextstep.Kind == NodeKinds.Decide)
                    {
                        var nsteps = oASteps.Where(p => nextstep.SelectNextStep.Any(q => p.Id.ToString() == q.Key));
                        if (nsteps == null || nsteps.Count() == 0)
                            throw new Exception(string.Format("流程异常，无法找到{0}的下一个流程节点{1}", step.Id, step.NextStepId));

                        //跳过Decide指向下面的子节点
                        foreach (var nstep in nsteps)
                        {
                            if (nstep.PreStepId == null)
                                nstep.PreStepId = new List<string>();
                            nstep.PreStepId.Add(step.Id.ToString());
                        }
                    }
                    else
                    {
                        if (nextstep.PreStepId == null)
                            nextstep.PreStepId = new List<string>();
                        nextstep.PreStepId.Add(step.Id.ToString());
                    }
                }
            }

            var oAStartStep = oASteps.Single(p => p.Kind == NodeKinds.Start);
            if (string.IsNullOrEmpty(oAStartStep.NextStepId))
            {
                throw new Exception("开始节点没有下一个节点");
            }

            string nextstepid = oAStartStep.NextStepId;
            var nodes = InitStep(oASteps, nextstepid);
            nodes.Insert(0, oAStartStep);

            FlowNodes.Add(viewModel, nodes);           

            Approve(oAStartStep, 100);
        }


        /// <summary>
        /// 初始化步骤
        /// </summary>
        /// <param name="oASteps"></param>
        /// <param name="nextstepid"></param>
        /// <returns></returns>
        public static List<FlowNode> InitStep(List<FlowNode> oASteps, string nextstepid)
        {
            List<FlowNode> outsteps = new List<FlowNode>();
            List<string> nextids = new List<string>();
            var step = oASteps.FirstOrDefault(p => p.Id.ToString() == nextstepid);
            if (step != null)
            {
                if (!string.IsNullOrEmpty(step.NextStepId))
                {
                    nextids.Add(step.NextStepId);
                }
                else if (step.SelectNextStep != null && step.SelectNextStep.Count > 0)
                {
                    nextids.AddRange(step.SelectNextStep.Keys);
                }

                outsteps.Add(step);
                oASteps.Remove(step);
            }

            int index = outsteps.IndexOf(step);

            nextids.Reverse();
            foreach (var next in nextids)
            {
                outsteps.InsertRange(index + 1, InitStep(oASteps, next));
            }
            return outsteps;
        }

        /// <summary>
        /// 审批动作,因为是客户端模拟，假设每次都是该节点的审批人进行的操作
        /// </summary>
        /// <param name="flowNode"></param>
        /// <param name="status"></param>
        /// <param name="remark"></param>
        public static void Approve(FlowNode flowNode, int status, string remark = null)
        {
            switch (status)
            {
                case 100:
                    if (flowNode is MiddleFlowNode middleFlowNode)
                    {
                        if (string.IsNullOrEmpty(flowNode.Remark))
                        {
                            remark = "审批人：" + remark;
                        }
                        else
                        {
                            remark = flowNode.Remark + "\r审批人：" + remark;
                        }

                        if (middleFlowNode.ActType == "and")//如果是与签，那么要都审批通过
                        {
                            if (middleFlowNode.UserIds != null && middleFlowNode.UserIds.Count > 1)
                            {
                                //实际情况不是这样的，这里只是演示，简化。
                                int count = remark.Split("审批人：", StringSplitOptions.RemoveEmptyEntries).Length;
                                if (middleFlowNode.UserIds.Count != count)
                                {
                                    SetStatus(flowNode, 1, remark);
                                    return;
                                }
                            }
                            else if (middleFlowNode.RoleIds != null && middleFlowNode.RoleIds.Count > 1)
                            {
                                //实际情况不是这样的，这里只是演示，简化。
                                int count = remark.Split("审批人：", StringSplitOptions.RemoveEmptyEntries).Length;
                                if (middleFlowNode.RoleIds.Count != count)
                                {
                                    SetStatus(flowNode, 1, remark);
                                    return;
                                }
                            }
                        }
                      
                    }

                    SetStatus(flowNode, status, remark);
                    if (!string.IsNullOrEmpty(flowNode.NextStepId))
                    {
                        Next(flowNode.NextStepId, flowNode);
                    }
                    else if (flowNode.SelectNextStep != null && flowNode.SelectNextStep.Count > 0)
                    {
                        foreach (var step in flowNode.SelectNextStep)
                        {
                            Next(step.Key, flowNode);
                        }
                    }
                    break;
                case 2:
                    if (Pre(flowNode))
                    {
                        SetStatus(flowNode, status, remark);
                    }
                    else
                    {
                        MessageBox.Show("该节点不支持驳回上一级");
                    }
                    break;
                case 3:
                    SetStatus(flowNode, status, remark);
                    FlowNodes[flowNode.Parent].ForEach(p => { if (p.Status == 100) p.Status = 0; });
                    Approve(FlowNodes[flowNode.Parent][0], 100);
                    MessageBox.Show("流程重新开始");
                    break;
                case 4:
                    SetStatus(flowNode, status, remark);
                    MessageBox.Show("流程否决");
                    break;
            }
        }

        /// <summary>
        /// 流向下一个节点
        /// </summary>
        /// <param name="stepid"></param>
        public static void Next(string stepid, FlowNode flowNode)
        {
            FlowNode nextNode = FlowNodes[flowNode.Parent].FirstOrDefault(p => p.Id.ToString() == stepid);
            SetStatus(nextNode, 1);

            switch (nextNode.Kind)
            {
                case NodeKinds.Start:
                    SetStatus(nextNode, 100);
                    Next(nextNode.NextStepId, nextNode);
                    break;
                case NodeKinds.End:
                    SetStatus(nextNode, 100);
                    MessageBox.Show("流程完成");
                    break;
                case NodeKinds.Decide:
                    foreach (var step in nextNode.SelectNextStep)
                    {
                        try
                        {
                            //按条件选择一个分支
                            string express = step.Value.Replace("data.Flag", nextNode.Text);
                            Expression e = new Expression(express);
                            var result = e.calculate();
                            if (result == 1)
                            {
                                SetStatus(nextNode, 100);
                                Next(step.Key, nextNode);
                                return;
                            }
                        }
                        catch { }
                    }
                    //如果表达式错了，就按第一个处理
                    Next(nextNode.SelectNextStep.FirstOrDefault().Key, nextNode);
                    break;
                case NodeKinds.COBegin:
                    foreach (var step in nextNode.SelectNextStep)//启动各个分支
                    {
                        SetStatus(nextNode, 100);
                        Next(step.Key, nextNode);
                    }
                    break;
                case NodeKinds.COEnd:
                    foreach (var prestep in nextNode.PreStepId)
                    {
                        var step = FlowNodes[flowNode.Parent].FirstOrDefault(p => p.Id.ToString() == prestep);
                        if (step.Status != 100)//如果并行分支没有都完成，那么并行结束节点也未完成
                        {
                            return;
                        }
                    }
                    SetStatus(nextNode, 100);
                    Next(nextNode.NextStepId, nextNode);
                    break;
            }
        }

        /// <summary>
        /// 流向上一个节点
        /// </summary>
        /// <param name="flowNode"></param>
        /// <returns></returns>
        public static bool Pre(FlowNode flowNode)
        {
            if (flowNode.PreStepId != null && flowNode.PreStepId.Count == 1)
            {
                FlowNode preNode = FlowNodes[flowNode.Parent].FirstOrDefault(p => p.Id.ToString() == flowNode.PreStepId[0]);
                if (preNode.Kind == NodeKinds.Middle)
                {
                    SetStatus(preNode, 1);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 设置颜色
        /// </summary>
        /// <param name="flowNode"></param>
        /// <param name="status"></param>
        /// <param name="remark"></param>
        public static void SetStatus(FlowNode flowNode, int status, string remark = null)
        {
            flowNode.Status = status;
            flowNode.Remark = remark;
            switch (status)
            {
                case 100:
                    flowNode.Color = Colors.Green.ToString();
                    break;
                case 0:
                    flowNode.Color = Colors.Yellow.ToString();
                    break;
                case 1:
                    flowNode.Color = Colors.Orange.ToString();
                    break;
                case 2:
                    flowNode.Color = Colors.Red.ToString();
                    break;
                case 3:
                    flowNode.Color = Colors.Red.ToString();
                    break;
                case 4:
                    flowNode.Color = Colors.Red.ToString();
                    break;
            }
        }

        public static void DisposeData(IDiagramViewModel viewModel)
        {
            FlowNodes.Remove(viewModel);
        }
    }
}
