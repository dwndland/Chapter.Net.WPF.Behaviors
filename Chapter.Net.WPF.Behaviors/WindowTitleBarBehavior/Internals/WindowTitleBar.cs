// -----------------------------------------------------------------------------------------------------------------
// <copyright file="WindowTitleBar.cs" company="my-libraries">
//     Copyright (c) David Wendland. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Interop;
using Chapter.Net.WinAPI;
using Chapter.Net.WinAPI.Data;

// ReSharper disable once CheckNamespace

namespace Chapter.Net.WPF.Behaviors.Internals;

internal static class WindowTitleBar
{
    internal static void RemoveTitleItems(Window window)
    {
        var hwnd = new WindowInteropHelper(window).Handle;
        var windowLong = User32.GetWindowLong(hwnd, GWL.STYLE);
        windowLong &= ~WS.SYSMENU;
        User32.SetWindowLong(hwnd, GWL.STYLE, windowLong);
    }

    internal static void DisableSystemMenu(Window window)
    {
        var hwnd = new WindowInteropHelper(window).Handle;
        var windowLong = User32.GetWindowLong(hwnd, GWL.EXSTYLE);
        windowLong |= WS.EX_DLGMODALFRAME;
        uint windowFlags = SWP.NOMOVE | SWP.NOSIZE | SWP.NOZORDER | SWP.FRAMECHANGED;
        User32.SetWindowLong(hwnd, GWL.EXSTYLE, windowLong);
        User32.SetWindowPos(hwnd, IntPtr.Zero, 0, 0, 0, 0, windowFlags);
    }

    internal static void DisableMinimizeButton(Window window)
    {
        var hwnd = new WindowInteropHelper(window).Handle;
        var windowLong = User32.GetWindowLong(hwnd, GWL.STYLE);
        windowLong &= ~WS.MINIMIZEBOX;
        User32.SetWindowLong(hwnd, GWL.STYLE, windowLong);
    }

    internal static void DisableMaximizeButton(Window window)
    {
        var hwnd = new WindowInteropHelper(window).Handle;
        var windowLong = User32.GetWindowLong(hwnd, GWL.STYLE);
        windowLong &= ~WS.MAXIMIZEBOX;
        User32.SetWindowLong(hwnd, GWL.STYLE, windowLong);
    }

    internal static void DisableCloseButton(Window window)
    {
        if (PresentationSource.FromVisual(window) is HwndSource hwndSource)
            hwndSource.AddHook(DisableCloseButtonHook);
    }

    private static IntPtr DisableCloseButtonHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
        if (msg == WM.SHOWWINDOW)
        {
            var hMenu = User32.GetSystemMenu(hwnd, false);
            if (hMenu != IntPtr.Zero)
                User32.EnableMenuItem(hMenu, SC.CLOSE, MF.BYCOMMAND | MF.GRAYED);
        }

        return IntPtr.Zero;
    }
}