using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using System.ComponentModel;

namespace Util.DiagramDesigner
{
    public interface IDiagramViewModel
    {
        string Name { get; set; }
        List<SelectableDesignerItemViewModelBase> SelectedItems { get; }
        ObservableCollection<SelectableDesignerItemViewModelBase> Items { get; }
        SelectionService SelectionService { get; }

        SimpleCommand CreateNewDiagramCommand { get; }
        SimpleCommand DirectAddItemCommand { get; }
        SimpleCommand AddItemCommand { get; }
        SimpleCommand RemoveItemCommand { get; }
        SimpleCommand DirectRemoveItemCommand { get; }
        SimpleCommand ClearSelectedItemsCommand { get; }
        SimpleCommand AlignTopCommand { get; }
        SimpleCommand AlignVerticalCentersCommand { get; }
        SimpleCommand AlignBottomCommand { get; }
        SimpleCommand AlignLeftCommand { get; }
        SimpleCommand AlignHorizontalCentersCommand { get; }
        SimpleCommand AlignRightCommand { get; }
        SimpleCommand BringForwardCommand { get; }
        SimpleCommand BringToFrontCommand { get; }
        SimpleCommand SendBackwardCommand { get; }
        SimpleCommand SendToBackCommand { get; }

        SimpleCommand DistributeHorizontalCommand { get; }
        SimpleCommand DistributeVerticalCommand { get; }
        SimpleCommand SelectAllCommand { get; }
        SimpleCommand UndoCommand { get; }
        SimpleCommand RedoCommand { get; }

        Func<SelectableDesignerItemViewModelBase, bool> OutAddVerify { get; set; }
        void ClearSelectedItems();
        bool BelongToSameGroup(IGroupable item1, IGroupable item2);
        Rect GetBoundingRectangle(IEnumerable<DesignerItemViewModelBase> items);
        void UpdateZIndex();
        Size PageSize { get;}
        PageSizeType PageSizeType { get; set; }
        bool ShowGrid { get; set; }
        Size GridCellSize { get; set; }
        PageSizeOrientation PageSizeOrientation { get; set; }
        double GridMargin { get; set; }
        Color GridColor { get; set; }
        Point CurrentPoint { get; set; }
        Color CurrentColor { get; set; }

        DiagramType DiagramType { get; set; }

        CellHorizontalAlignment CellHorizontalAlignment { get; set; }
        CellVerticalAlignment CellVerticalAlignment { get; set; }

        event PropertyChangedEventHandler PropertyChanged;

    }
}
