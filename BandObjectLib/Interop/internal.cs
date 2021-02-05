
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Text;
using System.Runtime.InteropServices;
using System.Security;

namespace BandObjectLib.Interop
{
    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("68284faa-6a48-11d0-8c78-00c04fd918b4"), SuppressUnmanagedCodeSecurity]
    internal interface IInputObject
    {
        void UIActivateIO(int fActivate, ref MSG msg);
        [PreserveSig]
        int HasFocusIO();
        [PreserveSig]
        int TranslateAcceleratorIO(ref MSG msg);
    }
    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("EB0FE172-1A3A-11D0-89B3-00A0C90A90AC"), SuppressUnmanagedCodeSecurity]
    internal interface IDeskBand
    {
        void GetWindow(out IntPtr phwnd);
        void ContextSensitiveHelp([In] bool fEnterMode);
        void ShowDW([In] bool fShow);
        void CloseDW([In] uint dwReserved);
        void ResizeBorderDW(IntPtr prcBorder, [In, MarshalAs(UnmanagedType.IUnknown)] object punkToolbarSite, bool fReserved);
        void GetBandInfo(uint dwBandID, uint dwViewMode, ref DESKBANDINFO pdbi);
    }
    [ComImport, Guid("012dd920-7b26-11d0-8ca9-00a0c92dbfe8"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), SuppressUnmanagedCodeSecurity]
    internal interface IDockingWindow
    {
        void GetWindow(out IntPtr phwnd);
        void ContextSensitiveHelp([In] bool fEnterMode);
        void ShowDW([In] bool fShow);
        void CloseDW([In] uint dwReserved);
        void ResizeBorderDW(IntPtr prcBorder, [In, MarshalAs(UnmanagedType.IUnknown)] object punkToolbarSite, bool fReserved);
    }
    namespace BandObjectLib
    {
        [ComImport, SuppressUnmanagedCodeSecurity, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("00000114-0000-0000-C000-000000000046")]
        internal interface IOleWindow
        {
            void GetWindow(out IntPtr phwnd);
            void ContextSensitiveHelp([In] bool fEnterMode);
        }
    }
    namespace QTTabBar
    {
        [ComImport, SuppressUnmanagedCodeSecurity, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("00000114-0000-0000-C000-000000000046")]
        public interface IOleWindow
        {
            [PreserveSig]
            int GetWindow(out IntPtr phwnd);
            [PreserveSig]
            int ContextSensitiveHelp(bool fEnterMode);
        }
    }
}
