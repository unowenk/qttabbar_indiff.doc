//    This file is part of QTTabBar, a shell extension for Microsoft
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
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using OpenFileDialog = System.Windows.Forms.OpenFileDialog;
namespace QTTabBarLib
{
    internal partial class Options12_Plugins : OptionsDialogTab {
        private ObservableCollection<PluginEntry> CurrentPlugins;

        public Options12_Plugins() {
            InitializeComponent();
        }

        public override void InitializeConfig() {
            CurrentPlugins = new ObservableCollection<PluginEntry>();
            foreach(PluginAssembly assembly in PluginManager.PluginAssemblies) {
                CreatePluginEntry(assembly, false);
            }
            lstPluginView.ItemsSource = CurrentPlugins;
        }

        public override void ResetConfig() {
            // Should we do anything here?
        }
        /// <summary>
        /// 提交配置信息
        /// </summary>
        public override void CommitConfig() {
            HashSet<string> paths = new HashSet<string>();
            HashSet<PluginAssembly> toDispose = new HashSet<PluginAssembly>();

            // Don't dispose the assemblies here.  That will be done by the plugin manager
            // when the plugins are unloaded.
            for(int i = 0; i < CurrentPlugins.Count; ++i) {
                if(CurrentPlugins[i].UninstallOnClose) {
                    CurrentPlugins[i].Enabled = false;
                    CurrentPlugins.RemoveAt(i--);
                }
            }

            List<string> enabled = new List<string>();
            foreach(PluginEntry entry in CurrentPlugins) {
                paths.Add(entry.PluginAssembly.Path);
                if(entry.DisableOnClose) {
                    entry.Enabled = false;
                }
                else if(entry.EnableOnClose) {
                    entry.Enabled = true;
                }
                else if(entry.InstallOnClose) {
                    entry.Enabled = true;
                    toDispose.Add(entry.PluginAssembly);
                    // Newly installed PluginAssemblies are loaded by the options dialog.
                    // They will also be loaded by the PluginManager, so we have to 
                    // dispose of the ones we loaded here.
                }
                entry.EnableOnClose = entry.DisableOnClose = entry.InstallOnClose = false;

                if(entry.Enabled) enabled.Add(entry.PluginID);
            }
            WorkingConfig.plugin.Enabled = enabled.ToArray();
            foreach(PluginAssembly asm in toDispose) {
                asm.Dispose();
            }
            PluginManager.SavePluginAssemblyPaths(paths.ToList());
            
            // Entries are invalid now, some assemblies may have been Disposed.
            CurrentPlugins = new ObservableCollection<PluginEntry>();
        }

        private void btnPluginOptions_Click(object sender, RoutedEventArgs e) {
            PluginEntry entry = (PluginEntry)((Button)sender).DataContext;
            string pid = entry.PluginID;
            // Unfortunately, we can't call Plugin.OnOption on plugins that are
            // loaded in a non-static context.
            InstanceManager.InvokeMain(tabbar => {
                Plugin p;
                if(!tabbar.pluginServer.TryGetPlugin(pid, out p) || p.Instance == null) return;
                try {
                    p.Instance.OnOption();
                }
                catch(Exception ex) {
                    PluginManager.HandlePluginException(ex, new WindowInteropHelper(Window.GetWindow(this)).Handle,
                            entry.Name, "Open plugin option.");
                }
            });
        }
        /// <summary>
        ///  启用禁用插件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPluginEnableDisable_Click(object sender, RoutedEventArgs e) {
            PluginEntry entry = (PluginEntry)((Button)sender).DataContext; 
            if(entry.DisableOnClose) {
                entry.DisableOnClose = false;
            }
            else if(entry.EnableOnClose) {
                entry.EnableOnClose = false;
            }
            else if(entry.Enabled) {
                entry.DisableOnClose = true;
            }
            else {
                entry.EnableOnClose = true;
            }
        }

        /// <summary>
        ///     删除插件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPluginRemove_Click(object sender, RoutedEventArgs e) {
            PluginEntry entry = (PluginEntry)((Button)sender).DataContext; 
            PluginAssembly pluginAssembly = entry.PluginAssembly;
            if(pluginAssembly.PluginInformations.Count > 1) {
                string plugins = pluginAssembly.PluginInformations.Select(info => info.Name).StringJoin(", ");
                if(MessageBox.Show(
                        QTUtility.TextResourcesDic["Options_Page12_Plugins"][8] +
                        Environment.NewLine + Environment.NewLine + plugins + Environment.NewLine + Environment.NewLine +
                        QTUtility.TextResourcesDic["Options_Page12_Plugins"][9],
                        QTUtility.TextResourcesDic["OptionsDialog"][3],
                        MessageBoxButton.OKCancel, MessageBoxImage.Question) != MessageBoxResult.OK) {
                    return;
                }
            }
            for(int i = 0; i < CurrentPlugins.Count; i++) {
                PluginEntry otherEntry = CurrentPlugins[i];
                if(otherEntry.PluginAssembly == entry.PluginAssembly) {
                    if(otherEntry.InstallOnClose) {
                        CurrentPlugins.RemoveAt(i);
                        --i;
                    }
                    else {
                        otherEntry.UninstallOnClose = true;
                    }
                }
            }
            if(entry.InstallOnClose) {
                entry.PluginAssembly.Dispose();
            }
        }

        private void btnBrowsePlugin_Click(object sender, RoutedEventArgs e) {
            using(OpenFileDialog ofd = new OpenFileDialog()) {
                ofd.Filter = QTUtility.TextResourcesDic["FileFilters"][2] + "|*.dll";
                ofd.RestoreDirectory = true;
                ofd.Multiselect = true;

                if(System.Windows.Forms.DialogResult.OK != ofd.ShowDialog()) return;
                bool fFirst = true;
                foreach(string path in ofd.FileNames) {
                    PluginAssembly pa = new PluginAssembly(path);
                    if(!pa.PluginInfosExist) continue;
                    CreatePluginEntry(pa, true);
                    if(!fFirst) continue;
                    fFirst = false;
                    lstPluginView.SelectedItem = CurrentPlugins[CurrentPlugins.Count - 1];
                    lstPluginView.ScrollIntoView(lstPluginView.SelectedItem);
                }
            }
        }

        /// <summary>
        /// 启用所有插件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEnablePlugin_Click(object sender, RoutedEventArgs e)
        {
            foreach (PluginEntry entry in CurrentPlugins)
            {
                
                entry.Enabled = true;
                entry.EnableOnClose = true;
            }
        }

        /// <summary>
        /// 禁用所有插件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDisablePlugin_Click(object sender, RoutedEventArgs e)
        {
            foreach (PluginEntry entry in CurrentPlugins)
            {
                entry.Enabled = false;
                entry.EnableOnClose = false;
                entry.DisableOnClose = true;
            }
        }

        private void txtUndo_MouseUp(object sender, MouseButtonEventArgs e) {
            PluginEntry entry = (PluginEntry)((TextBlock)sender).DataContext;
            if(entry.UninstallOnClose) {
                foreach(var other in CurrentPlugins) {
                    if(entry.PluginAssembly == other.PluginAssembly) {
                        other.UninstallOnClose = false;       
                    }
                }
            }
            else if(entry.InstallOnClose) {
                entry.IsSelected = true;
                btnPluginRemove_Click(sender, null);
            }
            else if(entry.DisableOnClose) {
                entry.DisableOnClose = false;
            }
            else if(entry.EnableOnClose) {
                entry.EnableOnClose = false;
            }
        }

        private void CreatePluginEntry(PluginAssembly pa, bool fAddedByUser) {
            if(!pa.PluginInfosExist || CurrentPlugins.Any(pe => pe.Path == pa.Path)) {
                return;
            }      
            foreach(PluginInformation pi in pa.PluginInformations) {
                if (null != pi && null != pi.Path) {
                    // Check the file exists by qwop ?
                    if (!File.Exists(pi.Path)) {
                        continue;
                    }
                    // Check the file exists by qwop ?
                    PluginEntry entry = new PluginEntry(this, pi, pa) { InstallOnClose = fAddedByUser };
                    int i = 0;
                    while (i < CurrentPlugins.Count && string.Compare(CurrentPlugins[i].Title, entry.Title, true) <= 0) ++i;
                    CurrentPlugins.Insert(i, entry);
                }
            }
        }

    }
}
