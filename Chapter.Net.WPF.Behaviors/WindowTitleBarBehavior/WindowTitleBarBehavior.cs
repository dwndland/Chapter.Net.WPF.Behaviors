// -----------------------------------------------------------------------------------------------------------------
// <copyright file="WindowTitleBarBehavior.cs" company="dwndland">
//     Copyright (c) David Wendland. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------------------------------------------------

using System;
using System.Windows;
using Chapter.Net.WPF.Behaviors.Internals;

// ReSharper disable CheckNamespace

namespace Chapter.Net.WPF.Behaviors;

/// <summary>
///     Brings the feature to the Window to disable or hide elements in the title bar.
/// </summary>
public static class WindowTitleBarBehavior
{
    /// <summary>
    ///     Defines the RemoveTitleItems attached dependency property.
    /// </summary>
    public static readonly DependencyProperty RemoveTitleItemsProperty =
        DependencyProperty.RegisterAttached("RemoveTitleItems", typeof(bool), typeof(WindowTitleBarBehavior), new UIPropertyMetadata(false, OnRemoveTitleItemsChanged));

    /// <summary>
    ///     Defines the DisableMinimizeButton attached dependency property.
    /// </summary>
    public static readonly DependencyProperty DisableMinimizeButtonProperty =
        DependencyProperty.RegisterAttached("DisableMinimizeButton", typeof(bool), typeof(WindowTitleBarBehavior), new UIPropertyMetadata(false, OnDisableMinimizeButtonChanged));

    /// <summary>
    ///     Defines the DisableMaximizeButton attached dependency property.
    /// </summary>
    public static readonly DependencyProperty DisableMaximizeButtonProperty =
        DependencyProperty.RegisterAttached("DisableMaximizeButton", typeof(bool), typeof(WindowTitleBarBehavior), new UIPropertyMetadata(false, OnDisableMaximizeButtonChanged));

    /// <summary>
    ///     Defines the DisableCloseButton attached dependency property.
    /// </summary>
    public static readonly DependencyProperty DisableCloseButtonProperty =
        DependencyProperty.RegisterAttached("DisableCloseButton", typeof(bool), typeof(WindowTitleBarBehavior), new PropertyMetadata(false, OnDisableCloseButtonChanged));

    /// <summary>
    ///     Defines the DisableSystemMenu attached dependency property.
    /// </summary>
    public static readonly DependencyProperty DisableSystemMenuProperty =
        DependencyProperty.RegisterAttached("DisableSystemMenu", typeof(bool), typeof(WindowTitleBarBehavior), new PropertyMetadata(false, OnDisableSystemMenuChanged));

    /// <summary>
    ///     Gets a value the indicates if the window has to show title bar items or not.
    /// </summary>
    /// <param name="obj">The element from which the property value is read.</param>
    /// <returns>The WindowTitleBarBehavior.RemoveTitleItems property value for the element.</returns>
    public static bool GetRemoveTitleItems(DependencyObject obj)
    {
        return (bool)obj.GetValue(RemoveTitleItemsProperty);
    }

    /// <summary>
    ///     Attaches a value the indicates if the window has to show title bar items or not.
    /// </summary>
    /// <param name="obj">The element to which the attached property is written.</param>
    /// <param name="value">The needed WindowTitleBarBehavior.RemoveTitleItems value.</param>
    public static void SetRemoveTitleItems(DependencyObject obj, bool value)
    {
        obj.SetValue(RemoveTitleItemsProperty, value);
    }

    /// <summary>
    ///     Gets a value the indicates if the window has an enabled minimize button or not.
    /// </summary>
    /// <param name="obj">The element from which the property value is read.</param>
    /// <returns>The WindowTitleBarBehavior.DisableMinimizeButton property value for the element.</returns>
    public static bool GetDisableMinimizeButton(DependencyObject obj)
    {
        return (bool)obj.GetValue(DisableMinimizeButtonProperty);
    }

    /// <summary>
    ///     Attaches a value the indicates if the window has an enabled minimize button or not.
    /// </summary>
    /// <param name="obj">The element to which the attached property is written.</param>
    /// <param name="value">The needed WindowTitleBarBehavior.DisableMinimizeButton value.</param>
    public static void SetDisableMinimizeButton(DependencyObject obj, bool value)
    {
        obj.SetValue(DisableMinimizeButtonProperty, value);
    }

    /// <summary>
    ///     Gets a value the indicates if the window has an enabled maximize button or not.
    /// </summary>
    /// <param name="obj">The element from which the property value is read.</param>
    /// <returns>The WindowTitleBarBehavior.DisableMaximizeButton property value for the element.</returns>
    public static bool GetDisableMaximizeButton(DependencyObject obj)
    {
        return (bool)obj.GetValue(DisableMaximizeButtonProperty);
    }

    /// <summary>
    ///     Attaches a value the indicates if the window has an enabled maximize button or not.
    /// </summary>
    /// <param name="obj">The element to which the attached property is written.</param>
    /// <param name="value">The needed WindowTitleBarBehavior.DisableMaximizeButton value.</param>
    public static void SetDisableMaximizeButton(DependencyObject obj, bool value)
    {
        obj.SetValue(DisableMaximizeButtonProperty, value);
    }

    /// <summary>
    ///     Gets a value that indicates if the window has an enabled close button or not.
    /// </summary>
    /// <param name="obj">The element to which the attached property is written.</param>
    /// <returns>The WindowTitleBarBehavior.DisableCloseButton property value for the element.</returns>
    public static bool GetDisableCloseButton(DependencyObject obj)
    {
        return (bool)obj.GetValue(DisableCloseButtonProperty);
    }

    /// <summary>
    ///     Attaches a value the indicates if the window has an enabled close button or not.
    /// </summary>
    /// <param name="obj">The element to which the attached property is written.</param>
    /// <param name="value">The needed WindowTitleBarBehavior.DisableCloseButton value.</param>
    public static void SetDisableCloseButton(DependencyObject obj, bool value)
    {
        obj.SetValue(DisableCloseButtonProperty, value);
    }

    /// <summary>
    ///     Gets a value that indicates if the window has a system menu or not.
    /// </summary>
    /// <param name="obj">The element to which the attached property is written.</param>
    /// <returns>The WindowTitleBarBehavior.DisableSystemMenu property value for the element.</returns>
    public static bool GetDisableSystemMenu(DependencyObject obj)
    {
        return (bool)obj.GetValue(DisableSystemMenuProperty);
    }

    /// <summary>
    ///     Attaches a value the indicates if the window has a system menu or not.
    /// </summary>
    /// <param name="obj">The element to which the attached property is written.</param>
    /// <param name="value">The needed WindowTitleBarBehavior.DisableSystemMenu value.</param>
    public static void SetDisableSystemMenu(DependencyObject obj, bool value)
    {
        obj.SetValue(DisableSystemMenuProperty, value);
    }

    private static void OnRemoveTitleItemsChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
        var window = GetWindow(sender);

        if (e.OldValue != null)
            window.SourceInitialized -= RemoveTitleItems_SourceInitialized;
        if (e.NewValue != null)
            window.SourceInitialized += RemoveTitleItems_SourceInitialized;
    }

    private static void OnDisableMinimizeButtonChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
        var window = GetWindow(sender);

        if (e.OldValue != null)
            window.SourceInitialized -= DisableMinimizeButton_SourceInitialized;
        if (e.NewValue != null)
            window.SourceInitialized += DisableMinimizeButton_SourceInitialized;
    }

    private static void OnDisableMaximizeButtonChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
        var window = GetWindow(sender);

        if (e.OldValue != null)
            window.SourceInitialized -= DisableMaximizeButton_SourceInitialized;
        if (e.NewValue != null)
            window.SourceInitialized += DisableMaximizeButton_SourceInitialized;
    }

    private static void OnDisableCloseButtonChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
        var window = GetWindow(sender);

        if (e.OldValue != null)
            window.SourceInitialized -= DisableCloseButton_SourceInitialized;
        if (e.NewValue != null)
            window.SourceInitialized += DisableCloseButton_SourceInitialized;
    }

    private static void OnDisableSystemMenuChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
        var window = GetWindow(sender);

        if (e.OldValue != null)
            window.SourceInitialized -= DisableSystemMenu_SourceInitialized;
        if (e.NewValue != null)
            window.SourceInitialized += DisableSystemMenu_SourceInitialized;
    }

    private static Window GetWindow(DependencyObject sender)
    {
        if (!(sender is Window window))
            throw new InvalidOperationException("The WindowTitleBarBehavior can be attached to a window only.");
        return window;
    }

    private static void RemoveTitleItems_SourceInitialized(object sender, EventArgs e)
    {
        var window = (Window)sender;
        WindowTitleBar.RemoveTitleItems(window);
    }

    private static void DisableMinimizeButton_SourceInitialized(object sender, EventArgs e)
    {
        var window = (Window)sender;
        WindowTitleBar.DisableMinimizeButton(window);
    }

    private static void DisableMaximizeButton_SourceInitialized(object sender, EventArgs e)
    {
        var window = (Window)sender;
        WindowTitleBar.DisableMaximizeButton(window);
    }

    private static void DisableCloseButton_SourceInitialized(object sender, EventArgs e)
    {
        var window = (Window)sender;
        WindowTitleBar.DisableCloseButton(window);
    }

    private static void DisableSystemMenu_SourceInitialized(object sender, EventArgs e)
    {
        var window = (Window)sender;
        WindowTitleBar.DisableSystemMenu(window);
    }
}