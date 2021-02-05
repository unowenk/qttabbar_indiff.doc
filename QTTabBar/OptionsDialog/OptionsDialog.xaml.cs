﻿//    This file is part of QTTabBar, a shell extension for Microsoft
//    Windows Explorer.
//    Copyright (C) 2007-2010  Quizo, Paul Accisano
//
//    QTTabBar is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    QTTabBar is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with QTTabBar.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using QTTabBarLib.Interop;
using Rectangle = System.Drawing.Rectangle;
using Font = System.Drawing.Font;
using Bitmap = System.Drawing.Bitmap;
using Keys = System.Windows.Forms.Keys;
using Screen = System.Windows.Forms.Screen;
using System.Reflection;
using System.IO;
using System.Text;


using BandObjectLib.Interop;
using BandObjectLib.Interop.QTTabBar;

namespace QTTabBarLib {
    /// <summary>
    /// Interaction logic for OptionsDialog.xaml
    /// </summary>
    internal partial class OptionsDialog : Window {
        private static OptionsDialog instance;
        private static Thread instanceThread;
        private static Thread launchingThread;
        private Config WorkingConfig;
        
        #region ---------- Static Methods ----------

        public static void Open() {
            InstanceManager.ExecuteOnServerProcess(OpenInternal, false);
        }

        private static void OpenInternal() {
            lock(typeof(OptionsDialog)) {
                // Prevent reentrant calls that might happen during the Wait call below.
                if(launchingThread == Thread.CurrentThread) return;
                try {
                    launchingThread = Thread.CurrentThread;

                    if(instance == null) {
                        instanceThread = new Thread(ThreadEntry) { IsBackground = true };
                        instanceThread.SetApartmentState(ApartmentState.STA);
                        lock(instanceThread) {
                            instanceThread.Start();
                            // Don't return until we know that the instance is created!
                            Monitor.Wait(instanceThread);
                        }
                    }
                    else {
                        instance.Dispatcher.Invoke(new Action(() => {
                            if(instance.WindowState == WindowState.Minimized) {
                               // MessageBox.Show("instance" + " -> " + instance.WindowState + " ->" + instance.WindowState) ;
                                instance.WindowState = WindowState.Normal;
                            }
                            else {
                                instance.Topmost = true;
                                instance.Activate();
                                instance.Topmost = false;

                                // add by qwop.

                                /*MessageBox.Show("activate" + " -> " + instance.lastSelected);
                                if (null != instance.lastSelected)
                                instance.lastSelected.IsSelected = true;*/
                            }
                        }));
                    }
                }
                finally {
                    launchingThread = null;
                }
            }
        }

        public static void ForceClose() {
            lock(typeof(OptionsDialog)) {
                if(instance != null) {
                    instance.Dispatcher.Invoke(new Action(() => instance.Close()));
                }
            }
        }

        private static void ThreadEntry() {
            instance = new OptionsDialog();
            lock(instanceThread) {
                Monitor.Pulse(instanceThread);
            }
            instance.Closed += (sender, e) => {
                // We can't immediately shut down here, because ForceClose may be holding the lock.
                Dispatcher.CurrentDispatcher.BeginInvokeShutdown(DispatcherPriority.Input);
            };
            Dispatcher.CurrentDispatcher.ShutdownStarted += (sender, e) => {
                lock(typeof(OptionsDialog)) {
                    instance = null;
                }
            };
            instance.Show();

            /***  TO delete */
            // load the remember lastSelectedIndex by qwop.
            // MessageBox.Show("" + lastSelectedIndex);
            instance.lstCategories.SelectedIndex = instance.WorkingConfig.desktop.lstSelectedIndex;
            /***  TO delete */

            Dispatcher.Run();
        }

        #endregion

        private OptionsDialog() {
            Initialized += (sender, args) => Topmost = true;
            ContentRendered += (sender, args) => Topmost = false;
            InitializeComponent();

            int i = 0;
            tabbedPanel.ItemsSource = new OptionsDialogTab[] {
                new Options01_Window        { Index = i++},
                new Options02_Tabs          { Index = i++},
                new Options03_Tweaks        { Index = i++},
                new Options04_Tooltips      { Index = i++},
                new Options05_General       { Index = i++},
                new Options06_Appearance    { Index = i++},
                new Options07_Mouse         { Index = i++},
                new Options08_Keys          { Index = i++},
                new Options09_Groups        { Index = i++},
                new Options10_Apps          { Index = i++},
                new Options11_ButtonBar     { Index = i++},
                new Options12_Plugins       { Index = i++},
                new Options13_Language      { Index = i++},
                new Options14_About         { Index = i}
            };

            // For some reason, on XP, the Options dialog starts up with a blank tab
            // This is the only way I've found to fix it
            // TODO: Investigate and see if there's a better way
            Loaded += (sender, args) => {
                tabbedPanel.SelectedIndex = 1;
                tabbedPanel.SelectedIndex = 0;
            };

            WorkingConfig = QTUtility2.DeepClone(ConfigManager.LoadedConfig);
            foreach(OptionsDialogTab tab in tabbedPanel.Items) {
                tab.WorkingConfig = WorkingConfig;
                IHotkeyContainer ihc = tab as IHotkeyContainer;
                if(ihc != null) ihc.NewHotkeyRequested += ProcessNewHotkey;
                tab.InitializeConfig();
            }


            //////////// setting by qwop .
            setByQwop();
        }


        #region setting by qwop
        /// <summary>
        /// 利用主屏幕的宽度设置，选项窗体的宽度， 和绝对高度。
        /// 可以生成 WorkingConfig 配置 初始化的 值。 
        /// 方法: generateInitConfig()
        /// </summary>
        private void setByQwop() {
 /*           // 必须使用  using Rectangle = System.Drawing.Rectangle;
            // 不然会有二义性 ambiguous
            Rectangle rect = Screen.PrimaryScreen.Bounds;
            // (屏幕的宽度 - 窗体宽度) / 2
            double left = (rectangle.Width - this.Width) / 2;
            double top = (rectangle.Height); 
            // 向左偏移 10 个像素
            left -= 10;
            this.Left = left;
*/
            Rectangle rect = Screen.PrimaryScreen.Bounds;
            this.Left = ((rect.Width - this.Width) / 2) - 10;
            this.Top = 0; //  rect.Height * (0.15);


            ///////////////////// change last selected index.
            //lstCategories.SelectedIndex = WorkingConfig.desktop.lstSelectedIndex;

            ////////////////////////////////////////
            // generateInitConfig();
        }

        /// <summary>
        /// 反射当前的 WorkingConfig 配置的内部属性所有的值
        /// 如果内部的值为空则生成赋空.
        /// Author: qwop
        /// Date:   2012-07-03
        /// </summary>
        private void generateInitConfig() {
            StreamWriter sw = File.CreateText("c:\\qttabbar_default_config_init.txt");

            PropertyInfo[] configProperties = WorkingConfig.GetType().GetProperties();
            Object _configObj = null;
            PropertyInfo[] _configObjProperties = null;
            foreach (PropertyInfo p in configProperties)
            {
                _configObj = p.GetValue(WorkingConfig, null);

                if (_configObj != null)
                {
                    _configObjProperties = _configObj.GetType().GetProperties();
                    sw.WriteLine(_configObj);
                    foreach (PropertyInfo _configProperty in _configObjProperties)
                    {
                        StringBuilder b = new StringBuilder();

                        object po = _configProperty.GetValue(_configObj, null);

                        if (null != po)
                            if (_configProperty.PropertyType == typeof(String))
                            {
                                b.Append("\"").Append(_configProperty.GetValue(_configObj, null)).Append("\"");
                            }
                            else if (_configProperty.PropertyType.IsArray) /* property type is array. */
                            {
                                /* join like this "new System.Int32[] {1, 2, 3}; " */
                                Array arr = (Array)_configProperty.GetValue(_configObj, null);
                                b.Append("new ").Append(arr.GetType()).Append("{");
                                for (int i = 0; i < arr.Length; i++)
                                {
                                    b.Append(arr.GetValue(i)).Append(",");
                                }
                                b.Append("}");
                            }
                            else
                            {
                                b.Append(_configProperty.GetValue(_configObj, null).ToString().ToLower());
                            }
                        else
                        {
                            b.Append("null");
                        }
                        b.Append(";");
                        sw.WriteLine(_configProperty.Name + "\t=\t" + b.ToString());
                    }// end for each 
                }
                sw.WriteLine();
            }
            sw.Flush();
            sw.Close();         
        }
        #endregion

        private void UpdateOptions() {
            foreach(OptionsDialogTab tab in tabbedPanel.Items) {
                tab.CommitConfig();
            }
            ConfigManager.LoadedConfig = QTUtility2.DeepClone(WorkingConfig);
            ConfigManager.WriteConfig();
            ConfigManager.UpdateConfig();
        }

        private void CategoryListBoxItem_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e) {
            lstCategories.Focus();
            e.Handled = true;
        }

        private void CategoryListBoxItem_PreviewMouseDown(object sender, MouseButtonEventArgs e) {
            ListBoxItem lbt = ((ListBoxItem)sender);
            lbt.Focus();
            lbt.IsSelected = true;
            e.Handled = true;

            // the last selected list box item.
            WorkingConfig.desktop.lstSelectedIndex = lstCategories.SelectedIndex;
        }

        private void CategoryListBoxItem_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            
//            ListBoxItem lbt = ((ListBoxItem)sender);
//           
//            lbt.Focus();
//            lbt.IsSelected = true;
//            e.Handled = true;
//
//            // the last selected list box item.
//            WorkingConfig.desktop.lstSelectedIndex = lstCategories.SelectedIndex;
//
//
//           
/*
            if (e.Delta < 0)
            {
                MessageBox.Show("fucdk you:" + e.Delta);
            }
            else
            {
                MessageBox.Show("fucdk you:" + e.Delta);
            }
*/
        }


        private void lstCategories_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {

  /*          
            int index = WorkingConfig.desktop.lstSelectedIndex;
            int maxSize = 14 - 1;
            int minSize = 0;
            if (e.Delta < 0)
            {
                if (index == minSize)
                {
                    index = maxSize;
                }
                else
                {
                    index--;
                }
            }
            else
            {
                if (index == maxSize)
                {
                    index = minSize;
                }
                else
                {
                    index++;
                }
            }

            bool up = true, traverseFolders = true;
            ITreeViewItem sel = lstCategories.SelectedItem as ITreeViewItem;
            if (sel == null) return;
            IList list = sel.ParentList;
            index = list.IndexOf(sel);
            if (index == -1) return;
            bool expanded = sel.IsExpanded;
            if (up && index == 0)
            {
                if (!traverseFolders || sel.ParentItem == null) return;
                IList parentList = sel.ParentItem.ParentList;
                int parentIndex = parentList.IndexOf(sel.ParentItem);
                if (parentIndex == -1) return;
                list.RemoveAt(index);
                parentList.Insert(parentIndex, sel);
            }
            else if (!up && index == list.Count - 1)
            {
                if (!traverseFolders || sel.ParentItem == null) return;
                IList parentList = sel.ParentItem.ParentList;
                int parentIndex = parentList.IndexOf(sel.ParentItem);
                if (parentIndex == -1) return;
                list.RemoveAt(index);
                parentList.Insert(parentIndex + 1, sel);
            }
            else
            {
                ITreeViewItem next = (ITreeViewItem)list[index + (up ? -1 : 1)];
                if (traverseFolders && next.ChildrenList != null && (next.IsExpanded || next.ChildrenList.Count == 0))
                {
                    list.RemoveAt(index);
                    list = next.ChildrenList;
                    list.Insert(up ? list.Count : 0, sel);
                    next.IsExpanded = true;
                }
                else
                {
                    list.RemoveAt(index);
                    list.Insert(index + (up ? -1 : 1), sel);
                }
            }
            sel.IsExpanded = expanded;
            sel.IsSelected = true;
*/
           
//			MessageBox.Show( "" + index  + " " + lstCategories.Items[index].ToString() );
         /*   ListBoxItem lbt = (ListBoxItem)lstCategories.Items[index];
			lbt.Focus();
			lbt.IsSelected = true;
			e.Handled = true;
			lstCategories.SelectedIndex = WorkingConfig.desktop.lstSelectedIndex = index; */
        }

        private void btnResetPage_Click(object sender, RoutedEventArgs e) {
            MessageBoxResult response = MessageBox.Show(
                    QTUtility.TextResourcesDic["OptionsDialog"][1],
                    QTUtility.TextResourcesDic["OptionsDialog"][3],
                    MessageBoxButton.OKCancel, MessageBoxImage.Question, MessageBoxResult.Cancel);
            if(response == MessageBoxResult.OK) {
                ((OptionsDialogTab)tabbedPanel.SelectedItem).ResetConfig();   
            }
        }

        private void btnResetAll_Click(object sender, RoutedEventArgs e) {
            MessageBoxResult response = MessageBox.Show(
                    QTUtility.TextResourcesDic["OptionsDialog"][2],
                    QTUtility.TextResourcesDic["OptionsDialog"][3],
                    MessageBoxButton.OKCancel, MessageBoxImage.Question, MessageBoxResult.Cancel);
            if(response == MessageBoxResult.OK) {
                foreach(OptionsDialogTab tab in tabbedPanel.Items) {
                    tab.ResetConfig();
                }
            }
        }

        private void btnOK_Click(object sender, RoutedEventArgs e) {
            UpdateOptions();
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e) {
            Close();
        }

        private void btnApply_Click(object sender, RoutedEventArgs e) {
            UpdateOptions();
            foreach(OptionsDialogTab tab in tabbedPanel.Items) {
                tab.InitializeConfig();
            }
        }

        private bool ProcessNewHotkey(KeyEventArgs e, Keys current, out Keys finalKey) {
            finalKey = Keys.None;
            Key wpfKey = (e.Key == Key.System ? e.SystemKey : e.Key);
            ModifierKeys wpfModKeys = Keyboard.Modifiers;

            // Ignore modifier keys.
            if(wpfKey == Key.LeftShift || wpfKey == Key.RightShift
                    || wpfKey == Key.LeftCtrl || wpfKey == Key.RightCtrl
                    || wpfKey == Key.LeftAlt || wpfKey == Key.RightAlt
                    || wpfKey == Key.LWin || wpfKey == Key.RWin) {
                return false;
            }

            Keys hotkey = (Keys)KeyInterop.VirtualKeyFromKey(wpfKey);
            if(hotkey == Keys.Escape) {
                // Escape = clear
                return true;
            }

            // Urgh, so many conversions between WPF and WinForms...
            Keys modkey = Keys.None;
            if((wpfModKeys & ModifierKeys.Alt) != 0)        modkey |= Keys.Alt;
            if((wpfModKeys & ModifierKeys.Control) != 0)    modkey |= Keys.Control;
            if((wpfModKeys & ModifierKeys.Shift) != 0)      modkey |= Keys.Shift;

            // don't allow keystrokes without at least one modifier key
            if(modkey == Keys.None) {
                return false;
            }

            modkey |= hotkey;
            if(modkey == current) {
                // trying to assign the same hotkey
                return false;
            }

            // keys not allowed even with any modifier keys 
            switch(hotkey) {
                case Keys.None:
                case Keys.Enter:
                case Keys.ControlKey:
                case Keys.ShiftKey:
                case Keys.Menu:				// Alt itself
                case Keys.NumLock:
                case Keys.ProcessKey:
                case Keys.IMEConvert:
                case Keys.IMENonconvert:
                case Keys.KanaMode:
                case Keys.KanjiMode:
                    return false;
            }

            // keys not allowed as one key
            switch(modkey) {
                case Keys.LWin:
                case Keys.RWin:
                case Keys.Delete:
                case Keys.Apps:
                case Keys.Tab:
                case Keys.Left:
                case Keys.Up:
                case Keys.Right:
                case Keys.Down:
                    return false;
            }

            // invalid key combinations 
            switch(modkey) {
                case Keys.Control | Keys.C:
                case Keys.Control | Keys.A:
                case Keys.Control | Keys.Z:
                case Keys.Control | Keys.V:
                case Keys.Control | Keys.X:
                // case Keys.Alt | Keys.Left:
                // case Keys.Alt | Keys.Right:
                case Keys.Alt | Keys.F4:
                    System.Media.SystemSounds.Hand.Play();
                    return false;
            }

            // check for key conflicts
            string Conflict =
                    QTUtility.TextResourcesDic["Options_Page08_Keys"][6] +
                    Environment.NewLine + "{0}" + Environment.NewLine + Environment.NewLine +
                    QTUtility.TextResourcesDic["Options_Page08_Keys"][7];
            IHotkeyEntry conflictingEntry = tabbedPanel.Items
                    .OfType<IHotkeyContainer>()
                    .SelectMany(hc => hc.GetHotkeyEntries())
                    .FirstOrDefault(entry => entry.ShortcutKey == modkey);
            if(conflictingEntry != null) {
                if(MessageBoxResult.OK != MessageBox.Show(
                        string.Format(Conflict, conflictingEntry.KeyActionText),
                        QTUtility.TextResourcesDic["Options_Page08_Keys"][8],
                        MessageBoxButton.OKCancel, MessageBoxImage.Warning)) {
                    return false;
                }
                conflictingEntry.ShortcutKey = Keys.None;              
            }
            finalKey = modkey;
            return true;
        }

        #region ---------- Converters ----------

        // Inverts the value of a boolean
        internal class BoolInverterConverter : IValueConverter {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
                return value is bool ? !(bool)value : value;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
                return value is bool ? !(bool)value : value;
            }
        }

        // Converts between booleans and one using logical and.
        internal class LogicalAndMultiConverter : IMultiValueConverter {
            public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
                return values.All(b => b is bool && (bool)b);
            }

            public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
                return new object[] { value };
            }
        }

        // Converts between many booleans and a string by StringJoining them.
        internal class BoolJoinMultiConverter : IMultiValueConverter {
            public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
                return values.StringJoin(",");
            }

            public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
                return ((string)value).Split(',').Select(s => (object)bool.Parse(s)).ToArray();
            }
        }

        // Converts between a boolean and a string by comparing the string to the 
        // passed parameter.
        [ValueConversion(typeof(string), typeof(bool))]
        internal class StringEqualityConverter : IValueConverter {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
                return (string)parameter == (string)value;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
                return (bool)value ? parameter : Binding.DoNothing;
            }
        }

        // Converts Bitmaps to ImageSources.
        [ValueConversion(typeof(Bitmap), typeof(ImageSource))]
        internal class BitmapToImageSourceConverter : IValueConverter {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
                if(value == null || !(value is Bitmap)) return null;
                IntPtr hBitmap = ((Bitmap)value).GetHbitmap();
                try {
                    return Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero,
                            Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                }
                finally {
                    PInvoke.DeleteObject(hBitmap);
                }
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
                throw new NotSupportedException();
            }
        }

        // Converts between Colors and Brushes
        [ValueConversion(typeof(System.Drawing.Color), typeof(Brush))]
        internal class ColorToBrushConverter : IValueConverter {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
                var c = (System.Drawing.Color)(value ?? System.Drawing.Color.Red);
                return new SolidColorBrush(Color.FromArgb(c.A, c.R, c.G, c.B));
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
                throw new NotSupportedException();
            }
        }

        // Converts between Fonts and strings.
        [ValueConversion(typeof(Font), typeof(string))]
        internal class FontStringConverter : IValueConverter {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
                if(value == null) return "";
                Font font = (Font)value;
                return string.Format("{0}, {1} pt", font.Name, Math.Round(font.SizeInPoints));
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
                throw new NotSupportedException();
            }
        }

        // Converts an object to its class name.
        // You can create ObjectToTypeConverter instead of this,
        // but VS2010 WPF Designer would refuse an expression like {x:Type SomeClass+NestedClass}
        [ValueConversion(typeof(object), typeof(string))]
        internal class ObjectToClassNameConverter : IValueConverter {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
                return value == null ? null : value.GetType().Name;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
                throw new NotSupportedException();
            }
        }

        #endregion
    }

    internal interface IHotkeyEntry {
        Keys ShortcutKey { get; set; }
        string KeyActionText { get; }
    }

    internal delegate bool NewHotkeyRequestedHandler(KeyEventArgs keyEvent, Keys currentKey, out Keys newKey);
    internal interface IHotkeyContainer {
        IEnumerable<IHotkeyEntry> GetHotkeyEntries();
        event NewHotkeyRequestedHandler NewHotkeyRequested;
    }

    /// <summary>
    /// The base class for the tab pages of the OptionsDialog.
    /// Contains a few things common to more than one page.
    /// </summary>
    internal abstract class OptionsDialogTab : UserControl {
        public static readonly DependencyProperty WorkingConfigProperty =
                DependencyProperty.Register("WorkingConfig", typeof(Config), typeof(OptionsDialogTab),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public Config WorkingConfig {
            get { return (Config)GetValue(WorkingConfigProperty); }
            set { SetValue(WorkingConfigProperty, value); }
        }

        // This is the index of the resource string that will be displayed in the category list.
        public int Index { get; set; }

        // Called when the options dialog is first shown, and when the user clicks Apply (after commit)
        public abstract void InitializeConfig();

        // Called when the user clicks the Reset buttons.
        public abstract void ResetConfig();

        // Called when the user clicks Apply or OK.
        public abstract void CommitConfig();
        

        #region ---------- Interfaces / Helper Classes ----------

        // Interface for Binding Classes that have some editable component
        protected interface IEditableEntry {
            bool IsEditing { get; set; }
        }

        // Interface for Binding Classes that belong to a ParentedCollection list.
        protected interface IChildItem {
            IList ParentList { get; set; }
            ITreeViewItem ParentItem { get; set; }
        }

        // Interface for TreeView items, to control selectedness and expandedness.
        protected interface ITreeViewItem : IChildItem {
            bool IsSelected { get; set; }
            bool IsExpanded { get; set; }
            IList ChildrenList { get; }
        }

        // A subclass of ObservableCollection that allows the parent list and item to be accessed from its children.
        protected sealed class ParentedCollection<TChild> : ObservableCollection<TChild>
            where TChild : class, IChildItem {
            private ITreeViewItem ParentItem;
            public ParentedCollection(ITreeViewItem parentItem, IEnumerable<TChild> collection = null) {
                ParentItem = parentItem;
                if(collection != null) {
                    foreach(TChild child in collection) {
                        child.ParentItem = ParentItem;
                        child.ParentList = this;
                        Add(child);
                    }
                }
                CollectionChanged += ParentedCollection_CollectionChanged;
            }

            private void ParentedCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
                if(e.NewItems != null) {
                    foreach(TChild newItem in e.NewItems) {
                        newItem.ParentItem = ParentItem;
                        newItem.ParentList = this;
                    }
                }
                if(e.OldItems != null) {
                    foreach(TChild oldItem in e.OldItems) {
                        oldItem.ParentItem = null;
                        oldItem.ParentList = null;
                    }
                }
            }
        }

        // Simple overloaded ColorDialog to start with the color picker active
        protected sealed class ColorDialogEx : System.Windows.Forms.ColorDialog {
            protected override int Options {
                get {
                    return (base.Options | 2);
                }
            }
        }

        #endregion

        // Common Font Chooser button click handler.
        protected void btnFontChoose_Click(object sender, RoutedEventArgs e) {
            var button = (Button)sender;
            try {
                using(var dialog = new System.Windows.Forms.FontDialog()) {
                    dialog.Font = (Font)button.Tag;
                    dialog.ShowEffects = false;
                    dialog.AllowVerticalFonts = false;
                    if(System.Windows.Forms.DialogResult.OK == dialog.ShowDialog()) {
                        button.Tag = dialog.Font;
                    }
                }
            }
            catch { }
        }

        // Utility method to move nodes up and down in a TreeView.
        protected static void UpDownOnTreeView(TreeView tvw, bool up, bool traverseFolders) {
            ITreeViewItem sel = tvw.SelectedItem as ITreeViewItem;
            if(sel == null) return;
            IList list = sel.ParentList;
            int index = list.IndexOf(sel);
            if(index == -1) return;
            bool expanded = sel.IsExpanded;
            if(up && index == 0) {
                if(!traverseFolders || sel.ParentItem == null) return;
                IList parentList = sel.ParentItem.ParentList;
                int parentIndex = parentList.IndexOf(sel.ParentItem);
                if(parentIndex == -1) return;
                list.RemoveAt(index);
                parentList.Insert(parentIndex, sel);
            }
            else if(!up && index == list.Count - 1) {
                if(!traverseFolders || sel.ParentItem == null) return;
                IList parentList = sel.ParentItem.ParentList;
                int parentIndex = parentList.IndexOf(sel.ParentItem);
                if(parentIndex == -1) return;
                list.RemoveAt(index);
                parentList.Insert(parentIndex + 1, sel);
            }
            else {
                ITreeViewItem next = (ITreeViewItem)list[index + (up ? -1 : 1)];
                if(traverseFolders && next.ChildrenList != null && (next.IsExpanded || next.ChildrenList.Count == 0)) {
                    list.RemoveAt(index);
                    list = next.ChildrenList;
                    list.Insert(up ? list.Count : 0, sel);
                    next.IsExpanded = true;
                }
                else {
                    list.RemoveAt(index);
                    list.Insert(index + (up ? -1 : 1), sel);                    
                }
            }
            sel.IsExpanded = expanded;
            sel.IsSelected = true;
        }
    }
}