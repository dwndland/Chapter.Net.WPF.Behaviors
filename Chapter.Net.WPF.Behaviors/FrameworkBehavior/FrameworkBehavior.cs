// -----------------------------------------------------------------------------------------------------------------
// <copyright file="FrameworkBehavior.cs" company="my-libraries">
//     Copyright (c) David Wendland. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Input;

// ReSharper disable once CheckNamespace

namespace Chapter.Net.WPF.Behaviors
{
    /// <summary>
    ///     Brings commands for actions on FrameworkElements.
    /// </summary>
    public static class FrameworkBehavior
    {
        /// <summary>
        ///     Defines the LoadedCommand attached dependency property.
        /// </summary>
        public static readonly DependencyProperty LoadedCommandProperty =
            DependencyProperty.RegisterAttached("LoadedCommand", typeof(ICommand), typeof(FrameworkBehavior), new PropertyMetadata(OnLoadedCommandChanged));

        /// <summary>
        ///     Defines the LoadedCommandParameter attached dependency property.
        /// </summary>
        public static readonly DependencyProperty LoadedCommandParameterProperty =
            DependencyProperty.RegisterAttached("LoadedCommandParameter", typeof(object), typeof(FrameworkBehavior), new PropertyMetadata(null));

        /// <summary>
        ///     Defines the UnloadedCommand attached dependency property.
        /// </summary>
        public static readonly DependencyProperty UnloadedCommandProperty =
            DependencyProperty.RegisterAttached("UnloadedCommand", typeof(ICommand), typeof(FrameworkBehavior), new PropertyMetadata(OnUnloadedCommandChanged));

        /// <summary>
        ///     Defines the UnloadedCommandParameter attached dependency property.
        /// </summary>
        public static readonly DependencyProperty UnloadedCommandParameterProperty =
            DependencyProperty.RegisterAttached("UnloadedCommandParameter", typeof(object), typeof(FrameworkBehavior), new PropertyMetadata(null));

        /// <summary>
        ///     Gets the command to raise if the attached framework elements gets loaded.
        /// </summary>
        /// <param name="obj">The element from which the property value is read.</param>
        /// <returns>The loaded command.</returns>
        public static ICommand GetLoadedCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(LoadedCommandProperty);
        }

        /// <summary>
        ///     Sets the command to raise if the attached framework elements gets loaded.
        /// </summary>
        /// <param name="obj">The element to which the attached property is written.</param>
        /// <param name="value">The loaded command.</param>
        public static void SetLoadedCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(LoadedCommandProperty, value);
        }

        /// <summary>
        ///     Gets the command parameter which gets raised with the LoadedCommand.
        /// </summary>
        /// <param name="obj">The element from which the property value is read.</param>
        /// <returns>The loaded command parameter.</returns>
        public static object GetLoadedCommandParameter(DependencyObject obj)
        {
            return obj.GetValue(LoadedCommandParameterProperty);
        }

        /// <summary>
        ///     Sets the command parameter to raise with the LoadedCommand.
        /// </summary>
        /// <param name="obj">The element to which the attached property is written.</param>
        /// <param name="value">The loaded command parameter.</param>
        public static void SetLoadedCommandParameter(DependencyObject obj, object value)
        {
            obj.SetValue(LoadedCommandParameterProperty, value);
        }

        /// <summary>
        ///     Gets the command to raise if the attached framework elements gets unloaded.
        /// </summary>
        /// <param name="obj">The element from which the property value is read.</param>
        /// <returns>The unloaded command.</returns>
        public static ICommand GetUnloadedCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(UnloadedCommandProperty);
        }

        /// <summary>
        ///     Sets the command to raise if the attached framework elements gets unloaded.
        /// </summary>
        /// <param name="obj">The element to which the attached property is written.</param>
        /// <param name="value">The unloaded command.</param>
        public static void SetUnloadedCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(UnloadedCommandProperty, value);
        }

        /// <summary>
        ///     Gets the command parameter which gets raised with the UnloadedCommand.
        /// </summary>
        /// <param name="obj">The element from which the property value is read.</param>
        /// <returns>The unloaded command parameter.</returns>
        public static object GetUnloadedCommandParameter(DependencyObject obj)
        {
            return obj.GetValue(UnloadedCommandParameterProperty);
        }

        /// <summary>
        ///     Sets the command parameter to raise with the UnloadedCommand.
        /// </summary>
        /// <param name="obj">The element to which the attached property is written.</param>
        /// <param name="value">The unloaded command parameter.</param>
        public static void SetUnloadedCommandParameter(DependencyObject obj, object value)
        {
            obj.SetValue(UnloadedCommandParameterProperty, value);
        }

        private static void OnLoadedCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is FrameworkElement control))
                throw new InvalidOperationException("The FrameworkBehavior.LoadedCommand can be attached to FrameworkElements only.");

            if (e.OldValue != null)
                control.Loaded -= OnLoaded;
            if (e.NewValue != null)
                control.Loaded += OnLoaded;
        }

        private static void OnUnloadedCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is FrameworkElement control))
                throw new InvalidOperationException("The FrameworkBehavior.UnloadedCommand can be attached to FrameworkElements only.");

            if (e.OldValue != null)
                control.Unloaded -= OnUnloaded;
            if (e.NewValue != null)
                control.Unloaded += OnUnloaded;
        }

        private static void OnLoaded(object sender, RoutedEventArgs e)
        {
            var element = (DependencyObject)sender;
            var command = GetLoadedCommand(element);
            var commandParameter = GetLoadedCommandParameter(element);
            if (command != null && command.CanExecute(commandParameter))
                command.Execute(commandParameter);
        }

        private static void OnUnloaded(object sender, RoutedEventArgs e)
        {
            var element = (DependencyObject)sender;
            var command = GetUnloadedCommand(element);
            var commandParameter = GetUnloadedCommandParameter(element);
            if (command != null && command.CanExecute(commandParameter))
                command.Execute(commandParameter);
        }
    }
}