// -----------------------------------------------------------------------------------------------------------------
// <copyright file="DoubleClickBehavior.cs" company="dwndland">
//     Copyright (c) David Wendland. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Input;

// ReSharper disable CheckNamespace

namespace Chapter.Net.WPF.Behaviors;

/// <summary>
///     Brings the feature to be able to double click any UI element.
/// </summary>
public static class DoubleClickBehavior
{
    /// <summary>
    ///     Defines the Command attached dependency property.
    /// </summary>
    public static readonly DependencyProperty CommandProperty =
        DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(DoubleClickBehavior), new UIPropertyMetadata(OnCommandChanged));

    /// <summary>
    ///     Defines the CommandParameter attached dependency property.
    /// </summary>
    public static readonly DependencyProperty CommandParameterProperty =
        DependencyProperty.RegisterAttached("CommandParameter", typeof(object), typeof(DoubleClickBehavior), new UIPropertyMetadata(null));

    /// <summary>
    ///     Gets the command to be called when the element gets double clicked.
    /// </summary>
    /// <param name="obj">The element from which the property value is read.</param>
    /// <returns>The DoubleClickBehavior.Command property value for the element.</returns>
    public static ICommand GetCommand(DependencyObject obj)
    {
        return (ICommand)obj.GetValue(CommandProperty);
    }

    /// <summary>
    ///     Attaches the command to be called when the element gets double clicked.
    /// </summary>
    /// <param name="obj">The element to which the attached property is written.</param>
    /// <param name="value">The needed DoubleClickBehavior.Command value.</param>
    public static void SetCommand(DependencyObject obj, ICommand value)
    {
        obj.SetValue(CommandProperty, value);
    }

    /// <summary>
    ///     Gets the command parameter to be passed when the called when DoubleClickBehavior.Command gets called.
    /// </summary>
    /// <param name="obj">The element from which the property value is read.</param>
    /// <returns>The DoubleClickBehavior.CommandParameter property value for the element.</returns>
    public static object GetCommandParameter(DependencyObject obj)
    {
        return obj.GetValue(CommandParameterProperty);
    }

    /// <summary>
    ///     Attaches the command parameter to be passed when the called when DoubleClickBehavior.Command gets called.
    /// </summary>
    /// <param name="obj">The element to which the attached property is written.</param>
    /// <param name="value">The needed DoubleClickBehavior.CommandParameter value.</param>
    public static void SetCommandParameter(DependencyObject obj, object value)
    {
        obj.SetValue(CommandParameterProperty, value);
    }

    private static void OnCommandChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
        if (!(sender is UIElement control))
            throw new InvalidOperationException("The DoubleClickBehavior can only attached to an UIElement");

        if (e.OldValue != null)
            control.PreviewMouseLeftButtonDown -= MouseButtonDown;
        if (e.NewValue != null)
            control.PreviewMouseLeftButtonDown += MouseButtonDown;
    }

    private static void MouseButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ClickCount != 2)
            return;

        var dependencyObject = (DependencyObject)sender;
        var command = GetCommand(dependencyObject);
        var parameter = GetCommandParameter(dependencyObject);
        if (parameter == null && sender is FrameworkElement element)
            parameter = element.DataContext;
        if (command != null && command.CanExecute(parameter))
            command.Execute(parameter);
    }
}