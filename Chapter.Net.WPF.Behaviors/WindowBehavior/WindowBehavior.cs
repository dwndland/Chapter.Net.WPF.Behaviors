// -----------------------------------------------------------------------------------------------------------------
// <copyright file="WindowBehavior.cs" company="my-libraries">
//     Copyright (c) David Wendland. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using Chapter.Net.WPF.Theming;

// ReSharper disable CheckNamespace

namespace Chapter.Net.WPF.Behaviors;

/// <summary>
///     Brings the feature to a Window to bind loading and closing action or easy close with dialog result.
/// </summary>
public static class WindowBehavior
{
    /// <summary>
    ///     Defines the DialogResult attached dependency property.
    /// </summary>
    public static readonly DependencyProperty DialogResultProperty =
        DependencyProperty.RegisterAttached("DialogResult", typeof(bool?), typeof(WindowBehavior), new UIPropertyMetadata(OnDialogResultChanged));

    /// <summary>
    ///     Defines the DialogResultCommand attached dependency property.
    /// </summary>
    public static readonly DependencyProperty DialogResultCommandProperty =
        DependencyProperty.RegisterAttached("DialogResultCommand", typeof(ICommand), typeof(WindowBehavior), new UIPropertyMetadata(OnDialogResultChanged));

    /// <summary>
    ///     Defines the DialogResultCommandParameter attached dependency property.
    /// </summary>
    public static readonly DependencyProperty DialogResultCommandParameterProperty =
        DependencyProperty.RegisterAttached("DialogResultCommandParameter", typeof(object), typeof(WindowBehavior), new PropertyMetadata(null));

    /// <summary>
    ///     Defines the ClosingCommand attached dependency property.
    /// </summary>
    public static readonly DependencyProperty ClosingCommandProperty =
        DependencyProperty.RegisterAttached("ClosingCommand", typeof(ICommand), typeof(WindowBehavior), new UIPropertyMetadata(OnClosingCommandChanged));

    /// <summary>
    ///     Defines the ClosingCommandParameter attached dependency property.
    /// </summary>
    public static readonly DependencyProperty ClosingCommandParameterProperty =
        DependencyProperty.RegisterAttached("ClosingCommandParameter", typeof(object), typeof(WindowBehavior), new PropertyMetadata(null));

    /// <summary>
    ///     Defines the ClosedCommand attached dependency property.
    /// </summary>
    public static readonly DependencyProperty ClosedCommandProperty =
        DependencyProperty.RegisterAttached("ClosedCommand", typeof(ICommand), typeof(WindowBehavior), new UIPropertyMetadata(OnClosedCommandChanged));

    /// <summary>
    ///     Defines the ClosedCommandParameter attached dependency property.
    /// </summary>
    public static readonly DependencyProperty ClosedCommandParameterProperty =
        DependencyProperty.RegisterAttached("ClosedCommandParameter", typeof(object), typeof(WindowBehavior), new PropertyMetadata(null));

    /// <summary>
    ///     Defines the IsClose attached dependency property.
    /// </summary>
    public static readonly DependencyProperty IsCloseProperty =
        DependencyProperty.RegisterAttached("IsClose", typeof(bool), typeof(WindowBehavior), new UIPropertyMetadata(OnIsCloseCommandChanged));

    /// <summary>
    ///     Defines the IsCloseCommand attached dependency property.
    /// </summary>
    public static readonly DependencyProperty IsCloseCommandProperty =
        DependencyProperty.RegisterAttached("IsCloseCommand", typeof(ICommand), typeof(WindowBehavior), new PropertyMetadata(OnIsCloseCommandChanged));

    /// <summary>
    ///     Defines the IsCloseCommandParameter attached dependency property.
    /// </summary>
    public static readonly DependencyProperty IsCloseCommandParameterProperty =
        DependencyProperty.RegisterAttached("IsCloseCommandParameter", typeof(object), typeof(WindowBehavior), new PropertyMetadata(null));

    /// <summary>
    ///     Defines the WinApiMessages attached dependency property.
    /// </summary>
    public static readonly DependencyProperty WinApiMessagesProperty =
        DependencyProperty.RegisterAttached("WinApiMessages", typeof(string), typeof(WindowBehavior), new UIPropertyMetadata(OnWinApiMessagesChanged));

    /// <summary>
    ///     Defines the WinApiCommand attached dependency property.
    /// </summary>
    public static readonly DependencyProperty WinApiCommandProperty =
        DependencyProperty.RegisterAttached("WinApiCommand", typeof(ICommand), typeof(WindowBehavior), new UIPropertyMetadata(OnWinApiCommandChanged));

    /// <summary>
    ///     Defines the WindowActivatedCommand attached dependency property.
    /// </summary>
    public static readonly DependencyProperty WindowActivatedCommandProperty =
        DependencyProperty.RegisterAttached("WindowActivatedCommand", typeof(ICommand), typeof(WindowBehavior), new PropertyMetadata(OnWindowActivatedCommandChanged));

    /// <summary>
    ///     Defines the WindowActivatedCommandParameter attached dependency property.
    /// </summary>
    public static readonly DependencyProperty WindowActivatedCommandParameterProperty =
        DependencyProperty.RegisterAttached("WindowActivatedCommandParameter", typeof(object), typeof(WindowBehavior), new PropertyMetadata(null));

    /// <summary>
    ///     Defines the WindowDeactivatedCommand attached dependency property.
    /// </summary>
    public static readonly DependencyProperty WindowDeactivatedCommandProperty =
        DependencyProperty.RegisterAttached("WindowDeactivatedCommand", typeof(ICommand), typeof(WindowBehavior), new PropertyMetadata(OnWindowDeactivatedCommandChanged));

    /// <summary>
    ///     Defines the WindowDeactivatedCommandParameter attached dependency property.
    /// </summary>
    public static readonly DependencyProperty WindowDeactivatedCommandParameterProperty =
        DependencyProperty.RegisterAttached("WindowDeactivatedCommandParameter", typeof(object), typeof(WindowBehavior), new PropertyMetadata(null));

    /// <summary>
    ///     Defines the WindowStateChangedCommand attached dependency property.
    /// </summary>
    public static readonly DependencyProperty WindowStateChangedCommandProperty =
        DependencyProperty.RegisterAttached("WindowStateChangedCommand", typeof(ICommand), typeof(WindowBehavior), new PropertyMetadata(OnWindowStateChangedCommandChanged));

    /// <summary>
    ///     Defines the Theme attached dependency property.
    /// </summary>
    public static readonly DependencyProperty ThemeProperty =
        DependencyProperty.RegisterAttached("Theme", typeof(WindowTheme), typeof(WindowBehavior), new PropertyMetadata(OnThemeChanged));

    private static readonly DependencyProperty ObserverProperty =
        DependencyProperty.RegisterAttached("Observer", typeof(WindowObserver), typeof(WindowBehavior), new UIPropertyMetadata(null));

    /// <summary>
    ///     Gets the dialog result from a button to be called on the owner window.
    /// </summary>
    /// <param name="obj">The element from which the property value is read.</param>
    /// <returns>The WindowBehavior.DialogResult property value for the element.</returns>
    public static bool? GetDialogResult(DependencyObject obj)
    {
        return (bool?)obj.GetValue(DialogResultProperty);
    }

    /// <summary>
    ///     Attaches the dialog result to a button to be called on the owner window.
    /// </summary>
    /// <param name="obj">The element to which the attached property is written.</param>
    /// <param name="value">The needed WindowBehavior.DialogResult value.</param>
    public static void SetDialogResult(DependencyObject obj, bool? value)
    {
        obj.SetValue(DialogResultProperty, value);
    }

    /// <summary>
    ///     Gets the dialog result command from a button to get the dialog result called on the owner window.
    /// </summary>
    /// <param name="obj">The element from which the property value is read.</param>
    /// <returns>The WindowBehavior.DialogResultCommand property value for the element.</returns>
    public static ICommand GetDialogResultCommand(DependencyObject obj)
    {
        return (ICommand)obj.GetValue(DialogResultCommandProperty);
    }

    /// <summary>
    ///     Attaches the dialog result command to a button to get the dialog result called on the owner window.
    /// </summary>
    /// <param name="obj">The element to which the attached property is written.</param>
    /// <param name="value">The needed WindowBehavior.DialogResultCommand value.</param>
    public static void SetDialogResultCommand(DependencyObject obj, ICommand value)
    {
        obj.SetValue(DialogResultCommandProperty, value);
    }

    /// <summary>
    ///     Gets the dialog result command parameter passed with the WindowDialogResultArgs.
    /// </summary>
    /// <param name="obj">The element from which the property value is read.</param>
    /// <returns>The WindowBehavior.DialogResultCommandParameter property value for the element.</returns>
    public static object GetDialogResultCommandParameter(DependencyObject obj)
    {
        return obj.GetValue(DialogResultCommandParameterProperty);
    }

    /// <summary>
    ///     Attaches the dialog result command parameter to be passed with the WindowDialogResultArgs.
    /// </summary>
    /// <param name="obj">The element to which the attached property is written.</param>
    /// <param name="value">The needed WindowBehavior.DialogResultCommandParameter value.</param>
    public static void SetDialogResultCommandParameter(DependencyObject obj, object value)
    {
        obj.SetValue(DialogResultCommandParameterProperty, value);
    }

    private static void OnDialogResultChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
        if (!(sender is ButtonBase button))
            throw new InvalidOperationException("'WindowBehavior.DialogResultCommand' only can be attached to a 'ButtonBase' object");

        if (e.OldValue != null)
            button.Click -= DialogResultButton_Click;
        if (e.NewValue != null)
            button.Click += DialogResultButton_Click;
    }

    private static void DialogResultButton_Click(object sender, RoutedEventArgs e)
    {
        var button = (Button)sender;
        var window = Window.GetWindow(button);
        if (window == null)
            return;

        var resultCommand = GetDialogResultCommand(button);
        var args = new WindowDialogResultArgs(GetDialogResultCommandParameter(button));
        if (resultCommand != null && resultCommand.CanExecute(args))
        {
            resultCommand.Execute(args);
            if (!args.Cancel)
                window.DialogResult = args.DialogResult;
        }
        else
        {
            window.DialogResult = GetDialogResult(button);
        }
    }

    /// <summary>
    ///     Gets the command from a window which get called when the window closes. A WindowClosingArgs is passed as a
    ///     parameter to change the dialog result and cancel the close.
    /// </summary>
    /// <param name="obj">The element from which the property value is read.</param>
    /// <returns>The WindowBehavior.ClosingCommand property value for the element.</returns>
    public static ICommand GetClosingCommand(DependencyObject obj)
    {
        return (ICommand)obj.GetValue(ClosingCommandProperty);
    }

    /// <summary>
    ///     Attaches the command to a window which get called when the window closes. A WindowClosingArgs is passed as a
    ///     parameter to change the dialog result and cancel the close.
    /// </summary>
    /// <param name="obj">The element to which the attached property is written.</param>
    /// <param name="value">The needed WindowBehavior.ClosingCommand value.</param>
    public static void SetClosingCommand(DependencyObject obj, ICommand value)
    {
        obj.SetValue(ClosingCommandProperty, value);
    }

    /// <summary>
    ///     Gets the closing command parameter passed with the WindowClosingArgs.
    /// </summary>
    /// <param name="obj">The element from which the property value is read.</param>
    /// <returns>The WindowBehavior.ClosingCommandParameter property value for the element.</returns>
    public static object GetClosingCommandParameter(DependencyObject obj)
    {
        return obj.GetValue(ClosingCommandParameterProperty);
    }

    /// <summary>
    ///     Attaches the closing command parameter to be passed with the WindowClosingArgs.
    /// </summary>
    /// <param name="obj">The element to which the attached property is written.</param>
    /// <param name="value">The needed WindowBehavior.ClosingCommandParameter value.</param>
    public static void SetClosingCommandParameter(DependencyObject obj, object value)
    {
        obj.SetValue(ClosingCommandParameterProperty, value);
    }

    private static void OnClosingCommandChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
        if (!(sender is Window window))
            throw new InvalidOperationException("'WindowBehavior.ClosingCommand' only can be attached to a 'Window' object");

        if (e.OldValue != null)
            window.Closing -= Window_Closing;
        if (e.NewValue != null)
            window.Closing += Window_Closing;
    }

    private static void Window_Closing(object sender, CancelEventArgs e)
    {
        var window = (DependencyObject)sender;
        var command = GetClosingCommand(window);
        var args = new WindowClosingArgs(GetClosingCommandParameter(window));
        if (command != null && command.CanExecute(args))
        {
            command.Execute(args);
            e.Cancel = args.Cancel;
        }
    }

    /// <summary>
    ///     Gets the command from a window which get called when the window has been closed.
    /// </summary>
    /// <param name="obj">The element from which the property value is read.</param>
    /// <returns>The WindowBehavior.ClosedCommand property value for the element.</returns>
    public static ICommand GetClosedCommand(DependencyObject obj)
    {
        return (ICommand)obj.GetValue(ClosedCommandProperty);
    }

    /// <summary>
    ///     Attaches the command to a window which get called when the window closes.
    /// </summary>
    /// <param name="obj">The element to which the attached property is written.</param>
    /// <param name="value">The needed WindowBehavior.ClosingCommand value.</param>
    public static void SetClosedCommand(DependencyObject obj, ICommand value)
    {
        obj.SetValue(ClosedCommandProperty, value);
    }

    /// <summary>
    ///     Gets the closed command parameter passed with the ClosedCommand.
    /// </summary>
    /// <param name="obj">The element from which the property value is read.</param>
    /// <returns>The WindowBehavior.ClosedCommandParameter property value for the element.</returns>
    public static object GetClosedCommandParameter(DependencyObject obj)
    {
        return obj.GetValue(ClosedCommandParameterProperty);
    }

    /// <summary>
    ///     Attaches the closed command parameter to be passed with the ClosedCommand.
    /// </summary>
    /// <param name="obj">The element to which the attached property is written.</param>
    /// <param name="value">The needed WindowBehavior.ClosedCommandParameter value.</param>
    public static void SetClosedCommandParameter(DependencyObject obj, object value)
    {
        obj.SetValue(ClosedCommandParameterProperty, value);
    }

    private static void OnClosedCommandChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
        if (!(sender is Window window))
            throw new InvalidOperationException("'WindowBehavior.ClosedCommand' only can be attached to a 'Window' object");

        if (e.OldValue != null)
            window.Closed -= Window_Closed;
        if (e.NewValue != null)
            window.Closed += Window_Closed;
    }

    private static void Window_Closed(object sender, EventArgs e)
    {
        var window = (DependencyObject)sender;
        var command = GetClosedCommand(window);
        var parameter = GetClosedCommandParameter(window);
        if (command != null && command.CanExecute(parameter))
            command.Execute(parameter);
    }

    /// <summary>
    ///     Gets a value from a button that indicates that the window have to be closed when the button is pressed without
    ///     using the dialog result.
    /// </summary>
    /// <param name="obj">The element from which the property value is read.</param>
    /// <returns>The WindowBehavior.IsClose property value for the element.</returns>
    public static bool GetIsClose(DependencyObject obj)
    {
        return (bool)obj.GetValue(IsCloseProperty);
    }

    /// <summary>
    ///     Attaches a value from a button that indicates that the window have to be closed when the button is pressed without
    ///     using the dialog result.
    /// </summary>
    /// <param name="obj">The element to which the attached property is written.</param>
    /// <param name="value">The needed WindowBehavior.IsClose value.</param>
    public static void SetIsClose(DependencyObject obj, bool value)
    {
        obj.SetValue(IsCloseProperty, value);
    }

    /// <summary>
    ///     Gets the IsClose command from a button to close the owner window.
    /// </summary>
    /// <param name="obj">The element from which the property value is read.</param>
    /// <returns>The WindowBehavior.IsCloseCommand property value for the element.</returns>
    public static ICommand GetIsCloseCommand(DependencyObject obj)
    {
        return (ICommand)obj.GetValue(IsCloseCommandProperty);
    }

    /// <summary>
    ///     Attaches the IsClose command to a button to close the owner window.
    /// </summary>
    /// <param name="obj">The element to which the attached property is written.</param>
    /// <param name="value">The needed WindowBehavior.IsCloseCommand value.</param>
    public static void SetIsCloseCommand(DependencyObject obj, ICommand value)
    {
        obj.SetValue(IsCloseCommandProperty, value);
    }

    private static void OnIsCloseCommandChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
        if (!(sender is ButtonBase button))
            throw new InvalidOperationException("'WindowBehavior.IsCloseCommand' only can be attached to a 'ButtonBase' object");

        if (e.OldValue != null)
            button.Click -= IsCloseButton_Click;
        if (e.NewValue != null)
            button.Click += IsCloseButton_Click;
    }

    /// <summary>
    ///     Gets the IsClose command parameter passed with the IsCloseCommand.
    /// </summary>
    /// <param name="obj">The element from which the property value is read.</param>
    /// <returns>The WindowBehavior.IsCloseCommandParameter property value for the element.</returns>
    public static object GetIsCloseCommandParameter(DependencyObject obj)
    {
        return obj.GetValue(IsCloseCommandParameterProperty);
    }

    /// <summary>
    ///     Attaches the IsClose command parameter to be passed with the IsCloseCommand.
    /// </summary>
    /// <param name="obj">The element to which the attached property is written.</param>
    /// <param name="value">The needed WindowBehavior.IsCloseCommandParameter value.</param>
    public static void SetIsCloseCommandParameter(DependencyObject obj, object value)
    {
        obj.SetValue(IsCloseCommandParameterProperty, value);
    }

    private static void IsCloseButton_Click(object sender, RoutedEventArgs e)
    {
        var button = (Button)sender;
        var window = Window.GetWindow(button);
        if (window == null)
            return;

        var resultCommand = GetIsCloseCommand(button);
        var args = new WindowIsCloseArgs(GetIsCloseCommandParameter(button));
        if (resultCommand != null && resultCommand.CanExecute(args))
        {
            resultCommand.Execute(args);
            if (!args.Cancel)
                window.Close();
        }
        else
        {
            if (GetIsClose(button))
                window.Close();
        }
    }

    /// <summary>
    ///     Gets a list of hex values of a WinAPI messages to listen and forwarded to the WindowBehavior.WinApiCommand.
    /// </summary>
    /// <param name="obj">The element from which the property value is read.</param>
    /// <returns>The WindowBehavior.WinApiMessages property value for the element.</returns>
    public static string GetWinApiMessages(DependencyObject obj)
    {
        return (string)obj.GetValue(WinApiMessagesProperty);
    }

    /// <summary>
    ///     Attaches a list of hex values of a WinAPI messages to listen and forwarded to the WindowBehavior.WinApiCommand.
    /// </summary>
    /// <param name="obj">The element to which the attached property is written.</param>
    /// <param name="value">The needed WindowBehavior.WinApiMessages value.</param>
    public static void SetWinApiMessages(DependencyObject obj, string value)
    {
        obj.SetValue(WinApiMessagesProperty, value);
    }

    private static void OnWinApiMessagesChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
        var messages = e.NewValue as string;
        if (string.IsNullOrWhiteSpace(messages))
            return;

        var observer = GetOrCreateObserver(sender);
        if (observer == null)
            return;

        observer.ClearCallbacks();

        if (messages.ToLower().Trim() == "all")
            observer.AddCallback(EventNotified);
        else
            foreach (var id in StringToIntegerList(messages))
                observer.AddCallbackFor(id, EventNotified);
    }

    private static void EventNotified(NotifyEventArgs e)
    {
        var command = GetWinApiCommand(e.ObservedWindow);
        if (command != null && command.CanExecute(e))
            command.Execute(e);
    }

    private static IEnumerable<int> StringToIntegerList(string messages)
    {
        var idTexts = messages.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
        var ids = new List<int>();
        foreach (var idText in idTexts)
            try
            {
                ids.Add(int.TryParse(idText, NumberStyles.HexNumber, new CultureInfo(1033), out var id) ? id : Convert.ToInt32(idText, 16));
            }
            catch
            {
                throw new ArgumentException("The attached WinApiMessages cannot be parsed to a list of integers. Supported are just integer numbers separated by a semicolon, e.g. '3;42' or hex values (base of 16) like '0x03;0x2A'.");
            }

        return ids;
    }

    /// <summary>
    ///     Gets a command which get called if one of the message attached by the WindowBehavior.WinApiMessages occurs.
    /// </summary>
    /// <param name="obj">The element from which the property value is read.</param>
    /// <returns>The WindowBehavior.WinApiCommand property value for the element.</returns>
    public static ICommand GetWinApiCommand(DependencyObject obj)
    {
        return (ICommand)obj.GetValue(WinApiCommandProperty);
    }

    /// <summary>
    ///     Attaches a command which get called if one of the message attached by the WindowBehavior.WinApiMessages occurs.
    /// </summary>
    /// <param name="obj">The element to which the attached property is written.</param>
    /// <param name="value">The needed WindowBehavior.WinApiCommand value.</param>
    public static void SetWinApiCommand(DependencyObject obj, ICommand value)
    {
        obj.SetValue(WinApiCommandProperty, value);
    }

    private static void OnWinApiCommandChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
        if (e.NewValue != null)
            GetOrCreateObserver(sender);
    }

    /// <summary>
    ///     Gets the command to be called if the window got activated.
    /// </summary>
    /// <param name="obj">The window from which the property value is read.</param>
    /// <returns>The command.</returns>
    public static ICommand GetWindowActivatedCommand(DependencyObject obj)
    {
        return (ICommand)obj.GetValue(WindowActivatedCommandProperty);
    }

    /// <summary>
    ///     Sets the command to be called if the window got activated.
    /// </summary>
    /// <param name="obj">The element to which the attached property is written.</param>
    /// <param name="value">The command to attach.</param>
    public static void SetWindowActivatedCommand(DependencyObject obj, ICommand value)
    {
        obj.SetValue(WindowActivatedCommandProperty, value);
    }

    /// <summary>
    ///     Gets the command parameter for the WindowActivatedCommand.
    /// </summary>
    /// <param name="obj">The window from which the property value is read.</param>
    /// <returns>The command parameter.</returns>
    public static object GetWindowActivatedCommandParameter(DependencyObject obj)
    {
        return obj.GetValue(WindowActivatedCommandParameterProperty);
    }

    /// <summary>
    ///     Sets the command parameter for the WindowActivatedCommand.
    /// </summary>
    /// <param name="obj">The element to which the attached property is written.</param>
    /// <param name="value">The parameter to attach.</param>
    public static void SetWindowActivatedCommandParameter(DependencyObject obj, object value)
    {
        obj.SetValue(WindowActivatedCommandParameterProperty, value);
    }

    private static void OnWindowActivatedCommandChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
        if (!(sender is Window window))
            throw new InvalidOperationException("'WindowBehavior.WindowActivatedCommand' only can be attached to a 'Window' object");

        if (e.OldValue != null)
            window.Activated -= OnWindowActivated;
        if (e.NewValue != null)
            window.Activated += OnWindowActivated;
    }

    private static void OnWindowActivated(object sender, EventArgs e)
    {
        var dependencyObject = (DependencyObject)sender;
        var command = GetWindowActivatedCommand(dependencyObject);
        var parameter = GetWindowActivatedCommandParameter(dependencyObject);
        if (command != null && command.CanExecute(parameter))
            command.Execute(parameter);
    }

    /// <summary>
    ///     Gets the command to be called if the window got deactivated.
    /// </summary>
    /// <param name="obj">The window from which the property value is read.</param>
    /// <returns>The command.</returns>
    public static ICommand GetWindowDeactivatedCommand(DependencyObject obj)
    {
        return (ICommand)obj.GetValue(WindowDeactivatedCommandProperty);
    }

    /// <summary>
    ///     Sets the command to be called if the window got deactivated.
    /// </summary>
    /// <param name="obj">The element to which the attached property is written.</param>
    /// <param name="value">The command to attach.</param>
    public static void SetWindowDeactivatedCommand(DependencyObject obj, ICommand value)
    {
        obj.SetValue(WindowDeactivatedCommandProperty, value);
    }

    /// <summary>
    ///     Gets the parameter for the WindowDeactivatedCommand.
    /// </summary>
    /// <param name="obj">The window from which the property value is read.</param>
    /// <returns>The command parameter.</returns>
    public static object GetWindowDeactivatedCommandParameter(DependencyObject obj)
    {
        return obj.GetValue(WindowDeactivatedCommandParameterProperty);
    }

    /// <summary>
    ///     Sets the parameter for the WindowDeactivatedCommand.
    /// </summary>
    /// <param name="obj">The element to which the attached property is written.</param>
    /// <param name="value">The parameter to attach.</param>
    public static void SetWindowDeactivatedCommandParameter(DependencyObject obj, object value)
    {
        obj.SetValue(WindowDeactivatedCommandParameterProperty, value);
    }

    private static void OnWindowDeactivatedCommandChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
        if (!(sender is Window window))
            throw new InvalidOperationException("'WindowBehavior.WindowDeactivatedCommand' only can be attached to a 'Window' object");

        if (e.OldValue != null)
            window.Deactivated -= OnWindowDeactivated;
        if (e.NewValue != null)
            window.Deactivated += OnWindowDeactivated;
    }

    private static void OnWindowDeactivated(object sender, EventArgs e)
    {
        var dependencyObject = (DependencyObject)sender;
        var command = GetWindowDeactivatedCommand(dependencyObject);
        var parameter = GetWindowDeactivatedCommandParameter(dependencyObject);
        if (command != null && command.CanExecute(parameter))
            command.Execute(parameter);
    }

    /// <summary>
    ///     Gets the command to ne called if the window changes its state.
    /// </summary>
    /// <param name="obj">The window from which the property value is read.</param>
    /// <returns>The command.</returns>
    public static ICommand GetWindowStateChangedCommand(DependencyObject obj)
    {
        return (ICommand)obj.GetValue(WindowStateChangedCommandProperty);
    }

    /// <summary>
    ///     Sets the command to ne called if the window changes its state.
    /// </summary>
    /// <param name="obj">The element to which the attached property is written.</param>
    /// <param name="value">The command to attach.</param>
    public static void SetWindowStateChangedCommand(DependencyObject obj, ICommand value)
    {
        obj.SetValue(WindowStateChangedCommandProperty, value);
    }

    /// <summary>
    ///     Gets the theme set on the window.
    /// </summary>
    /// <param name="obj">The window from which the property value is read.</param>
    /// <returns>The command.</returns>
    public static WindowTheme GetTheme(DependencyObject obj)
    {
        return (WindowTheme)obj.GetValue(ThemeProperty);
    }

    /// <summary>
    ///     Sets the theme to use for the window.
    /// </summary>
    /// <param name="obj">The element to which the attached property is written.</param>
    /// <param name="value">The theme to attach.</param>
    public static void SetTheme(DependencyObject obj, WindowTheme value)
    {
        obj.SetValue(ThemeProperty, value);
    }

    private static void OnWindowStateChangedCommandChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
        if (!(sender is Window window))
            throw new InvalidOperationException("'WindowBehavior.WindowStateChangedCommand' can be attached to a 'Window' object only");

        if (e.OldValue != null)
            window.StateChanged -= OnWindowStateChanged;
        if (e.NewValue != null)
            window.StateChanged += OnWindowStateChanged;
    }

    private static void OnWindowStateChanged(object sender, EventArgs e)
    {
        var window = (Window)sender;
        var command = GetWindowStateChangedCommand(window);
        var parameter = window.WindowState;
        if (command != null && command.CanExecute(parameter))
            command.Execute(parameter);
    }

    private static void OnThemeChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
        if (!(sender is Window window))
            throw new InvalidOperationException("'WindowBehavior.Theme' can be attached to a 'Window' object only");

        var theme = (WindowTheme)e.NewValue;
        var result = ThemeManager.SetWindowTheme(window, theme);
        if (!result)
        {
            void SourceInitialized2(object o, EventArgs _)
            {
                window.SourceInitialized -= SourceInitialized2;
                ThemeManager.SetWindowTheme(window, theme);
            }

            window.SourceInitialized += SourceInitialized2;
        }
    }

    private static WindowObserver GetOrCreateObserver(DependencyObject sender)
    {
        var observer = GetObserver(sender);
        if (observer == null)
            if (sender is Window window)
            {
                observer = new WindowObserver(window);
                SetObserver(sender, observer);
            }

        return observer;
    }

    private static WindowObserver GetObserver(DependencyObject obj)
    {
        return (WindowObserver)obj.GetValue(ObserverProperty);
    }

    private static void SetObserver(DependencyObject obj, WindowObserver value)
    {
        obj.SetValue(ObserverProperty, value);
    }
}