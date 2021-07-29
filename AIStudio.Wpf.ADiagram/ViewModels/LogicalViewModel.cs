using AIStudio.Wpf.ADiagram.ViewModels;
using AIStudio.Wpf.Logical.ViewModels;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Util.DiagramDesigner;

namespace AIStudio.Wpf.Logical
{
    public class LogicalViewModel : DiagramsViewModel
    {
        public LogicalViewModel(string title, string status, DiagramType diagramType) : base(title, status, diagramType)
        {

        }
        public LogicalViewModel(string filename) : base(filename)
        {

        }

        protected override void InitDiagramViewModel()
        {
            base.InitDiagramViewModel();

            DiagramViewModel.ShowGrid = true;
            DiagramViewModel.GridCellSize = new Size(150, 100);
            DiagramViewModel.PageSizeOrientation = PageSizeOrientation.Horizontal;
            DiagramViewModel.CellHorizontalAlignment = CellHorizontalAlignment.Left;
            DiagramViewModel.CellVerticalAlignment = CellVerticalAlignment.None;

            DiagramViewModel.Items.CollectionChanged += Items_CollectionChanged;
        }

        protected override void Init()
        {
            base.Init();

            TimerDesignerItemViewModel timer = new TimerDesignerItemViewModel() { Left = 0, Top = 10 };
            timer.Value = 0.5;
            DiagramViewModel.DirectAddItemCommand.Execute(timer);

            InputItemViewModel in1 = new InputItemViewModel() { Left = 0, Top = 50 };
            in1.LinkPoint = LogicalService.LinkPoint[0];
            DiagramViewModel.DirectAddItemCommand.Execute(in1);

            InputItemViewModel in2 = new InputItemViewModel() { Left = 0, Top = 80 };
            in2.LinkPoint = LogicalService.LinkPoint[1];
            DiagramViewModel.DirectAddItemCommand.Execute(in2);

            AddGateItemViewModel item1 = new AddGateItemViewModel() { Left = 150, Top = 50 };
            DiagramViewModel.DirectAddItemCommand.Execute(item1);

            ConstantDesignerItemViewModel constant = new ConstantDesignerItemViewModel() { Left = 150, Top = 118 };
            DiagramViewModel.DirectAddItemCommand.Execute(constant);

            GTGateItemViewModel gTGate = new GTGateItemViewModel() { Left = 300, Top = 50 };
            DiagramViewModel.DirectAddItemCommand.Execute(gTGate);

            InputItemViewModel in3 = new InputItemViewModel() { Left = 300, Top = 118 };
            in3.LinkPoint = LogicalService.LinkPoint[2];
            DiagramViewModel.DirectAddItemCommand.Execute(in3);

            InputItemViewModel in4 = new InputItemViewModel() { Left = 300, Top = 148 };
            in4.LinkPoint = LogicalService.LinkPoint[3];
            DiagramViewModel.DirectAddItemCommand.Execute(in4);

            SELGateItemViewModel sELGate = new SELGateItemViewModel() { Left = 450, Top = 50 };
            DiagramViewModel.DirectAddItemCommand.Execute(sELGate);

            OutputItemViewModel out1 = new OutputItemViewModel() { Left = 600, Top = 50 };
            out1.LinkPoint = LogicalService.LinkPoint[4];
            DiagramViewModel.DirectAddItemCommand.Execute(out1);

            ConnectorViewModel connector1 = new ConnectorViewModel(in1.Output[0], item1.Input[0]);
            DiagramViewModel.DirectAddItemCommand.Execute(connector1);

            ConnectorViewModel connector2 = new ConnectorViewModel(in2.Output[0], item1.Input[1]);
            DiagramViewModel.DirectAddItemCommand.Execute(connector2);

            ConnectorViewModel connector3 = new ConnectorViewModel(item1.Output[0], gTGate.Input[0]);
            DiagramViewModel.DirectAddItemCommand.Execute(connector3);

            ConnectorViewModel connector4 = new ConnectorViewModel(constant.Output[0], gTGate.Input[1]);
            DiagramViewModel.DirectAddItemCommand.Execute(connector4);

            ConnectorViewModel connector5 = new ConnectorViewModel(gTGate.Output[0], sELGate.Input[0]);
            DiagramViewModel.DirectAddItemCommand.Execute(connector5);

            ConnectorViewModel connector6 = new ConnectorViewModel(in3.Output[0], sELGate.Input[1]);
            DiagramViewModel.DirectAddItemCommand.Execute(connector6);

            ConnectorViewModel connector7 = new ConnectorViewModel(in4.Output[0], sELGate.Input[2]);
            DiagramViewModel.DirectAddItemCommand.Execute(connector7);

            ConnectorViewModel connector8 = new ConnectorViewModel(sELGate.Output[0], out1.Input[0]);
            DiagramViewModel.DirectAddItemCommand.Execute(connector8);
        }

        private void Items_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (var item in e.OldItems.OfType<TimerDesignerItemViewModel>())
                {
                    item.Do -= Do;
                }
            }
            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems.OfType<TimerDesignerItemViewModel>())
                {
                    item.Do += Do;
                }
            }

            RaisePropertyChanged("Items");
        }

        protected override bool AddVerify(SelectableDesignerItemViewModelBase arg)
        {
            if (base.AddVerify(arg) == false)
                return false;

            if (arg is ConnectorViewModel connector)
            {
                if (connector.SinkConnectorInfo is FullyCreatedConnectorInfo fully)
                {
                    if (DiagramViewModel.Items.OfType<ConnectorViewModel>().Any(p => p.SinkConnectorInfo == fully))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private void Do()
        {
            foreach (var item in DiagramViewModel.Items.OfType<ConstantDesignerItemViewModel>())
            {
                foreach (var output in item.Output)
                {
                    output.Value.ConnectorValue = item.Value;
                    output.Value.ColorViewModel.FillColor.Color = Colors.Green;
                }
            }

            foreach (var item in DiagramViewModel.Items.OfType<InputItemViewModel>())
            {
                if (item.LinkPoint != null)
                {
                    foreach (var output in item.Output)
                    {
                        output.Value.ConnectorValue = item.LinkPoint.Value;
                        output.Value.ColorViewModel.FillColor.Color = Colors.Green;
                    }
                }
            }

            foreach (var item in DiagramViewModel.Items.OfType<LogicalGateItemViewModelBase>().OrderBy(p => p.OrderNumber))
            {
                if (item.LogicalType != LogicalType.None)
                {
                    foreach (var input in item.Input)
                    {
                        var connector = GetSourceItem(input.Value);
                        if (connector == null)
                        {
                            continue;
                        }

                        if (connector.SourceConnectorInfo.DataItem is LogicalGateItemViewModelBase)
                        {
                            input.Value.ConnectorValue = connector.SourceConnectorInfo.ConnectorValue;

                            input.Value.ColorViewModel.FillColor.Color = connector.SourceConnectorInfo.ColorViewModel.FillColor.Color;
                            connector.ColorViewModel.LineColor.Color = connector.SourceConnectorInfo.ColorViewModel.FillColor.Color;

                            if (item.LogicalType == LogicalType.Output)
                            {
                                input.Value.ValueTypePoint = connector.SourceConnectorInfo.ValueTypePoint;
                            }
                            else if (item.LogicalType == LogicalType.NOT)
                            {
                                input.Value.ValueTypePoint = (connector.SourceConnectorInfo.ValueTypePoint == ValueTypePoint.Bool) ? ValueTypePoint.Bool : ValueTypePoint.Int;
                            }
                        }
                    }

                    foreach (var output in item.Output)
                    {
                        if (item.LogicalType == LogicalType.Output)
                        {
                            var first = item.Input.Values.FirstOrDefault();
                            output.Value.ConnectorValue = first.ConnectorValue;
                            output.Value.ValueTypePoint = first.ValueTypePoint;
                            (item as OutputItemViewModel).Value = first.ConnectorValue;
                            (item as OutputItemViewModel).LinkPoint.Value = first.ConnectorValue;
                        }
                        else if (item.LogicalType == LogicalType.ADD)
                        {
                            output.Value.ConnectorValue = item.Input.Values.Select(p => p.ConnectorValue).Sum();
                        }
                        else if (item.LogicalType == LogicalType.SUB)
                        {
                            var first = item.Input.Values.Select(p => p.ConnectorValue).FirstOrDefault();
                            var second = item.Input.Values.Where((value, index) => index != 0).Select(p => p.ConnectorValue).Sum();
                            output.Value.ConnectorValue = first - second;
                        }
                        else if (item.LogicalType == LogicalType.MUL)
                        {
                            double result = 0;
                            foreach (var input in item.Input.Values)
                            {
                                if (result == 0)
                                {
                                    result = 1;
                                }
                                result *= input.ConnectorValue;
                            }
                            output.Value.ConnectorValue = result;
                        }
                        else if (item.LogicalType == LogicalType.DIV)
                        {
                            double result = item.Input.Values.Select(p => p.ConnectorValue).FirstOrDefault();
                            foreach (var input in item.Input.Values.Where((value, index) => index != 0))
                            {
                                result /= input.ConnectorValue;
                            }
                            output.Value.ConnectorValue = result;
                        }
                        else if (item.LogicalType == LogicalType.AVE)
                        {
                            output.Value.ConnectorValue = item.Input.Values.Select(p => p.ConnectorValue).Average();
                        }
                        else if (item.LogicalType == LogicalType.MOD)
                        {
                            output.Value.ConnectorValue = item.Input[0].ConnectorValue % item.Input[1].ConnectorValue;
                        }
                        else if (item.LogicalType == LogicalType.AND)
                        {
                            output.Value.ConnectorValue = Convert.ToInt32(item.Input[0].ConnectorValue) & Convert.ToInt32(item.Input[1].ConnectorValue);
                        }
                        else if (item.LogicalType == LogicalType.OR)
                        {
                            output.Value.ConnectorValue = Convert.ToInt32(item.Input[0].ConnectorValue) | Convert.ToInt32(item.Input[1].ConnectorValue);
                        }
                        else if (item.LogicalType == LogicalType.XOR)
                        {
                            output.Value.ConnectorValue = Convert.ToInt32(Convert.ToInt32(item.Input[0].ConnectorValue) ^ Convert.ToInt32(item.Input[1].ConnectorValue));
                        }
                        else if (item.LogicalType == LogicalType.NOT)
                        {
                            if (item.Input[0].ValueTypePoint == ValueTypePoint.Bool)
                            {
                                output.Value.ConnectorValue = Convert.ToInt32(!Convert.ToBoolean(item.Input[0].ConnectorValue));
                                output.Value.ValueTypePoint = ValueTypePoint.Bool;
                            }
                            else
                            {
                                output.Value.ConnectorValue = ~Convert.ToInt32(item.Input[0].ConnectorValue);
                                output.Value.ValueTypePoint = ValueTypePoint.Int;
                            }

                        }
                        else if (item.LogicalType == LogicalType.SHL)
                        {
                            output.Value.ConnectorValue = Convert.ToInt32(item.Input[0].ConnectorValue) << Convert.ToInt32(item.Input[1].ConnectorValue);
                        }
                        else if (item.LogicalType == LogicalType.SHR)
                        {
                            output.Value.ConnectorValue = Convert.ToInt32(item.Input[0].ConnectorValue) >> Convert.ToInt32(item.Input[1].ConnectorValue);
                        }
                        else if (item.LogicalType == LogicalType.ROL)
                        {
                            output.Value.ConnectorValue = (Convert.ToInt32(item.Input[0].ConnectorValue) << Convert.ToInt32(item.Input[1].ConnectorValue)) | (Convert.ToInt32(item.Input[0].ConnectorValue) >> 32 - Convert.ToInt32(item.Input[1].ConnectorValue));
                        }
                        else if (item.LogicalType == LogicalType.ROR)
                        {
                            output.Value.ConnectorValue = (Convert.ToInt32(item.Input[0].ConnectorValue) >> Convert.ToInt32(item.Input[1].ConnectorValue)) | (Convert.ToInt32(item.Input[0].ConnectorValue) << 32 - Convert.ToInt32(item.Input[1].ConnectorValue));
                        }
                        else if (item.LogicalType == LogicalType.SEL)
                        {
                            output.Value.ConnectorValue = (item.Input[0].ConnectorValue == output.Key) ? item.Input[1].ConnectorValue : item.Input[2].ConnectorValue;
                        }
                        else if (item.LogicalType == LogicalType.MAX)
                        {
                            output.Value.ConnectorValue = item.Input.Values.Select(p => p.ConnectorValue).Max();
                        }
                        else if (item.LogicalType == LogicalType.MIN)
                        {
                            output.Value.ConnectorValue = item.Input.Values.Select(p => p.ConnectorValue).Min();
                        }
                        else if (item.LogicalType == LogicalType.LIMIT)
                        {
                            output.Value.ConnectorValue = (item.Input[0].ConnectorValue > item.Input[1].ConnectorValue) ? item.Input[1].ConnectorValue : item.Input[0].ConnectorValue;
                        }
                        else if (item.LogicalType == LogicalType.GT)
                        {
                            output.Value.ConnectorValue = (item.Input[0].ConnectorValue > item.Input[1].ConnectorValue) ? 1 : 0;
                        }
                        else if (item.LogicalType == LogicalType.LT)
                        {
                            output.Value.ConnectorValue = (item.Input[0].ConnectorValue < item.Input[1].ConnectorValue) ? 1 : 0;
                        }
                        else if (item.LogicalType == LogicalType.GE)
                        {
                            output.Value.ConnectorValue = (item.Input[0].ConnectorValue >= item.Input[1].ConnectorValue) ? 1 : 0;
                        }
                        else if (item.LogicalType == LogicalType.LE)
                        {
                            output.Value.ConnectorValue = (item.Input[0].ConnectorValue <= item.Input[1].ConnectorValue) ? 1 : 0;
                        }
                        else if (item.LogicalType == LogicalType.EQ)
                        {
                            output.Value.ConnectorValue = (item.Input[0].ConnectorValue == item.Input[1].ConnectorValue) ? 1 : 0;
                        }
                        else if (item.LogicalType == LogicalType.NE)
                        {
                            output.Value.ConnectorValue = (item.Input[0].ConnectorValue != item.Input[1].ConnectorValue) ? 1 : 0;
                        }
                        else if (item.LogicalType == LogicalType.ABS)
                        {
                            output.Value.ConnectorValue = Math.Abs(item.Input[0].ConnectorValue);
                        }
                        else if (item.LogicalType == LogicalType.SQRT)
                        {
                            output.Value.ConnectorValue = Math.Sqrt(item.Input[0].ConnectorValue);
                        }
                        else if (item.LogicalType == LogicalType.LN)
                        {
                            output.Value.ConnectorValue = Math.Log10(item.Input[0].ConnectorValue);
                        }
                        else if (item.LogicalType == LogicalType.LOG)
                        {
                            output.Value.ConnectorValue = Math.Log(item.Input[0].ConnectorValue);
                        }
                        else if (item.LogicalType == LogicalType.EXP)
                        {
                            output.Value.ConnectorValue = Math.Exp(item.Input[0].ConnectorValue);
                        }
                        else if (item.LogicalType == LogicalType.SIN)
                        {
                            output.Value.ConnectorValue = Math.Sin(item.Input[0].ConnectorValue);
                        }
                        else if (item.LogicalType == LogicalType.COS)
                        {
                            output.Value.ConnectorValue = Math.Cos(item.Input[0].ConnectorValue);
                        }
                        else if (item.LogicalType == LogicalType.TAN)
                        {
                            output.Value.ConnectorValue = Math.Tan(item.Input[0].ConnectorValue);
                        }
                        else if (item.LogicalType == LogicalType.ASIN)
                        {
                            output.Value.ConnectorValue = Math.Asin(item.Input[0].ConnectorValue);
                        }
                        else if (item.LogicalType == LogicalType.ACOS)
                        {
                            output.Value.ConnectorValue = Math.Acos(item.Input[0].ConnectorValue);
                        }
                        else if (item.LogicalType == LogicalType.ATAN)
                        {
                            output.Value.ConnectorValue = Math.Atan(item.Input[0].ConnectorValue);
                        }
                        else if (item.LogicalType == LogicalType.EXPT)
                        {
                            output.Value.ConnectorValue = Math.Exp(item.Input[0].ConnectorValue);
                        }

                        if (output.Value.ValueTypePoint == ValueTypePoint.Bool)
                        {
                            if (output.Value.ConnectorValue == 0)
                            {
                                output.Value.ColorViewModel.FillColor.Color = Colors.Red;
                                if (item.LogicalType == LogicalType.Output)
                                {
                                    item.ColorViewModel.FillColor.Color = Colors.Red;
                                }
                            }
                            else
                            {
                                output.Value.ColorViewModel.FillColor.Color = Colors.Green;
                                if (item.LogicalType == LogicalType.Output)
                                {
                                    item.ColorViewModel.FillColor.Color = Colors.Green;
                                }
                            }
                        }
                        else
                        {
                            output.Value.ColorViewModel.FillColor.Color = Colors.Green;
                        }
                    }
                }
            }
        }

        private ConnectorViewModel GetSourceItem(FullyCreatedConnectorInfo sinkConnector)
        {
            foreach (var connector in DiagramViewModel.Items.OfType<ConnectorViewModel>())
            {
                if (connector.SinkConnectorInfo == sinkConnector)
                {
                    return connector;
                }
            }
            return null;
        }
    }
}
