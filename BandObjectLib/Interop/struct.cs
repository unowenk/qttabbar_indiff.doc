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
using System.Drawing;
using System.Runtime.InteropServices;

namespace BandObjectLib.Interop
{

    public struct BROWSEINFO
    {
        // PENDING: or, creates the BIF class for the constants below
        #region ---------- Constants ----------
        public const uint BIF_RETURNONLYFSDIRS = 0x0001;  // For finding a folder to start document searching
        public const uint BIF_DONTGOBELOWDOMAIN = 0x0002;  // For starting the Find Computer
        public const uint BIF_STATUSTEXT = 0x0004;  // Top of the dialog has 2 lines of text for BROWSEINFO.lpszTitle and one line if
        // this flag is set.  Passing the message BFFM_SETSTATUSTEXTA to the hwnd can set the
        // rest of the text.  This is not used with BIF_USENEWUI and BROWSEINFO.lpszTitle gets
        // all three lines of text.
        public const uint BIF_RETURNFSANCESTORS = 0x0008;
        public const uint BIF_EDITBOX = 0x0010;   // Add an editbox to the dialog
        public const uint BIF_VALIDATE = 0x0020;   // insist on valid result (or CANCEL)

        public const uint BIF_NEWDIALOGSTYLE = 0x0040;   // Use the new dialog layout with the ability to resize
        // Caller needs to call OleInitialize() before using this API
        public const uint BIF_USENEWUI = 0x0040 + 0x0010; //(BIF_NEWDIALOGSTYLE | BIF_EDITBOX);

        public const uint BIF_BROWSEINCLUDEURLS = 0x0080;   // Allow URLs to be displayed or entered. (Requires BIF_USENEWUI)
        public const uint BIF_UAHINT = 0x0100;   // Add a UA hint to the dialog, in place of the edit box. May not be combined with BIF_EDITBOX
        public const uint BIF_NONEWFOLDERBUTTON = 0x0200;   // Do not add the "New Folder" button to the dialog.  Only applicable with BIF_NEWDIALOGSTYLE.
        public const uint BIF_NOTRANSLATETARGETS = 0x0400;  // don't traverse target as shortcut

        public const uint BIF_BROWSEFORCOMPUTER = 0x1000;  // Browsing for Computers.
        public const uint BIF_BROWSEFORPRINTER = 0x2000;// Browsing for Printers
        public const uint BIF_BROWSEINCLUDEFILES = 0x4000; // Browsing for Everything
        public const uint BIF_SHAREABLE = 0x8000;  // sharable resources displayed (remote shares, requires BIF_USENEWUI)
        #endregion

        public IntPtr hwndOwner;
        public IntPtr pidlRoot;
        public string pszDisplayName;
        public string lpszTitle;
        public uint ulFlags;
        public IntPtr lpfn;
        public IntPtr lParam;
        public int iImage;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct BITMAPINFOHEADER
    {
        public int biSize;
        public int biWidth;
        public int biHeight;
        public short biPlanes;
        public short biBitCount;
        public int biCompression;
        public int biSizeImage;
        public int biXPelsPerMeter;
        public int biYPelsPerMeter;
        public int biClrUsed;
        public int biClrImportant;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct RGBQUAD
    {
        public byte rgbBlue;
        public byte rgbGreen;
        public byte rgbRed;
        public byte rgbReserved;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct BITMAPINFO
    {
        public BITMAPINFOHEADER bmiHeader;
        public RGBQUAD bmiColors;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct CMINVOKECOMMANDINFO {
        public int cbSize;
        public int fMask;
        public IntPtr hwnd;
        public IntPtr lpVerb;
        public IntPtr lpParameters;
        public IntPtr lpDirectory;
        public int nShow;
        public int dwHotKey;
        public IntPtr hIcon;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct COPYDATASTRUCT
    {
        public IntPtr dwData;
        public int cbData;
        public IntPtr lpData;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct CWPSTRUCT
    {
        public IntPtr lParam;
        public IntPtr wParam;
        public uint message;
        public IntPtr hwnd;
    }
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct DESKBANDINFO
    {
        public DBIM dwMask;
        public Point ptMinSize;
        public Point ptMaxSize;
        public Point ptIntegral;
        public Point ptActual;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0xff)]
        public string wszTitle;
        public DBIMF dwModeFlags;
        public int crBkgnd;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct DEV_BROADCAST_HDR
    {
        public int dbch_size;
        public int dbch_devicetype;
        public int dbch_reserved;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct DEV_BROADCAST_VOLUME
    {
        public int dbcv_size;
        public int dbcv_devicetype;
        public int dbcv_reserved;
        public uint dbcv_unitmask;
        public short dbcv_flags;
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct DROPFILES
    {
        public int pFiles;
        public Point pt;
        public bool fNC;
        public bool fWide;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct DTTOPTS
    {
        public int dwSize;
        public uint dwFlags;
        public uint crText;
        public uint crBorder;
        public uint crShadow;
        public int iTextShadowType;
        public Point ptShadowOffset;
        public int iBorderSize;
        public int iFontPropId;
        public int iColorPropId;
        public int iStateId;
        public int fApplyOverlay;
        public int iGlowSize;
        public IntPtr pfnDrawTextCallback;
        public IntPtr lParam;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct HDITEM
    {
        public int mask;
        public int cxy;
        public IntPtr pszText;
        public IntPtr hbm;
        public int cchTextMax;
        public int fmt;
        public IntPtr lParam;
        public int iImage;
        public int iOrder;
        public int type;
        public IntPtr pvFilter;
        public int state;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct ICONINFO
    {
        public bool fIcon;
        public int xHotspot;
        public int yHotspot;
        public IntPtr hbmMask;
        public IntPtr hbmColor;
    }
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct LOGFONT
    {
        public int lfHeight;
        public int lfWidth;
        public int lfEscapement;
        public int lfOrientation;
        public int lfWeight;
        public byte lfItalic;
        public byte lfUnderline;
        public byte lfStrikeOut;
        public byte lfCharSet;
        public byte lfOutPrecision;
        public byte lfClipPrecision;
        public byte lfQuality;
        public byte lfPitchAndFamily;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
        public string lfFaceName;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct LVHITTESTINFO
    {
        public Point pt;
        public uint flags;
        public int iItem;
        public int iSubItem;
        public int iGroup;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct LVITEM
    {
        public uint mask;
        public int iItem;
        public int iSubItem;
        public uint state;
        public uint stateMask;
        public IntPtr pszText;
        public int cchTextMax;
        public int iImage;
        public IntPtr lParam;
        public int iIndent;
        public int iGroupId;
        public uint cColumns;
        public IntPtr puColumns;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct MARGINS
    {
        public int cxLeftWidth;
        public int cxRightWidth;
        public int cyTopHeight;
        public int cyBottomHeight;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct MOUSEHOOKSTRUCT
    {
        public Point pt;
        public IntPtr hwnd;
        public uint wHitTestCode;
        public IntPtr dwExtraInfo;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct MOUSEHOOKSTRUCTEX
    {
        public MOUSEHOOKSTRUCT mhs;
        public int mouseData;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct MSG
    {
        public IntPtr hwnd;
        public uint message;
        public IntPtr wParam;
        public IntPtr lParam;
        public uint time;
        public POINT pt;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct NMCUSTOMDRAW
    {
        public NMHDR hdr;
        public int dwDrawStage;
        public IntPtr hdc;
        public RECT rc;
        public IntPtr dwItemSpec;
        public int uItemState;
        public IntPtr lItemlParam;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct NMHDR
    {
        public IntPtr hwndFrom;
        public IntPtr idFrom;
        public int code;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct NMITEMACTIVATE
    {
        public NMHDR hdr;
        public int iItem;
        public int iSubItem;
        public int uNewState;
        public int uOldState;
        public int uChanged;
        public Point ptAction;
        public IntPtr lParam;
        public int uKeyFlags;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct NMLISTVIEW
    {
        public NMHDR hdr;
        public int iItem;
        public int iSubItem;
        public uint uNewState;
        public uint uOldState;
        public uint uChanged;
        public Point ptAction;
        public IntPtr lParam;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct NMLVCUSTOMDRAW
    {
        public NMCUSTOMDRAW nmcd;
        public int clrText;
        public int clrTextBk;
        public int iSubItem;
        public int dwItemType;
        public int clrFace;
        public int iIconEffect;
        public int iIconPhase;
        public int iPartId;
        public int iStateId;
        public RECT rcText;
        public int uAlign;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct NMLVDISPINFO
    {
        public NMHDR hdr;
        public LVITEM item;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct NMLVGETINFOTIP
    {
        public NMHDR hdr;
        public int dwFlags;
        public IntPtr pszText;
        public int cchTextMax;
        public int iItem;
        public int iSubItem;
        public IntPtr lParam;
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct NMLVKEYDOWN
    {
        public NMHDR hdr;
        public short wVKey;
        public uint flags;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct NMLVODSTATECHANGE
    {
        public NMHDR hdr;
        public int iFrom;
        public int iTo;
        public uint uNewState;
        public uint uOldState;
    }
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct NMTTDISPINFO
    {
        public NMHDR hdr;
        public IntPtr lpszText;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
        public string szText;
        public IntPtr hinst;
        public int uFlags;
        public IntPtr lParam;
        public IntPtr hbmp;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct NMUPDOWN
    {
        public NMHDR hdr;
        public int iPos;
        public int iDelta;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int x;
        public int y;
        public POINT(Point pnt)
        {
            x = pnt.X;
            y = pnt.Y;
        }

        public Point ToPoint()
        {
            return new Point(x, y);
        }
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct REBARBANDINFO
    {
        public int cbSize;
        public int fMask;
        public int fStyle;
        public int clrFore;
        public int clrBack;
        public IntPtr lpText;
        public int cch;
        public int iImage;
        public IntPtr hwndChild;
        public int cxMinChild;
        public int cyMinChild;
        public int cx;
        public IntPtr hbmBack;
        public int wID;
        public int cyChild;
        public int cyMaxChild;
        public int cyIntegral;
        public int cxIdeal;
        public IntPtr lParam;
        public int cxHeader;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
        public RECT(Rectangle rectangle)
        {
            left = rectangle.X;
            top = rectangle.Y;
            right = rectangle.Right;
            bottom = rectangle.Bottom;
        }

        public int Width
        {
            get
            {
                return Math.Abs((right - left));
            }
        }
        public int Height
        {
            get
            {
                return (bottom - top);
            }
        }
        public Rectangle ToRectangle()
        {
            return new Rectangle(left, top, Width, Height);
        }
    }
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct SHELLEXECUTEINFO
    {
        public int cbSize;
        public int fMask;
        public IntPtr hwnd;
        public IntPtr lpVerb;
        public IntPtr lpFile;
        public IntPtr lpParameters;
        public IntPtr lpDirectory;
        public int nShow;
        public IntPtr hInstApp;
        public IntPtr lpIDList;
        public IntPtr lpClass;
        public IntPtr hkeyClass;
        public int dwHotKey;
        public IntPtr hIconhMonitor;
        public IntPtr hProcess;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct SHELLSTATE
    {
        // fShowAllObjects : 1
        // fShowExtensions : 1
        // fNoConfirmRecycle : 1
        // fShowSysFiles : 1
        // fShowCompColor : 1
        // fDoubleClickInWebView : 1
        // fDesktopHTML : 1
        // fWin95Classic : 1
        // fDontPrettyPath : 1
        // fShowAttribCol : 1
        // fMapNetDrvBtn : 1
        // fShowInfoTip : 1
        // fHideIcons : 1
        // fWebView : 1
        // fFilter : 1
        // fShowSuperHidden : 1
        // fNoNetCrawling : 1
        private uint bitvector1;
        public uint dwWin95Unused;
        public uint uWin95Unused;
        public int lParamSort;
        public int iSortDirection;
        public uint version;
        public uint uNotUsed;
        // fSepProcess : 1
        // fStartPanelOn : 1
        // fShowStartPage : 1
        // fSpareFlags : 13
        private uint bitvector2;

        public uint fShowAllObjects
        {
            get { return (bitvector1 & 1u) / 1; }
            set { bitvector1 = (value * 1) | bitvector1; }
        }

        public uint fShowExtensions
        {
            get { return (bitvector1 & 2u) / 2; }
            set { bitvector1 = (value * 2) | bitvector1; }
        }

        public uint fNoConfirmRecycle
        {
            get { return (bitvector1 & 4u) / 4; }
            set { bitvector1 = (value * 4) | bitvector1; }
        }

        public uint fShowSysFiles
        {
            get { return (bitvector1 & 8u) / 8; }
            set { bitvector1 = (value * 8) | bitvector1; }
        }

        public uint fShowCompColor
        {
            get { return (bitvector1 & 16u) / 16; }
            set { bitvector1 = (value * 16) | bitvector1; }
        }

        public uint fDoubleClickInWebView
        {
            get { return (bitvector1 & 32u) / 32; }
            set { bitvector1 = (value * 32) | bitvector1; }
        }

        public uint fDesktopHTML
        {
            get { return (bitvector1 & 64u) / 64; }
            set { bitvector1 = (value * 64) | bitvector1; }
        }

        public uint fWin95Classic
        {
            get { return (bitvector1 & 128u) / 128; }
            set { bitvector1 = (value * 128) | bitvector1; }
        }

        public uint fDontPrettyPath
        {
            get { return (bitvector1 & 256u) / 256; }
            set { bitvector1 = (value * 256) | bitvector1; }
        }

        public uint fShowAttribCol
        {
            get { return (bitvector1 & 512u) / 512; }
            set { bitvector1 = (value * 512) | bitvector1; }
        }

        public uint fMapNetDrvBtn
        {
            get { return (bitvector1 & 1024u) / 1024; }
            set { bitvector1 = (value * 1024) | bitvector1; }
        }

        public uint fShowInfoTip
        {
            get { return (bitvector1 & 2048u) / 2048; }
            set { bitvector1 = (value * 2048) | bitvector1; }
        }

        public uint fHideIcons
        {
            get { return (bitvector1 & 4096u) / 4096; }
            set { bitvector1 = (value * 4096) | bitvector1; }
        }

        public uint fWebView
        {
            get { return (bitvector1 & 8192u) / 8192; }
            set { bitvector1 = (value * 8192) | bitvector1; }
        }

        public uint fFilter
        {
            get { return (bitvector1 & 16384u) / 16384; }
            set { bitvector1 = (value * 16384) | bitvector1; }
        }

        public uint fShowSuperHidden
        {
            get { return (bitvector1 & 32768u) / 32768; }
            set { bitvector1 = (value * 32768) | bitvector1; }
        }

        public uint fNoNetCrawling
        {
            get { return (bitvector1 & 65536u) / 65536; }
            set { bitvector1 = (value * 65536) | bitvector1; }
        }

        public uint fSepProcess
        {
            get { return bitvector2 & 1u; }
            set { bitvector2 = value | bitvector2; }
        }

        public uint fStartPanelOn
        {
            get { return (bitvector2 & 2u) / 2; }
            set { bitvector2 = (value * 2) | bitvector2; }
        }

        public uint fShowStartPage
        {
            get { return (bitvector2 & 4u) / 4; }
            set { bitvector2 = (value * 4) | bitvector2; }
        }

        public uint fSpareFlags
        {
            get { return (bitvector2 & 65528u) / 8; }
            set { bitvector2 = (value * 8) | bitvector2; }
        }
    }
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct SHFILEINFO
    {
        public IntPtr hIcon;
        public int iIcon;
        public uint dwAttributes;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string szDisplayName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
        public string szTypeName;
    }
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
    public struct SHFILEOPSTRUCT
    {
        public IntPtr hwnd;
        public int wFunc;
        public IntPtr pFrom;
        public IntPtr pTo;
        public short fFlags;
        public bool fAnyOperationsAborted;
        public IntPtr hNameMappings;
        public IntPtr lpszProgressTitle;
    }
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct SHFILEOPSTRUCT64
    {
        public IntPtr hwnd;
        public int wFunc;
        public IntPtr pFrom;
        public IntPtr pTo;
        public short fFlags;
        public bool fAnyOperationsAborted;
        public IntPtr hNameMappings;
        public IntPtr lpszProgressTitle;
    }
    [StructLayout(LayoutKind.Explicit, Size = 260)]
    public struct STRRETinternal
    {
        [FieldOffset(0)]
        public IntPtr cStr;
        [FieldOffset(0)]
        public IntPtr pOleStr;
        [FieldOffset(0)]
        public IntPtr pStr;
        [FieldOffset(0)]
        public uint uOffset;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct STRRET
    {
        public uint uType;
        public STRRETinternal data;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct TBBUTTON
    {
        public int iBitmap;
        public int idCommand;
        [StructLayout(LayoutKind.Explicit)]
        private struct TBBUTTON_U
        {
            [FieldOffset(0)]
            public byte fsState;
            [FieldOffset(1)]
            public byte fsStyle;
            [FieldOffset(0)]
            private IntPtr bReserved;
        }
        private TBBUTTON_U union;
        public byte fsState { get { return union.fsState; } set { union.fsState = value; } }
        public byte fsStyle { get { return union.fsStyle; } set { union.fsStyle = value; } }
        public UIntPtr dwData;
        public IntPtr iString;
    }
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct TBBUTTONINFO
    {
        public int cbSize;
        public int dwMask;
        public int idCommand;
        public int iImage;
        public byte fsState;
        public byte fsStyle;
        public short cx;
        public IntPtr lParam;
        public IntPtr pszText;
        public int cchText;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct TRACKMOUSEEVENT
    {
        public int cbSize;
        public int dwFlags;
        public IntPtr hwndTrack;
        public uint dwHoverTime;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct TVHITTESTINFO
    {
        public Point pt;
        public int flags;
        public IntPtr hItem;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct UIAutomationPropertyInfo
    {
        public Guid guid;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string pProgrammaticName;
        public int type;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct WTS_THUMBNAILID
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x10)]
        public byte[] rgbKey;
    }


    namespace QTTabBar
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct FOLDERSETTINGS
        {
            public int ViewMode;
            public int fFlags;
        }
    }
    namespace QTPluginLib
    {

        [StructLayout(LayoutKind.Sequential)]
        public struct FOLDERSETTINGS
        {
            public FVM ViewMode;
            public int fFlags;
        }
    }

    namespace QTPluginLib
    {
        //remove
        //using FOLDERVIEWMODE;


        //interface  QTPluginLib
        //using IEnumIDList;
        //using IFolderView;
        //using IPersistFolder2;
        //using IShellBrowser;
        //using IShellFolder;
        //using IShellFolder2;
        //using IShellView;
        //struct QTPluginLib
        //using FOLDERSETTINGS;



        //using PInvoke;

        ////struct com
        //using MSG;
        //using POINT;
        //using RECT;


        ////struct QTPlugin  && QTTabBar
        //using STRRET;
        //using STRRETinternal;



        ////struct sig
        //using SHCOLUMNID;
        //using SHELLDETAILS;
        //using VARIANT;
        //using VARIANT=BandObjectLib.Interop.VARIANT;

        [StructLayout(LayoutKind.Sequential)]
        public struct SHCOLUMNID
        {
            public Guid fmtid;
            public int pid;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SHELLDETAILS
        {
            public int fmt;
            public int cxChar;
            public STRRET str;
        }
        [StructLayout(LayoutKind.Explicit, Size = 0x10)]
        public struct VARIANT
        {
            [FieldOffset(8)]
            public byte bValue;
            [FieldOffset(8)]
            public double dValue;
            [FieldOffset(8)]
            public float fValue;
            [FieldOffset(8)]
            public int iValue;
            [FieldOffset(8)]
            public long lValue;
            [FieldOffset(8)]
            public IntPtr pValue;
            [FieldOffset(8)]
            public short sValue;
            [FieldOffset(0)]
            public short vt;
            [FieldOffset(2)]
            public short wReserved1;
            [FieldOffset(4)]
            public short wReserved2;
            [FieldOffset(6)]
            public short wReserved3;
        }
    }


}
