using System.Windows;
using System.Windows.Controls;

namespace Ricciolo.Controls
{
	public class TreeListViewItem : TreeViewItem
	{
		private int _level = -1;

		public int Level
		{
			get
			{
				if (_level == -1)
				{
					TreeListViewItem treeListViewItem = ItemsControl.ItemsControlFromItemContainer(this) as TreeListViewItem;
					_level = ((treeListViewItem != null) ? (treeListViewItem.Level + 1) : 0);
				}
				return _level;
			}
		}

		static TreeListViewItem()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(TreeListViewItem), new FrameworkPropertyMetadata(typeof(TreeListViewItem)));
		}

		protected override DependencyObject GetContainerForItemOverride()
		{
			return new TreeListViewItem();
		}

		protected override bool IsItemItsOwnContainerOverride(object item)
		{
			return item is TreeListViewItem;
		}
	}
}
