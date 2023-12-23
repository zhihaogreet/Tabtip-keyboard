using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text; 

namespace TabTipKeyboard
{
    public static class User32
    {
        #region 抓取投屏界面相关

        /// <summary>
        /// 将窗口置于 Z 顺序的底部。 如果 hWnd 参数标识最顶层的窗口，则窗口将失去其最顶层状态，并放置在所有其他窗口的底部。
        /// </summary>
        public static readonly IntPtr HWND_BOTTOM = new IntPtr(1);
        /// <summary>
        /// 将窗口置于所有非顶部窗口上方 (，即位于最顶部窗口) 后面。 如果窗口已经是非最顶部窗口，则此标志不起作用。
        /// </summary>
        public static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);
        /// <summary>
        /// 将窗口置于 Z 顺序的顶部。
        /// </summary>
        public static readonly IntPtr HWND_TOP = new IntPtr(0);
        /// <summary>
        /// 将窗口置于所有非最顶部窗口的上面。 该窗口即使已停用，也会保留在最高位置。
        /// </summary>
        public static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);

        public const UInt32 SWP_NOSIZE = 0x0001;
        public const UInt32 SWP_NOMOVE = 0x0002;
        public const UInt32 SWP_SHOWWINDOW = 0x0040;

        [DllImport("user32.dll")]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("user32", SetLastError = true)]
        public static extern int GetWindowText(IntPtr hwnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll")]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
        public struct RECT
        {
            public int left;

            public int top;

            public int right;

            public int bottom;

            public int Width => right - left;

            public int Height => bottom - top;

            public static bool operator ==(RECT r1, RECT r2)
            {
                if ((r1.left == r2.left && r1.right == r2.right) || (r1.top == r2.top && r1.bottom == r2.bottom))
                {
                    return true;
                }

                return false;
            }

            public static bool operator !=(RECT r1, RECT r2)
            {
                if ((r1.left == r2.left && r1.right == r2.right) || (r1.top == r2.top && r1.bottom == r2.bottom))
                {
                    return false;
                }

                return true;
            }
        }
        public delegate bool Win32Callback(IntPtr hwnd, IntPtr lParam);

        [DllImport("User32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(IntPtr hWnd, ref RECT rect);

        [DllImport("user32.Dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumChildWindows(IntPtr parentHandle, Win32Callback callback, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);


        #endregion

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        public static extern IntPtr SetCapture(IntPtr h);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

        [DllImport("user32.dll")]
        public static extern bool PrintWindow(IntPtr hwnd, IntPtr hdcBlt, UInt32 nFlags);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int nWidth, int nHeight);

        [DllImport("gdi32.dll")]
        public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

        [DllImport("gdi32.dll")]
        public static extern int DeleteDC(IntPtr hdc);

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr hwnd);

        [DllImport("user32")]
        public static extern int SetForegroundWindow(IntPtr hwnd);


        [DllImport("user32")]
        public static extern int mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

        /// <summary>
        /// 获取最后输入信息
        /// </summary>
        /// <param name="plii"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);

        /// <summary>
        /// 切换到指定窗口
        /// </summary>
        /// <param name="hWnd">窗口句柄</param>
        /// <param name="fAltTab">是否最前显示</param>
        [DllImport("user32.dll")]
        public static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);

        [DllImport("user32.dll", SetLastError = false)]
        public static extern uint GetWindowLong(IntPtr wnd, int index);

        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);

        [DllImport("user32.dll")]
        public static extern bool ClientToScreen(IntPtr hWnd, ref POINT lpPoint);

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public POINT(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }

            public POINT(System.Windows.Point pt)
            {
                X = Convert.ToInt32(pt.X);
                Y = Convert.ToInt32(pt.Y);
            }
        };

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int SystemParametersInfo(int uAction, int uParam, StringBuilder lpvParam, int fuWinIni);

        public const int SPI_GETDESKWALLPAPER = 0x0073;

        [DllImport("user32.dll", EntryPoint = "GetDesktopWindow", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll")]
        public static extern int GetClassNameW(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder lpString, int nMaxCount);

        public delegate bool WNDENUMPROC(IntPtr hWnd, int lParam);

        [DllImport("user32.dll")]
        public static extern bool EnumWindows(WNDENUMPROC lpEnumFunc, int lParam);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", EntryPoint = "FindWindowEx", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("user32.dll")]
        public static extern int SetParent(IntPtr hWndChild, IntPtr hWndParent);

        [DllImport("user32.dll")]
        public static extern IntPtr GetParent(IntPtr hWndChild);

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern bool DeleteObject(IntPtr hObject);

        [DllImport("user32.dll", EntryPoint = "AnimateWindow")]
        public static extern bool AnimateWindow(IntPtr handle, int ms, int flags);

        [DllImport("user32.dll")]
        public static extern IntPtr WindowFromPoint(System.Drawing.Point point);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        public delegate IntPtr CreateWindowExDelegate(
            uint dwExStyle,
            string lpClassName,
            string lpWindowName,
            uint dwStyle,
            int x,
            int y,
            int nWidth,
            int nHeight,
            IntPtr hWndParent,
            IntPtr hMenu,
            IntPtr hInstance,
            IntPtr lpParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SendMessages(IntPtr hWnd, uint Msg, uint wParam, uint lParam);

        public const uint WM_APPCOMMAND = 0x319;
        public const uint APPCOMMAND_VOLUME_UP = 0x0a;
        public const uint APPCOMMAND_VOLUME_DOWN = 0x09;
        public const uint APPCOMMAND_VOLUME_MUTE = 0x08;

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetForegroundWindows();

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        public delegate IntPtr DefWindowProcDelegate(IntPtr hwnd, WindowsMessage msg, IntPtr wparam, IntPtr lparam);

        public delegate int DestroyWindowDelegate(IntPtr hwnd);

     

        public delegate int IsProcessDPIAwareDelegate();

        public delegate int IsWindowDelegate(IntPtr hwnd);

        public delegate int IsWindowVisibleDelegate(IntPtr hwnd);

        public delegate int MoveWindowDelegate(IntPtr hwnd, int x, int y, int width, int height, int repaint); 

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        public delegate int SendMessageDelegate(IntPtr hwnd, WindowsMessage msg, IntPtr wparam, IntPtr lparam);

        public delegate bool SetLayeredWindowAttributesDelegate(IntPtr hwnd, uint crKey, byte bAlpha, uint dwFlags);

        public delegate IntPtr SetThreadDpiAwarenessContextDelegate(ref int dpiContext);

        public delegate int SetWindowPosDelegate(IntPtr hwnd, IntPtr hwndInsertAfter, int x, int y, int cx, int cy,
            uint flags);

        public delegate int ShowWindowDelegate(IntPtr hWnd, uint nCmdShow);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        public delegate int UnregisterClassDelegate(string lpClassName, IntPtr hInstance);

        public delegate bool UpdateWindowDelegate(IntPtr hWnd);

        public delegate int WaitMessageDelegate();

        public delegate int PostMessageWDelegate(IntPtr hwnd, WindowsMessage message, IntPtr wparam, IntPtr lparam);

        public delegate IntPtr GetForegroundWindowDelegate();

     

        [DllImport("powrprof.dll", EntryPoint = "PowerGetActiveScheme", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern uint PowerGetActiveScheme(IntPtr UserRootPowerKey, ref IntPtr p_ActivePolicyGuid);

        [DllImport("powrprof.dll", EntryPoint = "PowerWriteDCValueIndex", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern uint PowerWriteDCValueIndex(IntPtr RootPowerKey, ref Guid SchemeGuid, ref Guid SubGroupOfPowerSettingsGuid, ref Guid PowerSettingGuid, uint AcValueIndex);

        [DllImport("powrprof.dll", EntryPoint = "PowerReadDCValueIndex", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern uint PowerReadDCValueIndex(IntPtr RootPowerKey, ref Guid SchemeGuid, ref Guid SubGroupOfPowerSettingsGuid, ref Guid PowerSettingGuid, ref uint AcValueIndex);

        [DllImport("powrprof.dll", EntryPoint = "PowerWriteACValueIndex", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern uint PowerWriteACValueIndex(IntPtr RootPowerKey, ref Guid SchemeGuid, ref Guid SubGroupOfPowerSettingsGuid, ref Guid PowerSettingGuid, uint AcValueIndex);

        [DllImport("powrprof.dll", EntryPoint = "PowerReadACValueIndex", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern uint PowerReadACValueIndex(IntPtr RootPowerKey, ref Guid SchemeGuid, ref Guid SubGroupOfPowerSettingsGuid, ref Guid PowerSettingGuid, ref uint AcValueIndex);

        //休眠
        public static Guid GUID_SLEEP_SUBGROUP = new Guid(0x238C9FA8, 0x0AAD, 0x41ED, 0x83, 0xF4, 0x97, 0xBE, 0x24, 0x2C, 0x8F, 0x20);

        public static Guid GUID_STANDBY_TIMEOUT = new Guid(0x29F6C1DB, 0x86DA, 0x48C5, 0x9F, 0xDB, 0xF2, 0xB6, 0x7B, 0x1F, 0x44, 0xDA);

        //屏幕
        public static Guid GUID_VIDEO_SUBGROUP = new Guid(0x7516B95F, 0xF776, 0x4464, 0x8C, 0x53, 0x06, 0x16, 0x7F, 0x40, 0xCC, 0x99);

        public static Guid GUID_VIDEO_POWERDOWN_TIMEOUT = new Guid(0x3C0BC021, 0xC8A8, 0x4E07, 0xA9, 0x73, 0x6B, 0x14, 0xCB, 0xCB, 0x2B, 0x7E);

        //电源按钮
        //          0 = do nothing
        //          1 = sleep
        //          2 = hibernate
        //          3 = shut-down
        public static Guid GUID_SYSTEM_BUTTON_SUBGROUP = new Guid(0x4F971E89, 0xEEBD, 0x4455, 0xA8, 0xDE, 0x9E, 0x59, 0x04, 0x0E, 0x73, 0x47);

        public static Guid GUID_POWERBUTTON_ACTION = new Guid(0x7648EFA3, 0xDD9C, 0x4E3E, 0xB5, 0x66, 0x50, 0xF9, 0x29, 0x38, 0x62, 0x80);
         

        /// <summary>
        /// 改变指定窗口的属性
        /// </summary>
        /// <param name="hWnd">窗口句柄</param>
        /// <param name="nIndex">指定将设定的大于等于0的偏移值。有效值的范围从0到额外类的存储空间的字节数减4：例如若指定了12或多于12个字节的额外窗口存储空间，则应设索引位8来访问第三个4字节，同样设置0访问第一个4字节，4访问第二个4字节。要设置其他任何值，可以指定下面值之一
        /// 从 GetWindowLongFields 可以找到所有的值
        /// <para>
        /// GetWindowLongFields.GWL_EXSTYLE             -20    设定一个新的扩展风格。 </para>
        /// <para>GWL_HINSTANCE     -6	   设置一个新的应用程序实例句柄。</para>
        /// <para>GWL_ID            -12    设置一个新的窗口标识符。</para>
        /// <para>GWL_STYLE         -16    设定一个新的窗口风格。</para>
        /// <para>GWL_USERDATA      -21    设置与窗口有关的32位值。每个窗口均有一个由创建该窗口的应用程序使用的32位值。</para>
        /// <para>GWL_WNDPROC       -4    为窗口设定一个新的处理函数。</para>
        /// <para>GWL_HWNDPARENT    -8    改变子窗口的父窗口,应使用SetParent函数</para>
        /// </param>
        /// <param name="dwNewLong">指定的替换值</param>
        /// <returns></returns>
        [DllImport(LibraryName, CharSet = Properties.BuildCharSet, EntryPoint = "SetWindowLongPtr")]
        [Obsolete("请使用 SetWindowLongPtr 解决 x86 和 x64 需要使用不同方法")]
        public static extern IntPtr SetWindowLongPtr_x64(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        public const string LibraryName = "user32";

        internal static class Properties
        {
#if !ANSI
            public const CharSet BuildCharSet = CharSet.Unicode;
#else
            public const CharSet BuildCharSet = CharSet.Ansi;
#endif
        }

        [Obsolete("请使用 SetWindowLongPtr 解决 x86 和 x64 需要使用不同方法")]
        [DllImport(LibraryName, CharSet = Properties.BuildCharSet)]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        public static IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong)
        {
            return IntPtr.Size > 4
#pragma warning disable CS0618 // 类型或成员已过时
                    ? SetWindowLongPtr_x64(hWnd, nIndex, dwNewLong)
                : new IntPtr(SetWindowLong(hWnd, nIndex, dwNewLong.ToInt32()));
#pragma warning restore CS0618 // 类型或成员已过时
        }
         

        /// <summary>
        /// 获得指定窗口的信息
        /// </summary>
        /// <param name="hWnd">指定窗口的句柄</param>
        /// <param name="nIndex">需要获得的信息的类型 请使用<see cref="GetWindowLongFields"/></param>
        /// <returns></returns>
        // This static method is required because Win32 does not support
        // GetWindowLongPtr directly
        public static IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex)
        {
            return IntPtr.Size > 4
#pragma warning disable CS0618 // 类型或成员已过时
                    ? GetWindowLongPtr_x64(hWnd, nIndex)
                : new IntPtr(GetWindowLong(hWnd, nIndex));
#pragma warning restore CS0618 // 类型或成员已过时
        }

        /// <summary>
        /// 获得指定窗口的信息
        /// </summary>
        /// <param name="hWnd">指定窗口的句柄</param>
        /// <param name="nIndex">需要获得的信息的类型 请使用<see cref="GetWindowLongFields"/></param>
        /// <returns></returns>
        [Obsolete("请使用 GetWindowLongPtr 解决 x86 和 x64 需要使用不同方法")]
        [DllImport(LibraryName, CharSet = Properties.BuildCharSet, EntryPoint = "GetWindowLongPtr")]
        public static extern IntPtr GetWindowLongPtr_x64(IntPtr hWnd, int nIndex);

        #region =====视频捕获相关（如摄像头）-- avicap32.dll =====

        /// <summary>
        /// 包含了执行视频捕获的函数，它给AVI文件I/O和视频、音频设备驱动程序提供一个高级接口
        /// </summary>
        /// <param name="lpszWindowName">标识窗口的名称</param>
        /// <param name="dwStyle">标识窗口风格</param>
        /// <param name="x">标识窗口的左上角 x 坐标</param>
        /// <param name="y">标识窗口的左上角 y 坐标</param>
        /// <param name="nWidth">标识窗口的宽度</param>
        /// <param name="nHeight">标识窗口的高度</param>
        /// <param name="hwndParent">标识父窗口句柄</param>
        /// <param name="nID">标识窗口ID</param>
        /// <returns>视频捕捉窗口句柄</returns>
        [DllImport("avicap32.dll")]
        public static extern IntPtr capCreateCaptureWindowA(byte[] lpszWindowName, int dwStyle, int x, int y, int nWidth, int nHeight, IntPtr hWndParent, int nID);

        /// <summary>
        /// 获取捕获驱动的版本描述信息。
        /// </summary>
        /// <param name="wDriver">捕获驱动程序的索引。 索引的范围可以是0到9。首先枚举即插即用捕获驱动程序，然后是注册表中列出的捕获驱动程序，然后是SYSTEM.INI中列出的捕获驱动程序。</param>
        /// <param name="lpszName">指向缓冲区的指针，该缓冲区包含与捕获驱动程序名称相对应的以空结束的字符串。</param>
        /// <param name="cbName">lpszName所指向的缓冲区的长度，以字节为单位。</param>
        /// <param name="lpszVer">指向缓冲区的指针，该缓冲区包含与捕获驱动程序描述相对应的以空结束的字符串。</param>
        /// <param name="cbVer">lpszVer所指向缓冲区的长度(以字节为单位)。</param>
        /// <returns></returns>
        [DllImport("avicap32.dll")]
        public static extern bool capGetDriverDescriptionA(short wDriver, byte[] lpszName, int cbName, byte[] lpszVer, int cbVer);

        [DllImport("avicap32.dll")]
        public static extern int capGetVideoFormat(IntPtr hWnd, IntPtr psVideoFormat, int wSize);

        #endregion =====视频捕获相关（如摄像头）-- avicap32.dll =====

        [DllImport(LibraryName, CharSet = Properties.BuildCharSet, EntryPoint = "DestroyMenu")]
        public static extern void DestroyMenu(HandleRef createHandleRef);

        #region ====护眼模式和亮度API====

        [DllImport("user32.dll")]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr ReleaseDC(IntPtr hWnd);

        [DllImport("gdi32.dll")]
        internal static extern IntPtr GetCurrentObject(IntPtr hdc, ushort objectType);

        [DllImport("user32.dll")]
        public static extern IntPtr MonitorFromWindow([In] IntPtr hwnd, uint dwFlags);

        [DllImport("gdi32.dll")]
        public static extern bool DeviceIoControl(
            IntPtr hDevice,
            int dwIoControlCode,
            ref DISPLAY_BRIGHTNESS lpInBuffer,
            int nInBufferSize,
            IntPtr lpOutBuffer,
            int nOutBufferSize,
            ref int lpBytesReturned,
            IntPtr lpOverlapped);

        [DllImport("kernel32")]
        public static extern bool DeviceIoControl(
            IntPtr hDevice,
            int dwIoControlCode,
            IntPtr lpInBuffer,
            int nInBufferSize,
            ref DISPLAY_BRIGHTNESS lpOutBuffer,
            int nOutBufferSize,
            ref int lpBytesReturned,
            IntPtr lpOverlapped);

        [DllImport("kernel32")]
        public static extern bool DeviceIoControl(
            IntPtr hDevice,
            int dwIoControlCode,
            IntPtr lpInBuffer,
            int nInBufferSize,
            [MarshalAs(UnmanagedType.LPArray)] byte[] lpOutBuffer,
            int nOutBufferSize,
            ref int lpBytesReturned,
            IntPtr lpOverlapped);

        [DllImport("kernel32")]
        public static extern IntPtr CreateFile(
         string lpFileName,
         uint dwDesiredAccess,
         int dwShareMode,
         IntPtr lpSecurityAttributes,
         int dwCreationDisposition,
         int dwFlagsAndAttributes,
         IntPtr hTemplateFile);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern int CloseHandle(IntPtr hObject);

        public const uint GENERIC_READ = 0x80000000;
        public const uint GENERIC_WRITE = 0x40000000;
        public const int OPEN_EXISTING = 3;
        public const int FILE_DEVICE_VIDEO = 0x23;

        public const int FILE_ANY_ACCESS = 0;
        public const int FILE_SPECIAL_ACCESS = FILE_ANY_ACCESS;

        public const int METHOD_BUFFERED = 0;
        public const int METHOD_NEITHER = 3;

        public const int SHARE_ALL = 0x7;

        public const int ERROR_INSUFFICIENT_BUFFER = 122;

        [DllImport("kernel32.dll")]
        public static extern uint GetLastError();

        //requAate de lecture des niveaux d'Aclcairage supportAcs
        public static int IOCTL_VIDEO_QUERY_SUPPORTED_BRIGHTNESS = CTL_CODE(FILE_DEVICE_VIDEO, 293, METHOD_BUFFERED, FILE_ANY_ACCESS);

        //requAate de lecture des niveaux d'Acclairage en cours
        public static int IOCTL_VIDEO_QUERY_DISPLAY_BRIGHTNESS = CTL_CODE(FILE_DEVICE_VIDEO, 294, METHOD_BUFFERED, FILE_ANY_ACCESS);

        //requAate de dAcfinition des niveaux d'Acclairage en cours
        public static int IOCTL_VIDEO_SET_DISPLAY_BRIGHTNESS = CTL_CODE(FILE_DEVICE_VIDEO, 295, METHOD_BUFFERED, FILE_ANY_ACCESS);

        private static int CTL_CODE(int dwDeviceType, int dwFunction, int dwMethod, int dwAccess)
        {
            return ((dwDeviceType) << 16) | ((dwAccess) << 14) | ((dwFunction) << 2) | (dwMethod);
        }

        public struct DISPLAY_BRIGHTNESS
        {
            public byte ucDisplayPolicy;
            public byte ucACBrightness;
            public byte ucDCBrightness;
        }

        [DllImport("Advapi32.dll", EntryPoint = "RegGetValueW", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern Int32 RegGetValue(
          IntPtr hKey,
          string lpSubKey,
          string lpValue,
          RFlags dwFlags,
          out RType pdwType,
          IntPtr pvData,
          ref UInt32 pcbData);

        [Flags]
        internal enum RFlags
        {
            Any = 65535,
            RegNone = 1,
            Noexpand = 268435456,
            RegBinary = 8,
            Dword = 24,
            RegDword = 16,
            Qword = 72,
            RegQword = 64,
            RegSz = 2,
            RegMultiSz = 32,
            RegExpandSz = 4,
            RrfZeroonfailure = 536870912
        }

        /// <summary>
        /// http://msdn.microsoft.com/en-us/library/windows/desktop/ms724884(v=vs.85).aspx
        /// </summary>
        internal enum RType
        {
            RegNone = 0,

            RegSz = 1,
            RegExpandSz = 2,
            RegMultiSz = 7,

            RegBinary = 3,
            RegDword = 4,
            RegQword = 11,

            RegQwordLittleEndian = 11,
            RegDwordLittleEndian = 4,
            RegDwordBigEndian = 5,

            RegLink = 6,
            RegResourceList = 8,
            RegFullResourceDescriptor = 9,
            RegResourceRequirementsList = 10,
        }

        public enum RegValueType
        {
            REG_NONE,
            REG_SZ,
            REG_EXPAND_SZ,
            REG_BINARY,
            REG_DWORD,
            REG_DWORD_LITTLE_ENDIAN,
            REG_DWORD_BIG_ENDIAN,
            REG_LINK,
            REG_MULTI_SZ,
            REG_RESOURCE_LIST,
            REG_FULL_RESOURCE_DESCRIPTOR,
            REG_RESOURCE_REQUIREMENTS_LIST,
            REG_QWORD,
            REG_QWORD_LITTLE_ENDIAN
        }

        [DllImport("Advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int RegOpenKeyEx(IntPtr hKey, string lpSubKey, uint ulOptions, int samDesired, out IntPtr phkResult);

        [DllImport("Advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int RegSetValueEx(IntPtr hKey, string lpValueName, uint unReserved, RegValueType unType, byte[] lpData, uint dataCount);

        //关闭Key值
        [DllImport("Advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int RegCloseKey(IntPtr hKey);

        public static readonly IntPtr HKEY_CLASSES_ROOT = new IntPtr(unchecked((int)0x80000000));
        public static readonly IntPtr HKEY_CURRENT_USER = new IntPtr(unchecked((int)0x80000001));
        public static readonly IntPtr HKEY_LOCAL_MACHINE = new IntPtr(unchecked((int)0x80000002));
        public static readonly IntPtr HKEY_USERS = new IntPtr(unchecked((int)0x80000003));
        public static readonly IntPtr HKEY_PERFORMANCE_DATA = new IntPtr(unchecked((int)0x80000004));
        public static readonly IntPtr HKEY_CURRENT_CONFIG = new IntPtr(unchecked((int)0x80000005));
        public static readonly IntPtr HKEY_DYN_DATA = new IntPtr(unchecked((int)0x80000006));

        private const int DISPLAYPOLICY_AC = 1;
        private const int DISPLAYPOLICY_DC = 2;
        public const int DISPLAYPOLICY_BOTH = DISPLAYPOLICY_AC | DISPLAYPOLICY_DC;

        #endregion ====护眼模式和亮度API====

        #region ====wifi相关API -wlanapi.dll===

        [DllImport("wlanapi.dll")]
        public static extern int WlanDeleteProfile([In] IntPtr clientHandle, [In][MarshalAs(UnmanagedType.LPStruct)] Guid interfaceGuid, [In][MarshalAs(UnmanagedType.LPWStr)] string profileName, IntPtr reservedPtr);

        [DllImport("wlanapi.dll")]
        public static extern int WlanOpenHandle([In] uint clientVersion, [In][Out] IntPtr pReserved, out uint negotiatedVersion, out IntPtr clientHandle);

        [DebuggerStepThrough]
        public static void ThrowIfError(int win32ErrorCode)
        {
            if (win32ErrorCode != 0)
            {
                throw new Win32Exception(win32ErrorCode);
            }
        }

        // Token: 0x06000033 RID: 51
        [DllImport("wlanapi.dll")]
        public static extern int WlanGetProfileList([In] IntPtr clientHandle, [MarshalAs(UnmanagedType.LPStruct)][In] Guid interfaceGuid, [In] IntPtr pReserved, out IntPtr profileList);

        // Token: 0x06000034 RID: 52
        [DllImport("wlanapi.dll")]
        public static extern void WlanFreeMemory(IntPtr pMemory);

        #endregion ====wifi相关API -wlanapi.dll===

        #region 任务栏相关API
         

        public enum AppBarMessages
        {
            New =
                0x00000000,
            Remove =
                0x00000001,
            QueryPos =
                0x00000002,
            SetPos =
                0x00000003,
            GetState =
                0x00000004,
            GetTaskBarPos =
                0x00000005,
            Activate =
                0x00000006,
            GetAutoHideBar =
                0x00000007,
            SetAutoHideBar =
                0x00000008,
            WindowPosChanged =
                0x00000009,
            SetState =
                0x0000000a
        }

 

        public enum AppBarStates
        {
            AutoHide =
                0x00000001,
            AlwaysOnTop =
                0x00000002
        }

        /// <summary>
        /// 最后输入信息
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct LASTINPUTINFO
        {
            [MarshalAs(UnmanagedType.U4)]
            public int cbSize;

            [MarshalAs(UnmanagedType.U4)]
            public uint dwTime;
        }

        #endregion
    }
}
