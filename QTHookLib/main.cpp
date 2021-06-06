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

#include <Windows.h>
#include <ShObjIdl.h>
#include <Shlobj.h>
#include <UIAutomationCore.h>
#include "CComPtr.h"



/*

//%(projectdir)MinHook_133_lib\include
//%(projectdir)MinHook_133_lib\include;%(AdditionalIncludeDirectories)
//%(projectdir)MinHook_133_lib\lib
//%(projectdir)MinHook_133_lib\lib;%(AdditionalLibraryDirectories)

//https://zhuanlan.zhihu.com/p/78693248
//MSC    1.0   _MSC_VER == 100
//MSC    2.0   _MSC_VER == 200
//MSC    3.0   _MSC_VER == 300
//MSC    4.0   _MSC_VER == 400
//MSC    5.0   _MSC_VER == 500
//MSC    6.0   _MSC_VER == 600
//MSC    7.0   _MSC_VER == 700
//MSVC++ 1.0   _MSC_VER == 800
//MSVC++ 2.0   _MSC_VER == 900
//MSVC++ 4.0   _MSC_VER == 1000 (Developer Studio 4.0)
//MSVC++ 4.2   _MSC_VER == 1020 (Developer Studio 4.2)
//MSVC++ 5.0   _MSC_VER == 1100 (Visual Studio 97 version 5.0)
//MSVC++ 6.0   _MSC_VER == 1200 (Visual Studio 6.0 version 6.0)
//MSVC++ 7.0   _MSC_VER == 1300 (Visual Studio.NET 2002 version 7.0)
//MSVC++ 7.1   _MSC_VER == 1310 (Visual Studio.NET 2003 version 7.1)
//MSVC++ 8.0   _MSC_VER == 1400 (Visual Studio 2005 version 8.0)
//MSVC++ 9.0   _MSC_VER == 1500 (Visual Studio 2008 version 9.0)
//MSVC++ 10.0  _MSC_VER == 1600 (Visual Studio 2010 version 10.0)
//MSVC++ 11.0  _MSC_VER == 1700 (Visual Studio 2012 version 11.0)
//MSVC++ 12.0  _MSC_VER == 1800 (Visual Studio 2013 version 12.0)
//MSVC++ 14.0  _MSC_VER == 1900 (Visual Studio 2015 version 14.0)
//MSVC++ 14.1  _MSC_VER == 1910 (Visual Studio 2017 version 15.0)
//MSVC++ 14.11 _MSC_VER == 1911 (Visual Studio 2017 version 15.3)
//MSVC++ 14.12 _MSC_VER == 1912 (Visual Studio 2017 version 15.5)
//MSVC++ 14.13 _MSC_VER == 1913 (Visual Studio 2017 version 15.6)
//MSVC++ 14.14 _MSC_VER == 1914 (Visual Studio 2017 version 15.7)
//MSVC++ 14.15 _MSC_VER == 1915 (Visual Studio 2017 version 15.8)
//MSVC++ 14.16 _MSC_VER == 1916 (Visual Studio 2017 version 15.9)
//MSVC++ 14.2  _MSC_VER == 1920 (Visual Studio 2019 Version 16.0)
//MSVC++ 14.21 _MSC_VER == 1921 (Visual Studio 2019 Version 16.1)
//MSVC++ 14.22 _MSC_VER == 1922 (Visual Studio 2019 Version 16.2)
*/

#include "MinHook.h"

#if _MSC_VER <1600 // MSC    1.0   _MSC_VER == 100  ---- MS VC++ 9.0 _MSC_VER = 1500(VisualStudio 2008)
    #define Minhook_MSVC_Ver 90
#elif _MSC_VER >= 1600 && _MSC_VER <1700 // MSVC++ 10.0  _MSC_VER == 1600 (Visual Studio 2010 version 10.0)
    #define Minhook_MSVC_Ver 100
#elif _MSC_VER >= 1700 && _MSC_VER <1800 // MSVC++ 11.0  _MSC_VER == 1700 (Visual Studio 2012 version 11.0)
    #define Minhook_MSVC_Ver 110
#elif _MSC_VER >= 1800 && _MSC_VER <1900 // MSVC++ 12.0  _MSC_VER == 1800 (Visual Studio 2013 version 12.0)
    #define Minhook_MSVC_Ver 120
#elif _MSC_VER >= 1900 && _MSC_VER <1910 // MSVC++ 14.0  _MSC_VER == 1900 (Visual Studio 2015 version 14.0)
    #define Minhook_MSVC_Ver 140
#elif _MSC_VER >= 1910  // MSVC++ 14.1  _MSC_VER == 1910 (Visual Studio 2017 version 15.0)   ----    MSVC++ 14.2  _MSC_VER == 1920 (Visual Studio 2019 Version 16.0)
    #define Minhook_MSVC_Ver 141
#endif

#define STRING2(x) #x 
#define STRING(x) STRING2(x)  
#define Minhook_ver "v"STRING(Minhook_MSVC_Ver)

#if _WIN64
#define x86x64 "x64"
#else
#define x86x64 "x86"
#endif


#if _DEBUG
#define Flag_DEBUG "d"
#else
#define Flag_DEBUG ""
#endif

#if _DLL
#define __ConfigurationType "md"
#elif _MT
#define __ConfigurationType "mt"
#pragma comment(linker, "/NODEFAULTLIB:LIBCMT" Flag_DEBUG ".lib")
#pragma comment(linker, "/NODEFAULTLIB:LIBC" Flag_DEBUG ".lib")
#endif
#define MinHook_ConfigurationType "md"
//��Ȼ����ȥӦ����mt������mdҪ�ĵ���

#define Minhook_lib_name  "libMinHook-" x86x64 "-" Minhook_ver "-" MinHook_ConfigurationType  Flag_DEBUG ".lib"
//#pragma message(   Minhook_lib_name  )
#pragma comment(lib, Minhook_lib_name)


template <typename T>
inline MH_STATUS MH_CreateHookEx(LPVOID pTarget, LPVOID pDetour, T** ppOriginal)
{
    return MH_CreateHook(pTarget, pDetour, reinterpret_cast<LPVOID*>(ppOriginal));
}

template <typename T>
inline MH_STATUS MH_CreateHookApiEx(
    LPCWSTR pszModule, LPCSTR pszProcName, LPVOID pDetour, T** ppOriginal)
{
    return MH_CreateHookApi(
        pszModule, pszProcName, pDetour, reinterpret_cast<LPVOID*>(ppOriginal));
}

//// Create a hook for MessageBoxW, in disabled state.
//if (MH_CreateHookApiEx(L"user32", "MessageBoxW", &DetourMessageBoxW, &fpMessageBoxW) != MH_OK)
//{
//    return 1;
//}






// Hook declaration macro
#define DECLARE_HOOK(id, ret, name, params)                                         \
    typedef ret (WINAPI *__TYPE__##name)params; /* Function pointer type        */  \
    ret WINAPI Detour##name params;             /* Detour function              */  \
    __TYPE__##name fp##name = NULL;             /* Pointer to original function */  \
    const int hook##name = id;                  /* Hook ID                      */ 

// Hook creation macros
#define CREATE_HOOK(address, name, callbacks) {                                                            \
    MH_STATUS ret = MH_CreateHook(address, &Detour##name, reinterpret_cast<void**>(&fp##name)); \
    if(ret == MH_OK) ret = MH_EnableHook(address);                                              \
    callbacks.fpHookResult(hook##name, ret);                                                    \
}
#define CREATE_COM_HOOK(punk, idx, name, callbacks) \
    CREATE_HOOK((*(void***)((IUnknown*)(punk)))[idx], name, callbacks)
//void** ppv

// A few undocumented interfaces and classes, of which we only really need the IIDs.
MIDL_INTERFACE("0B907F92-1B63-40C6-AA54-0D3117F03578") IListControlHost     : public IUnknown {};
MIDL_INTERFACE("66A9CB08-4802-11d2-A561-00A0C92DBFE8") ITravelLog           : public IUnknown {};
MIDL_INTERFACE("3050F679-98B5-11CF-BB82-00AA00BDCE0B") ITravelLogEx         : public IUnknown {};
MIDL_INTERFACE("7EBFDD87-AD18-11d3-A4C5-00C04F72D6B8") ITravelLogEntry      : public IUnknown {};
MIDL_INTERFACE("489E9453-869B-4BCC-A1C7-48B5285FD9D8") ICommonExplorerHost  : public IUnknown {};
MIDL_INTERFACE("A86304A7-17CA-4595-99AB-523043A9C4AC") IExplorerFactory 	: public IUnknown {};
MIDL_INTERFACE("E93D4057-B9A2-42A5-8AF8-E5BBF177D365") IShellNavigationBand : public IUnknown {};
MIDL_INTERFACE("596742A5-1393-4E13-8765-AE1DF71ACAFB") CBreadcrumbBar {};
MIDL_INTERFACE("93A56381-E0CD-485A-B60E-67819E12F81B") CExplorerFactoryServer {};

// Win7's IShellBrowserService interface, of which we only need one function.
MIDL_INTERFACE("DFBC7E30-F9E5-455F-88F8-FA98C1E494CA")
IShellBrowserService_7 : public IUnknown {
public:
    virtual HRESULT STDMETHODCALLTYPE Unused0() = 0;
    virtual HRESULT STDMETHODCALLTYPE GetTravelLog(ITravelLog** ppTravelLog) = 0;
};

// Vista's IShellBrowserService.
MIDL_INTERFACE("42DAD0E2-9B43-4E7A-B9D4-E6D1FF85D173")
IShellBrowserService_Vista : public IUnknown {
public:
    virtual HRESULT STDMETHODCALLTYPE Unused0() = 0;
    virtual HRESULT STDMETHODCALLTYPE Unused1() = 0;
    virtual HRESULT STDMETHODCALLTYPE Unused2() = 0;
    virtual HRESULT STDMETHODCALLTYPE Unused3() = 0;
    virtual HRESULT STDMETHODCALLTYPE GetTravelLog(ITravelLog** ppTravelLog) = 0;
};

// Hooks
DECLARE_HOOK( 0, HRESULT, CoCreateInstance, (REFCLSID rclsid, LPUNKNOWN pUnkOuter, DWORD dwClsContext, REFIID riid, LPVOID FAR* ppv))
DECLARE_HOOK( 1, HRESULT, RegisterDragDrop, (HWND hwnd, LPDROPTARGET pDropTarget))
DECLARE_HOOK( 2, HRESULT, SHCreateShellFolderView, (const SFV_CREATE* pcsfv, IShellView** ppsv))
DECLARE_HOOK( 3, HRESULT, BrowseObject, (IShellBrowser* _this, PCUIDLIST_RELATIVE pidl, UINT wFlags))
DECLARE_HOOK( 4, HRESULT, CreateViewWindow3, (IShellView3* _this, IShellBrowser* psbOwner, IShellView* psvPrev, SV3CVW3_FLAGS dwViewFlags, FOLDERFLAGS dwMask, FOLDERFLAGS dwFlags, FOLDERVIEWMODE fvMode, const SHELLVIEWID* pvid, const RECT* prcView, HWND* phwndView))
DECLARE_HOOK( 5, HRESULT, MessageSFVCB, (IShellFolderViewCB* _this, UINT uMsg, WPARAM wParam, LPARAM lParam))
DECLARE_HOOK( 6, LRESULT, UiaReturnRawElementProvider, (HWND hwnd, WPARAM wParam, LPARAM lParam, IRawElementProviderSimple* el))
DECLARE_HOOK( 7, HRESULT, QueryInterface, (IRawElementProviderSimple* _this, REFIID riid, void** ppvObject))
DECLARE_HOOK( 8, HRESULT, TravelToEntry, (ITravelLogEx* _this, IUnknown* punk, ITravelLogEntry* ptle))
DECLARE_HOOK( 9, HRESULT, OnActivateSelection, (IListControlHost* _this, DWORD dwModifierKeys))
DECLARE_HOOK(10, HRESULT, SetNavigationState, (IShellNavigationBand* _this, unsigned long state))
DECLARE_HOOK(11, HRESULT, ShowWindow_Vista, (IExplorerFactory* _this, PCIDLIST_ABSOLUTE pidl, DWORD flags, DWORD mystery1, DWORD mystery2, POINT pt))
DECLARE_HOOK(11, HRESULT, ShowWindow_7, (ICommonExplorerHost* _this, PCIDLIST_ABSOLUTE pidl, DWORD flags, POINT pt, DWORD mystery))
DECLARE_HOOK(12, HRESULT, UpdateWindowList, (/*IShellBrowserService*/ IUnknown* _this))

// Messages
unsigned int WM_REGISTERDRAGDROP;
unsigned int WM_NEWTREECONTROL;
unsigned int WM_BROWSEOBJECT;
unsigned int WM_HEADERINALLVIEWS;
unsigned int WM_LISTREFRESHED;
unsigned int WM_ISITEMSVIEW;
unsigned int WM_ACTIVATESEL;
unsigned int WM_BREADCRUMBDPA;
unsigned int WM_CHECKPULSE;

// Callback struct
struct CallbackStruct {
    void (*fpHookResult)(int hookId, int retcode);
    bool (*fpNewWindow)(LPCITEMIDLIST pIDL);
} callbacks;

// Other stuff
HMODULE hModAutomation = NULL;
FARPROC fpRealRREP = NULL;
FARPROC fpRealCI = NULL;
extern "C" __declspec(dllexport) int Initialize(CallbackStruct* cb);
extern "C" __declspec(dllexport) int Dispose();
extern "C" __declspec(dllexport) int InitShellBrowserHook(IShellBrowser* psb);

//QTUtility() ����Initialize
//QTTabBarLib.QTTabBarLibClass.OnExplorerAttached()  ����InitShellBrowserHook

//QTTabBarLib.HookLibManager.Initialize_old() ����
//QTTabBarLib.HookLibManager.callbackStruct{HookResult,NewWindow}

BOOL APIENTRY DllMain(HMODULE hModule, DWORD  ul_reason_for_call, LPVOID lpReserved) {
    switch (ul_reason_for_call) {
        case DLL_PROCESS_DETACH:
            return Dispose();

        case DLL_PROCESS_ATTACH:
        case DLL_THREAD_ATTACH:
        case DLL_THREAD_DETACH:
            break;
    }
    return true;
}

int Initialize(CallbackStruct* cb) {

    volatile static long initialized;
    if(InterlockedIncrement(&initialized) != 1) {
        // Return if another thread has beaten us here.
        //initialized = 1;
        return MH_OK;
    }

    // Initialize MinHook.
    MH_STATUS ret = MH_Initialize();
    if(ret != MH_OK && ret != MH_ERROR_ALREADY_INITIALIZED) return ret;

    // Store the callback struct
    callbacks = *cb;

    // Register the messages.
    WM_REGISTERDRAGDROP = RegisterWindowMessageA("QTTabBar_RegisterDragDrop");
    WM_NEWTREECONTROL   = RegisterWindowMessageA("QTTabBar_NewTreeControl");
    WM_BROWSEOBJECT     = RegisterWindowMessageA("QTTabBar_BrowseObject");
    WM_HEADERINALLVIEWS = RegisterWindowMessageA("QTTabBar_HeaderInAllViews");
    WM_LISTREFRESHED    = RegisterWindowMessageA("QTTabBar_ListRefreshed");
    WM_ISITEMSVIEW      = RegisterWindowMessageA("QTTabBar_IsItemsView");
    WM_ACTIVATESEL      = RegisterWindowMessageA("QTTabBar_ActivateSelection");
    WM_BREADCRUMBDPA    = RegisterWindowMessageA("QTTabBar_BreadcrumbDPA");
    WM_CHECKPULSE       = RegisterWindowMessageA("QTTabBar_CheckPulse");

    // Create and enable the CoCreateInstance, RegisterDragDrop, and SHCreateShellFolderView hooks.
    CREATE_HOOK(&CoCreateInstance, CoCreateInstance, callbacks);
    CREATE_HOOK(&RegisterDragDrop, RegisterDragDrop, callbacks);
    CREATE_HOOK(&SHCreateShellFolderView, SHCreateShellFolderView, callbacks);

    // Create and enable the UiaReturnRawElementProvider hook (maybe)
    hModAutomation = LoadLibraryA("UIAutomationCore.dll");
    if(hModAutomation != NULL) {
        fpRealRREP = GetProcAddress(hModAutomation, "UiaReturnRawElementProvider");
        if(fpRealRREP != NULL) {
            CREATE_HOOK(fpRealRREP, UiaReturnRawElementProvider, callbacks);
        }
    }
    
    // Create an instance of the breadcrumb bar so we can hook it.
    CComPtr<IShellNavigationBand> psnb;
    if(psnb.Create(__uuidof(CBreadcrumbBar), CLSCTX_INPROC_SERVER)) {
        CREATE_COM_HOOK(psnb, 4, SetNavigationState, callbacks)
    }


    // Create an instance of CExplorerFactoryServer so we can hook it.
    // The interface in question is different on Vista and 7.
    CComPtr<IUnknown> punk;
    if(punk.Create(__uuidof(CExplorerFactoryServer), CLSCTX_INPROC_SERVER)) {
        CComPtr<ICommonExplorerHost> pceh;
        CComPtr<IExplorerFactory> pef;
        if(pceh.QueryFrom(punk)) {
            CREATE_COM_HOOK(pceh, 3, ShowWindow_7, callbacks)
        }
        else if(pef.QueryFrom(punk)) {
            CREATE_COM_HOOK(pef, 3, ShowWindow_Vista, callbacks)
        }
    }
    return MH_OK;
}

int InitShellBrowserHook(IShellBrowser* psb) {
    volatile static long initialized;
    if(InterlockedIncrement(&initialized) != 1) {
        // Return if another thread has beaten us here.
        //initialized = 1;
        return MH_OK;
    }

    // Create the BrowseObject hook
    CREATE_COM_HOOK(psb, 11, BrowseObject, callbacks);

    // Vista and 7 have different IShellBrowserService interfaces.
    // Hook UpdateWindowList in whichever one we have, and get the TravelLog.
    CComPtr<IShellBrowserService_7> psbs7;
    CComPtr<IShellBrowserService_Vista> psbsv;
    CComPtr<ITravelLog> ptl;
    if(psbs7.QueryFrom(psb)) {
        CREATE_COM_HOOK(psbs7, 10, UpdateWindowList, callbacks);
        psbs7->GetTravelLog(&ptl);
    }
    else if(psbsv.QueryFrom(psb)) {
        CREATE_COM_HOOK(psbsv, 17, UpdateWindowList, callbacks);
        psbsv->GetTravelLog(&ptl);
    }

    // Hook ITravelLogEx::TravelToEntry to catch the Search band navigation
    if(ptl != NULL) {
        CComPtr<ITravelLogEx> ptlex;
        if(ptlex.QueryFrom(ptl)) {
            CREATE_COM_HOOK(ptlex, 11, TravelToEntry, callbacks);
        }
    }
    return MH_OK;
}

int Dispose() {
    // Uninitialize MinHook.
    MH_Uninitialize();

    // Free the Automation library
    if(hModAutomation != NULL) {
        FreeLibrary(hModAutomation);
    }

    return S_OK;
}







//////////////////////////////
// Detour Functions
//////////////////////////////

// The purpose of this hook is to intercept the creation of the NameSpaceTreeControl object, and 
// send a reference to the control to QTTabBar.  We can use this reference to hit test the
// control, which is how opening new tabs from middle-click works.
HRESULT WINAPI DetourCoCreateInstance(REFCLSID rclsid, LPUNKNOWN pUnkOuter, DWORD dwClsContext, REFIID riid, LPVOID FAR* ppv) {
    HRESULT ret = fpCoCreateInstance(rclsid, pUnkOuter, dwClsContext, riid, ppv);
    if(SUCCEEDED(ret) && IsEqualIID(rclsid, CLSID_NamespaceTreeControl)) {
        PostThreadMessage(GetCurrentThreadId(), WM_NEWTREECONTROL, (WPARAM)(*ppv), NULL);
    }  
    return ret;
}

// The purpose of this hook is to allow QTTabBar to insert its IDropTarget wrapper in place of
// the real IDropTarget.  This is much more reliable than using the GetProp method.
HRESULT WINAPI DetourRegisterDragDrop(IN HWND hwnd, IN LPDROPTARGET pDropTarget) {
    LPDROPTARGET* ppDropTarget = &pDropTarget;
    SendMessage(hwnd, WM_REGISTERDRAGDROP, (WPARAM)ppDropTarget, NULL);
    return fpRegisterDragDrop(hwnd, *ppDropTarget);
}

// The purpose of this hook is just to set other hooks.  It is disabled once the other hooks are set.
HRESULT WINAPI DetourSHCreateShellFolderView(const SFV_CREATE* pcsfv, IShellView** ppsv) {
    HRESULT ret = fpSHCreateShellFolderView(pcsfv, ppsv);
    CComPtr<IShellView> psv(*ppsv);
    if(SUCCEEDED(ret) && psv.Implements(IID_CDefView)) {

        CREATE_COM_HOOK(pcsfv->psfvcb, 3, MessageSFVCB, callbacks);

        CComPtr<IShellView3> psv3;
        if(psv3.QueryFrom(psv)) {
            CREATE_COM_HOOK(psv3, 20, CreateViewWindow3, callbacks);
        }

        CComPtr<IListControlHost> plch;
        if(plch.QueryFrom(psv)) {
            CREATE_COM_HOOK(plch, 3, OnActivateSelection, callbacks);
        }

        // Disable this hook, no need for it anymore.
        MH_DisableHook(&SHCreateShellFolderView);
    }
    return ret;
}

// The purpose of this hook is to work around Explorer's BeforeNavigate2 bug.  It allows QTTabBar
// to be notified of navigations before they occur and have the chance to veto them.
HRESULT WINAPI DetourBrowseObject(IShellBrowser* _this, PCUIDLIST_RELATIVE pidl, UINT wFlags) {
    HWND hwnd;
    LRESULT result = 0;
    if(SUCCEEDED(_this->GetWindow(&hwnd))) {
        HWND parent = GetParent(hwnd);
        if(parent != 0) hwnd = parent;
        result = SendMessage(hwnd, WM_BROWSEOBJECT, (WPARAM)(&wFlags), (LPARAM)pidl);
    } 
    return result == 0 ? fpBrowseObject(_this, pidl, wFlags) : S_FALSE;
}

// The purpose of this hook is to enable the Header In All Views functionality, if the user has 
// opted to use it.
HRESULT WINAPI DetourCreateViewWindow3(IShellView3* _this, IShellBrowser* psbOwner, IShellView* psvPrev, SV3CVW3_FLAGS dwViewFlags, FOLDERFLAGS dwMask, FOLDERFLAGS dwFlags, FOLDERVIEWMODE fvMode, const SHELLVIEWID* pvid, const RECT* prcView, HWND* phwndView) {
    HWND hwnd;
    LRESULT result = 0;
    if(psbOwner != NULL && SUCCEEDED(psbOwner->GetWindow(&hwnd))) {
        HWND parent = GetParent(hwnd);
        if(parent != 0) hwnd = parent;
        if(SendMessage(hwnd, WM_HEADERINALLVIEWS, 0, 0) != 0) {
            dwMask |= FWF_NOHEADERINALLVIEWS;
            dwFlags &= ~FWF_NOHEADERINALLVIEWS;
        }
    }
    return fpCreateViewWindow3(_this, psbOwner, psvPrev, dwViewFlags, dwMask, dwFlags, fvMode, pvid, prcView, phwndView);
}

// The purpose of this hook is to notify QTTabBar whenever an Explorer refresh occurs.  This allows
// the search box to be cleared.
HRESULT WINAPI DetourMessageSFVCB(IShellFolderViewCB* _this, UINT uMsg, WPARAM wParam, LPARAM lParam) {
    if(uMsg == 0x11 /* SFVM_LISTREFRESHED */ && wParam != 0) {
        PostThreadMessage(GetCurrentThreadId(), WM_LISTREFRESHED, NULL, NULL);
    }
    return fpMessageSFVCB(_this, uMsg, wParam, lParam);
}

// The purpose of this hook is just to set another hook.  It is disabled once the other hook is set.
LRESULT WINAPI DetourUiaReturnRawElementProvider(HWND hwnd, WPARAM wParam, LPARAM lParam, IRawElementProviderSimple* el) {
    if(fpQueryInterface == NULL && (LONG)lParam == OBJID_CLIENT && SendMessage(hwnd, WM_ISITEMSVIEW, 0, 0) == 1) {
        CREATE_COM_HOOK(el, 0, QueryInterface, callbacks);
        // Disable this hook, no need for it anymore.
        MH_DisableHook(fpRealRREP);
    }
    return fpUiaReturnRawElementProvider(hwnd, wParam, lParam, el);
}

// The purpose of this hook is to work around kb2462524, aka the scrolling lag bug.
HRESULT WINAPI DetourQueryInterface(IRawElementProviderSimple* _this, REFIID riid, void** ppvObject) {
    return IsEqualIID(riid, __uuidof(IRawElementProviderAdviseEvents))
            ? E_NOINTERFACE
            : fpQueryInterface(_this, riid, ppvObject);
}

// The purpose of this hook is to make clearing a search go back to the original directory.
HRESULT WINAPI DetourTravelToEntry(ITravelLogEx* _this, IUnknown* punk, ITravelLogEntry* ptle) {
    CComPtr<IShellBrowser> psb;
    LRESULT result = 0;
    if(punk != NULL && psb.QueryFrom(punk)) {
        HWND hwnd;
        if(SUCCEEDED(psb->GetWindow(&hwnd))) {
            HWND parent = GetParent(hwnd);
            if(parent != 0) hwnd = parent;
            UINT wFlags = SBSP_NAVIGATEBACK | SBSP_SAMEBROWSER;
            result = SendMessage(parent, WM_BROWSEOBJECT, (WPARAM)(&wFlags), NULL);
        }
    }
    return result == 0 ? fpTravelToEntry(_this, punk, ptle) : S_OK;
}

// The purpose of this hook is let QTTabBar handle activating the selection, so that recently
// opened files can be logged (among other features).
HRESULT WINAPI DetourOnActivateSelection(IListControlHost* _this, DWORD dwModifierKeys) {
    CComPtr<IShellView> psv;
    LRESULT result = 0;
    if(psv.QueryFrom(_this)) {
        HWND hwnd;
        if(SUCCEEDED(psv->GetWindow(&hwnd))) {
            result = SendMessage(hwnd, WM_ACTIVATESEL, (WPARAM)(&dwModifierKeys), 0);
        }
    }
    return result == 0 ? fpOnActivateSelection(_this, dwModifierKeys) : S_OK;
}

// The purpose of this hook is to send the Breadcrumb Bar's internal DPA handle to QTTabBar,
// so that we can use it map the buttons to their corresponding IDLs.  This allows middle-click
// on the breadcrumb bar to work.  The DPA handle changes whenever this function is called.
HRESULT WINAPI DetourSetNavigationState(IShellNavigationBand* _this, unsigned long state) {
	HRESULT ret = fpSetNavigationState(_this, state);
    // I find the idea of reading an internal private variable of an undocumented class to
    // be quite unsettling.  Unfortunately, I see no way around it.  It's been in the same
    // location since the first Vista release, so I guess it should be safe...
    HDPA hdpa = (HDPA)(((void**)_this)[6]);
    CComPtr<IOleWindow> pow;
    if(pow.QueryFrom(_this)) {
        HWND hwnd;
        if(SUCCEEDED(pow->GetWindow(&hwnd))) {
            SendMessage(hwnd, WM_BREADCRUMBDPA, NULL, (LPARAM)hdpa);
        }
    }
    return ret;
}

// The purpose of this hook is to alert QTTabBar that a new window is opening, so that we can 
// intercept it if the user has enabled the appropriate option.  The hooked function is different
// on Vista and 7.
HRESULT WINAPI DetourShowWindow_7(ICommonExplorerHost* _this, PCIDLIST_ABSOLUTE pidl, DWORD flags, POINT pt, DWORD mystery) {
    return callbacks.fpNewWindow(pidl) ? S_OK : fpShowWindow_7(_this, pidl, flags, pt, mystery);
}
HRESULT WINAPI DetourShowWindow_Vista(IExplorerFactory* _this, PCIDLIST_ABSOLUTE pidl, DWORD flags, DWORD mystery1, DWORD mystery2, POINT pt) {
    return callbacks.fpNewWindow(pidl) ? S_OK : fpShowWindow_Vista(_this, pidl, flags, mystery1, mystery2, pt);
}

// The SHOpenFolderAndSelectItems function opens an Explorer window and waits for a New Window
// notification from IShellWindows.  The purpose of this hook is to wake up those threads by
// faking such a notification.  It's important that it happens after IShellBrowser::OnNavigate is
// called by the real Explorer window, which happens in IShellBrowserService::UpdateWindowList.
HRESULT WINAPI DetourUpdateWindowList(/* IShellBrowserService */ IUnknown* _this) {
    HRESULT hr = fpUpdateWindowList(_this);
    CComPtr<IShellBrowser> psb;
    LRESULT result = 0;
    if(psb.QueryFrom(_this)) {
        HWND hwnd;
        if(SUCCEEDED(psb->GetWindow(&hwnd))) {
            HWND parent = GetParent(hwnd);
            if(parent != 0) hwnd = parent;
            CComPtr<IDispatch> pdisp;
            if(SendMessage(parent, WM_CHECKPULSE, NULL, (WPARAM)(&pdisp)) != 0 && pdisp != NULL) {
                CComPtr<IShellWindows> psw;
                if(psw.Create(CLSID_ShellWindows, CLSCTX_ALL)) {
                    long cookie;
                    if(SUCCEEDED(psw->Register(pdisp, (long)hwnd, SWC_EXPLORER, &cookie))) {
                        psw->Revoke(cookie);
                    }
                }
            }
        }
    }
    return hr;
}
