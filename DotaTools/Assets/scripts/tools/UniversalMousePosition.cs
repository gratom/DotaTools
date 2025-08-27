#if UNITY_EDITOR && UNITY_EDITOR_WIN

using System;
using System.Runtime.InteropServices;
using System.Drawing;
using UnityEngine;

namespace Tools
{
    public static class UniversalMousePosition
    {
        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(out Point lpPoint);

        [DllImport("user32.dll")]
        private static extern IntPtr MonitorFromPoint(Point pt, uint dwFlags);

        [DllImport("user32.dll")]
        private static extern bool GetMonitorInfo(IntPtr hMonitor, ref MonitorInfo lpmi);

        private const uint MONITOR_DEFAULT_TO_NEAREST = 2; //type of setting to return the nearest monitor to the cursor point

        [StructLayout(LayoutKind.Sequential)]
        private struct MonitorInfo
        {
            public uint cbSize;
            public Rect rcMonitor;
            public Rect rcWork;
            public uint dwFlags;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct Rect
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        public static Vector2Int GetCursorPosition()
        {
            GetCursorPos(out Point lpPoint);
            return new Vector2Int(lpPoint.X, lpPoint.Y);
        }

        public static Vector2Int GetCurrentMonitorSize()
        {
            GetCursorPos(out Point pt);
            IntPtr hMonitor = MonitorFromPoint(pt, MONITOR_DEFAULT_TO_NEAREST);

            MonitorInfo info = new MonitorInfo();
            info.cbSize = (uint)Marshal.SizeOf(typeof(MonitorInfo));

            if (GetMonitorInfo(hMonitor, ref info))
            {
                int width = info.rcMonitor.Right - info.rcMonitor.Left;
                int height = info.rcMonitor.Bottom - info.rcMonitor.Top;
                return new Vector2Int(width, height);
            }

            return Vector2Int.zero;
        }
    }
}

#endif