using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Util.DiagramDesigner
{
    public class SelectionService
    {
        private IDiagramViewModel DiagramViewModel;

        public SelectionService(IDiagramViewModel diagramViewModel)
        {
            this.DiagramViewModel = diagramViewModel;
        }

        public void AddToSelection(ISelectable item)
        {
            if (item is IGroupable)
            {
                List<IGroupable> groupItems = GetGroupMembers(item as IGroupable);

                foreach (ISelectable groupItem in groupItems)
                {
                    groupItem.IsSelected = true;
                }
            }
            else
            {
                item.IsSelected = true;
            }
        }

        internal void RemoveFromSelection(ISelectable item)
        {
            if (item is IGroupable)
            {
                List<IGroupable> groupItems = GetGroupMembers(item as IGroupable);

                foreach (ISelectable groupItem in groupItems)
                {
                    groupItem.IsSelected = false;
                }
            }
            else
            {
                item.IsSelected = false;
            }
        }

        internal List<IGroupable> GetGroupMembers(IGroupable item)
        {
            IEnumerable<IGroupable> list = DiagramViewModel.Items.OfType<IGroupable>();
            IGroupable rootItem = GetRoot(list, item);
            if (rootItem == null)
            {
                return new List<IGroupable>();
            }
            return GetGroupMembers(list, rootItem);
        }

        internal IGroupable GetGroupRoot(IGroupable item)
        {
            IEnumerable<IGroupable> list = DiagramViewModel.Items.OfType<IGroupable>();
            return GetRoot(list, item);
        }

        private IGroupable GetRoot(IEnumerable<IGroupable> list, IGroupable node)
        {
            if (node == null || node.ParentId == Guid.Empty)
            {
                return node;
            }
            else
            {
                foreach (IGroupable item in list)
                {
                    if (item.Id == node.ParentId)
                    {
                        return GetRoot(list, item);
                    }
                }
                return null;
            }
        }

        private List<IGroupable> GetGroupMembers(IEnumerable<IGroupable> list, IGroupable parent)
        {
            List<IGroupable> groupMembers = new List<IGroupable>();
            groupMembers.Add(parent);

            var children = list.Where(node => node.ParentId == parent.Id && node.ParentId != Guid.Empty);

            foreach (IGroupable child in children)
            {
                groupMembers.AddRange(GetGroupMembers(list, child));
            }

            return groupMembers;
        }
    }
}
