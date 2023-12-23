using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Automation;
using System.IO; 
using System.Drawing;

namespace TabTipKeyboard
{
    /// <summary>
    /// 弹出键盘
    /// </summary>
    public class SoftKeyBoardManager
    {
        /// <summary>
        ///     虚拟键盘 窗口名称
        /// </summary>
        private const string TabTipWindowClassName = "IPTIP_Main_Window";

        private const string WindowParentClass = "ApplicationFrameWindow";
        private const string WindowClass = "Windows.UI.Core.CoreWindow";
        private const string WindowCaption = "Microsoft Text Input Application";

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool IsWindowVisible(IntPtr hWnd);

        /// <summary>
        /// 显示键盘
        /// </summary>
        /// <param name="isBig"></param>
        public static void ShowKeyboard()
        {
            if (!GetIsOpenKeyboard())
                KeyBoardShowAndHidden();
        }

        /// <summary>
        /// 隐藏键盘
        /// </summary>

        public static void CloseKeyboard()
        {

            if (GetIsOpenKeyboard())
                KeyBoardShowAndHidden();
        }

        /// <summary>
        /// 显示和隐藏键盘
        /// </summary>

        public static void KeyBoardShowAndHidden()
        {
            Task.Run(() =>
            {
                //使用com组件的方式来打开TabTip.exe
                var uiHostNoLaunch = new UIHostNoLaunch();
                // ReSharper disable once SuspiciousTypeConversion.Global
                var tipInvocation = uiHostNoLaunch as ITipInvocation;
                tipInvocation?.Toggle(User32.GetDesktopWindow());
                Marshal.ReleaseComObject(uiHostNoLaunch);
            });
        }

        /// <summary>
        /// 初始化键盘
        /// </summary>
        public static void InitKeyBoard()
        {
            try
            {
                Task.Run(() =>
                {
                    if (!IsTabTipProcessPresent())
                    {
                        var commonFilesPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles);
                        //程序集目标平台为X86时，获取到的是x86的Program Files，但TabTip.exe始终在Program Files目录下
                        if (commonFilesPath.Contains("Program Files (x86)"))
                        {
                            commonFilesPath = commonFilesPath.Replace("Program Files (x86)", "Program Files");
                        }
                        var tabTipPath = Path.Combine(commonFilesPath, @"microsoft shared\ink\TabTip.exe");
                        var processStartInfo = new ProcessStartInfo
                        {
                            FileName = tabTipPath,
                            UseShellExecute = true,
                            CreateNoWindow = true
                        };
                        Process.Start(processStartInfo);
                        //第一次系统软键盘启动时候，需要缓冲一下
                        Thread.Sleep(50);
                    }
                });
            }
            catch (Exception ex)
            {
                //启动键盘失败 
            }
        }

        private static bool IsTabTipProcessPresent()
        {
            var handle = User32.FindWindow(TabTipWindowClassName, "");
            return IntPtr.Size == 4
                ? handle.ToInt32() > 0
                : handle.ToInt64() > 0;
        }

        private static bool IsNeedCom()
        {
            var minWin10Version = new Version(10, 0, 14393, 0);
            return Environment.OSVersion.Version >= minWin10Version;
        }

        static bool IsTabTipVisible()
        {
            AutomationElement tabtipElement = AutomationElement.RootElement.FindFirst(
                TreeScope.Children,
                new PropertyCondition(AutomationElement.NameProperty, "Touch Keyboard"));

            if (tabtipElement != null)
            {
                return true; // TabTip is visible
            }

            return false; // TabTip is hidden
        }



        private static bool GetIsOpenKeyboard()
        {
            if (IsNeedCom())
                return MostNewVersion();
            else
                return OldVersion();
        }

        /// <summary>
        /// 10.0.14393之前版本判断窗口是否显示
        /// </summary>
        /// <returns></returns>
        private static bool OldVersion()
        {
            var touchHwnd = User32.FindWindow(TabTipWindowClassName, null);

            if (touchHwnd == IntPtr.Zero)
            {
                return false;
            }

            // 这里需要 unchecked 因为返回的是 int 转换为 WindowStyles 需要忽略负号
            var style = (WindowStyles)User32.GetWindowLongPtr(touchHwnd, -16);
            // 如果满足了下面的条件就可以判断显示键盘
            // 由于有的系统在键盘不显示时候只是多返回一个WS_DISABLED这个字段。所以加一个它的判断
            return style.HasFlag(WindowStyles.WS_CLIPSIBLINGS)
                   && style.HasFlag(WindowStyles.WS_VISIBLE)
                   && style.HasFlag(WindowStyles.WS_POPUP)
                   && !style.HasFlag(WindowStyles.WS_DISABLED);
        }

        /// <summary>
        /// 最新版本判断窗口是否显示（操作系统 19045.3570 左右应该都适配）
        /// </summary>
        /// <returns></returns>
        private static bool MostNewVersion()
        {
            var inputPane = (IFrameworkInputPane)new FrameworkInputPane();
            inputPane.Location(out var rect);
            var isOpen = !(rect.Width == 0 && rect.Height == 0);
            Debug.WriteLine($"=============键盘是否显示：{isOpen}============");
            return isOpen;
        }

        /// <summary>
        /// 新版本判断窗口是否显示（不知道哪个版本到哪个版本之间）
        /// </summary>
        /// <returns></returns>
        private static bool NewVersion()
        {
            var wnd = User32.FindWindowEx(IntPtr.Zero, IntPtr.Zero, WindowClass, WindowCaption);
            if (wnd != IntPtr.Zero)
                return false;

            var parent = User32.FindWindowEx(IntPtr.Zero, IntPtr.Zero, WindowParentClass, null);
            if (parent == IntPtr.Zero)
                return false; // no more windows, keyboard state is unknown

            // if it's a child of a WindowParentClass1709 window - the keyboard is open
            wnd = User32.FindWindowEx(parent, IntPtr.Zero, WindowClass, WindowCaption);
            if (wnd != IntPtr.Zero)
                return true;

            return false;
        }

        [ComImport, Guid("4ce576fa-83dc-4F88-951c-9d0782b4e376")]
        class UIHostNoLaunch
        {
        }

        [ComImport, Guid("37c994e7-432b-4834-a2f7-dce1f13b834b")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        interface ITipInvocation
        {
            void Toggle(IntPtr hwnd);
        }

        [DllImport("user32.dll", SetLastError = false)]
        static extern IntPtr GetDesktopWindow();
    }

    [ComImport, Guid("D5120AA3-46BA-44C5-822D-CA8092C1FC72")]
    public class FrameworkInputPane
    {
    }

    [ComImport, System.Security.SuppressUnmanagedCodeSecurity,
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
    Guid("5752238B-24F0-495A-82F1-2FD593056796")]
    public interface IFrameworkInputPane
    {
        [PreserveSig]
        int Advise(
            [MarshalAs(UnmanagedType.IUnknown)] object pWindow,
            [MarshalAs(UnmanagedType.IUnknown)] object pHandler,
            out int pdwCookie
            );

        [PreserveSig]
        int AdviseWithHWND(
            IntPtr hwnd,
            [MarshalAs(UnmanagedType.IUnknown)] object pHandler,
            out int pdwCookie
            );

        [PreserveSig]
        int Unadvise(
            int pdwCookie
            );

        [PreserveSig]
        int Location(
            out Rectangle prcInputPaneScreenLocation
            );
    }
}
