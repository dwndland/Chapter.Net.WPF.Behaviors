// -----------------------------------------------------------------------------------------------------------------
// <copyright file="KeyBlockBehavior.cs" company="my-libraries">
//     Copyright (c) David Wendland. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Input;

// ReSharper disable CheckNamespace

namespace Chapter.Net.WPF.Behaviors;

/// <summary>
///     Disabled specific keys on a control.
/// </summary>
public static class KeyBlockBehavior
{
    /// <summary>
    ///     Defines the BlockAll attached dependency property.
    /// </summary>
    public static readonly DependencyProperty BlockAllProperty =
        DependencyProperty.RegisterAttached("BlockAll", typeof(bool), typeof(KeyBlockBehavior), new PropertyMetadata(OnBlockAllChanged));

    /// <summary>
    ///     Defines the BlockKey attached dependency property.
    /// </summary>
    public static readonly DependencyProperty BlockKeyProperty =
        DependencyProperty.RegisterAttached("BlockKey", typeof(Key), typeof(KeyBlockBehavior), new PropertyMetadata(OnBlockKeyChanged));

    /// <summary>
    ///     Gets the indicator whether all key press shall be prevented on the attached control or not.
    /// </summary>
    /// <param name="obj">The element from which the property value is read.</param>
    /// <returns>True if all key press are blocked; otherwise false.</returns>
    public static bool GetBlockAll(DependencyObject obj)
    {
        return (bool)obj.GetValue(BlockAllProperty);
    }

    /// <summary>
    ///     Sets the indicator whether all key press shall be prevented on the attached control or not.
    /// </summary>
    /// <param name="obj">The element from which the property will be attached to.</param>
    /// <param name="value">The indicator whether all key press shall be prevented on the attached control or not.</param>
    public static void SetBlockAll(DependencyObject obj, bool value)
    {
        obj.SetValue(BlockAllProperty, value);
    }

    /// <summary>
    ///     Gets the indicator whether the given key press shall be prevented on the attached control or not.
    /// </summary>
    /// <param name="obj">The element from which the property value is read.</param>
    /// <returns>True if a specific key press will be blocked; otherwise false.</returns>
    public static Key GetBlockKey(DependencyObject obj)
    {
        return (Key)obj.GetValue(BlockKeyProperty);
    }

    /// <summary>
    ///     Sets the indicator whether the given key press shall be prevented on the attached control or not.
    /// </summary>
    /// <param name="obj">The element from which the property will be attached to.</param>
    /// <param name="value">The indicator whether the given key press shall be prevented on the attached control or not.</param>
    public static void SetBlockKey(DependencyObject obj, Key value)
    {
        obj.SetValue(BlockKeyProperty, value);
    }

    private static void OnBlockAllChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (!(d is UIElement element))
            throw new InvalidOperationException("The KeyBlockBehavior.BlockAll can be attached to a UIElement only.");

        void ElementOnPreviewKeyDown(object sender, KeyEventArgs o)
        {
            o.Handled = true;
        }

        if ((bool)e.OldValue)
            element.PreviewKeyDown -= ElementOnPreviewKeyDown;
        if ((bool)e.NewValue)
            element.PreviewKeyDown += ElementOnPreviewKeyDown;
    }

    private static void OnBlockKeyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (!(d is UIElement element))
            throw new InvalidOperationException("The KeyBlockBehavior.BlockKey can be attached to a UIElement only.");

        var key = GetBlockKey(element);

        void ElementOnPreviewKeyDown(object sender, KeyEventArgs o)
        {
            if (o.Key == key)
                o.Handled = true;
        }

        element.PreviewKeyDown -= ElementOnPreviewKeyDown;
        element.PreviewKeyDown += ElementOnPreviewKeyDown;
    }
}