using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Util.DiagramDesigner;

namespace AIStudio.Wpf.ADiagram.Demos.Flowchart
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



        public static List<FlowNode> FlowNodes { get; set; }


        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="json"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static void InitOAData(List<FlowNode> oASteps, List<ConnectorViewModel> connectors)
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
            FlowNodes = InitStep(oASteps, nextstepid);
            FlowNodes.Insert(0, oAStartStep);

            Approve(oAStartStep, 100);
        }


        /// <summary>
        /// 获取下一个节点
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

        public static void Approve(FlowNode flowNode, int status, string remark = null)
        {
            switch (status)
            {
                case 100:
                    SetStatus(flowNode, status, remark);
                    flowNode.Color = Colors.Green.ToString();
                    if (!string.IsNullOrEmpty(flowNode.NextStepId))
                    {
                        Next(flowNode.NextStepId);
                    }
                    else if (flowNode.SelectNextStep != null && flowNode.SelectNextStep.Count > 0)
                    {
                        foreach (var step in flowNode.SelectNextStep)
                        {
                            Next(step.Key);
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
                    FlowNodes.ForEach(p => { if (p.Status == 100) p.Status = 0; });
                    Approve(FlowNodes[0], 100);
                    MessageBox.Show("流程重新开始");
                    break;
                case 4:
                    SetStatus(flowNode, status, remark);
                    MessageBox.Show("流程否决");
                    break;
            }
        }

        public static void Next(string stepid)
        {
            FlowNode nextNode = FlowNodes.FirstOrDefault(p => p.Id.ToString() == stepid);
            SetStatus(nextNode, 1);

            switch (nextNode.Kind)
            {
                case NodeKinds.Start: 
                    SetStatus(nextNode, 100); 
                    Next(nextNode.NextStepId); 
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
                            //暂未实现表达式比较
                            step.Value.Replace("data.Flag", nextNode.Text);
                            //先按第一个表达式成立处理。
                            SetStatus(nextNode, 100);
                            Next(step.Key);
                            break;
                        }
                        catch { }
                    }
                    break;
                case NodeKinds.COBegin:
                    foreach (var step in nextNode.SelectNextStep)
                    {
                        SetStatus(nextNode, 100);
                        Next(step.Key);
                    }
                    break;
                case NodeKinds.COEnd:
                    foreach (var prestep in nextNode.PreStepId)
                    {
                        var step = FlowNodes.FirstOrDefault(p => p.Id.ToString() == prestep);
                        if (step.Status != 100)
                        {
                            return;
                        }
                    }
                    SetStatus(nextNode, 100);
                    Next(nextNode.NextStepId);
                    break;
            }
        }

        public static bool Pre(FlowNode flowNode)
        {
            if (flowNode.PreStepId != null && flowNode.PreStepId.Count == 1)
            {
                FlowNode preNode = FlowNodes.FirstOrDefault(p => p.Id.ToString() == flowNode.PreStepId[0]);
                if (preNode.Kind == NodeKinds.Middle)
                {
                    SetStatus(preNode, 1);
                    return true;
                }
            }

            return false;
        }

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
    }
}
