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
using System.Windows.Forms;

namespace QTTabBarLib {
    internal class QMenuItem : ToolStripMenuItem {
        private bool fCut;
        private bool fImageLoaded;
        private bool fVirtualQueried;
        private ImageReservationKey imageReservationKey;
        private MenuItemArguments mia;
        private string originalImageKey;
        private string path;

        public event EventHandler QueryVirtualMenu;

        public QMenuItem(string title, MenuGenre menuGenre)
            : base(title) {
            BandObjectLib.Logging.Add_DEBUG("Constructor.log", "QMenuItem");

            Genre = menuGenre;
        }

        public QMenuItem(string title, MenuItemArguments mia)
            : base(title) {
            if(mia == null) return;
            path = mia.Path;
            MenuItemArguments = mia;
            Target = mia.Target;
            Genre = mia.Genre;
        }

        public QMenuItem(string title, MenuTarget menuTarget, MenuGenre menuGenre)
            : base(title) {
            Target = menuTarget;
            Genre = menuGenre;
        }

        protected override void Dispose(bool disposing) {
            base.Dispose(disposing);
            if(!disposing || !base.HasDropDownItems || DropDown.IsAutoGenerated) return;
            DropDown.Dispose();
            DropDown = null;
        }

        protected override void OnMouseHover(EventArgs e) {
            if(!string.IsNullOrEmpty(ToolTipText)) {
                DropDownMenuBase parent = Parent as DropDownMenuBase;
                if(parent != null && parent.UpdateToolTip_OnTheEdge(this)) return;
            }
            base.OnMouseHover(e);
        }

        protected override void OnPaint(PaintEventArgs e) {
            if(!fImageLoaded) {
                fImageLoaded = true;
                if(imageReservationKey != null) {
                    QTUtility.LoadReservedImage(imageReservationKey);
                }
            }
            if(!fVirtualQueried) {
                fVirtualQueried = true;
                if(QueryVirtualMenu != null) {
                    QueryVirtualMenu(this, EventArgs.Empty);
                    QueryVirtualMenu = null;
                }
            }
            base.OnPaint(e);
        }

        internal void RestoreOriginalImage() {
            if(!base.ImageKey.Equals(originalImageKey)) {
                base.ImageKey = originalImageKey;
            }
        }

        internal void RestoreOriginalImage(bool changeImageSelected, bool fForceSelectedImage) {
            if(mia != null && (fForceSelectedImage || (changeImageSelected && Selected))) {
                base.ImageKey = mia.IsBack
                    ? (mia.Index == 0 ? "current" : "back")
                    : "forward";
            }
            else if(!base.ImageKey.Equals(originalImageKey)) {
                base.ImageKey = originalImageKey;
            }
        }

        public void SetImageReservationKey(string path, string ext) {
            imageReservationKey = QTUtility.ReserveImageKey(this, path, ext);
            ImageKey = imageReservationKey.ImageKey;
            fImageLoaded = imageReservationKey.ImageKey == "folder";
        }

        public bool Exists                  { get; set; }
        public virtual string Extension     { get; set; }
        public bool ForceToolTip            { get; set; }
        public MenuGenre Genre              { get; private set; }
        public bool HasIcon                 { get; set; }
        public byte[] IDLData               { get; set; }
        public byte[] IDLDataChild          { get; set; }
        public virtual string OriginalTitle { get; set; }
        public string PathChild             { get; set; }
        public MenuTarget Target            { get; private set; }
        public virtual string TargetPath    { get; set; }

        new public string ImageKey {
            get {
                return base.ImageKey;
            }
            set {
                if(base.ImageKey.Length == 0) {
                    originalImageKey = value;
                }
                base.ImageKey = value;
            }
        }

        public bool IsCut {
            get {
                return fCut;
            }
            set {
                if(fCut ^ value) {
                    fCut = value;
                    Invalidate();
                }
            }
        }

        public MenuItemArguments MenuItemArguments {
            get {
                return mia;
            }
            set {
                mia = value;
            }
        }

        public virtual string Path {
            get {
                return path;
            }
            set {
                path = value;
            }
        }
    }

    internal sealed class DirectoryMenuItem : QMenuItem {
        private DateTime dtDirMod;
        private EventPack ep;
        public bool OnceOpened;

        public DirectoryMenuItem(string text)
            : base(text, MenuTarget.Folder, MenuGenre.Application) {
        }

        public EventPack EventPack {
            get {
                return ep;
            }
            set {
                ep = value;
            }
        }

        public DateTime ModifiedDate {
            get {
                return dtDirMod;
            }
            set {
                dtDirMod = value;
            }
        }
    }

    internal sealed class MenuItemArguments {
        public MenuGenre Genre;
        public int Index;
        public bool IsBack;
        public string Path;
        public MenuTarget Target;
        public UserApp App;
        public ShellBrowserEx ShellBrowser;

        public MenuItemArguments(string path, MenuTarget target, MenuGenre genre) {
            Path = path;
            Genre = genre;
            Target = target;
        }

        public MenuItemArguments(string path, bool fback, int index, MenuGenre genre) {
            Path = path;
            IsBack = fback;
            Index = index;
            Genre = genre;
        }

        public MenuItemArguments(UserApp app, ShellBrowserEx shellBrowser, MenuGenre genre) {
            App = app;
            Path = app.Path;
            ShellBrowser = shellBrowser;
            Genre = genre;
        }
    }

    internal enum MenuTarget {
        Folder,
        File,
        VirtualFolder
    }

    internal enum MenuGenre {
        Navigation,
        Branch,
        History,
        Group,
        Application,
        SubDirTip,
        RecentFile
    }
}
