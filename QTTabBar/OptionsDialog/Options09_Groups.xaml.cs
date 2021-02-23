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

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Keys = System.Windows.Forms.Keys;

namespace QTTabBarLib
{
    internal partial class Options09_Groups : OptionsDialogTab, IHotkeyContainer {
        private ParentedCollection<GroupEntry> CurrentGroups;
        public event NewHotkeyRequestedHandler NewHotkeyRequested;

        public Options09_Groups() {
            InitializeComponent();
        }

        public override void InitializeConfig() {
            tvwGroups.ItemsSource = CurrentGroups = new ParentedCollection<GroupEntry>(null,
                    GroupsManager.Groups.Select(g => new GroupEntry(
                    g.Name, g.ShortcutKey, g.Startup, g.Paths.Select(p => new FolderEntry(p)))));
        }

        public override void ResetConfig() {
            // Should we do anything here?
        }

        public override void CommitConfig() {
            GroupsManager.Groups = new List<Group>(
                    CurrentGroups.Select(g => new Group(
                    g.Name, g.ShortcutKey, g.Startup, g.Folders.Select(f => f.Path).ToList())));

        }

        public IEnumerable<IHotkeyEntry> GetHotkeyEntries() {
            return CurrentGroups.Cast<IHotkeyEntry>();
        }

        private void btnGroupsAddGroup_Click(object sender, RoutedEventArgs e) {
            GroupEntry item = new GroupEntry(QTUtility.TextResourcesDic["Options_Page09_Groups"][6]);
            tvwGroups.Focus();
            IList col = (IList)tvwGroups.ItemsSource;
            object sel = tvwGroups.SelectedItem;
            int idx = sel == null
                    ? tvwGroups.Items.Count
                    : CurrentGroups.IndexOf(sel as GroupEntry ?? (GroupEntry)((FolderEntry)sel).ParentItem) + 1;
            col.Insert(idx, item);
            item.IsSelected = true;
            item.IsEditing = true;
        }

        private void btnGroupsMoveNodeUpDown_Click(object sender, RoutedEventArgs e) {
            UpDownOnTreeView(tvwGroups, sender == btnGroupsMoveNodeUp, false);
        }

        private void btnGroupsAddFolder_Click(object sender, RoutedEventArgs e) {
            GroupEntry group;
            int index;
            bool editGroup;
            if(tvwGroups.Items.Count == 0) {
                group = new GroupEntry(QTUtility.TextResourcesDic["Options_Page09_Groups"][6]);
                CurrentGroups.Add(group);
                group.IsSelected = true;
                index = 0;
                editGroup = true;
            }
            else {
                object sel = tvwGroups.SelectedItem;
                if(sel == null) return;
                if(sel is FolderEntry) {
                    FolderEntry entry = (FolderEntry)sel;
                    group = (GroupEntry)entry.ParentItem;
                    index = group.Folders.IndexOf(entry) + 1;
                }
                else {
                    group = (GroupEntry)sel;
                    index = group.Folders.Count;
                }
                editGroup = false;
            }

            FolderBrowserDialogEx dlg = new FolderBrowserDialogEx();
            if(dlg.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;
            FolderEntry folder = new FolderEntry(dlg.SelectedPath);
            group.Folders.Insert(index, folder);
            group.IsExpanded = true;
            
            if(editGroup) {
                group.IsSelected = true;
                group.IsEditing = true;
            }
            else {
                folder.IsSelected = true;   
            }
        }

        private void btnGroupsRemoveNode_Click(object sender, RoutedEventArgs e) {
            ITreeViewItem sel = tvwGroups.SelectedItem as ITreeViewItem;
            if(sel == null) return;
            IList col = sel.ParentList;
            int index = col.IndexOf(sel);
            col.RemoveAt(index);
            if(col.Count == 0) return;
            if(index == col.Count) --index;
            ((ITreeViewItem)col[index]).IsSelected = true;
        }

        private void tvwGroups_MouseDown(object sender, MouseButtonEventArgs e) {
            IEditableEntry entry = ((TreeView)sender).SelectedItem as IEditableEntry;
            if(entry != null) entry.IsEditing = false;
        }

        private void tvwGroups_PreviewKeyDown(object sender, KeyEventArgs e) {
            if(NewHotkeyRequested == null) return;
            GroupEntry entry = tvwGroups.SelectedItem as GroupEntry;
            if(entry == null) return;
            Keys newKey;
            if(!NewHotkeyRequested(e, entry.ShortcutKey, out newKey)) return;
            entry.ShortcutKey = newKey;
            e.Handled = true;
        }

    }
}
