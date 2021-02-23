

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using Image = System.Drawing.Image;
using Keys = System.Windows.Forms.Keys;
using Padding = System.Windows.Forms.Padding;
using Color = System.Windows.Media.Color;


using QTPlugin;
using BandObjectLib.Interop;

namespace QTTabBarLib
{

    public partial class MarginCombo
    {
        #region ---------- Binding Classes ----------
        // INotifyPropertyChanged is implemented automatically by Notify Property Weaver!
#pragma warning disable 0067 // "The event 'PropertyChanged' is never used"
        // ReSharper disable MemberCanBePrivate.Local
        // ReSharper disable UnusedMember.Local
        // ReSharper disable UnusedAutoPropertyAccessor.Local

        private class MarginEntry : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;
            private MarginCombo Parent;

            public Thickness PanelMargin
            {
                get
                {
                    Thickness t = new Thickness(5);
                    if (Index > 0) t.Top = 1;
                    if (Index < 4) t.Bottom = 0;
                    return t;
                }
            }
            public Thickness LabelMargin
            {
                get
                {
                    return new Thickness(Index == 0 ? 5 : 15, 0, 10, 0);
                }
            }

            public string TextBoxValue
            {
                get
                {
                    return Value >= 0 ? Value.ToString() : "";
                }
                set
                {
                    int i;
                    if (!int.TryParse(value.Trim(), out i) || i < 0 || i > VAL_MAX) return;
                    Padding current = Parent.Value;
                    switch (Index)
                    {
                        case 1: current.Left = i; break;
                        case 2: current.Top = i; break;
                        case 3: current.Right = i; break;
                        case 4: current.Bottom = i; break;
                        default: current = new Padding(i); break;
                    }
                    Parent.Value = current;
                    OnPropertyChanged("TextBoxValue");
                }
            }
            public int Value
            {
                get
                {
                    return Value;
                }
                set
                {
                    if (Value != value)
                    {
                        Value = value;
                        OnPropertyChanged("TextBoxValue");
                        OnPropertyChanged("Value");
                    }
                }
            }
            public int Index
            {
                get
                {
                    return Index;
                }
                set
                {
                    if (Index != value)
                    {
                        Index = value;
                        OnPropertyChanged("PanelMargin");
                        OnPropertyChanged("LabelMargin");
                        OnPropertyChanged("Index");
                    }
                }
            }

            public MarginEntry(MarginCombo parent, int idx)
            {
                Index = idx;
                Parent = parent;
            }

            public override string ToString()
            {
                Padding t = Parent.Value;
                return t.Left + ", " + t.Top + ", " + t.Right + ", " + t.Bottom;
            }
            public virtual void OnPropertyChanged(string propertyName)
            {
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
                }
            }
        }

        #endregion

    }

    internal partial class Options04_Tooltips
    {
        #region ---------- Binding Classes ----------
        // INotifyPropertyChanged is implemented automatically by Notify Property Weaver!
#pragma warning disable 0067 // "The event 'PropertyChanged' is never used"
        // ReSharper disable MemberCanBePrivate.Local
        // ReSharper disable UnusedMember.Local
        // ReSharper disable UnusedAutoPropertyAccessor.Local

        private class FileTypeEntry : INotifyPropertyChanged, IEditableEntry
        {
            public event PropertyChangedEventHandler PropertyChanged;
            private Options04_Tooltips parent;

            private bool _IsEditing;
            public bool IsEditing
            {
                get { return _IsEditing; }
                set
                {
                    _IsEditing = value;
                    OnPropertyChanged("IsEditing");
                    if (!_IsEditing && string.IsNullOrEmpty(Extension))
                    {
                        parent.TextFileTypes.Remove(this);
                        parent.MediaFileTypes.Remove(this);
                    }
                }
            }

            public bool IsSelected
            {
                get
                {
                    return IsSelected;
                }
                set
                {
                    if (IsSelected != value)
                    {
                        IsSelected = value;
                        OnPropertyChanged("IsSelected");
                    }
                }
            }
            public string Extension
            {
                get
                {
                    return Extension;
                }
                set
                {
                    if (!string.Equals(Extension, value))
                    {
                        Extension = value;
                        OnPropertyChanged("DotExtension");
                        OnPropertyChanged("FriendlyName");
                        OnPropertyChanged("Icon");
                        OnPropertyChanged("Extension");
                    }
                }
            }

            public string DotExtension
            {
                get
                {
                    return "." + Extension;
                }
                set
                {
                    if (!value.StartsWith("."))
                    {
                        throw new ArgumentException();
                    }
                    Extension = value.Substring(1);
                    OnPropertyChanged("FriendlyName");
                    OnPropertyChanged("Icon");
                    OnPropertyChanged("DotExtension");
                }
            }
            public string FriendlyName
            {
                get
                {
                    // PENDING: Instead of something like GetFileType.

                    SHFILEINFO psfi = new SHFILEINFO();
                    int sz = System.Runtime.InteropServices.Marshal.SizeOf(psfi);
                    // SHGFI_TYPENAME | SHGFI_USEFILEATTRIBUTES
                    if (IntPtr.Zero == PInvoke.SHGetFileInfo("*" + DotExtension, 0x80, ref psfi, sz, 0x400 | 0x10))
                    {
                        return null;
                    }
                    else if (string.IsNullOrEmpty(psfi.szTypeName))
                    {
                        return null;
                    }
                    return psfi.szTypeName;
                }
            }
            public Image Icon
            {
                get
                {
                    return QTUtility.GetIcon(DotExtension, true).ToBitmap();
                }
            }
            public FileTypeEntry(Options04_Tooltips parent, string extension)
            {
                this.parent = parent;
                if (!extension.StartsWith("."))
                {
                    extension = "." + extension;
                }
                DotExtension = extension;
            }
            public virtual void OnPropertyChanged(string propertyName)
            {
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
                }
            }
        }

        #endregion
    }
    internal partial class Options07_Mouse
    {
        #region ---------- Binding Classes ----------
        // INotifyPropertyChanged is implemented automatically by Notify Property Weaver!
#pragma warning disable 0067 // "The event 'PropertyChanged' is never used"
        // ReSharper disable MemberCanBePrivate.Local
        // ReSharper disable UnusedMember.Local
        // ReSharper disable UnusedAutoPropertyAccessor.Local

        private class MouseEntry : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;
            private bool isSelected;
            public bool IsSelected
            {
                get
                {
                    return isSelected;
                }
                set
                {
                    if (isSelected != value)
                    {
                        isSelected = value;
                        OnPropertyChanged("IsSelected");
                        if (!isSelected) IsEditing = false;
                    }
                }
            }
            private bool isEditing;
            public bool IsEditing
            {
                get
                {
                    return isEditing;
                }
                set
                {
                    if (isEditing != value)
                    {
                        isEditing = value;
                        OnPropertyChanged("IsEditing");
                        if (isEditing) IsSelected = true;
                    }
                }
            }
            public IEnumerable<int> ComboBoxItems
            {
                get { return MouseTargetActions[Target].Select(ActionToResx); }
            }
            public string GestureModifiers
            {
                get
                {
                    return new MouseChord[] { MouseChord.Ctrl, MouseChord.Shift, MouseChord.Alt }
                            .Where(mod => (Chord & mod) == mod)
                            .Select(c => MouseModifierStrings[c])
                            .StringJoin("");
                }
            }
            public int ButtonIdx
            {
                get
                {
                    return MouseButtonResx[Chord & ~(MouseChord.Alt | MouseChord.Ctrl | MouseChord.Shift)];
                }
            }
            public int TargetIdx { get { return MouseTargetResx[Target]; } }
            public MouseTarget Target
            {
                get
                {
                    return Target;
                }
                private set
                {
                    if (Target != value)
                    {
                        Target = value;
                        OnPropertyChanged("ComboBoxItems");
                        OnPropertyChanged("TargetIdx");
                        OnPropertyChanged("Target");
                    }
                }
            }
            public BindAction Action
            {
                get
                {
                    return Action;
                }
                set
                {
                    if (Action != value)
                    {
                        Action = value;
                        OnPropertyChanged("ActionIdx");
                        OnPropertyChanged("Action");
                    }
                }
            }
            public MouseChord Chord
            {
                get
                {
                    return Chord;
                }
                private set
                {
                    if (Chord != value)
                    {
                        Chord = value;
                        OnPropertyChanged("GestureModifiers");
                        OnPropertyChanged("ButtonIdx");
                        OnPropertyChanged("Chord");
                    }
                }
            }
            public int ActionIdx
            {
                get { return ActionToResx(Action); }
                set
                {
                    if (ActionIdx != value)
                    {
                        Action = ResxToAction(value);
                        OnPropertyChanged("ActionIdx");
                    }
                }
            }

            public MouseEntry(MouseTarget target, MouseChord chord, BindAction action)
            {
                Target = target;
                Action = action;
                Chord = chord;
            }
            public virtual void OnPropertyChanged(string propertyName)
            {
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
                }
            }
        }

        #endregion
    }
    internal partial class Options08_Keys
    {
        #region ---------- Binding Classes ----------
        // INotifyPropertyChanged is implemented automatically by Notify Property Weaver!
#pragma warning disable 0067 // "The event 'PropertyChanged' is never used"
        // ReSharper disable MemberCanBePrivate.Local
        // ReSharper disable UnusedMember.Local
        // ReSharper disable UnusedAutoPropertyAccessor.Local

        private class HotkeyEntry : INotifyPropertyChanged, IHotkeyEntry
        {
            public event PropertyChangedEventHandler PropertyChanged;
            public virtual void OnPropertyChanged(string propertyName)
            {
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
                }
            }
            private int[] raws;
            public int RawKey
            {
                get { return raws[Index]; }
                set
                {
                    if (RawKey != value)
                    {
                        raws[Index] = value;
                        OnPropertyChanged("Enabled");
                        OnPropertyChanged("ShortcutKey");
                        OnPropertyChanged("Assigned");
                        OnPropertyChanged("HotkeyString");
                        OnPropertyChanged("RawKey");
                    }
                }
            }
            public bool Enabled
            {
                get { return (RawKey & QTUtility.FLAG_KEYENABLED) != 0 && RawKey != QTUtility.FLAG_KEYENABLED; }
                set 
                {
                    if (Enabled != value)
                    {
                        if (value) RawKey |= QTUtility.FLAG_KEYENABLED;
                        else RawKey &= ~QTUtility.FLAG_KEYENABLED;
                        OnPropertyChanged("Enabled");
                    }
                }
            }
            public Keys ShortcutKey
            {
                get { return (Keys)(RawKey & ~QTUtility.FLAG_KEYENABLED); }
                set 
                { 
					if (ShortcutKey != value)
					{
                        RawKey = (int)value | (Enabled ? QTUtility.FLAG_KEYENABLED : 0);
                        OnPropertyChanged("Assigned");
                        OnPropertyChanged("HotkeyString");
                        OnPropertyChanged("ShortcutKey");
                    }
                }
            }
            public bool Assigned
            {
                get { return ShortcutKey != Keys.None; }
            }
            public string HotkeyString
            {
                get { return QTUtility2.MakeKeyString(ShortcutKey); }
            }
            public string PluginName
            {
                get
                {
                    return PluginName;
                }
                set
                {
                    if (!string.Equals(PluginName, value))
                    {
                        PluginName = value;
                        OnPropertyChanged("KeyActionText");
                        OnPropertyChanged("PluginName");
                    }
                }
            }
            public string KeyActionText
            {
                get
                {
                    return PluginName == ""
                            ? QTUtility.TextResourcesDic["ShortcutKeys_ActionNames"][Index]
                            : pluginAction;
                }
            }
            public int Index
            {
                get
                {
                    return Index;
                }
                set
                {
                    if (Index != value)
                    {
                        Index = value;
                        OnPropertyChanged("RawKey");
                        OnPropertyChanged("Enabled");
                        OnPropertyChanged("ShortcutKey");
                        OnPropertyChanged("Assigned");
                        OnPropertyChanged("HotkeyString");
                        OnPropertyChanged("KeyActionText");
                        OnPropertyChanged("Index");
                    }
                }
            }

            private string pluginAction;
            public HotkeyEntry(int[] raws, int index, string action = "", string pluginName = "")
            {
                this.raws = raws;
                Index = index;
                pluginAction = action;
                PluginName = pluginName;
            }
        }

        #endregion
    }
    internal partial class Options09_Groups
    {
        #region ---------- Binding Classes ----------
        // INotifyPropertyChanged is implemented automatically by Notify Property Weaver!
#pragma warning disable 0067 // "The event 'PropertyChanged' is never used"
        // ReSharper disable MemberCanBePrivate.Local
        // ReSharper disable UnusedMember.Local
        // ReSharper disable UnusedAutoPropertyAccessor.Local

        private class FolderEntry : INotifyPropertyChanged, IEditableEntry, ITreeViewItem
        {
            public event PropertyChangedEventHandler PropertyChanged;
            public virtual void OnPropertyChanged(string propertyName)
            {
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
                }
            }
            public IList ParentList
            {
                get
                {
                    return ParentList;
                }
                set
                {
                    if (ParentList != value)
                    {
                        ParentList = value;
                        OnPropertyChanged("ParentList");
                    }
                }
            }
            public ITreeViewItem ParentItem
            {
                get
                {
                    return ParentItem;
                }
                set
                {
                    if (ParentItem != value)
                    {
                        ParentItem = value;
                        OnPropertyChanged("ParentItem");
                    }
                }
            }
            public string Path
            {
                get
                {
                    return Path;
                }
                set
                {
                    if (!string.Equals(Path, value))
                    {
                        Path = value;
                        OnPropertyChanged("DisplayText");
                        OnPropertyChanged("Icon");
                        OnPropertyChanged("IsVirtualFolder");
                        OnPropertyChanged("Path");
                    }
                }
            }
            public bool IsEditing
            {
                get
                {
                    return IsEditing;
                }
                set
                {
                    if (IsEditing != value)
                    {
                        IsEditing = value;
                        OnPropertyChanged("IsEditing");
                    }
                }
            }
            public bool IsSelected
            {
                get
                {
                    return IsSelected;
                }
                set
                {
                    if (IsSelected != value)
                    {
                        IsSelected = value;
                        OnPropertyChanged("IsSelected");
                    }
                }
            }
            public bool IsExpanded
            {
                get
                {
                    return IsExpanded;
                }
                set
                {
                    if (IsExpanded != value)
                    {
                        IsExpanded = value;
                        OnPropertyChanged("IsExpanded");
                    }
                }
            }
            public IList ChildrenList { get { return null; } }

            public string DisplayText
            {
                get
                {
                    return QTUtility2.MakePathDisplayText(Path, true);
                }
            }
            public Image Icon
            {
                get
                {
                    return QTUtility.GetIcon(Path, false).ToBitmap();
                }
            }
            public bool IsVirtualFolder
            {
                get
                {
                    return Path.StartsWith("::");
                }
            }

            public FolderEntry(string path)
            {
                Path = path;
            }

            public FolderEntry()
            {
            }
        }

        #endregion
    }
    internal partial class Options09_Groups
    {
        #region ---------- Binding Classes ----------

        private class GroupEntry : INotifyPropertyChanged, IEditableEntry, ITreeViewItem, IHotkeyEntry
        {
            public event PropertyChangedEventHandler PropertyChanged;
            public virtual void OnPropertyChanged(string propertyName)
            {
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
                }
            }
            public IList ParentList
            {
                get
                {
                    return ParentList;
                }
                set
                {
                    if (ParentList != value)
                    {
                        ParentList = value;
                        OnPropertyChanged("ParentList");
                    }
                }
            }
            public ITreeViewItem ParentItem
            {
                get
                {
                    return ParentItem;
                }
                set
                {
                    if (ParentItem != value)
                    {
                        ParentItem = value;
                        OnPropertyChanged("ParentItem");
                    }
                }
            }
            public string Name
            {
                get
                {
                    return Name;
                }
                set
                {
                    if (!string.Equals(Name, value))
                    {
                        Name = value;
                        OnPropertyChanged("KeyActionText");
                        OnPropertyChanged("Name");
                    }
                }
            }
            public Image Icon
            {
                get
                {
                    return Icon;
                }
                private set
                {
                    if (Icon != value)
                    {
                        Icon = value;
                        OnPropertyChanged("Icon");
                    }
                }
            }
            public ParentedCollection<FolderEntry> Folders
            {
                get
                {
                    return Folders;
                }
                private set
                {
                    if (Folders != value)
                    {
                        Folders = value;
                        OnPropertyChanged("ChildrenList");
                        OnPropertyChanged("Folders");
                    }
                }
            }
            public bool Startup
            {
                get
                {
                    return Startup;
                }
                set
                {
                    if (Startup != value)
                    {
                        Startup = value;
                        OnPropertyChanged("Startup");
                    }
                }
            }
            public Keys ShortcutKey
            {
                get
                {
                    return ShortcutKey;
                }
                set
                {
                    if (ShortcutKey != value)
                    {
                        ShortcutKey = value;
                        OnPropertyChanged("HotkeyString");
                        OnPropertyChanged("ShortcutKey");
                    }
                }
            }

            public string KeyActionText
            {
                get
                {
                    string GroupPrefix = QTUtility.TextResourcesDic["Options_Page09_Groups"][7];
                    return string.Format(GroupPrefix, Name);
                }
            }

            public string HotkeyString
            {
                get { return QTUtility2.MakeKeyString(ShortcutKey); }
            }
            public bool IsEditing
            {
                get
                {
                    return IsEditing;
                }
                set
                {
                    if (IsEditing != value)
                    {
                        IsEditing = value;
                        OnPropertyChanged("IsEditing");
                    }
                }
            }
            public bool IsSelected
            {
                get
                {
                    return IsSelected;
                }
                set
                {
                    if (IsSelected != value)
                    {
                        IsSelected = value;
                        OnPropertyChanged("IsSelected");
                    }
                }
            }
            public bool IsExpanded
            {
                get
                {
                    return IsExpanded;
                }
                set
                {
                    if (IsExpanded != value)
                    {
                        IsExpanded = value;
                        OnPropertyChanged("IsExpanded");
                    }
                }
            }
            public IList ChildrenList { get { return Folders; } }

            private void Folders_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
            {
                if (e.OldItems != null)
                {
                    foreach (FolderEntry child in e.OldItems)
                    {
                        child.PropertyChanged -= FolderEntry_PropertyChanged;
                    }
                }
                if (e.NewItems != null)
                {
                    foreach (FolderEntry child in e.NewItems)
                    {
                        child.PropertyChanged += FolderEntry_PropertyChanged;
                    }
                }
                RefreshIcon();
            }

            private void FolderEntry_PropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                if (Folders.Count > 0 && sender == Folders.First())
                {
                    RefreshIcon();
                }
            }

            private void RefreshIcon()
            {
                Icon = Folders.Count == 0 ? QTUtility.ImageListGlobal.Images["folder"] : Folders.First().Icon;
            }

            public GroupEntry(string name, Keys shortcutKey, bool startup, IEnumerable<FolderEntry> folders)
            {
                Name = name;
                Startup = startup;
                ShortcutKey = shortcutKey;
                Folders = new ParentedCollection<FolderEntry>(this, folders);
                Folders.CollectionChanged += Folders_CollectionChanged;
                RefreshIcon();
            }

            public GroupEntry(string name)
            {
                Name = name;
                Folders = new ParentedCollection<FolderEntry>(this);
                Folders.CollectionChanged += Folders_CollectionChanged;
                RefreshIcon();
            }

            public GroupEntry()
            {
                Folders.CollectionChanged += Folders_CollectionChanged;
                RefreshIcon();
            }
        }

        #endregion
    }
    internal partial class Options10_Apps
    {
        #region ---------- Binding Classes ----------
        // INotifyPropertyChanged is implemented automatically by Notify Property Weaver!
#pragma warning disable 0067 // "The event 'PropertyChanged' is never used"
        // ReSharper disable MemberCanBePrivate.Local
        // ReSharper disable UnusedMember.Local
        // ReSharper disable UnusedAutoPropertyAccessor.Local

        private class AppEntry : INotifyPropertyChanged, IEditableEntry, ITreeViewItem, IHotkeyEntry
        {
            public event PropertyChangedEventHandler PropertyChanged;
            public virtual void OnPropertyChanged(string propertyName)
            {
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
                }
            }
            public IList ParentList
            {
                get
                {
                    return ParentList;
                }
                set
                {
                    if (ParentList != value)
                    {
                        ParentList = value;
                        OnPropertyChanged("ParentList");
                    }
                }
            }
            public ITreeViewItem ParentItem
            {
                get
                {
                    return ParentItem;
                }
                set
                {
                    if (ParentItem != value)
                    {
                        ParentItem = value;
                        OnPropertyChanged("ParentItem");
                    }
                }
            }
            public ParentedCollection<AppEntry> Children
            {
                get
                {
                    return Children;
                }
                set
                {
                    if (Children != value)
                    {
                        Children = value;
                        OnPropertyChanged("IsFolder");
                        OnPropertyChanged("Icon");
                        OnPropertyChanged("ChildrenList");
                        OnPropertyChanged("Children");
                    }
                }
            }
            public bool IsFolder { get { return Children != null; } }
            public bool IsEditing
            {
                get
                {
                    return IsEditing;
                }
                set
                {
                    if (IsEditing != value)
                    {
                        IsEditing = value;
                        OnPropertyChanged("IsEditing");
                    }
                }
            }
            public bool IsSelected
            {
                get
                {
                    return IsSelected;
                }
                set
                {
                    if (IsSelected != value)
                    {
                        IsSelected = value;
                        OnPropertyChanged("IsSelected");
                    }
                }
            }
            public bool IsExpanded
            {
                get
                {
                    return IsExpanded;
                }
                set
                {
                    if (IsExpanded != value)
                    {
                        IsExpanded = value;
                        OnPropertyChanged("IsExpanded");
                    }
                }
            }
            public IList ChildrenList { get { return Children; } }

            public string Name
            {
                get
                {
                    return Name;
                }
                set
                {
                    if (!string.Equals(Name, value))
                    {
                        Name = value;
                        OnPropertyChanged("KeyActionText");
                        OnPropertyChanged("Name");
                    }
                }
            }
            public string Path
            {
                get
                {
                    return Path;
                }
                set
                {
                    if (!string.Equals(Path, value))
                    {
                        Path = value;
                        OnPropertyChanged("Icon");
                        OnPropertyChanged("Path");
                    }
                }
            }
            public string Args
            {
                get
                {
                    return Args;
                }
                set
                {
                    if (!string.Equals(Args, value))
                    {
                        Args = value;
                        OnPropertyChanged("Args");
                    }
                }
            }
            public string WorkingDir
            {
                get
                {
                    return WorkingDir;
                }
                set
                {
                    if (!string.Equals(WorkingDir, value))
                    {
                        WorkingDir = value;
                        OnPropertyChanged("WorkingDir");
                    }
                }
            }
            public Keys ShortcutKey
            {
                get
                {
                    return ShortcutKey;
                }
                set
                {
                    if (ShortcutKey != value)
                    {
                        ShortcutKey = value;
                        OnPropertyChanged("HotkeyString");
                        OnPropertyChanged("ShortcutKey");
                    }
                }
            }
            public string KeyActionText
            {
                get
                {
                    string AppPrefix = QTUtility.TextResourcesDic["Options_Page10_Apps"][17];
                    return string.Format(AppPrefix, Name);
                }
            }

            public string HotkeyString
            {
                get { return QTUtility2.MakeKeyString(ShortcutKey); }
            }

            public Image Icon
            {
                get
                {
                    return IsFolder
                      ? QTUtility.ImageListGlobal.Images["folder"]
                      : QTUtility.GetIcon(Path, false).ToBitmap();
                }
            }

            public AppEntry(string folderName, IEnumerable<AppEntry> children)
            {
                Name = folderName;
                Children = new ParentedCollection<AppEntry>(this, children);
            }

            public AppEntry(string name, string path)
            {
                Path = path;
                Name = name;
            }

            public AppEntry(UserApp app)
            {
                Path = app.Path;
                Name = app.Name;
                Args = app.Args;
                WorkingDir = app.WorkingDir;
                ShortcutKey = app.ShortcutKey;
            }
        }
        #endregion
    }
    internal partial class Options11_ButtonBar
    {
        #region ---------- Binding Classes ----------
        // INotifyPropertyChanged is implemented automatically by Notify Property Weaver!
#pragma warning disable 0067 // "The event 'PropertyChanged' is never used"
        // ReSharper disable MemberCanBePrivate.Local
        // ReSharper disable UnusedMember.Local
        // ReSharper disable UnusedAutoPropertyAccessor.Local

        private class ButtonEntry : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;
            public virtual void OnPropertyChanged(string propertyName)
            {
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
                }
            }
            private Options11_ButtonBar parent;

            public PluginInformation PluginInfo
            {
                get
                {
                    return PluginInfo;
                }
                private set
                {
                    if (PluginInfo != value)
                    {
                        PluginInfo = value;
                        OnPropertyChanged("IsPluginButton");
                        OnPropertyChanged("PluginButtonText");
                        OnPropertyChanged("LargeImage");
                        OnPropertyChanged("SmallImage");
                        OnPropertyChanged("PluginInfo");
                    }
                }
            }
            public int Index
            {
                get
                {
                    return Index;
                }
                private set
                {
                    if (Index != value)
                    {
                        Index = value;
                        OnPropertyChanged("PluginButtonText");
                        OnPropertyChanged("LargeImage");
                        OnPropertyChanged("SmallImage");
                        OnPropertyChanged("Index");
                    }
                }
            }
            public int Order
            {
                get
                {
                    return Order;
                }
                private set
                {
                    if (Order != value)
                    {
                        Order = value;
                        OnPropertyChanged("Order");
                    }
                }
            }
            public bool IsPluginButton { get { return PluginInfo != null; } }
            public string PluginButtonText
            {
                get
                {
                    if (!IsPluginButton) return "";
                    if (PluginInfo.PluginType == PluginType.BackgroundMultiple)
                    {
                        Plugin plugin;
                        if (PluginManager.TryGetStaticPluginInstance(PluginInfo.PluginID, out plugin))
                        {
                            try
                            {
                                return ((IBarMultipleCustomItems)plugin.Instance).GetName(Index);
                            }
                            catch { }
                        }
                    }
                    return PluginInfo.Name;
                }
            }

            public Image LargeImage { get { return getImage(true); } }
            public Image SmallImage { get { return getImage(false); } }
            private Image getImage(bool large)
            {
                if (IsPluginButton)
                {
                    if (PluginInfo.PluginType == PluginType.BackgroundMultiple)
                    {
                        Plugin plugin;
                        if (PluginManager.TryGetStaticPluginInstance(PluginInfo.PluginID, out plugin))
                        {
                            try
                            {
                                return ((IBarMultipleCustomItems)plugin.Instance).GetImage(large, Index);
                            }
                            catch { }
                        }
                    }
                    return large
                            ? PluginInfo.ImageLarge ?? Resources_Image.imgPlugin24
                            : PluginInfo.ImageSmall ?? Resources_Image.imgPlugin16;
                }
                else if (Index == 0 || Index >= QTButtonBar.BII_WINDOWOPACITY)
                {
                    return null;
                }
                else
                {
                    return large
                            ? parent.imageStripLarge[Index - 1]
                            : parent.imageStripSmall[Index - 1];
                }
            }

            public ButtonEntry(Options11_ButtonBar parent, int Order, int Index, PluginInformation PluginInfo = null)
            {
                this.parent = parent;
                this.Order = Order;
                this.Index = Index;
                this.PluginInfo = PluginInfo;
            }
        }

        #endregion

    }
    internal partial class Options12_Plugins
    {
        #region ---------- Binding Classes ----------
        // INotifyPropertyChanged is implemented automatically by Notify Property Weaver!
#pragma warning disable 0067 // "The event 'PropertyChanged' is never used"
        // ReSharper disable MemberCanBePrivate.Local
        // ReSharper disable UnusedMember.Local
        // ReSharper disable UnusedAutoPropertyAccessor.Local

        private class PluginEntry : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;
            public virtual void OnPropertyChanged(string propertyName)
            {
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
                }
            }
            private Options12_Plugins parent;
            private PluginInformation PluginInfo;
            public PluginAssembly PluginAssembly
            {
                get
                {
                    return PluginAssembly;
                }
                private set
                {
                    if (PluginAssembly != value)
                    {
                        PluginAssembly = value;
                        OnPropertyChanged("PluginAssembly");
                    }
                }
            }

            public Image Icon { get { return PluginInfo.ImageLarge ?? Resources_Image.imgPlugin24; } }
            public string Name { get { return PluginInfo.Name; } }
            public string Title
            {
                get
                {
                    return Name + "  " + PluginInfo.Version;
                }
            }
            public string Author { get { return PluginInfo.Author; } }
            public string Desc { get { return PluginInfo.Description; } }
            public bool IsSelected
            {
                get
                {
                    return IsSelected;
                }
                set
                {
                    if (IsSelected != value)
                    {
                        IsSelected = value;
                        OnPropertyChanged("IsSelected");
                    }
                }
            }
            public double IconOpacity { get { return Enabled ? 1.0 : 0.5; } }
            public bool DisableOnClose
            {
                get
                {
                    return DisableOnClose;
                }
                set
                {
                    if (DisableOnClose != value)
                    {
                        DisableOnClose = value;
                        OnPropertyChanged("StatusVisibility");
                        OnPropertyChanged("BackgroundColor");
                        OnPropertyChanged("StatusTextIdx");
                        OnPropertyChanged("StatusColor");
                        OnPropertyChanged("ShowEnableButton");
                        OnPropertyChanged("ShowDisableButton");
                        OnPropertyChanged("DisableOnClose");
                    }
                }
            }
            public bool EnableOnClose
            {
                get
                {
                    return EnableOnClose;
                }
                set
                {
                    if (EnableOnClose != value)
                    {
                        EnableOnClose = value;
                        OnPropertyChanged("StatusVisibility");
                        OnPropertyChanged("BackgroundColor");
                        OnPropertyChanged("StatusTextIdx");
                        OnPropertyChanged("StatusColor");
                        OnPropertyChanged("ShowEnableButton");
                        OnPropertyChanged("ShowDisableButton");
                        OnPropertyChanged("EnableOnClose");
                    }
                }
            }
            public bool InstallOnClose
            {
                get
                {
                    return InstallOnClose;
                }
                set
                {
                    if (InstallOnClose != value)
                    {
                        InstallOnClose = value;
                        OnPropertyChanged("StatusVisibility");
                        OnPropertyChanged("BackgroundColor");
                        OnPropertyChanged("StatusTextIdx");
                        OnPropertyChanged("StatusColor");
                        OnPropertyChanged("ShowDisabledTitle");
                        OnPropertyChanged("InstallOnClose");
                    }
                }
            }
            public bool UninstallOnClose
            {
                get
                {
                    return UninstallOnClose;
                }
                set
                {
                    if (UninstallOnClose != value)
                    {
                        UninstallOnClose = value;
                        OnPropertyChanged("StatusVisibility");
                        OnPropertyChanged("BackgroundColor");
                        OnPropertyChanged("StatusTextIdx");
                        OnPropertyChanged("MainBodyVisibility");
                        OnPropertyChanged("StatusColor");
                        OnPropertyChanged("UninstallOnClose");
                    }
                }
            }
            public bool Enabled
            {
                get
                {
                    return PluginInfo.Enabled;
                }
                set
                {
                    if (Enabled != value)
                    {
                        PluginInfo.Enabled = value;
                        OnPropertyChanged("IconOpacity");
                        OnPropertyChanged("BackgroundColor");
                        OnPropertyChanged("ShowEnableButton");
                        OnPropertyChanged("ShowDisableButton");
                        OnPropertyChanged("ShowDisabledTitle");
                        OnPropertyChanged("HasOptions");
                        OnPropertyChanged("Enabled");
                    }
                }
            }
            public string PluginID { get { return PluginInfo.PluginID; } }
            public string Path { get { return PluginInfo.Path; } }
            public Visibility StatusVisibility
            {
                get
                {
                    return DisableOnClose || EnableOnClose || InstallOnClose || UninstallOnClose
                            ? Visibility.Visible
                            : Visibility.Collapsed;
                }
            }
            public int StatusTextIdx
            {
                get
                {
                    if (UninstallOnClose) return 4;
                    if (InstallOnClose) return 5;
                    if (EnableOnClose) return 6;
                    if (DisableOnClose) return 7;
                    return int.MaxValue;
                }
            }
            public Visibility MainBodyVisibility
            {
                get
                {
                    return UninstallOnClose ? Visibility.Collapsed : Visibility.Visible;
                }
            }
            public Color BackgroundColor
            {
                get
                {
                    if (StatusVisibility == Visibility.Visible) return Color.FromArgb(0x10, 0x60, 0xA0, 0xFF);
                    if (!Enabled) return Color.FromArgb(0x10, 0x00, 0x00, 0x00);
                    return Colors.Transparent;
                }
            }
            public Color StatusColor
            {
                get
                {
                    if (EnableOnClose || InstallOnClose) return Color.FromRgb(0x20, 0x80, 0x20);
                    if (DisableOnClose || UninstallOnClose) return Color.FromRgb(0x80, 0x80, 0x80);
                    return Colors.Transparent;
                }
            }
            public bool ShowEnableButton
            {
                get
                {
                    if (DisableOnClose) return true;
                    if (EnableOnClose) return false;
                    return !Enabled;
                }
            }
            public bool ShowDisabledTitle { get { return !(Enabled || InstallOnClose); } }
            public bool ShowDisableButton { get { return !ShowEnableButton; } }

            private bool cachedHasOptions;
            private bool optionsQueried;

            public bool HasOptions
            {
                get
                {
                    if (!Enabled) return false;
                    if (optionsQueried) return cachedHasOptions;
                    Plugin p;
                    if (PluginManager.TryGetStaticPluginInstance(PluginID, out p))
                    {
                        try
                        {
                            cachedHasOptions = p.Instance.HasOption;
                            optionsQueried = true;
                            return cachedHasOptions;
                        }
                        catch (Exception ex)
                        {
                            PluginManager.HandlePluginException(ex, new WindowInteropHelper(Window.GetWindow(parent)).Handle, Name,
                                    "Checking if the plugin has options.");
                        }
                    }
                    return false;
                }
            }

            public PluginEntry(Options12_Plugins parent, PluginInformation pluginInfo, PluginAssembly pluginAssembly)
            {
                this.parent = parent;
                PluginInfo = pluginInfo;
                PluginAssembly = pluginAssembly;
            }
        }

        #endregion
    }
    internal partial class Options13_Language
    {

        #region ---------- Binding Classes ----------
        // INotifyPropertyChanged is implemented automatically by Notify Property Weaver!
#pragma warning disable 0067 // "The event 'PropertyChanged' is never used"
        // ReSharper disable MemberCanBePrivate.Local
        // ReSharper disable UnusedMember.Local
        // ReSharper disable UnusedAutoPropertyAccessor.Local

        private class LangEntry : INotifyPropertyChanged, IEditableEntry {
            public event PropertyChangedEventHandler PropertyChanged;
            public virtual void OnPropertyChanged(string propertyName)
            {
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
                }
            }
            public string Original { get { return Index < 0 ? Key : QTUtility.TextResourcesDic[Key][Index]; } }
            public int Index
            {
                get
                {
                    return Index;
                }
                set
                {
                    if (Index != value)
                    {
                        Index = value;
                        OnPropertyChanged("Original");
                        OnPropertyChanged("Location");
                        OnPropertyChanged("Index");
                    }
                }
            }
            public bool IsEditing
            {
                get
                {
                    return IsEditing;
                }
                set
                {
                    if (IsEditing != value)
                    {
                        IsEditing = value;
                        OnPropertyChanged("IsEditing");
                    }
                }
            }
            public string Location { get {
                return Index < 0 ? "** Metadata **" : Key;
                // todo
            }}

            public string Key
            {
                get
                {
                    return Key;
                }
                set
                {
                    if (!string.Equals(Key, value))
                    {
                        Key = value;
                        OnPropertyChanged("Original");
                        OnPropertyChanged("Location");
                        OnPropertyChanged("Key");
                    }
                }
            }
            public string Translated
            {
                get
                {
                    return Translated;
                }
                set
                {
                    if (!string.Equals(Translated, value))
                    {
                        Translated = value;
                        OnPropertyChanged("Translated");
                    }
                }
            }

            public LangEntry(string key, int index) {
                Key = key;
                Index = index;
                Reset();
            }

            public void Reset() {
                string[] res;
                if(Index >= 0) {
                    Translated = Original;
                }
                else if(QTUtility.TextResourcesDic.TryGetValue(Key, out res)) {
                    Translated = res[0];
                }
                else {
                    Translated = "";
                }
            }
        }

        #endregion
    }
    partial class Resx
    {
        private class ResxListener : INotifyPropertyChanged, IWeakEventListener {
            #pragma warning disable 0067 // "The event 'PropertyChanged' is never used"
            public event PropertyChangedEventHandler PropertyChanged;
            #pragma warning restore 0067

            // ReSharper disable MemberCanBePrivate.Local
            // ReSharper disable UnusedAutoPropertyAccessor.Local
            public string Value 
            {
                get
                {
                    return Value;
                }
				set
				{
					if (!string.Equals(Value, value))
					{
						Value = value;
						OnPropertyChanged("Value");
					}
				}
            }
            // ReSharper restore UnusedAutoPropertyAccessor.Local
            // ReSharper restore MemberCanBePrivate.Local
            
            private Resx parent;

            public ResxListener(Resx parent) {
                this.parent = parent;
                Value = parent.GetValue();
            }

            public bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e) {
                Value = parent.GetValue();
                return true;
            }
			public virtual void OnPropertyChanged(string propertyName)
			{
                if(this.PropertyChanged!=null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
                }
			}
        }
    }

}
