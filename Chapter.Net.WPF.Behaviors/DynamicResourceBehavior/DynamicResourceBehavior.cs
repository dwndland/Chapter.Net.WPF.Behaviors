// -----------------------------------------------------------------------------------------------------------------
// <copyright file="DynamicResourceBehavior.cs" company="dwndland">
//     Copyright (c) David Wendland. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Controls;

// ReSharper disable once CheckNamespace

namespace Chapter.Net.WPF.Behaviors;

/// <summary>
///     Brings the possibility to create a dynamic resource out of a binding value.
/// </summary>
public static class DynamicResourceBehavior
{
    /// <summary>
    ///     The ResourceKey attached dependency property.
    /// </summary>
    public static readonly DependencyProperty ResourceKeyProperty =
        DependencyProperty.RegisterAttached("ResourceKey", typeof(string), typeof(DynamicResourceBehavior), new PropertyMetadata(OnResourceKeyChanged));

    /// <summary>
    ///     Gets the resource key attached on a control.
    /// </summary>
    /// <param name="obj">The control with the value attached.</param>
    /// <returns>The resource key attached on a control.</returns>
    public static string GetResourceKey(DependencyObject obj)
    {
        return (string)obj.GetValue(ResourceKeyProperty);
    }

    /// <summary>
    ///     Sets the resource key attached on a control.
    /// </summary>
    /// <param name="obj">The control with the value attached.</param>
    /// <param name="value">The resource key attached on a control.</param>
    public static void SetResourceKey(DependencyObject obj, string value)
    {
        obj.SetValue(ResourceKeyProperty, value);
    }

    private static void OnResourceKeyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (e.NewValue == null)
            return;

        switch (d)
        {
            case TextBlock textBlock:
                textBlock.SetResourceReference(TextBlock.TextProperty, e.NewValue);
                break;
            case ContentControl control:
                control.SetResourceReference(ContentControl.ContentProperty, e.NewValue);
                break;
            default:
                throw new InvalidOperationException("The DynamicResourceBehavior.ResourceKey can be attached to TextBlock or ContentControl only.");
        }
    }
}