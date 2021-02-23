


using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security;

namespace BandObjectLib.Interop
{

    namespace MSBandObject
    {
        //https://docs.microsoft.com/en-us/windows/win32/shell/extending-explorer-bumper
        //https://docs.microsoft.com/en-us/windows/win32/shell/band-objects
        //(IUnknown+IClassFactory)(COM_DLL)+(IDeskBand+IObjectWithSite+IPersistStream)+(注册组件 类型（CATID）+其容器)
        //（CATID）
        //垂直浏览器栏	CATID_InfoBand
        //水平浏览器栏 CATID_CommBand
        //公桌带 CATID_DeskBand
        //+(IInputObject(接受用户输入)+IContextMenu(添加到快捷菜单))
        //
        //
        //插件控件+子窗口处理消息。
        //调用band对象的IDeskBand :: GetBandInfo方法时，容器使用dwBandID参数为band对象分配一个【标识符】，
        //该标识符用于三个命令。
        //支持四个IOleCommandTarget :: Exec命令ID。
        //DBID_BANDINFOCHANGED//信息已更改。将pvaIn参数设置为最近一次IDeskBand :: GetBandInfo调用中接收到的波段标识符。
        //DBID_MAXIMIZEBAND//最大化乐队。将pvaIn参数设置为最近一次IDeskBand :: GetBandInfo调用中接收到的波段标识符。
        //DBID_SHOWONLY//打开或关闭容器中的其他带。
        //    使用以下值之一将pvaIn参数设置为VT_UNKNOWN类型
        //    pUnk    指向band对象的IUnknown接口的指针。 所有其他桌面乐队将被隐藏。
        //    0	Hide all desk bands.
        //    1	Show all desk bands.
        //DBID_PUSHCHEVRON//显示人字形菜单。容器发送RB_PUSHCHEVRON消息，并且band对象收到RBN_CHEVRONPUSHED通知，提示其显示人字形菜单。
        //    最近一次IDeskBand :: GetBandInfo调用中接收到的波段标识符。pvaIn参数设置为VT_I4类型。
        //    //{B722BCCB-4E68-101B-A2BC-00AA00404770}
        //    //C:\Windows\System32\ieproxy.dll\IOleCommandTarget.PSFactoryBuffer
        //    //https://docs.microsoft.com/en-us/windows/win32/api/docobj/nn-docobj-iolecommandtarget
        //
        //
        //to (IDeskBand+IObjectWithSite+IPersistStream)+(IInputObject(接受用户输入)+IContextMenu(添加到快捷菜单))

        //因为band对象使用子窗口进行显示，所以它必须实现一个窗口过程来处理Windows消息传递。该波段示例具有最少的功能，因此其窗口过程仅处理五个消息：
        //WM_NCCREATE
        //WM_PAINT
        //WM_COMMAND
        //WM_SETFOCUS
        //WM_KILLFOCUS


        //OneCoreUAPCommonProxyStub.dll\IDeskBand.PSFactoryBuffer
        //Explorer Bar示例的GetWindow实现

        [ComImport, Guid("EB0FE172-1A3A-11D0-89B3-00A0C90A90AC"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), SuppressUnmanagedCodeSecurity]
        internal interface IDeskBand
        {
            //IDockingWindow.IOleWindow
            /// <remarks> 返回了Explorer Bar的子窗口句柄m_hwnd。 </remarks>
            void GetWindow(out IntPtr phwnd);
            /// <remarks> 如国未实现上下文相关帮助，返回E_NOTIMPL。 </remarks>
            void ContextSensitiveHelp([In] bool fEnterMode);
            //IDockingWindow
            /// <remarks> 显示或隐藏浏览器栏的窗口 </remarks>
            void ShowDW([In] bool fShow);
            /// <remarks> 销毁浏览器栏的窗口 </remarks>
            void CloseDW([In] uint dwReserved);
            /// <remarks> 不使用任何类型的带对象的使用，并且应该总是返回E_NOTIMPL。？ </remarks>
            void ResizeBorderDW(IntPtr prcBorder, [In, MarshalAs(UnmanagedType.IUnknown)] object punkToolbarSite, bool fReserved);

            /// <remarks> 指定资源管理器栏的标识符和查看模式。并用请求的数据填充DESKBANDINFO结构。? </remarks>
            void GetBandInfo(uint dwBandID, uint dwViewMode, ref DESKBANDINFO pdbi);
        }

        //oleaut32.dll\IObjectWithSite.PSFactoryBuffer
        [ComImport, Guid("FC4801A3-2BA9-11CF-A229-00AA003D7352"), SuppressUnmanagedCodeSecurity, InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IObjectWithSite
        {
            /// <summary>
            /// 刷新重置输入站点。创建子窗口，或删除带。当用户选择浏览器栏时，容器将调用相应的band对象的IObjectWithSite :: SetSite方法。
            /// 该punkSite参数将被设置为站点的IUnknown指针。
            /// </summary>
            /// <remarks>
            /// 通常，SetSite实现应执行以下步骤：
            /// 释放当前保留的所有站点(IInputObjectSite)指针。
            /// 如果传递给SetSite的指针设置为NULL，则将删除波段。SetSite可以返回S_OK。
            /// 如果传递给SetSite的指针为非NULL，则将设置一个新站点。SetSite应该执行以下操作：
            ///     在站点上为其IOleWindow接口调用QueryInterface。
            ///     调用IOleWindow::GetWindow以获取父窗口的句柄。保存手柄以备后用。如果不再需要，请释放IOleWindow。
            ///     将band对象的窗口创建为上一步中获得的窗口的子级。不要将其创建为可见窗口。
            ///     如果band对象实现IInputObject，则在站点上为其IInputObjectSite接口调用QueryInterface。存储指向该接口的指针，以备后用。
            ///     如果所有步骤都成功，则返回S_OK。如果不是，则返回OLE定义的错误代码，指示失败的原因。
            /// </remarks>
            /// <param name="pUnkSite">创建子窗口。刷新设置IInputObjectSite指针，与父窗体指针、实现IInputObject的band对象指针</param>
            void SetSite([In, MarshalAs(UnmanagedType.IUnknown)] object pUnkSite);
            /// <remarks> 无定义。示例是QueryInterface的包装 </remarks>
            void GetSite(ref Guid riid, [MarshalAs(UnmanagedType.IUnknown)] out object ppvSite);
        }


        //combase.dll\IPersistStream.PSFactoryBuffer
        /// <remarks> 允许资源管理器栏加载或保存持久数据。如果没有持久性数据，则这些方法仍必须返回成功代码。 </remarks>
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


        //OneCoreUAPCommonProxyStub.dll\IInputObject.PSFactoryBuffer
        [ComImport, Guid("68284faa-6a48-11d0-8c78-00c04fd918b4"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), SuppressUnmanagedCodeSecurity]
        internal interface IInputObject
        {
            void UIActivateIO(int fActivate, ref MSG msg);
            [PreserveSig]
            int HasFocusIO();
            [PreserveSig]
            int TranslateAcceleratorIO(ref MSG msg);
        }



        [Flags]
        public enum DBIM : uint
        {
            MINSIZE = 1,
            MAXSIZE = 2,
            INTEGRAL = 4,
            ACTUAL = 8,
            TITLE = 0x10,
            MODEFLAGS = 0x20,
            BKCOLOR = 0x40,
        }
        [Flags]
        public enum DBIMF : uint
        {
            NORMAL = 0,
            FIXED = 1,
            FIXEDBMP = 4,
            VARIABLEHEIGHT = 8,
            UNDELETEABLE = 0x10,
            DEBOSSED = 0x20,
            BKCOLOR = 0x40,
            USECHEVRON = 0x80,
            BREAK = 0x100,
            ADDTOFRONT = 0x200,
            TOPALIGN = 0x400,
        }
        //https://docs.microsoft.com/en-us/windows/win32/api/shobjidl_core/ns-shobjidl_core-deskbandinfo
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
    }


    namespace BandObjectLib
    {
        //combase.dll\IOleWindow.PSFactoryBuffer
        [ComImport, Guid("00000114-0000-0000-C000-000000000046"), SuppressUnmanagedCodeSecurity, InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        internal interface IOleWindow
        {
            void GetWindow(out IntPtr phwnd);
            void ContextSensitiveHelp([In] bool fEnterMode);
        }

        //OneCoreUAPCommonProxyStub.dll\IDockingWindow.PSFactoryBuffer
        [ComImport, Guid("012dd920-7b26-11d0-8ca9-00a0c92dbfe8"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), SuppressUnmanagedCodeSecurity]
        internal interface IDockingWindow
        {
            //IOleWindow
            void GetWindow(out IntPtr phwnd);
            void ContextSensitiveHelp([In] bool fEnterMode);

            void ShowDW([In] bool fShow);
            void CloseDW([In] uint dwReserved);
            void ResizeBorderDW(IntPtr prcBorder, [In, MarshalAs(UnmanagedType.IUnknown)] object punkToolbarSite, bool fReserved);
        }
    }
    namespace QTTabBar
    {
        //combase.dll\IOleWindow.PSFactoryBuffer
        [ComImport, Guid("00000114-0000-0000-C000-000000000046"), SuppressUnmanagedCodeSecurity, InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IOleWindow
        {
            [PreserveSig]
            int GetWindow(out IntPtr phwnd);
            [PreserveSig]
            int ContextSensitiveHelp(bool fEnterMode);
        }
    }
}