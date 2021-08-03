using AIStudio.Wpf.ADiagram.Models;
using AIStudio.Wpf.ADiagram.ViewModels;
using AIStudio.Wpf.Flowchart.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Util.DiagramDesigner;

namespace AIStudio.Wpf.Flowchart
{
    public class FlowchartViewModel : DiagramsViewModel
    {
        public FlowchartViewModel(string title, string status, DiagramType diagramType) : base(title, status, diagramType)
        {

        }
        public FlowchartViewModel(string filename, DiagramDocument diagramDocument) : base(filename, diagramDocument)
        {
            if (DiagramViewModel != null)
            {
                FlowchartService.InitData(DiagramViewModel.Items.OfType<FlowNode>().ToList(), DiagramViewModel.Items.OfType<ConnectorViewModel>().ToList(), DiagramViewModel);
            }
        }

        protected override void InitDiagramViewModel()
        {
            base.InitDiagramViewModel();

            DiagramViewModel.ShowGrid = true;
            DiagramViewModel.GridCellSize = new Size(100, 100);
            DiagramViewModel.CellHorizontalAlignment = CellHorizontalAlignment.Center;
            DiagramViewModel.CellVerticalAlignment = CellVerticalAlignment.Center;
        }

        protected override void Init()
        {
            base.Init();

            DesignerItemViewModelBase start = new StartFlowNode() { Left = 100, Top = 0, Color = Colors.Yellow.ToString() };
            DiagramViewModel.DirectAddItemCommand.Execute(start);

            DesignerItemViewModelBase middle1 = new MiddleFlowNode() { Left = 100, Top = 100, Color = Colors.Yellow.ToString(), Text = "主管审批", UserIds= new List<string> { "操作员1", "操作员2" }, ActType = "or" };
            DiagramViewModel.DirectAddItemCommand.Execute(middle1);

            DesignerItemViewModelBase decide = new DecideFlowNode() { Left = 100, Top = 200, Color = Colors.Yellow.ToString(), Text = "5" };
            DiagramViewModel.DirectAddItemCommand.Execute(decide);

            DesignerItemViewModelBase middle2 = new MiddleFlowNode() { Left = 200, Top = 300, Color = Colors.Yellow.ToString(), Text = "分管领导", UserIds = new List<string> { "操作员1", "操作员2" }, ActType = "and" };
            DiagramViewModel.DirectAddItemCommand.Execute(middle2);

            DesignerItemViewModelBase cobegin = new COBeginFlowNode() { Left = 100, Top = 400, Color = Colors.Yellow.ToString() };
            DiagramViewModel.DirectAddItemCommand.Execute(cobegin);

            DesignerItemViewModelBase middle3 = new MiddleFlowNode() { Left = 100, Top = 500, Color = Colors.Yellow.ToString(), Text = "财务审批", UserIds = new List<string> { "Admin" }, ActType = "or" };
            DiagramViewModel.DirectAddItemCommand.Execute(middle3);

            DesignerItemViewModelBase middle4 = new MiddleFlowNode() { Left = 200, Top = 500, Color = Colors.Yellow.ToString(), Text = "人力审批", RoleIds = new List<string> { "操作员", "管理员" }, ActType = "or" };
            DiagramViewModel.DirectAddItemCommand.Execute(middle4);

            DesignerItemViewModelBase coend = new COEndFlowNode() { Left = 100, Top = 600, Color = Colors.Yellow.ToString() };
            DiagramViewModel.DirectAddItemCommand.Execute(coend);

            DesignerItemViewModelBase end = new EndFlowNode() { Left = 100, Top = 700, Color = Colors.Yellow.ToString() };
            DiagramViewModel.DirectAddItemCommand.Execute(end);

            ConnectorViewModel connector1 = new ConnectorViewModel(start.BottomConnector, middle1.TopConnector);     
            DiagramViewModel.DirectAddItemCommand.Execute(connector1);

            ConnectorViewModel connector2 = new ConnectorViewModel(middle1.BottomConnector, decide.TopConnector);
            DiagramViewModel.DirectAddItemCommand.Execute(connector2);

            ConnectorViewModel connector3 = new ConnectorViewModel(decide.RightConnector, middle2.TopConnector);
            DiagramViewModel.DirectAddItemCommand.Execute(connector3);
            connector3.AddText(">=3");

            ConnectorViewModel connector4 = new ConnectorViewModel(middle2.BottomConnector, cobegin.TopConnector);
            DiagramViewModel.DirectAddItemCommand.Execute(connector4);

            ConnectorViewModel connector5 = new ConnectorViewModel(decide.BottomConnector, cobegin.TopConnector);
            DiagramViewModel.DirectAddItemCommand.Execute(connector5);
            connector5.AddText("<3");

            ConnectorViewModel connector6 = new ConnectorViewModel(cobegin.BottomConnector, middle3.TopConnector);
            DiagramViewModel.DirectAddItemCommand.Execute(connector6);

            ConnectorViewModel connector7 = new ConnectorViewModel(cobegin.BottomConnector, middle4.TopConnector);
            DiagramViewModel.DirectAddItemCommand.Execute(connector7);

            ConnectorViewModel connector8 = new ConnectorViewModel(middle3.BottomConnector, coend.TopConnector);
            DiagramViewModel.DirectAddItemCommand.Execute(connector8);

            ConnectorViewModel connector9 = new ConnectorViewModel(middle4.BottomConnector, coend.TopConnector);
            DiagramViewModel.DirectAddItemCommand.Execute(connector9);

            ConnectorViewModel connector10 = new ConnectorViewModel(coend.BottomConnector, end.TopConnector);
            DiagramViewModel.DirectAddItemCommand.Execute(connector10);

            DiagramViewModel.ClearSelectedItems();

            FlowchartService.InitData(DiagramViewModel.Items.OfType<FlowNode>().ToList(), DiagramViewModel.Items.OfType<ConnectorViewModel>().ToList(), DiagramViewModel);
        }

        public override void Dispose()
        {
            base.Dispose();

            foreach (var viewModel in DiagramViewModels)
            {
                FlowchartService.DisposeData(viewModel);
            }
        }
    }
}
