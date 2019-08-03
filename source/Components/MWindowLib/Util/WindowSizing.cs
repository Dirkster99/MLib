namespace MWindowLib.Util
{
    using MWindowLib.Native;
    using System;
    using System.Runtime.InteropServices;
    using System.Windows;

    /// <summary>
    /// Source: https://codekong.wordpress.com/2010/11/10/custom-window-style-and-accounting-for-the-taskbar/
    ///
    /// We have to add some code to a window, with WindowStyle set to None, to make our
    /// new window accommodate for the taskbar. One thing we must be especially aware of
    /// is accommodating for the taskbar when it is set to auto-hide. When the taskbar is
    /// auto-hidden, you have to leave 2 pixels available for the bar on the docked edge
    /// so the user can mouse over that area to restore the hidden taskbar.
    /// 
    /// This public static WindowSizing class that should handle the bar correctly.
    /// 
    /// http://blog.opennetcf.com/ayakhnin/content/binary/OfficeStyleWindow.zip
    /// </summary>
    internal static class WindowSizing
    {
        /// <summary>
        /// Determines the default monitor for a window whos taskbar is to be taken into account.
        /// (tasbars can be configured to dock different on multiple displays).
        /// </summary>
        const int MONITOR_DEFAULTTONEAREST = 0x00000002;

        #region DLLImports

        [DllImport("shell32", CallingConvention = CallingConvention.StdCall)]
        public static extern int SHAppBarMessage(int dwMessage, ref APPBARDATA pData);

        [DllImport("user32", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32")]
        internal static extern bool GetMonitorInfo(IntPtr hMonitor, MONITORINFO lpmi);

        [DllImport("user32")]
        internal static extern IntPtr MonitorFromWindow(IntPtr handle, int flags);

        #endregion

        /// <summary>
        /// Adjust Window Size for the case when
        /// - window is maximized and
        /// - taskbar is set to autohide
        /// </summary>
        /// <param name="monitorContainingApplication"></param>
        /// <param name="mmi"></param>
        /// <returns></returns>
        private static MINMAXINFO AdjustWorkingAreaForAutoHide(IntPtr monitorContainingApplication, MINMAXINFO mmi)
        {
            IntPtr hwnd = FindWindow("Shell_TrayWnd", null);
            if (hwnd == null) return mmi;
            IntPtr monitorWithTaskbarOnIt = MonitorFromWindow(hwnd, MONITOR_DEFAULTTONEAREST);
            if (!monitorContainingApplication.Equals(monitorWithTaskbarOnIt)) return mmi;
            APPBARDATA abd = new APPBARDATA();
            abd.cbSize = Marshal.SizeOf(abd);
            abd.hWnd = hwnd;
            SHAppBarMessage((int)ABMsg.ABM_GETTASKBARPOS, ref abd);
            int uEdge = GetEdge(abd.rc);
            bool autoHide = System.Convert.ToBoolean(SHAppBarMessage((int)ABMsg.ABM_GETSTATE, ref abd));

            if (!autoHide) return mmi;

            switch (uEdge)
            {
                case (int)ABEdge.ABE_LEFT:
                    mmi.ptMaxPosition.x += 2;
                    mmi.ptMaxTrackSize.x -= 2;
                    mmi.ptMaxSize.x -= 2;
                    break;
                case (int)ABEdge.ABE_RIGHT:
                    mmi.ptMaxSize.x -= 2;
                    mmi.ptMaxTrackSize.x -= 2;
                    break;
                case (int)ABEdge.ABE_TOP:
                    mmi.ptMaxPosition.y += 2;
                    mmi.ptMaxTrackSize.y -= 2;
                    mmi.ptMaxSize.y -= 2;
                    break;
                case (int)ABEdge.ABE_BOTTOM:
                    mmi.ptMaxSize.y -= 2;
                    mmi.ptMaxTrackSize.y -= 2;
                    break;
                default:
                    return mmi;
            }
            return mmi;
        }

        private static int GetEdge(RECT rc)
        {
            int uEdge = -1;
            if (rc.top == rc.left && rc.bottom > rc.right)
                uEdge = (int)ABEdge.ABE_LEFT;
            else if (rc.top == rc.left && rc.bottom < rc.right)
                uEdge = (int)ABEdge.ABE_TOP;
            else if (rc.top > rc.left)
                uEdge = (int)ABEdge.ABE_BOTTOM;
            else
                uEdge = (int)ABEdge.ABE_RIGHT;
            return uEdge;
        }

        public static void WindowInitialized(Window window)
        {
            IntPtr handle = (new System.Windows.Interop.WindowInteropHelper(window)).Handle;
            System.Windows.Interop.HwndSource.FromHwnd(handle).AddHook(new System.Windows.Interop.HwndSourceHook(WindowProc));
        }

        private static bool _AboutToBeMaximized = false;

        private static IntPtr WindowProc(System.IntPtr hwnd,
                                         int msg,
                                         System.IntPtr wParam,
                                         System.IntPtr lParam,
                                         ref bool handled)
        {
            switch (msg)
            {
                // The SC_MAXIMIZE event is always fired BEFOR the WM_GETMINMAXINFO event
                //
                // https://stackoverflow.com/questions/1295999/event-when-a-window-gets-maximized-un-maximized
                // Check your window state here
                case (int)Constants.SYSCOMMAND:                       // 0x0112
                    if (wParam == new IntPtr(Constants.SC_MAXIMIZE)) // Maximize event - SC_MAXIMIZE from Winuser.h
                        _AboutToBeMaximized = true;
                    else
                        _AboutToBeMaximized = false;
                    break;

                case Constants.WM_GETMINMAXINFO:       // 0x0024
                    // This code should only be invoked when the window is maximized
                    // Otherwise, for resize and so forth, we let WPF go ahead and do the hard work
                    if (_AboutToBeMaximized == true)
                    {
                        WmGetMinMaxInfo(hwnd, lParam);
                        handled = true;
                    }
                    break;
            }

            return (IntPtr)0;
        }

        private static void WmGetMinMaxInfo(IntPtr hwnd, IntPtr lParam)
        {
            MINMAXINFO mmi = (MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(MINMAXINFO));
            IntPtr monitorContainingApplication = MonitorFromWindow(hwnd, MONITOR_DEFAULTTONEAREST);

            if (monitorContainingApplication != System.IntPtr.Zero)
            {
                MONITORINFO monitorInfo = new MONITORINFO();
                GetMonitorInfo(monitorContainingApplication, monitorInfo);
                RECT rcWorkArea = monitorInfo.rcWork;
                RECT rcMonitorArea = monitorInfo.rcMonitor;

                mmi.ptMaxPosition.x = Math.Abs(rcWorkArea.left - rcMonitorArea.left);
                mmi.ptMaxPosition.y = Math.Abs(rcWorkArea.top - rcMonitorArea.top);
                mmi.ptMaxSize.x = Math.Abs(rcWorkArea.right - rcWorkArea.left);
                mmi.ptMaxSize.y = Math.Abs(rcWorkArea.bottom - rcWorkArea.top);

                mmi.ptMaxTrackSize.x = mmi.ptMaxSize.x;                                // maximum drag X size for the window
                mmi.ptMaxTrackSize.y = mmi.ptMaxSize.y;                                // maximum drag Y size for the window
                mmi.ptMinTrackSize.x = (int)SystemParameters.MinimumWindowWidth;       // minimum drag X size for the window
                mmi.ptMinTrackSize.y = (int)SystemParameters.MinimumWindowHeight;      // minimum drag Y size for the window
                mmi = AdjustWorkingAreaForAutoHide(monitorContainingApplication, mmi); // need to adjust sizing if taskbar is set to autohide
            }

            Marshal.StructureToPtr(mmi, lParam, true);
        }

        public enum ABEdge
        {
            ABE_LEFT = 0,
            ABE_TOP = 1,
            ABE_RIGHT = 2,
            ABE_BOTTOM = 3
        }

        public enum ABMsg
        {
            ABM_NEW = 0,
            ABM_REMOVE = 1,
            ABM_QUERYPOS = 2,
            ABM_SETPOS = 3,
            ABM_GETSTATE = 4,
            ABM_GETTASKBARPOS = 5,
            ABM_ACTIVATE = 6,
            ABM_GETAUTOHIDEBAR = 7,
            ABM_SETAUTOHIDEBAR = 8,
            ABM_WINDOWPOSCHANGED = 9,
            ABM_SETSTATE = 10
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct APPBARDATA
        {
            public int cbSize;
            public IntPtr hWnd;
            public int uCallbackMessage;
            public int uEdge;
            public RECT rc;
            public bool lParam;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MINMAXINFO
        {
            public POINT ptReserved;
            public POINT ptMaxSize;
            public POINT ptMaxPosition;
            public POINT ptMinTrackSize;
            public POINT ptMaxTrackSize;
        };

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class MONITORINFO
        {
            public int cbSize = Marshal.SizeOf(typeof(MONITORINFO));
            public RECT rcMonitor = new RECT();
            public RECT rcWork = new RECT();
            public int dwFlags = 0;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int x;
            public int y;

            public POINT(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }
    }
}
