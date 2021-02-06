using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;

namespace Ricciolo.Controls
{
	public class TreeListView : TreeView
	{
		private GridViewColumnCollection _columns;

		public GridViewColumnCollection Columns
		{
			get
			{
				if (_columns == null)
				{
					_columns = new GridViewColumnCollection();
				}
				return _columns;
			}
		}

		static TreeListView()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(TreeListView), new FrameworkPropertyMetadata(typeof(TreeListView)));
		}

		protected override DependencyObject GetContainerForItemOverride()
		{
			return new TreeListViewItem();
		}

		protected override bool IsItemItsOwnContainerOverride(object item)
		{
			return item is TreeListViewItem;
		}

		protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
		{
			base.OnItemsChanged(e);
		}




		//
		// ժҪ:
		//     Identifies the System.Windows.Controls.ScrollViewer.CanContentScroll dependency
		////     property.
		//public static readonly DependencyProperty CanContentScrollProperty;
		////
		//// ժҪ:
		////     Identifies the System.Windows.Controls.ScrollViewer.VerticalScrollBarVisibility
		////     dependency property.
		//public static readonly DependencyProperty VerticalScrollBarVisibilityProperty;
		////
		//// ժҪ:
		////     Identifies the System.Windows.Controls.ScrollViewer.ScrollChanged routed event.
		//public static readonly RoutedEvent ScrollChangedEvent;
		////
		//// ժҪ:
		////     Identifies the System.Windows.Controls.ScrollViewer.ScrollableWidth dependency
		////     property.
		//public static readonly DependencyProperty ScrollableWidthProperty;
		////
		//// ժҪ:
		////     Identifies the System.Windows.Controls.ScrollViewer.ScrollableHeight dependency
		////     property.
		//public static readonly DependencyProperty ScrollableHeightProperty;
		////
		//// ժҪ:
		////     Identifies the System.Windows.Controls.ScrollViewer.HorizontalScrollBarVisibility
		////     dependency property.
		//public static readonly DependencyProperty HorizontalScrollBarVisibilityProperty;
		////
		//// ժҪ:
		////     Identifies the System.Windows.Controls.ScrollViewer.ComputedVerticalScrollBarVisibility
		////     dependency property.
		//public static readonly DependencyProperty ComputedVerticalScrollBarVisibilityProperty;
		////
		//// ժҪ:
		////     Identifies the System.Windows.Controls.ScrollViewer.ComputedHorizontalScrollBarVisibility
		////     dependency property.
		//public static readonly DependencyProperty ComputedHorizontalScrollBarVisibilityProperty;
	}
}
