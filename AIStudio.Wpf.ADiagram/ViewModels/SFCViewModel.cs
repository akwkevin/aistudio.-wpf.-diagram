using AIStudio.Wpf.ADiagram.ViewModels;
using AIStudio.Wpf.Flowchart.ViewModels;
using AIStudio.Wpf.SFC;
using AIStudio.Wpf.SFC.ViewModels;
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
    public class SFCViewModel : DiagramsViewModel
    {
        public SFCViewModel(string title, string status, DiagramType diagramType) : base(title, status, diagramType)
        {

        }
        public SFCViewModel(string filename) : base(filename)
        {

        }

        protected override void InitDiagramViewModel()
        {
            base.InitDiagramViewModel();

            DiagramViewModel.ShowGrid = true;
            DiagramViewModel.GridCellSize = new Size(100, 60);
            DiagramViewModel.CellHorizontalAlignment = CellHorizontalAlignment.Center;
            DiagramViewModel.CellVerticalAlignment = CellVerticalAlignment.Center;
        }

        protected override void Init()
        {
            base.Init();

            SFCStartNode start = new SFCStartNode() { Left = 0, Top = 60, Text = "S0" };
            DiagramViewModel.DirectAddItemCommand.Execute(start);

            SFCConditionNode condition1 = new SFCConditionNode() { Left = 0, Top = 120, Text = "X0" };
            DiagramViewModel.DirectAddItemCommand.Execute(condition1);

            SFCNodeNode step1 = new SFCNodeNode() { Left = 0, Top = 180, Text = "S1" };
            DiagramViewModel.DirectAddItemCommand.Execute(step1);

            SFCActionNode action11 = new SFCActionNode() { Left = 100, Top = 180, Text = "SET_V1" };
            DiagramViewModel.DirectAddItemCommand.Execute(action11);

            SFCActionNode action12 = new SFCActionNode() { Left = 200, Top = 180, Text = "SET_V2" };
            DiagramViewModel.DirectAddItemCommand.Execute(action12);

            SFCActionNode action13 = new SFCActionNode() { Left = 300, Top = 180, Text = "SET_V3" };
            DiagramViewModel.DirectAddItemCommand.Execute(action13);

            SFCActionNode action14 = new SFCActionNode() { Left = 400, Top = 180, Text = "RES_V4" };
            DiagramViewModel.DirectAddItemCommand.Execute(action14);

            SFCConditionNode condition2 = new SFCConditionNode() { Left = 0, Top = 240, Text = "X1" };
            DiagramViewModel.DirectAddItemCommand.Execute(condition2);

            SFCNodeNode step2 = new SFCNodeNode() { Left = 0, Top = 300, Text = "S2" };
            DiagramViewModel.DirectAddItemCommand.Execute(step2);

            SFCActionNode action2 = new SFCActionNode() { Left = 100, Top = 300, Text = "SET_V4" };
            DiagramViewModel.DirectAddItemCommand.Execute(action2);

            SFCConditionNode condition3 = new SFCConditionNode() { Left = 0, Top = 360, Text = "X2" };
            DiagramViewModel.DirectAddItemCommand.Execute(condition3);

            SFCNodeNode step3 = new SFCNodeNode() { Left = 0, Top = 420, Text = "S3" };
            DiagramViewModel.DirectAddItemCommand.Execute(step3);

            SFCActionNode action3 = new SFCActionNode() { Left = 100, Top = 420, Text = "RES_V1" };
            DiagramViewModel.DirectAddItemCommand.Execute(action3);

            SFCConditionNode condition4 = new SFCConditionNode() { Left = 0, Top = 480, Text = "X4" };
            DiagramViewModel.DirectAddItemCommand.Execute(condition4);

            SFCCOBeginNode cobegin = new SFCCOBeginNode() { Left = 38, Top = 540, Text = "" };
            DiagramViewModel.DirectAddItemCommand.Execute(cobegin);

            SFCNodeNode step4 = new SFCNodeNode() { Left = 0, Top = 600, Text = "S4" };
            DiagramViewModel.DirectAddItemCommand.Execute(step4);

            SFCActionNode action4 = new SFCActionNode() { Left = 100, Top = 600, Text = "RES_V2" };
            DiagramViewModel.DirectAddItemCommand.Execute(action4);

            SFCNodeNode step5 = new SFCNodeNode() { Left = 200, Top = 600, Text = "S5" };
            DiagramViewModel.DirectAddItemCommand.Execute(step5);

            SFCActionNode action5 = new SFCActionNode() { Left = 300, Top = 600, Text = "RES_V3" };
            DiagramViewModel.DirectAddItemCommand.Execute(action5);

            SFCCOEndNode coend = new SFCCOEndNode() { Left = 38, Top = 660, Text = "" };
            DiagramViewModel.DirectAddItemCommand.Execute(coend);

            ConnectorViewModel connector1 = new ConnectorViewModel(start.Output[0], condition1.Input[0]);
            DiagramViewModel.DirectAddItemCommand.Execute(connector1);

            ConnectorViewModel connector2 = new ConnectorViewModel(condition1.Output[0], step1.Input[0]);
            DiagramViewModel.DirectAddItemCommand.Execute(connector2);

            ConnectorViewModel connector31 = new ConnectorViewModel(step1.Action[0], action11.Input[0]);
            DiagramViewModel.DirectAddItemCommand.Execute(connector31);

            ConnectorViewModel connector32 = new ConnectorViewModel(step1.Action[0], action12.Input[0]);
            DiagramViewModel.DirectAddItemCommand.Execute(connector32);

            ConnectorViewModel connector33 = new ConnectorViewModel(step1.Action[0], action13.Input[0]);
            DiagramViewModel.DirectAddItemCommand.Execute(connector33);

            ConnectorViewModel connector34 = new ConnectorViewModel(step1.Action[0], action14.Input[0]);
            DiagramViewModel.DirectAddItemCommand.Execute(connector34);

            ConnectorViewModel connector4 = new ConnectorViewModel(step1.Output[0], condition2.Input[0]);
            DiagramViewModel.DirectAddItemCommand.Execute(connector4);

            ConnectorViewModel connector5 = new ConnectorViewModel(condition2.Output[0], step2.Input[0]);
            DiagramViewModel.DirectAddItemCommand.Execute(connector5);

            ConnectorViewModel connector6 = new ConnectorViewModel(step2.Action[0], action2.Input[0]);
            DiagramViewModel.DirectAddItemCommand.Execute(connector6);

            ConnectorViewModel connector7 = new ConnectorViewModel(step2.Output[0], condition3.Input[0]);
            DiagramViewModel.DirectAddItemCommand.Execute(connector7);

            ConnectorViewModel connector8 = new ConnectorViewModel(condition3.Output[0], step3.Input[0]);
            DiagramViewModel.DirectAddItemCommand.Execute(connector8);

            ConnectorViewModel connector9 = new ConnectorViewModel(step3.Action[0], action3.Input[0]);
            DiagramViewModel.DirectAddItemCommand.Execute(connector9);

            ConnectorViewModel connector10 = new ConnectorViewModel(step3.Output[0], condition4.Input[0]);
            DiagramViewModel.DirectAddItemCommand.Execute(connector10);

            ConnectorViewModel connector11 = new ConnectorViewModel(condition4.Output[0], cobegin.Input[0]);
            DiagramViewModel.DirectAddItemCommand.Execute(connector11);

            ConnectorViewModel connector12 = new ConnectorViewModel(cobegin.Output[0], step4.Input[0]);
            DiagramViewModel.DirectAddItemCommand.Execute(connector12);

            ConnectorViewModel connector13 = new ConnectorViewModel(step4.Action[0], action4.Input[0]);
            DiagramViewModel.DirectAddItemCommand.Execute(connector13);

            ConnectorViewModel connector14 = new ConnectorViewModel(cobegin.Output[1], step5.Input[0]);
            DiagramViewModel.DirectAddItemCommand.Execute(connector14);

            ConnectorViewModel connector15 = new ConnectorViewModel(step5.Action[0], action5.Input[0]);
            DiagramViewModel.DirectAddItemCommand.Execute(connector15);

            ConnectorViewModel connector16 = new ConnectorViewModel(step4.Output[0], coend.Input[0]);
            DiagramViewModel.DirectAddItemCommand.Execute(connector16);

            ConnectorViewModel connector17 = new ConnectorViewModel(step5.Output[0], coend.Input[1]);
            DiagramViewModel.DirectAddItemCommand.Execute(connector17);

            ConnectorViewModel connector18 = new ConnectorViewModel(coend.Output[0], start.Input[0]);
            DiagramViewModel.DirectAddItemCommand.Execute(connector18);

            #region 模拟部分
            TextDesignerItemViewModel despcription = new TextDesignerItemViewModel()
            {
                Left = 230,
                Top = 270,
                ItemWidth = 300,
                ItemHeight = 120,
                Text = @"模拟一个容器的高低液位控制方法
1.按下启动按钮, 程序启动
2.当液位低于20%的时候, V1,V2,V3打开, V4关闭
3.当液位高于50%的时候, V4打开
4.当液位高于70%的时侯, V1关闭
5.当液位高于80%的时候, V2,V3并行关闭"
            };
            despcription.FontViewModel.HorizontalAlignment = HorizontalAlignment.Left;
            despcription.FontViewModel.VerticalAlignment = VerticalAlignment.Top;
            DiagramViewModel.DirectAddItemCommand.Execute(despcription);

            Simulate_ListViewModel list = new Simulate_ListViewModel()
            {
                Left = 410,
                Top = 390,
            };
            DiagramViewModel.DirectAddItemCommand.Execute(list);

            Simulate_StartViewModel btnstart = new Simulate_StartViewModel() { Left = 0, Top = 0, LinkPoint = SFCService.LinkPoint.FirstOrDefault(p => p.Name == "S0"), };
            DiagramViewModel.DirectAddItemCommand.Execute(btnstart);

            Simulate_TankViewModel tank1 = new Simulate_TankViewModel() { Left = 100, Top = 43, ItemWidth = 48, ItemHeight = 60, Text = "T1", LinkPoint = SFCService.LinkPoint.FirstOrDefault(p => p.Name == "T1") };
            DiagramViewModel.DirectAddItemCommand.Execute(tank1);

            Simulate_SolenoidViewModel k1 = new Simulate_SolenoidViewModel() { Left = 200, Top = 0, Text = "K1", DILinkPoint = SFCService.LinkPoint.FirstOrDefault(p => p.Name == "K1_DI"), DOLinkPoint = SFCService.LinkPoint.FirstOrDefault(p => p.Name == "K1_DO") };
            DiagramViewModel.DirectAddItemCommand.Execute(k1);

            Simulate_SolenoidViewModel k2 = new Simulate_SolenoidViewModel() { Left = 200, Top = 60, Text = "K2", DILinkPoint = SFCService.LinkPoint.FirstOrDefault(p => p.Name == "K2_DI"), DOLinkPoint = SFCService.LinkPoint.FirstOrDefault(p => p.Name == "K2_DO") };
            DiagramViewModel.DirectAddItemCommand.Execute(k2);

            Simulate_SolenoidViewModel k3 = new Simulate_SolenoidViewModel() { Left = 200, Top = 120, Text = "K3", DILinkPoint = SFCService.LinkPoint.FirstOrDefault(p => p.Name == "K3_DI"), DOLinkPoint = SFCService.LinkPoint.FirstOrDefault(p => p.Name == "K3_DO") };
            DiagramViewModel.DirectAddItemCommand.Execute(k3);

            Simulate_TankViewModel tank2 = new Simulate_TankViewModel() { Left = 300, Top = 28, Text = "T2", LinkPoint = SFCService.LinkPoint.FirstOrDefault(p => p.Name == "T2") };

            DiagramViewModel.DirectAddItemCommand.Execute(tank2);

            Simulate_SolenoidViewModel k4 = new Simulate_SolenoidViewModel() { Left = 400, Top = 60, Text = "K4", DILinkPoint = SFCService.LinkPoint.FirstOrDefault(p => p.Name == "K4_DI"), DOLinkPoint = SFCService.LinkPoint.FirstOrDefault(p => p.Name == "K4_DO") };
            DiagramViewModel.DirectAddItemCommand.Execute(k4);

            Simulate_TankViewModel tank3 = new Simulate_TankViewModel() { Left = 500, Top = 103, ItemWidth = 48, ItemHeight = 60, Text = "T3", LinkPoint = SFCService.LinkPoint.FirstOrDefault(p => p.Name == "T3") };
            DiagramViewModel.DirectAddItemCommand.Execute(tank3);

            ConnectorViewModel conn1 = new ConnectorViewModel(tank1.Output[0], k1.Input[0]);
            DiagramViewModel.DirectAddItemCommand.Execute(conn1);

            ConnectorViewModel conn2 = new ConnectorViewModel(tank1.Output[0], k2.Input[0]);
            DiagramViewModel.DirectAddItemCommand.Execute(conn2);

            ConnectorViewModel conn3 = new ConnectorViewModel(tank1.Output[0], k3.Input[0]);
            DiagramViewModel.DirectAddItemCommand.Execute(conn3);

            ConnectorViewModel conn4 = new ConnectorViewModel(k1.Output[0], tank2.Input[0]);
            DiagramViewModel.DirectAddItemCommand.Execute(conn4);

            ConnectorViewModel conn5 = new ConnectorViewModel(k2.Output[0], tank2.Input[0]);
            DiagramViewModel.DirectAddItemCommand.Execute(conn5);

            ConnectorViewModel conn6 = new ConnectorViewModel(k3.Output[0], tank2.Input[0]);
            DiagramViewModel.DirectAddItemCommand.Execute(conn6);

            ConnectorViewModel conn7 = new ConnectorViewModel(tank2.Output[1], k4.Input[0]);
            DiagramViewModel.DirectAddItemCommand.Execute(conn7);

            ConnectorViewModel conn8 = new ConnectorViewModel(k4.Output[0], tank3.Input[0]);
            DiagramViewModel.DirectAddItemCommand.Execute(conn8);
            #endregion

            DiagramViewModel.ClearSelectedItems();


        }


    }
}
