//    This file is part of QTTabBar, a shell extension for Microsoft
//    Windows Explorer.
//    Copyright (C) 2002-2010  Pavel Zolnikov, Quizo, Paul Accisano
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

namespace BandObjectLib.Interop
{


    public enum ScrollAmount
    {
        LargeDecrement = 0,
        SmallDecrement = 1,
        NoAmount = 2,
        LargeIncrement = 3,
        SmallIncrement = 4
    }
    [Flags]
    public enum SVGIO : uint
    {
        BACKGROUND = 0x0,
        SELECTION = 0x1,
        ALLVIEW = 0x2,
        CHECKED = 0x3,
        TYPE_MASK = 0xF,
        FLAG_VIEWORDER = 0x80000000
    }

    [Flags]
    public enum SVSIF
    {
        DESELECT = 0x000,
        SELECT = 0x001,
        EDIT = 0x003,
        DESELECTOTHERS = 0x004,
        ENSUREVISIBLE = 0x008,
        FOCUSED = 0x010,
        TRANSLATEPT = 0x020,
        SELECTIONMARK = 00040,
        POSITIONITEM = 0x080,
        CHECK = 0x100,
        CHECK2 = 0x200,
        KEYBOARDSELECT = 0x401,
        NOTAKEFOCUS = 0x40000000
    }

    // public enum FOLDERVIEWMODE
    public enum FVM
    {
        AUTO = -1,
        ICON = 1,
        SMALLICON = 2,
        LIST = 3,
        DETAILS = 4,
        THUMBNAIL = 5,
        TILE = 6,
        THUMBSTRIP = 7,
        CONTENT = 8, // Windows7
    }

    [Flags]
    public enum FWF : uint
    {
        NONE = 0x00000000,
        AUTOARRANGE = 0x00000001,
        ABBREVIATEDNAMES = 0x00000002,
        SNAPTOGRID = 0x00000004,
        OWNERDATA = 0x00000008,
        BESTFITWINDOW = 0x00000010,
        DESKTOP = 0x00000020,
        SINGLESEL = 0x00000040,
        NOSUBFOLDERS = 0x00000080,
        TRANSPARENT = 0x00000100,
        NOCLIENTEDGE = 0x00000200,
        NOSCROLL = 0x00000400,
        ALIGNLEFT = 0x00000800,
        NOICONS = 0x00001000,
        SHOWSELALWAYS = 0x00002000,
        NOVISIBLE = 0x00004000,
        SINGLECLICKACTIVATE = 0x00008000,
        NOWEBVIEW = 0x00010000,
        HIDEFILENAMES = 0x00020000,
        CHECKSELECT = 0x00040000,
        NOENUMREFRESH = 0x00080000,
        NOGROUPING = 0x00100000,
        FULLROWSELECT = 0x00200000,
        NOFILTERS = 0x00400000,
        NOCOLUMNHEADER = 0x00800000,
        NOHEADERINALLVIEWS = 0x01000000,
        EXTENDEDTILES = 0x02000000,
        TRICHECKSELECT = 0x04000000,
        AUTOCHECKSELECT = 0x08000000,
        NOBROWSERVIEWSTATE = 0x10000000,
        SUBSETGROUPS = 0x20000000,
        USESEARCHFOLDER = 0x40000000,
        ALLOWRTLREADING = 0x80000000,
    }

    [Flags]
    public enum SBSP : uint
    {
        DEFBROWSER = 0x00000000,
        SAMEBROWSER = 0x00000001,
        NEWBROWSER = 0x00000002,
        DEFMODE = 0x00000000,
        OPENMODE = 0x00000010,
        EXPLOREMODE = 0x00000020,
        HELPMODE = 0x00000040,
        NOTRANSFERHIST = 0x00000080,
        AUTONAVIGATE = 0x00000100,
        RELATIVE = 0x00001000,
        PARENT = 0x00002000,
        NAVIGATEBACK = 0x00004000,
        NAVIGATEFORWARD = 0x00008000,
        ALLOW_AUTONAVIGATE = 0x00010000,
        KEEPSAMETEMPLATE = 0x00020000,
        KEEPWORDWHEELTEXT = 0x00040000,
        ACTIVATE_NOFOCUS = 0x00080000,
        CREATENOHISTORY = 0x00100000,
        PLAYNOSOUND = 0x00200000,
        CALLERUNTRUSTED = 0x00800000,
        TRUSTFIRSTDOWNLOAD = 0x01000000,
        UNTRUSTEDFORDOWNLOAD = 0x02000000,
        NOAUTOSELECT = 0x04000000,
        WRITENOHISTORY = 0x08000000,
        TRUSTEDFORACTIVEX = 0x10000000,
        FEEDNAVIGATION = 0x20000000,
        REDIRECT = 0x40000000,
        INITIATEDBYHLINKFRAME = 0x80000000,
    }


    public static class FVO
    {
        public const int DEFAULT = 0x00;
        public const int VISTALAYOUT = 0x01;
        public const int CUSTOMPOSITION = 0x02;
        public const int CUSTOMORDERING = 0x04;
        public const int SUPPORTHYPERLINKS = 0x08;
        public const int NOANIMATIONS = 0x10;
        public const int NOSCROLLTIPS = 0x20;
    }


    public static class LVS_EX
    {
        public const uint GRIDLINES = 0x00000001;
        public const uint SUBITEMIMAGES = 0x00000002;
        public const uint CHECKBOXES = 0x00000004;
        public const uint TRACKSELECT = 0x00000008;
        public const uint HEADERDRAGDROP = 0x00000010;
        public const uint FULLROWSELECT = 0x00000020; // applies to report mode only
        public const uint ONECLICKACTIVATE = 0x00000040;
        public const uint TWOCLICKACTIVATE = 0x00000080;
        public const uint FLATSB = 0x00000100;
        public const uint REGIONAL = 0x00000200;
        public const uint INFOTIP = 0x00000400; // listview does InfoTips for you
        public const uint UNDERLINEHOT = 0x00000800;
        public const uint UNDERLINECOLD = 0x00001000;
        public const uint MULTIWORKAREAS = 0x00002000;
        public const uint LABELTIP = 0x00004000; // listview unfolds partly hidden labels if it does not have infotip text
        public const uint BORDERSELECT = 0x00008000; // border selection style instead of highlight
        public const uint DOUBLEBUFFER = 0x00010000;
        public const uint HIDELABELS = 0x00020000;
        public const uint SINGLEROW = 0x00040000;
        public const uint SNAPTOGRID = 0x00080000; // Icons automatically snap to grid.
        public const uint SIMPLESELECT = 0x00100000; // Also changes overlay rendering to top right for icon mode.
        public const uint JUSTIFYCOLUMNS = 0x00200000; // Icons are lined up in columns that use up the whole view area.
        public const uint TRANSPARENTBKGND = 0x00400000; // Background is painted by the parent via WM_PRINTCLIENT
        public const uint TRANSPARENTSHADOWTEXT = 0x00800000; // Enable shadow text on transparent backgrounds only =useful with bitmaps
        public const uint AUTOAUTOARRANGE = 0x01000000; // Icons automatically arrange if no icon positions have been set
        public const uint HEADERINALLVIEWS = 0x02000000; // Display column header in all view modes
        public const uint AUTOCHECKSELECT = 0x08000000;
        public const uint AUTOSIZECOLUMNS = 0x10000000;
        public const uint COLUMNSNAPPOINTS = 0x40000000;
        public const uint COLUMNOVERFLOW = 0x80000000;
    };


    public static class LVIS
    {
        public const int FOCUSED = 0x0001;
        public const int SELECTED = 0x0002;
        public const int CUT = 0x0004;
        public const int DROPHILITED = 0x0008;
        public const int GLOW = 0x0010;
        public const int ACTIVATING = 0x0020;
        public const int OVERLAYMASK = 0x0F00;
        public const int STATEIMAGEMASK = 0xF000;
    }


    public static class LVNI
    {
        public const int ALL = 0x0000;
        public const int FOCUSED = 0x0001;
        public const int SELECTED = 0x0002;
        public const int CUT = 0x0004;
        public const int DROPHILITED = 0x0008;
        public const int STATEMASK = (FOCUSED | SELECTED | CUT | DROPHILITED);
        public const int VISIBLEORDER = 0x0010;
        public const int PREVIOUS = 0x0020;
        public const int VISIBLEONLY = 0x0040;
        public const int SAMEGROUPONLY = 0x0080;
        public const int ABOVE = 0x0100;
        public const int BELOW = 0x0200;
        public const int TOLEFT = 0x0400;
        public const int TORIGHT = 0x0800;
        public const int DIRECTIONMASK = (ABOVE | BELOW | TOLEFT | TORIGHT);
    }


    public static class CDDS
    {
        public const int PREPAINT = 1;
        public const int POSTPAINT = 2;
        public const int PREERASE = 3;
        public const int POSTERASE = 4;
        public const int ITEM = 0x10000;
        public const int ITEMPREPAINT = (ITEM | PREPAINT);
        public const int ITEMPOSTPAINT = (ITEM | POSTPAINT);
        public const int ITEMPREERASE = (ITEM | PREERASE);
        public const int ITEMPOSTERASE = (ITEM | POSTERASE);
        public const int SUBITEM = 0x20000;
    }

    public static class CDRF
    {
        public const int DODEFAULT = 0x000;
        public const int NEWFONT = 0x002;
        public const int SKIPDEFAULT = 0x004;
        public const int DOERASE = 0x008;
        public const int NOTIFYPOSTPAINT = 0x010;
        public const int NOTIFYITEMDRAW = 0x020;
        public const int NOTIFYSUBITEMDRAW = 0x020;
        public const int NOTIFYPOSTERASE = 0x040;
        public const int SKIPPOSTPAINT = 0x100;
    }
}
