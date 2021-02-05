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
using System.Drawing;
using System.Windows.Forms;
using System.Text;
using System.Runtime.InteropServices;
using System.Security;

using IDataObject = System.Runtime.InteropServices.ComTypes.IDataObject;

namespace BandObjectLib.Interop
{
    public delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);
    public delegate bool EnumWndProc(IntPtr hwnd, IntPtr lParam);



    //OneCoreCommonProxyStub.dll/IServiceProvider.PSFactoryBuffer
    [ComImport, Guid("6d5140c1-7436-11ce-8034-00aa006009fa"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), SuppressUnmanagedCodeSecurity]
    public interface _IServiceProvider {
        void QueryService(
                [In, MarshalAs(UnmanagedType.LPStruct)] Guid guid,
                [In, MarshalAs(UnmanagedType.LPStruct)] Guid riid,
                [MarshalAs(UnmanagedType.Interface)] out object Obj);
    }

    //combase.dll/IDropTarget.PSFactoryBuffer
    [ComImport, Guid("00000122-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), SuppressUnmanagedCodeSecurity]
    public interface _IDropTarget
    {
        [PreserveSig]
        int DragEnter(IDataObject pDataObj, int grfKeyState, Point pt, ref DragDropEffects pdwEffect);
        [PreserveSig]
        int DragOver(int grfKeyState, Point pt, ref DragDropEffects pdwEffect);
        [PreserveSig]
        int DragLeave();
        [PreserveSig]
        int DragDrop(IDataObject pDataObj, int grfKeyState, Point pt, ref DragDropEffects pdwEffect);
    }


    public abstract class ExplorerGUIDs
    {
        //OneCoreUAPCommonProxyStub.dll/IDeskBand.PSFactoryBuffer
        public static readonly Guid CGID_DeskBand = new Guid("{EB0FE172-1A3A-11D0-89B3-00A0C90A90AC}");
        //xplorerframe.dll/Task Bar Communication.PSFactoryBuffer
        public static readonly Guid CLSID_TaskbarList = new Guid("56FDF344-FD6D-11d0-958A-006097C9A090");
        //null
        public static readonly Guid IID_IContextMenu = new Guid("{000214e4-0000-0000-c000-000000000046}");
        //combase.dll/IDataObject.PSFactoryBuffer
        public static readonly Guid IID_IDataObject = new Guid("{0000010e-0000-0000-C000-000000000046}");
        //combase.dll/IDropTarget.PSFactoryBuffer
        public static readonly Guid IID_IDropTarget = new Guid("00000122-0000-0000-C000-000000000046");
        //null
        public static readonly Guid IID_IDropTargetHelper = new Guid("4657278B-411B-11D2-839A-00C04FD918D0");
        //OneCoreUAPCommonProxyStub.dll/IEnumIDList.PSFactoryBuffer
        public static readonly Guid IID_IEnumIDList = new Guid("{000214F2-0000-0000-C000-000000000046}");
        //OneCoreUAPCommonProxyStub.dll/IExtractImage.PSFactoryBuffer
        public static readonly Guid IID_IExtractImage = new Guid("BB2E617C-0920-11d1-9A0B-00C04FC2D6C1");
        //OneCoreUAPCommonProxyStub.dll/IPersistFolder2.PSFactoryBuffer
        public static readonly Guid IID_IPersistFolder2 = new Guid("{1AC3D9F0-175C-11d1-95BE-00609797EA4F}");
        //null
        public static readonly Guid IID_IQueryInfo = new Guid("00021500-0000-0000-c000-000000000046");
        //OneCoreUAPCommonProxyStub.dll/IShellBrowser.PSFactoryBuffer
        public static readonly Guid IID_IShellBrowser = new Guid("{000214E2-0000-0000-C000-000000000046}");
        //OneCoreUAPCommonProxyStub.dll/IShellFolder.PSFactoryBuffer
        public static readonly Guid IID_IShellFolder = new Guid("{000214E6-0000-0000-C000-000000000046}");
        //OneCoreUAPCommonProxyStub.dll/ITaskbarList.PSFactoryBuffer
        public static readonly Guid IID_ITaskbarList = new Guid("56FDF342-FD6D-11d0-958A-006097C9A090");
        //ActXPrxy.dll/ITravelLogStg.PSFactoryBuffer
        public static readonly Guid IID_ITravelLogStg = new Guid("{7EBFDD80-AD18-11d3-A4C5-00C04F72D6B8}");
        //IUnknown
        public static readonly Guid IID_IUnknown = new Guid("{00000000-0000-0000-C000-000000000046}");
        //oleaut32.dll/IWebBrowserApp.PSOAInterface
        //TypeLib==Microsoft Internet Controls
        public static readonly Guid IID_IWebBrowserApp = new Guid("{0002DF05-0000-0000-C000-000000000046}");
    }




    //null
    [ComImport, Guid("000214F4-0000-0000-c000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), SuppressUnmanagedCodeSecurity]
    public interface IContextMenu2
    {
        [PreserveSig]
        int QueryContextMenu(IntPtr hMenu, uint indexMenu, uint idCmdFirst, uint idCmdLast, uint uFlags);
        [PreserveSig]
        int InvokeCommand(ref CMINVOKECOMMANDINFO pici);
        void GetCommandString(uint idCmd, uint uFlags, ref int pwReserved, IntPtr commandstring, uint cch);
        [PreserveSig]
        int HandleMenuMsg(int uMsg, IntPtr wParam, IntPtr lParam);
    }

    //ActXPrxy.dll/IDeskBand2.PSFactoryBuffer
    [ComImport, SuppressUnmanagedCodeSecurity, Guid("79D16DE4-ABEE-4021-8D9D-9169B261D657"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDeskBand2
    {
        void GetWindow(out IntPtr phwnd);
        void ContextSensitiveHelp([In] bool fEnterMode);
        void ShowDW([In] bool fShow);
        void CloseDW([In] uint dwReserved);
        void ResizeBorderDW(IntPtr prcBorder, [In, MarshalAs(UnmanagedType.IUnknown)] object punkToolbarSite, bool fReserved);
        void GetBandInfo(uint dwBandID, uint dwViewMode, ref DESKBANDINFO pdbi);
        void CanRenderComposited(out bool pfCanRenderComposited);
        void SetCompositionState(bool fCompositionEnabled);
        void GetCompositionState(out bool pfCompositionEnabled);
    }
    namespace QTTabBar
    {
        //OneCoreUAPCommonProxyStub.dll\IEnumIDList.PSFactoryBuffer
        [ComImport, Guid("000214F2-0000-0000-C000-000000000046"), SuppressUnmanagedCodeSecurity, InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IEnumIDList
        {
            [PreserveSig]
            int Next(uint celt, out IntPtr rgelt, object pceltFetched);
            void Skip(uint celt);
            void Reset();
            void Clone(out IEnumIDList ppenum);
        }
    }
    namespace QTPluginLib
    {
        //OneCoreUAPCommonProxyStub.dll\IEnumIDList.PSFactoryBuffer
        [ComImport, Guid("000214F2-0000-0000-C000-000000000046"), SuppressUnmanagedCodeSecurity, InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IEnumIDList
        {
            [PreserveSig]
            int Next(uint celt, out IntPtr rgelt, out IntPtr pceltFetched);
            [PreserveSig]
            int Skip(uint celt);
            [PreserveSig]
            int Reset();
            [PreserveSig]
            int Clone(out IEnumIDList ppenum);
        }
    }
    //ActXPrxy.dll\IEnumTravelLogEntry.PSFactoryBuffer
    [ComImport, Guid("7EBFDD85-AD18-11d3-A4C5-00C04F72D6B8"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), SuppressUnmanagedCodeSecurity]
    public interface IEnumTravelLogEntry
    {
        [PreserveSig]
        int Next([In] uint cElt, [MarshalAs(UnmanagedType.Interface)] out ITravelLogEntry rgElt, [Out] uint pcEltFetched);
        [PreserveSig]
        int Skip([In] uint cElt);
        [PreserveSig]
        int Reset();
        [PreserveSig]
        int Clone([MarshalAs(UnmanagedType.Interface)] out IEnumTravelLogEntry ppEnum);
    }
    //OneCoreUAPCommonProxyStub.dll\IExtractImage.PSFactoryBuffer
    [ComImport, Guid("BB2E617C-0920-11d1-9A0B-00C04FC2D6C1"), SuppressUnmanagedCodeSecurity, InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IExtractImage
    {
        [PreserveSig]
        int GetLocation(StringBuilder pszPathBuffer, int cch, ref int pdwPriority, ref Size prgSize, int dwRecClrDepth, ref int pdwFlags);
        [PreserveSig]
        int Extract(out IntPtr phBmpThumbnail);
    }

    namespace QTTabBar
    {
        //OneCoreUAPCommonProxyStub.dll\IFolderView.PSFactoryBuffer
        [ComImport, SuppressUnmanagedCodeSecurity, Guid("cde725b0-ccc9-4519-917e-325d72fab4ce"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IFolderView
        {
            [PreserveSig]
            int GetCurrentViewMode(ref FVM pViewMode);
            [PreserveSig]
            int SetCurrentViewMode(FVM ViewMode);
            [PreserveSig]
            int GetFolder(ref Guid riid, out IPersistFolder2 ppv);
            [PreserveSig]
            int Item(int iItemIndex, out IntPtr ppidl);
            [PreserveSig]
            int ItemCount(SVGIO uFlags, out int pcItems);
            [PreserveSig]
            int Items(SVGIO uFlags, ref Guid riid, out IEnumIDList ppv);
            [PreserveSig]
            int GetSelectionMarkedItem(out int piItem);
            [PreserveSig]
            int GetFocusedItem(out int piItem);
            [PreserveSig]
            int GetItemPosition(IntPtr pidl, out Point ppt);
            [PreserveSig]
            int GetSpacing(ref Point ppt);
            [PreserveSig]
            int GetDefaultSpacing(ref Point ppt);
            [PreserveSig]
            int GetAutoArrange();
            [PreserveSig]
            int SelectItem(int iItem, SVSIF dwFlags);
            [PreserveSig]
            int SelectAndPositionItems(uint cidl, IntPtr apidl, IntPtr apt, SVSIF dwFlags);
        }

        //OneCoreUAPCommonProxyStub.dll\IFolderView2.PSFactoryBuffer
        [ComImport, SuppressUnmanagedCodeSecurity, Guid("1af3a467-214f-4298-908e-06b03e0b39f9"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IFolderView2
        {
            [PreserveSig]
            int GetCurrentViewMode(ref int pViewMode);
            [PreserveSig]
            int SetCurrentViewMode(int ViewMode);
            [PreserveSig]
            int GetFolder(ref Guid riid, out IPersistFolder2 ppv);
            [PreserveSig]
            int Item(int iItemIndex, out IntPtr ppidl);
            [PreserveSig]
            int ItemCount(uint uFlags, out int pcItems);
            [PreserveSig]
            int Items(uint uFlags, ref Guid riid, out IEnumIDList ppv);
            [PreserveSig]
            int GetSelectionMarkedItem(out int piItem);
            [PreserveSig]
            int GetFocusedItem(out int piItem);
            [PreserveSig]
            int GetItemPosition(IntPtr pidl, out Point ppt);
            [PreserveSig]
            int GetSpacing(ref Point ppt);
            [PreserveSig]
            int GetDefaultSpacing(ref Point ppt);
            [PreserveSig]
            int GetAutoArrange();
            [PreserveSig]
            int SelectItem(int iItem, uint dwFlags);
            [PreserveSig]
            int SelectAndPositionItems(uint cidl, IntPtr apidl, IntPtr apt, int dwFlags);
            [PreserveSig]
            int SetGroupBy(ref int key, bool fAscending);
            [PreserveSig]
            int GetGroupBy(out int pkey, out bool pfAscending);
            [PreserveSig]
            /* NOT DECLARED */
            int SetViewProperty(IntPtr pidl, ref int propkey, ref object propvar); // ?
            [PreserveSig]
            /* NOT DECLARED */
            int GetViewProperty(IntPtr pidl, ref int propkey, out object propvar); // ?
            [PreserveSig]
            int SetTileViewProperties(IntPtr pidl, [MarshalAs(UnmanagedType.LPWStr)] string pszPropList);
            [PreserveSig]
            int SetExtendedTileViewProperties(IntPtr pidl, [MarshalAs(UnmanagedType.LPWStr)] string pszPropList);
            [PreserveSig]
            int SetText(int iType, [MarshalAs(UnmanagedType.LPWStr)] string pwszText);
            [PreserveSig]
            int SetCurrentFolderFlags(int dwMask, int dwFlags);
            [PreserveSig]
            int GetCurrentFolderFlags(out int pdwFlags);
            [PreserveSig]
            int GetSortColumnCount(out int pcColumns);
            [PreserveSig]
            /* NOT DECLARED */
            int SetSortColumns(/*ref SORTCOLUMN rgSortColumns, int cColumns*/);
            [PreserveSig]
            /* NOT DECLARED */
            int GetSortColumns(/*out SORTCOLUMN, int cColumns*/);
            [PreserveSig]
            int GetItem(int iItem, ref Guid riid, [MarshalAs(UnmanagedType.IUnknown)] out object ppv);
            [PreserveSig]
            int GetVisibleItem(int iStart, bool fPrevious, out int piItem);
            [PreserveSig]
            int GetSelectedItem(int iStart, out int piItem);
            [PreserveSig]
            /* NOT DECLARED */
            int GetSelection(/* bool fNoneImpliesFolder, out IShellItemArray ppsia */);
            [PreserveSig]
            int GetSelectionState(IntPtr pidl, out int pdwFlags);
            [PreserveSig]
            int InvokeVerbOnSelection([MarshalAs(UnmanagedType.LPWStr)] string pszVerb);
            [PreserveSig]
            int SetViewModeAndIconSize(int uViewMode, int iImageSize);
            [PreserveSig]
            int GetViewModeAndIconSize(out int puViewMode, out int piImageSize);
            [PreserveSig]
            int SetGroupSubsetCount(uint cVisibleRows);
            [PreserveSig]
            int GetGroupSubsetCount(out uint pcVisibleRows);
            [PreserveSig]
            int SetRedraw(bool fRedrawOn);
            [PreserveSig]
            int IsMoveInSameFolder();
            [PreserveSig]
            int DoRename();
        }


    }

    namespace QTPluginLib
    {
        //OneCoreUAPCommonProxyStub.dll\IFolderView.PSFactoryBuffer
        [ComImport, SuppressUnmanagedCodeSecurity, Guid("cde725b0-ccc9-4519-917e-325d72fab4ce"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IFolderView
        {
            [PreserveSig]
            int GetCurrentViewMode(ref FVM pViewMode);
            [PreserveSig]
            int SetCurrentViewMode(FVM ViewMode);
            [PreserveSig]
            int GetFolder(ref Guid riid, out IPersistFolder2 ppv);
            [PreserveSig]
            int Item(int iItemIndex, out IntPtr ppidl);
            [PreserveSig]
            int ItemCount(uint uFlags, out int pcItems);
            [PreserveSig]
            int Items(uint uFlags, [In] ref Guid riid, out object ppv);
            [PreserveSig]
            int GetSelectionMarkedItem(out int piItem);
            [PreserveSig]
            int GetFocusedItem(out int piItem);
            [PreserveSig]
            int GetItemPosition(IntPtr pidl, out POINT ppt);
            [PreserveSig]
            int GetSpacing(ref POINT ppt);
            [PreserveSig]
            int GetDefaultSpacing(ref POINT ppt);
            [PreserveSig]
            int GetAutoArrange();
            [PreserveSig]
            int SelectItem(int iItem, int dwFlags);
            [PreserveSig]
            int SelectAndPositionItems(uint cidl, IntPtr apidl, IntPtr apt, int dwFlags);
        }
    }
    //ActXPrxy.dll\IFolderViewOptions.PSFactoryBuffer
    [ComImport, SuppressUnmanagedCodeSecurity, Guid("3cc974d2-b302-4d36-ad3e-06d93f695d3f"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IFolderViewOptions
    {
        [PreserveSig]
        int SetFolderViewOptions(int fvoMask, int fvoFlags);
        [PreserveSig]
        int GetFolderViewOptions(out int pfvoFlags);
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

    //OneCoreUAPCommonProxyStub.dll\IInputObjectSite.PSFactoryBuffer
    [ComImport, SuppressUnmanagedCodeSecurity, Guid("f1db8392-7331-11d0-8c99-00a0c92dbfe8"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IInputObjectSite
    {
        [PreserveSig]
        int OnFocusChangeIS([MarshalAs(UnmanagedType.IUnknown)] object punkObj, int fSetFocus);
    }

    //OneCoreUAPCommonProxyStub.dll\INameSpaceTreeControl.PSFactoryBuffer
    [ComImport, Guid("028212A3-B627-47e9-8856-C14265554E4F"), SuppressUnmanagedCodeSecurity, InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface INameSpaceTreeControl
    {
        [PreserveSig]
        int Initialize(IntPtr hwndParent, ref RECT prc, int nsctsFlags);
        [PreserveSig]
        int TreeAdvise(IntPtr punk, out int pdwCookie);
        [PreserveSig]
        int TreeUnadvise(int dwCookie);
        [PreserveSig]
        int AppendRoot(IShellItem psiRoot, int grfEnumFlags, int grfRootStyle, /*IShellItemFilter*/ IntPtr pif);
        [PreserveSig]
        int InsertRoot(int iIndex, IShellItem psiRoot, int grfEnumFlags, int grfRootStyle, /*IShellItemFilter*/ IntPtr pif);
        [PreserveSig]
        int RemoveRoot(IShellItem psiRoot);
        [PreserveSig]
        int RemoveAllRoots();
        [PreserveSig]
        int GetRootItems(out /*IShellItemArray*/ IntPtr ppsiaRootItems);
        [PreserveSig]
        int SetItemState(IShellItem psi, int nstcisMask, int nstcisFlags);
        [PreserveSig]
        int GetItemState(IShellItem psi, int nstcisMask, out int pnstcisFlags);
        [PreserveSig]
        int GetSelectedItems(out /*IShellItemArray*/ IntPtr psiaItems);
        [PreserveSig]
        int GetItemCustomState(IShellItem psi, out int piStateNumber);
        [PreserveSig]
        int SetItemCustomState(IShellItem psi, int iStateNumber);
        [PreserveSig]
        int EnsureItemVisible(IShellItem psi);
        [PreserveSig]
        int SetTheme(string pszTheme);
        [PreserveSig]
        int GetNextItem(IShellItem psi, int nstcgi, out IShellItem ppsiNext);
        [PreserveSig]
        int HitTest([In] ref Point ppt, out IShellItem ppsiOut);
        [PreserveSig]
        int GetItemRect(IShellItem psi, out RECT prect);
        [PreserveSig]
        int CollapseAll();
    }
    [ComImport, SuppressUnmanagedCodeSecurity, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("FC4801A3-2BA9-11CF-A229-00AA003D7352")]
    public interface IObjectWithSite
    {
        void SetSite([In, MarshalAs(UnmanagedType.IUnknown)] object pUnkSite);
        void GetSite(ref Guid riid, [MarshalAs(UnmanagedType.IUnknown)] out object ppvSite);
    }
    [ComImport, SuppressUnmanagedCodeSecurity, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("B722BCCB-4E68-101B-A2BC-00AA00404770")]
    public interface IOleCommandTarget
    {
        [PreserveSig]
        int QueryStatus([In] ref Guid pguidCmdGroup, int cCmds, IntPtr prgCmds, IntPtr pCmdText);
        [PreserveSig]
        int Exec([In] ref Guid pguidCmdGroup, uint nCmdID, uint nCmdExecOpt, IntPtr pvaIn, IntPtr pvaOut);
    }

    namespace QTTabBar
    {
        [ComImport, SuppressUnmanagedCodeSecurity, Guid("1AC3D9F0-175C-11d1-95BE-00609797EA4F"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IPersistFolder2
        {
            void GetClassID(out Guid pClassID);
            void Initialize(IntPtr pidl);
            [PreserveSig]
            int GetCurFolder(out IntPtr ppidl);
        }
    }

    namespace QTPluginLib
    {
        [ComImport, SuppressUnmanagedCodeSecurity, Guid("1AC3D9F0-175C-11d1-95BE-00609797EA4F"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IPersistFolder2
        {
            [PreserveSig]
            int GetClassID(out Guid pClassID);
            [PreserveSig]
            int Initialize(IntPtr pidl);
            [PreserveSig]
            int GetCurFolder(out IntPtr ppidl);
        }
    }

    [ComImport, Guid("00000109-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), SuppressUnmanagedCodeSecurity]
    public interface IPersistStream
    {
        void GetClassID(out Guid pClassID);
        [PreserveSig]
        int IsDirty();
        void IPersistStreamLoad([In, MarshalAs(UnmanagedType.Interface)] object pStm);
        void Save([In, MarshalAs(UnmanagedType.Interface)] IntPtr pStm, [In, MarshalAs(UnmanagedType.Bool)] bool fClearDirty);
        [PreserveSig]
        int GetSizeMax(out ulong pcbSize);
    }
    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), SuppressUnmanagedCodeSecurity, Guid("00021500-0000-0000-c000-000000000046")]
    public interface IQueryInfo
    {
        [PreserveSig]
        int GetInfoTip(int dwFlags, [MarshalAs(UnmanagedType.LPWStr)] out string ppwszTip);
        [PreserveSig]
        int GetInfoFlags(out IntPtr pdwFlags);
    }
    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), SuppressUnmanagedCodeSecurity, Guid("091162a4-bc96-411f-aae8-c5122cd03363")]
    public interface ISharedBitmap
    {
        [PreserveSig]
        int GetSharedBitmap(out IntPtr phbm);
        [PreserveSig]
        int GetSize(out Size pSize);
        [PreserveSig]
        int GetFormat(out uint pat);
        [PreserveSig]
        int InitializeBitmap(IntPtr hbm, uint wtsAT);
        [PreserveSig]
        int Detach(out IntPtr phbm);
    }


    namespace QTTabBar
    {
        [ComImport, Guid("000214E2-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), SuppressUnmanagedCodeSecurity]
        public interface IShellBrowser
        {
            [PreserveSig]
            int GetWindow(out IntPtr phwnd);
            [PreserveSig]
            int ContextSensitiveHelp(bool fEnterMode);
            [PreserveSig]
            int InsertMenusSB(IntPtr hmenuShared, IntPtr lpMenuWidths);
            [PreserveSig]
            int SetMenuSB(IntPtr hmenuShared, IntPtr holemenuRes, IntPtr hwndActiveObject);
            [PreserveSig]
            int RemoveMenusSB(IntPtr hmenuShared);
            [PreserveSig]
            int SetStatusTextSB([MarshalAs(UnmanagedType.BStr)] string pszStatusText);
            [PreserveSig]
            int EnableModelessSB(bool fEnable);
            [PreserveSig]
            int TranslateAcceleratorSB(MSG pmsg, ushort wID);
            [PreserveSig]
            int BrowseObject(IntPtr pidl, SBSP wFlags);
            [PreserveSig]
            int GetViewStateStream(uint grfMode, out IntPtr ppStrm);
            [PreserveSig]
            int GetControlWindow(uint id, out IntPtr phwnd);
            [PreserveSig]
            int SendControlMsg(uint id, uint uMsg, IntPtr wParam, IntPtr lParam, out IntPtr pret);
            [PreserveSig]
            int QueryActiveShellView(out IShellView ppshv);
            [PreserveSig]
            int OnViewWindowActive(IntPtr pshv);
            [PreserveSig]
            int SetToolbarItems(IntPtr lpButtons, uint nButtons, uint uFlags);
        }
    }

    namespace QTPluginLib
    {
        [ComImport, Guid("000214E2-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), SuppressUnmanagedCodeSecurity]
        public interface IShellBrowser
        {
            [PreserveSig]
            int GetWindow(out IntPtr phwnd);
            [PreserveSig]
            int ContextSensitiveHelp(bool fEnterMode);
            [PreserveSig]
            int InsertMenusSB(IntPtr hmenuShared, IntPtr lpMenuWidths);
            [PreserveSig]
            int SetMenuSB(IntPtr hmenuShared, IntPtr holemenuRes, IntPtr hwndActiveObject);
            [PreserveSig]
            int RemoveMenusSB(IntPtr hmenuShared);
            [PreserveSig]
            int SetStatusTextSB([MarshalAs(UnmanagedType.BStr)] string pszStatusText);
            [PreserveSig]
            int EnableModelessSB(bool fEnable);
            [PreserveSig]
            int TranslateAcceleratorSB(ref MSG pmsg, ushort wID);
            [PreserveSig]
            int BrowseObject(IntPtr pidl, uint wFlags);
            [PreserveSig]
            int GetViewStateStream(uint grfMode, IntPtr ppStrm);
            [PreserveSig]
            int GetControlWindow(uint id, out IntPtr phwnd);
            [PreserveSig]
            int SendControlMsg(uint id, uint uMsg, IntPtr wParam, IntPtr lParam, out IntPtr pret);
            [PreserveSig]
            int QueryActiveShellView(out IShellView ppshv);
            [PreserveSig]
            int OnViewWindowActive(IShellView pshv);
            [PreserveSig]
            int SetToolbarItems(IntPtr lpButtons, uint nButtons, uint uFlags);
        }
    }


    namespace QTTabBar
    {
        [ComImport, SuppressUnmanagedCodeSecurity, Guid("000214E6-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IShellFolder
        {
            [PreserveSig]
            int ParseDisplayName(IntPtr hwnd, IntPtr pbc, [MarshalAs(UnmanagedType.LPWStr)] string pszDisplayName, ref uint pchEaten, out IntPtr ppidl, ref uint pdwAttributes);
            [PreserveSig]
            int EnumObjects(IntPtr hwnd, int grfFlags, out IEnumIDList ppenumIDList);
            [PreserveSig]
            int BindToObject(IntPtr pidl, IntPtr pbc, [In] ref Guid riid, out IShellFolder ppv);
            [PreserveSig]
            int BindToStorage(IntPtr pidl, IntPtr pbc, Guid riid, out IntPtr ppv);
            [PreserveSig]
            int CompareIDs(IntPtr lParam, IntPtr pidl1, IntPtr pidl2);
            [PreserveSig]
            int CreateViewObject(IntPtr hwndOwner, ref Guid riid, out IShellView ppv);
            [PreserveSig]
            int GetAttributesOf(uint cidl, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0, SizeConst = 0)] IntPtr[] apidl, ref uint rgfInOut);
            [PreserveSig]
            int GetUIObjectOf(IntPtr hwndOwner, uint cidl, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1, SizeConst = 0)] IntPtr[] apidl, [In] ref Guid riid, ref uint rgfReserved, [MarshalAs(UnmanagedType.Interface)] out object ppv);
            [PreserveSig]
            int GetDisplayNameOf(IntPtr pidl, uint uFlags, out STRRET pName);
            [PreserveSig]
            int SetNameOf(IntPtr hwnd, IntPtr pidl, [MarshalAs(UnmanagedType.LPWStr)] string pszName, uint uFlags, out IntPtr ppidlOut);
        }
    }

    namespace QTPluginLib
    {
        [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("000214E6-0000-0000-C000-000000000046")]
        public interface IShellFolder
        {
            [PreserveSig]
            int ParseDisplayName(IntPtr hwnd, IntPtr pbc, [MarshalAs(UnmanagedType.LPWStr)] string pszDisplayName, ref uint pchEaten, out IntPtr ppidl, ref uint pdwAttributes);
            [PreserveSig]
            int EnumObjects(IntPtr hwnd, int grfFlags, out IEnumIDList ppenumIDList);
            [PreserveSig]
            int BindToObject(IntPtr pidl, IntPtr pbc, [In] ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out object ppv);
            [PreserveSig]
            int BindToStorage(IntPtr pidl, IntPtr pbc, [In] ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out object ppv);
            [PreserveSig]
            int CompareIDs(IntPtr lParam, IntPtr pidl1, IntPtr pidl2);
            [PreserveSig]
            int CreateViewObject(IntPtr hwndOwner, [In] ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out object ppv);
            [PreserveSig]
            int GetAttributesOf(uint cidl, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0, SizeConst = 0)] IntPtr[] apidl, ref uint rgfInOut);
            [PreserveSig]
            int GetUIObjectOf(IntPtr hwndOwner, uint cidl, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1, SizeConst = 0)] IntPtr[] apidl, [In] ref Guid riid, ref uint rgfReserved, [MarshalAs(UnmanagedType.Interface)] out object ppv);
            [PreserveSig]
            int GetDisplayNameOf(IntPtr pidl, uint uFlags, out STRRET pName);
            [PreserveSig]
            int SetNameOf(IntPtr hwndOwner, IntPtr pidl, [MarshalAs(UnmanagedType.LPWStr)] string pszName, uint uFlags, out IntPtr ppidlOut);
        }


        [ComImport, Guid("93F2F68C-1D1B-11d3-A30E-00C04F79ABD1"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IShellFolder2
        {
            [PreserveSig]
            int ParseDisplayName(IntPtr hwnd, IntPtr pbc, [MarshalAs(UnmanagedType.LPWStr)] string pszDisplayName, ref uint pchEaten, out IntPtr ppidl, ref uint pdwAttributes);
            [PreserveSig]
            int EnumObjects(IntPtr hwnd, int grfFlags, out IEnumIDList ppenumIDList);
            [PreserveSig]
            int BindToObject(IntPtr pidl, IntPtr pbc, [In] ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out object ppv);
            [PreserveSig]
            int BindToStorage(IntPtr pidl, IntPtr pbc, [In] ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out object ppv);
            [PreserveSig]
            int CompareIDs(IntPtr lParam, IntPtr pidl1, IntPtr pidl2);
            [PreserveSig]
            int CreateViewObject(IntPtr hwndOwner, [In] ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out object ppv);
            [PreserveSig]
            int GetAttributesOf(uint cidl, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0, SizeConst = 0)] IntPtr[] apidl, ref uint rgfInOut);
            [PreserveSig]
            int GetUIObjectOf(IntPtr hwndOwner, uint cidl, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1, SizeConst = 0)] IntPtr[] apidl, [In] ref Guid riid, ref uint rgfReserved, [MarshalAs(UnmanagedType.Interface)] out object ppv);
            [PreserveSig]
            int GetDisplayNameOf(IntPtr pidl, uint uFlags, out STRRET pName);
            [PreserveSig]
            int SetNameOf(IntPtr hwndOwner, IntPtr pidl, [MarshalAs(UnmanagedType.LPWStr)] string pszName, uint uFlags, out IntPtr ppidlOut);
            [PreserveSig]
            int GetDefaultSearchGUID(out Guid pguid);
            [PreserveSig]
            int EnumSearches(out IntPtr ppenum);
            [PreserveSig]
            int GetDefaultColumn(int dwRes, out uint pSort, out uint pDisplay);
            [PreserveSig]
            int GetDefaultColumnState(int iColumn, out uint pcsFlags);
            [PreserveSig]
            int GetDetailsEx(IntPtr pidl, [In] ref SHCOLUMNID pscid, out VARIANT pv);
            [PreserveSig]
            int GetDetailsOf(IntPtr pidl, int iColumn, out SHELLDETAILS psd);
            [PreserveSig]
            int MapColumnToSCID(int iColumn, out SHCOLUMNID pscid);
        }


    }


    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("37A378C0-F82D-11CE-AE65-08002B2E1262"), SuppressUnmanagedCodeSecurity]
    public interface IShellFolderView
    {
        [PreserveSig]
        int Rearrange(IntPtr lParamSort);
        [PreserveSig]
        int GetArrangeParam(out IntPtr plParamSort);
        void ArrangeGrid();
        void AutoArrange();
        void GetAutoArrange();
        [PreserveSig]
        int AddObject(IntPtr pidl, out int puItem);
        [PreserveSig]
        int GetObject(out IntPtr ppidl, int uItem);
        [PreserveSig]
        int RemoveObject(IntPtr pidl, out int puItem);
        void GetObjectCount(out int puCount);
        void SetObjectCount(int uCount, int dwFlags);
        void UpdateObject(IntPtr pidlOld, IntPtr pidlNew, out int puItem);
        void RefreshObject(IntPtr pidl, out int puItem);
        [PreserveSig]
        int SetRedraw(bool bRedraw);
        void GetSelectedCount(out int puSelected);
        void GetSelectedObjects(out IntPtr pppidl, out int puItems);
        void IsDropOnSource(IntPtr pDropTarget);
        void GetDragPoint(ref Point ppt);
        void GetDropPoint(ref Point ppt);
        void MoveIcons(IntPtr pDataObject);
        void SetItemPos(IntPtr pidl, ref Point ppt);
        void IsBkDropTarget(IntPtr pDropTarget);
        void SetClipboard(bool bMove);
        void SetPoints(ref IntPtr pDataObject);
        void GetItemSpacing(IntPtr pSpacing);
        void SetCallback(IntPtr pNewCB, out IntPtr ppOldCB);
        void Select(int dwFlags);
        void QuerySupport(IntPtr pdwSupport);
        void SetAutomationObject(IntPtr pdisp);
    }


    [ComImport, Guid("43826d1e-e718-42ee-bc55-a1e261c37bfe"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), SuppressUnmanagedCodeSecurity]
    public interface IShellItem
    {
        [PreserveSig]
        int BindToHandler(IntPtr pbc, [In] ref Guid bhid, [In] ref Guid riid, out IntPtr ppvOut);
        [PreserveSig]
        int GetParent(out IShellItem ppsi);
        [PreserveSig]
        int GetDisplayName(uint sigdnName, [MarshalAs(UnmanagedType.LPWStr)] out string ppszName);
        [PreserveSig]
        int GetAttributes(int sfgaoMask, out int psfgaoAttribs);
        [PreserveSig]
        int Compare(IShellItem psi, int hint, out int piOrder);
    }
    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), SuppressUnmanagedCodeSecurity, Guid("000214F9-0000-0000-C000-000000000046")]
    public interface IShellLinkW
    {
        [PreserveSig]
        int GetPath([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszFile, int cch, IntPtr pfd, uint fFlags);
        [PreserveSig]
        int GetIDList(out IntPtr ppidl);
        void SetIDList(IntPtr pidl);
        void GetDescription([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszName, int cch);
        void SetDescription([MarshalAs(UnmanagedType.LPWStr)] string pszName);
        void GetWorkingDirectory([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszDir, int cch);
        [PreserveSig]
        int SetWorkingDirectory([MarshalAs(UnmanagedType.LPWStr)] string pszDir);
        void GetArguments([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszArgs, int cch);
        void SetArguments([MarshalAs(UnmanagedType.LPWStr)] string pszArgs);
        void GetHotkey(out ushort pwHotkey);
        void SetHotkey(ushort wHotkey);
        void GetShowCmd(out int piShowCmd);
        void SetShowCmd(int iShowCmd);
        void GetIconLocation([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszIconPath, int cch, out int piIcon);
        void SetIconLocation([MarshalAs(UnmanagedType.LPWStr)] string pszIconPath, int iIcon);
        void SetRelativePath([MarshalAs(UnmanagedType.LPWStr)] string pszPathRel, uint dwReserved);
        void Resolve(IntPtr hwnd, uint fFlags);
        [PreserveSig]
        int SetPath([MarshalAs(UnmanagedType.LPWStr)] string pszFile);
    }




    namespace QTTabBar
    {
        //https://docs.microsoft.com/en-us/previous-versions/aa930086(v=msdn.10)
        [ComImport, Guid("000214E3-0000-0000-C000-000000000046"), SuppressUnmanagedCodeSecurity, InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IShellView
        {
            void GetWindow(out IntPtr phwnd);
            void ContextSensitiveHelp(bool fEnterMode);
            void TranslateAccelerator(ref MSG pmsg);
            void EnableModeless(bool fEnable);
            void UIActivate(uint uState);
            void Refresh();
            [PreserveSig]
            int CreateViewWindow(IShellView psvPrevious, ref FOLDERSETTINGS lpfs, IShellBrowser psb, ref RECT prcView, out IntPtr phWnd);
            void DestroyViewWindow();
            [PreserveSig]
            int GetCurrentInfo(ref FOLDERSETTINGS lpfs);
            void AddPropertySheetPages(int dwReserved, IntPtr pfn, IntPtr lparam);
            [PreserveSig]
            int SaveViewState();
            [PreserveSig]
            int SelectItem(IntPtr pidlItem, SVSIF uFlags);
            void GetItemObject(uint uItem, ref Guid riid, [MarshalAs(UnmanagedType.IUnknown)] out object ppv);
        }
    }


    namespace QTPluginLib
    {
        //https://docs.microsoft.com/en-us/previous-versions/aa930086(v=msdn.10)
        [ComImport, Guid("000214E3-0000-0000-C000-000000000046"), SuppressUnmanagedCodeSecurity, InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IShellView
        {
            [PreserveSig]
            int GetWindow(out IntPtr phwnd);
            [PreserveSig]
            int ContextSensitiveHelp(bool fEnterMode);
            [PreserveSig]
            int TranslateAccelerator(ref MSG pmsg);
            [PreserveSig]
            int EnableModeless(bool fEnable);
            [PreserveSig]
            int UIActivate(uint uState);
            [PreserveSig]
            int Refresh();
            [PreserveSig]
            int CreateViewWindow(IShellView psvPrevious, ref FOLDERSETTINGS pfs, ref IShellBrowser psb, ref RECT prcView, out IntPtr phWnd);
            [PreserveSig]
            int DestroyViewWindow();
            [PreserveSig]
            int GetCurrentInfo(ref FOLDERSETTINGS lpfs);
            [PreserveSig]
            int AddPropertySheetPages(int dwReserved, IntPtr pfn, IntPtr lparam);
            [PreserveSig]
            int SaveViewState();
            [PreserveSig]
            int SelectItem(IntPtr pidlItem, uint uFlags);
            [PreserveSig]
            int GetItemObject(uint uItem, ref Guid riid, [MarshalAs(UnmanagedType.IUnknown)] out object ppv);
        }
    }

    [ComImport, Guid("56FDF342-FD6D-11d0-958A-006097C9A090"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), SuppressUnmanagedCodeSecurity]
    public interface ITaskbarList
    {
        [PreserveSig]
        int HrInit();
        [PreserveSig]
        int AddTab(IntPtr hwnd);
        [PreserveSig]
        int DeleteTab(IntPtr hwnd);
        [PreserveSig]
        int ActivateTab(IntPtr hwnd);
        [PreserveSig]
        int SetActiveAlt(IntPtr hwnd);
    }
    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), SuppressUnmanagedCodeSecurity, Guid("F676C15D-596A-4ce2-8234-33996F445DB1")]
    public interface IThumbnailCache
    {
        [PreserveSig]
        int GetThumbnail(IShellItem pShellItem, uint cxyRequestedThumbSize, uint flags, out ISharedBitmap ppvThumb, ref uint pOutFlags, [In, Out] ref WTS_THUMBNAILID pThumbnailID);
        [PreserveSig]
        int GetThumbnailByID(ref WTS_THUMBNAILID thumbnailID, uint cxyRequestedThumbSize, out ISharedBitmap ppvThumb, ref uint pOutFlags);
    }
    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("7EBFDD87-AD18-11d3-A4C5-00C04F72D6B8"), SuppressUnmanagedCodeSecurity]
    public interface ITravelLogEntry
    {
        [PreserveSig]
        int GetTitle(out IntPtr ppszTitle);
        [PreserveSig]
        int GetURL(out IntPtr ppszURL);
    }
    [ComImport, Guid("7EBFDD80-AD18-11d3-A4C5-00C04F72D6B8"), SuppressUnmanagedCodeSecurity, InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface ITravelLogStg
    {
        [PreserveSig]
        int CreateEntry([In, MarshalAs(UnmanagedType.BStr)] string pszUrl, [In, MarshalAs(UnmanagedType.BStr)] string pszTitle, [In, MarshalAs(UnmanagedType.Interface)] ITravelLogEntry ptleRelativeTo, [In] bool fPrepend, [MarshalAs(UnmanagedType.Interface)] out ITravelLogEntry pptle);
        [PreserveSig]
        int TravelTo([In, MarshalAs(UnmanagedType.Interface)] ITravelLogEntry ptle);
        [PreserveSig]
        int EnumEntries([In] int flags, [MarshalAs(UnmanagedType.Interface)] out IEnumTravelLogEntry ppenum);
        [PreserveSig]
        int FindEntries([In] int flags, [In, MarshalAs(UnmanagedType.BStr)] string pszUrl, [MarshalAs(UnmanagedType.Interface)] out IEnumTravelLogEntry ppenum);
        [PreserveSig]
        int GetCount([In] int flags, out int pcEntries);
        [PreserveSig]
        int RemoveEntry([In, MarshalAs(UnmanagedType.Interface)] ITravelLogEntry ptle);
        [PreserveSig]
        int GetRelativeEntry([In] int iOffset, [MarshalAs(UnmanagedType.Interface)] out ITravelLogEntry ptle);
    }
    [ComImport, Guid("30cbe57d-d9d0-452a-ab13-7ac5ac4825ee"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IUIAutomation
    {
        /* NOT DECLARED */
        void CompareElements();
        /* NOT DECLARED */
        void CompareRuntimeIds();
        /* NOT DECLARED */
        void GetRootElement();
        void ElementFromHandle(IntPtr hwnd, out IUIAutomationElement element);
        void ElementFromPoint(Point pt, out IUIAutomationElement element);
        void GetFocusedElement(out IUIAutomationElement element);
        /* NOT DECLARED */
        void GetRootElementBuildCache();
        /* NOT DECLARED */
        void ElementFromHandleBuildCache();
        /* NOT DECLARED */
        void ElementFromPointBuildCache();
        /* NOT DECLARED */
        void GetFocusedElementBuildCache();
        /* NOT DECLARED */
        void CreateTreeWalker();
        void get_ControlViewWalker(out IUIAutomationTreeWalker walker);
        /* NOT DECLARED */
        void get_ContentViewWalker();
        /* NOT DECLARED */
        void get_RawViewWalker();
        /* NOT DECLARED */
        void get_RawViewCondition();
        /* NOT DECLARED */
        void get_ControlViewCondition();
        /* NOT DECLARED */
        void get_ContentViewCondition();
        /* NOT DECLARED */
        void CreateCacheRequest();
        /* NOT DECLARED */
        void CreateTrueCondition();
        /* NOT DECLARED */
        void CreateFalseCondition();
        /* NOT DECLARED */
        void CreatePropertyCondition();
        /* NOT DECLARED */
        void CreatePropertyConditionEx();
        /* NOT DECLARED */
        void CreateAndCondition();
        /* NOT DECLARED */
        void CreateAndConditionFromArray();
        /* NOT DECLARED */
        void CreateAndConditionFromNativeArray();
        /* NOT DECLARED */
        void CreateOrCondition();
        /* NOT DECLARED */
        void CreateOrConditionFromArray();
        /* NOT DECLARED */
        void CreateOrConditionFromNativeArray();
        /* NOT DECLARED */
        void CreateNotCondition();
        /* NOT DECLARED */
        void AddAutomationEventHandler();
        /* NOT DECLARED */
        void RemoveAutomationEventHandler();
        /* NOT DECLARED */
        void AddPropertyChangedEventHandlerNativeArray();
        /* NOT DECLARED */
        void AddPropertyChangedEventHandler();
        /* NOT DECLARED */
        void RemovePropertyChangedEventHandler();
        /* NOT DECLARED */
        void AddStructureChangedEventHandler();
        /* NOT DECLARED */
        void RemoveStructureChangedEventHandler();
        /* NOT DECLARED */
        void AddFocusChangedEventHandler();
        /* NOT DECLARED */
        void RemoveFocusChangedEventHandler();
        /* NOT DECLARED */
        void RemoveAllEventHandlers();
        /* NOT DECLARED */
        void IntNativeArrayToSafeArray();
        /* NOT DECLARED */
        void IntSafeArrayToNativeArray();
        /* NOT DECLARED */
        void RectToVariant();
        /* NOT DECLARED */
        void VariantToRect();
        /* NOT DECLARED */
        void SafeArrayToRectNativeArray();
        /* NOT DECLARED */
        void CreateProxyFactoryEntry();
        /* NOT DECLARED */
        void get_ProxyFactoryMapping();
        /* NOT DECLARED */
        void GetPropertyProgrammaticName();
        /* NOT DECLARED */
        void GetPatternProgrammaticName();
        /* NOT DECLARED */
        void PollForPotentialSupportedPatterns();
        /* NOT DECLARED */
        void PollForPotentialSupportedProperties();
        /* NOT DECLARED */
        void CheckNotSupported();
        /* NOT DECLARED */
        void get_ReservedNotSupportedValue();
        /* NOT DECLARED */
        void get_ReservedMixedAttributeValue();
        /* NOT DECLARED */
        void ElementFromIAccessible();
        /* NOT DECLARED */
        void ElementFromIAccessibleBuildCache();
    };
    [ComImport, Guid("d22108aa-8ac5-49a5-837b-37bbb3d7591e"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IUIAutomationElement
    {
        /* NOT DECLARED */
        void SetFocus();
        /* NOT DECLARED */
        void GetRuntimeId();
        /* NOT DECLARED */
        void FindFirst();
        /* NOT DECLARED */
        void FindAll();
        /* NOT DECLARED */
        void FindFirstBuildCache();
        /* NOT DECLARED */
        void FindAllBuildCache();
        /* NOT DECLARED */
        void BuildUpdatedCache();
        void GetCurrentPropertyValue(int propertyId, out object retVal);
        /* NOT DECLARED */
        void GetCurrentPropertyValueEx();
        /* NOT DECLARED */
        void GetCachedPropertyValue();
        /* NOT DECLARED */
        void GetCachedPropertyValueEx();
        /* NOT DECLARED */
        void GetCurrentPatternAs();
        /* NOT DECLARED */
        void GetCachedPatternAs();
        void GetCurrentPattern(int patternId, [MarshalAs(UnmanagedType.IUnknown)] out object patternObject);
        /* NOT DECLARED */
        void GetCachedPattern();
        /* NOT DECLARED */
        void GetCachedParent();
        /* NOT DECLARED */
        void GetCachedChildren();
        /* NOT DECLARED */
        void get_CurrentProcessId();
        /* NOT DECLARED */
        void get_CurrentControlType();
        /* NOT DECLARED */
        void get_CurrentLocalizedControlType();
        /* NOT DECLARED */
        void get_CurrentName();
        /* NOT DECLARED */
        void get_CurrentAcceleratorKey();
        /* NOT DECLARED */
        void get_CurrentAccessKey();
        /* NOT DECLARED */
        void get_CurrentHasKeyboardFocus();
        /* NOT DECLARED */
        void get_CurrentIsKeyboardFocusable();
        /* NOT DECLARED */
        void get_CurrentIsEnabled();
        /* NOT DECLARED */
        void get_CurrentAutomationId();
        /* NOT DECLARED */
        void get_CurrentClassName();
        /* NOT DECLARED */
        void get_CurrentHelpText();
        /* NOT DECLARED */
        void get_CurrentCulture();
        /* NOT DECLARED */
        void get_CurrentIsControlElement();
        /* NOT DECLARED */
        void get_CurrentIsContentElement();
        /* NOT DECLARED */
        void get_CurrentIsPassword();
        /* NOT DECLARED */
        void get_CurrentNativeWindowHandle();
        /* NOT DECLARED */
        void get_CurrentItemType();
        /* NOT DECLARED */
        void get_CurrentIsOffscreen();
        /* NOT DECLARED */
        void get_CurrentOrientation();
        /* NOT DECLARED */
        void get_CurrentFrameworkId();
        /* NOT DECLARED */
        void get_CurrentIsRequiredForForm();
        /* NOT DECLARED */
        void get_CurrentItemStatus();
        /* NOT DECLARED */
        void get_CurrentBoundingRectangle();
        /* NOT DECLARED */
        void get_CurrentLabeledBy();
        /* NOT DECLARED */
        void get_CurrentAriaRole();
        /* NOT DECLARED */
        void get_CurrentAriaProperties();
        /* NOT DECLARED */
        void get_CurrentIsDataValidForForm();
        /* NOT DECLARED */
        void get_CurrentControllerFor();
        /* NOT DECLARED */
        void get_CurrentDescribedBy();
        /* NOT DECLARED */
        void get_CurrentFlowsTo();
        /* NOT DECLARED */
        void get_CurrentProviderDescription();
        /* NOT DECLARED */
        void get_CachedProcessId();
        /* NOT DECLARED */
        void get_CachedControlType();
        /* NOT DECLARED */
        void get_CachedLocalizedControlType();
        /* NOT DECLARED */
        void get_CachedName();
        /* NOT DECLARED */
        void get_CachedAcceleratorKey();
        /* NOT DECLARED */
        void get_CachedAccessKey();
        /* NOT DECLARED */
        void get_CachedHasKeyboardFocus();
        /* NOT DECLARED */
        void get_CachedIsKeyboardFocusable();
        /* NOT DECLARED */
        void get_CachedIsEnabled();
        /* NOT DECLARED */
        void get_CachedAutomationId();
        /* NOT DECLARED */
        void get_CachedClassName();
        /* NOT DECLARED */
        void get_CachedHelpText();
        /* NOT DECLARED */
        void get_CachedCulture();
        /* NOT DECLARED */
        void get_CachedIsControlElement();
        /* NOT DECLARED */
        void get_CachedIsContentElement();
        /* NOT DECLARED */
        void get_CachedIsPassword();
        /* NOT DECLARED */
        void get_CachedNativeWindowHandle();
        /* NOT DECLARED */
        void get_CachedItemType();
        /* NOT DECLARED */
        void get_CachedIsOffscreen();
        /* NOT DECLARED */
        void get_CachedOrientation();
        /* NOT DECLARED */
        void get_CachedFrameworkId();
        /* NOT DECLARED */
        void get_CachedIsRequiredForForm();
        /* NOT DECLARED */
        void get_CachedItemStatus();
        /* NOT DECLARED */
        void get_CachedBoundingRectangle();
        /* NOT DECLARED */
        void get_CachedLabeledBy();
        /* NOT DECLARED */
        void get_CachedAriaRole();
        /* NOT DECLARED */
        void get_CachedAriaProperties();
        /* NOT DECLARED */
        void get_CachedIsDataValidForForm();
        /* NOT DECLARED */
        void get_CachedControllerFor();
        /* NOT DECLARED */
        void get_CachedDescribedBy();
        /* NOT DECLARED */
        void get_CachedFlowsTo();
        /* NOT DECLARED */
        void get_CachedProviderDescription();
        /* NOT DECLARED */
        void GetClickablePoint();
    };
    [ComImport, Guid("414c3cdc-856b-4f5b-8538-3131c6302550"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IUIAutomationGridPattern
    {
        void GetItem(int row, int column, out IUIAutomationElement element);
        void get_CurrentRowCount(out int retVal);
        void get_CurrentColumnCount(out int retVal);
        void get_CachedRowCount(out int retVal);
        void get_CachedColumnCount(out int retVal);
    }
    [ComImport, Guid("8609c4ec-4a1a-4d88-a357-5a66e060e1cf"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IUIAutomationRegistrar
    {
        void RegisterProperty(ref UIAutomationPropertyInfo property, out int propertyId);
        /* NOT DECLARED */
        void RegisterEvent();
        /* NOT DECLARED */
        void RegisterPattern();
    }
    [ComImport, Guid("88f4d42a-e881-459d-a77c-73bbbb7e02dc"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IUIAutomationScrollPattern
    {
        void Scroll(ScrollAmount horizontalAmount, ScrollAmount verticalAmount);
        void SetScrollPercent(double horizontalPercent, double verticalPercent);
        void get_CurrentHorizontalScrollPercent(out double retVal);
        void get_CurrentVerticalScrollPercent(out double retVal);
        void get_CurrentHorizontalViewSize(out double retVal);
        void get_CurrentVerticalViewSize(out double retVal);
        void get_CurrentHorizontallyScrollable(out bool retVal);
        void get_CurrentVerticallyScrollable(out bool retVal);
        void get_CachedHorizontalScrollPercent(out double retVal);
        void get_CachedVerticalScrollPercent(out double retVal);
        void get_CachedHorizontalViewSize(out double retVal);
        void get_CachedVerticalViewSize(out double retVal);
        void get_CachedHorizontallyScrollable(out bool retVal);
        void get_CachedVerticallyScrollable(out bool retVal);
    }
    [ComImport, Guid("a8efa66a-0fda-421a-9194-38021f3578ea"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IUIAutomationSelectionItemPattern
    {
        void Select();
        void AddToSelection();
        void RemoveFromSelection();
        void get_CurrentIsSelected(out bool pRetVal);
        /* NOT DECLARED */
        void get_CurrentSelectionContainer();
        void get_CachedIsSelected(out bool pRetVal);
        /* NOT DECLARED */
        void get_CachedSelectionContainer();
    }
    [ComImport, Guid("4042c624-389c-4afc-a630-9df854a541fc"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IUIAutomationTreeWalker
    {
        void GetParentElement(IUIAutomationElement element, out IUIAutomationElement parent);
        void GetFirstChildElement(IUIAutomationElement element, out IUIAutomationElement first);
        void GetLastChildElement(IUIAutomationElement element, out IUIAutomationElement last);
        void GetNextSiblingElement(IUIAutomationElement element, out IUIAutomationElement next);
        void GetPreviousSiblingElement(IUIAutomationElement element, out IUIAutomationElement previous);
        void NormalizeElement(IUIAutomationElement element, out IUIAutomationElement normalized);
        /* NOT DECLARED */
        void GetParentElementBuildCache();
        /* NOT DECLARED */
        void GetFirstChildElementBuildCache();
        /* NOT DECLARED */
        void GetLastChildElementBuildCache();
        /* NOT DECLARED */
        void GetNextSiblingElementBuildCache();
        /* NOT DECLARED */
        void GetPreviousSiblingElementBuildCache();
        /* NOT DECLARED */
        void NormalizeElementBuildCache();
        /* NOT DECLARED */
        void get_Condition();
    }

}
