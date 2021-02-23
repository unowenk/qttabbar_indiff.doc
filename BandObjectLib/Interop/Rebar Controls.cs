


using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace BandObjectLib.Interop
{
    namespace RebarControls
    {

        /// <remarks> 控制消息 </remarks>
        /// <see cref="https://docs.microsoft.com/en-us/previous-versions/windows/embedded/ee499383(v=winembedded.80)"/>
        public static class RB
        {
            /// <remarks>  </remarks>
            public const int INSERTBANDA = (0x400 + 1);
            /// <remarks>  </remarks>
            public const int INSERTBANDW = (0x400 + 10);
            /// <remarks> 插入新的带 </remarks>
            public const int INSERTBAND = INSERTBANDW;
            /// <remarks> 删除带 </remarks>
            public const int DELETEBAND = (0x400 + 2);



            //retrieves 检索 or set 设置。属性
            /// <remarks> 检索有关钢筋控件及其使用的图像列表的信息 </remarks>
            public const int GETBARINFO = (0x400 + 3);
            /// <remarks> 设置钢筋控件的特征 </remarks>
            public const int SETBARINFO = (0x400 + 4);
            /// <remarks> 设置钢筋控件的父窗口 </remarks>
            public const int SETPARENT = (0x400 + 7);
            /// <remarks> 确定钢筋带的哪一部分在屏幕上的指定点处 </remarks>
            public const int HITTEST = (0x400 + 8);
            /// <remarks> 检索钢筋控件中指定波段的边界矩形 </remarks>
            public const int GETRECT = (0x400 + 9);
            /// <remarks> 检索当前在rebar controls 中的带的数量 </remarks>
            public const int GETBANDCOUNT = (0x400 + 12);
            /// <remarks> 检索钢筋控件中的带行数 </remarks>
            public const int GETROWCOUNT = (0x400 + 13);
            /// <remarks> 检索钢筋控件中指定行的高度 </remarks>
            public const int GETROWHEIGHT = (0x400 + 14);
            /// <remarks> 将带标识符转换为带索引 </remarks>
            public const int IDTOINDEX = (0x400 + 16); // wParam == id
            /// <remarks>  </remarks>
            public const int GETTOOLTIPS = (0x400 + 17);
            /// <remarks>  </remarks>
            public const int SETTOOLTIPS = (0x400 + 18);
            /// <remarks> 设置钢筋控件的默认背景颜色 </remarks>
            public const int SETBKCOLOR = (0x400 + 19); // sets the default BK color
            /// <remarks> 检索钢筋控件的默认背景颜色 </remarks>
            public const int GETBKCOLOR = (0x400 + 20); // defaults to CLR_NONE
            /// <remarks> 设置钢筋控件的默认文本颜色 </remarks>
            public const int SETTEXTCOLOR = (0x400 + 21);
            /// <remarks> 检索钢筋控件的默认文本颜色 </remarks>
            public const int GETTEXTCOLOR = (0x400 + 22); // defaults to 0x00000000
            /// <remarks> 试图使钢筋控件适合指定的矩形 </remarks>
            public const int SIZETORECT = (0x400 + 23); // resize the rebar/break bands and such to this rect (lparam)
            /// <remarks>  </remarks>
            public const int BEGINDRAG = (0x400 + 24);
            /// <remarks>  </remarks>
            public const int ENDDRAG = (0x400 + 25);
            /// <remarks>  </remarks>
            public const int DRAGMOVE = (0x400 + 26);
            /// <remarks> 检索钢筋控件的高度 </remarks>
            public const int GETBARHEIGHT = (0x400 + 27);
            /// <remarks>  </remarks>
            public const int GETBANDINFOW = (0x400 + 28);
            /// <remarks>  </remarks>
            public const int GETBANDINFOA = (0x400 + 29);
            /// <remarks> 检索有关钢筋控制中指定带的信息 </remarks>
            public const int GETBANDINFO = GETBANDINFOW;
            /// <remarks>  </remarks>
            public const int SETBANDINFOA = (0x400 + 6);
            /// <remarks>  </remarks>
            public const int SETBANDINFOW = (0x400 + 11);
            /// <remarks> 设置钢筋控件中现有带的特征 </remarks>
            public const int SETBANDINFO = SETBANDINFOW;
            /// <remarks> 钢筋控件中的条带调整为最小尺寸 </remarks>
            public const int MINIMIZEBAND = (0x400 + 30);
            /// <remarks> 将钢筋控件中的条带调整为理想或最大尺寸 </remarks>
            public const int MAXIMIZEBAND = (0x400 + 31);
            /// <remarks> 检索带的边界 </remarks>
            public const int GETBANDBORDERS = (0x400 + 34); // returns in lparam = lprc the amount of edges added to band wparam
            /// <remarks> 显示或隐藏钢筋控件中的指定带 </remarks>
            public const int SHOWBAND = (0x400 + 35); // show/hide band
            /// <remarks>  </remarks>
            public const int SETPALETTE = (0x400 + 37);
            /// <remarks>  </remarks>
            public const int GETPALETTE = (0x400 + 38);
            /// <remarks>  </remarks>
            public const int MOVEBAND = (0x400 + 39);
            /// <remarks>  </remarks>
            public const int GETBANDMARGINS = (0x400 + 40);
            /// <remarks>  </remarks>
            public const int SETEXTENDEDSTYLE = (0x400 + 41);
            /// <remarks>  </remarks>
            public const int GETEXTENDEDSTYLE = (0x400 + 42);
            /// <remarks>  </remarks>
            public const int PUSHCHEVRON = (0x400 + 43);
            /// <remarks>  </remarks>
            public const int SETBANDWIDTH = (0x400 + 44); // set width for docked band
        }
        /// <remarks> 标志，指示此结构的哪些成员有效或必须填写。RBBIM </remarks>
        /// <see cref="https://docs.microsoft.com/en-us/previous-versions/windows/embedded/ee503418(v=winembedded.80)"/>
        [Flags]
        public enum RBBIM : uint
        {
            STYLE = 0x0001,
            COLORS = 0x0002,
            TEXT = 0x0004,
            IMAGE = 0x0008,
            CHILD = 0x0010,
            CHILDSIZE = 0x0020,
            SIZE = 0x0040,
            BACKGROUND = 0x0080,
            ID = 0x0100,
            IDEALSIZE = 0x0200,
            LPARAM = 0x0400,

            HEADERSIZE = 0x0800, // control the size of the header
            CHEVRONLOCATION = 0x1000,
            CHEVRONSTATE = 0x2000,
        }

        /// <remarks> 指定风格。RBBS </remarks>
        /// <see cref="https://docs.microsoft.com/en-us/previous-versions/windows/embedded/ee503418(v=winembedded.80)"/>
        [Flags]
        public enum RBBS : uint
        {
            BREAK = 0x0001,  // break to new line
            FIXEDSIZE = 0x0002,  // band can't be sized
            CHILDEDGE = 0x0004,  // edge around top & bottom of child window
            HIDDEN = 0x0008,  // don't show
            NOVERT = 0x0010,  // don't show when vertical
            FIXEDBMP = 0x0020,  // bitmap doesn't move during band resize
            VARIABLEHEIGHT = 0x0040,  // allow autosizing of this child vertically

            GRIPPERALWAYS = 0x0080,  // always show the gripper
            NOGRIPPER = 0x0100,  // never show the gripper
            USECHEVRON = 0x0200,  // display drop-down button for this band if it's sized smaller than ideal width
            HIDETITLE = 0x0400,  // keep band title hidden
            TOPALIGN = 0x0800,  // keep band in top row
        }


        /// <remarks> 消息循环，信息携带结构。
        /// 在RebarController、BandObject产生，
        /// 在BandObject.WndProc、在RebarController.RebarMessageCaptured处理 </remarks>
        /// <see cref="https://docs.microsoft.com/en-us/previous-versions/windows/embedded/ee503418(v=winembedded.80)"/>
        [StructLayout(LayoutKind.Sequential)]
        public struct REBARBANDINFO
        {
            /// <remarks> 此结构的大小，以字节为单位。 </remarks>
            public int cbSize;
            /// <remarks> 标志，指示此结构的哪些成员有效或必须填写。RBBIM </remarks>
            public RBBIM fMask;
            /// <remarks> 指定风格。RBBS </remarks>
            public RBBS fStyle;
            /// <remarks> 前景色 </remarks>
            public int clrFore;
            /// <remarks> 背景色 </remarks>
            public int clrBack;
            /// <remarks> 缓冲区的长指针 </remarks>
            public IntPtr lpText;
            /// <remarks> 缓冲区大小 </remarks>
            public int cch;
            /// <remarks> 显示的任何图像的从零开始的索引 </remarks>
            public int iImage;
            /// <remarks> 包含的子窗口（如果有）的句柄 </remarks>
            public IntPtr hwndChild;
            /// <remarks> 子窗口的最小长度，以像素为单位 </remarks>
            public int cxMinChild;
            /// <remarks> 子窗口的最小厚度，以像素为单位 </remarks>
            public int cyMinChild;
            /// <remarks> 长度，以像素为单位 </remarks>
            public int cx;
            /// <remarks> 背景的位图 </remarks>
            public IntPtr hbmBack;
            /// <remarks> 标识“自定义绘图”消息 </remarks>
            public int wID;
            /// <remarks> RBBS_VARIABLEHEIGHT样式有效。初始高度，以像素为单位 </remarks>
            public int cyChild;
            /// <remarks> RBBS_VARIABLEHEIGHT样式有效。初始高度，以像素为单位 </remarks>
            public int cyMaxChild;
            /// <remarks> RBBS_VARIABLEHEIGHT样式有效。可以增大或缩小的步长值。 </remarks>
            public int cyIntegral;
            /// <remarks> 指定波段的理想宽度，以像素为单位。 </remarks>
            public int cxIdeal;
            /// <remarks> 自定义值 </remarks>
            public IntPtr lParam;
            /// <remarks> ? </remarks>
            public int cxHeader;
            public static REBARBANDINFO Instantiate()
            {
                REBARBANDINFO info = new REBARBANDINFO();
                info.cbSize = Marshal.SizeOf(info);
                return info;
            }


        }


    }

}