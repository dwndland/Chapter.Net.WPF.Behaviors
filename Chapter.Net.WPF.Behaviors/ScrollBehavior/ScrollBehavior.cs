// -----------------------------------------------------------------------------------------------------------------
// <copyright file="ScrollBehavior.cs" company="my-libraries">
//     Copyright (c) David Wendland. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

// ReSharper disable CheckNamespace

namespace Chapter.Net.WPF.Behaviors
{
    /// <summary>
    ///     Brings the feature to modify the scroll position of an items control.
    /// </summary>
    public sealed class ScrollBehavior
    {
        /// <summary>
        ///     Defines the ScrollToItem attached dependency property.
        /// </summary>
        public static readonly DependencyProperty ScrollToItemProperty =
            DependencyProperty.RegisterAttached("ScrollToItem", typeof(object), typeof(ScrollBehavior), new UIPropertyMetadata(OnScrollChanged));

        /// <summary>
        ///     Defines the AutoScrollToLast attached dependency property.
        /// </summary>
        public static readonly DependencyProperty AutoScrollToLastProperty =
            DependencyProperty.RegisterAttached("AutoScrollToLast", typeof(bool), typeof(ScrollBehavior), new UIPropertyMetadata(OnScrollChanged));

        /// <summary>
        ///     Defines the AutoScrollToSelected attached dependency property.
        /// </summary>
        public static readonly DependencyProperty AutoScrollToSelectedProperty =
            DependencyProperty.RegisterAttached("AutoScrollToSelected", typeof(bool), typeof(ScrollBehavior), new UIPropertyMetadata(OnScrollChanged));

        /// <summary>
        ///     Defines the Disable attached dependency property.
        /// </summary>
        public static readonly DependencyProperty DisableProperty =
            DependencyProperty.RegisterAttached("Disable", typeof(bool), typeof(ScrollBehavior), new PropertyMetadata(false, OnDisableChanged));

        private static readonly DependencyProperty ScrollBehaviorProperty =
            DependencyProperty.RegisterAttached("ScrollBehavior", typeof(ScrollBehavior), typeof(ScrollBehavior), new UIPropertyMetadata(null));

        private ScrollBehavior()
        {
        }

        /// <summary>
        ///     Gets the item to which it has to scroll in a list.
        /// </summary>
        /// <param name="obj">The element from which the property value is read.</param>
        /// <returns>The ScrollBehavior.ScrollToItem property value for the element.</returns>
        public static object GetScrollToItem(DependencyObject obj)
        {
            return obj.GetValue(ScrollToItemProperty);
        }

        /// <summary>
        ///     Attaches the item to which it has to scroll in a list.
        /// </summary>
        /// <param name="obj">The element to which the attached property is written.</param>
        /// <param name="value">The needed ScrollBehavior.ScrollToItem value.</param>
        public static void SetScrollToItem(DependencyObject obj, object value)
        {
            obj.SetValue(ScrollToItemProperty, value);
        }

        /// <summary>
        ///     Gets a value that indicates if a list automatically have to scroll to the last item if the item collection changes.
        /// </summary>
        /// <param name="obj">The element from which the property value is read.</param>
        /// <returns>The ScrollBehavior.AutoScrollToLast property value for the element.</returns>
        public static bool GetAutoScrollToLast(DependencyObject obj)
        {
            return (bool)obj.GetValue(AutoScrollToLastProperty);
        }

        /// <summary>
        ///     Attaches a value that indicates if a list automatically have to scroll to the last item if the item collection
        ///     changes.
        /// </summary>
        /// <param name="obj">The element to which the attached property is written.</param>
        /// <param name="value">The needed ScrollBehavior.AutoScrollToLast value.</param>
        public static void SetAutoScrollToLast(DependencyObject obj, bool value)
        {
            obj.SetValue(AutoScrollToLastProperty, value);
        }

        /// <summary>
        ///     Gets a value that indicates if a list automatically have to scroll to the selected item if the selection has been
        ///     changed.
        /// </summary>
        /// <param name="obj">The element from which the property value is read.</param>
        /// <returns>The ScrollBehavior.AutoScrollToSelected property value for the element.</returns>
        public static bool GetAutoScrollToSelected(DependencyObject obj)
        {
            return (bool)obj.GetValue(AutoScrollToSelectedProperty);
        }

        /// <summary>
        ///     Attaches a value that indicates if a list automatically have to scroll to the selected item if the selection has
        ///     been changed.
        /// </summary>
        /// <param name="obj">The element to which the attached property is written.</param>
        /// <param name="value">The needed ScrollBehavior.AutoScrollToSelected value.</param>
        public static void SetAutoScrollToSelected(DependencyObject obj, bool value)
        {
            obj.SetValue(AutoScrollToSelectedProperty, value);
        }

        /// <summary>
        ///   Gets the indicator if mouse wheel or touch swipe shall be disabled or not.
        /// </summary>
        /// <param name="obj">The element from which the property value is read.</param>
        /// <returns>The indicator if mouse wheel or touch swipe shall be disabled or not.</returns>
        public static bool GetDisable(DependencyObject obj)
        {
            return (bool)obj.GetValue(DisableProperty);
        }

        /// <summary>
        ///   Sets the indicator if mouse wheel or touch swipe shall be disabled or not.
        /// </summary>
        /// <param name="obj">The element to which the attached property is written.</param>
        /// <param name="value">The indicator if mouse wheel or touch swipe shall be disabled or not.</param>
        public static void SetDisable(DependencyObject obj, bool value)
        {
            obj.SetValue(DisableProperty, value);
        }

        private static ScrollBehavior GetScrollBehavior(DependencyObject obj)
        {
            return (ScrollBehavior)obj.GetValue(ScrollBehaviorProperty);
        }

        private static void SetScrollBehavior(DependencyObject obj, ScrollBehavior value)
        {
            obj.SetValue(ScrollBehaviorProperty, value);
        }

        private static void OnScrollChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (!(sender is ListBox) && !(sender is DataGrid))
                throw new InvalidOperationException("The ScrollBehavior.ScrollToItem only can be attached to an ListBox, ListView or DataGrid.");

            var scrollBehavior = GetScrollBehavior(sender);
            if (scrollBehavior == null)
            {
                scrollBehavior = new ScrollBehavior();
                SetScrollBehavior(sender, scrollBehavior);
                ((FrameworkElement)sender).Loaded += scrollBehavior.Element_Loaded;
            }

            scrollBehavior.Element_Loaded(sender, null);
        }

        private void Element_Loaded(object sender, RoutedEventArgs _)
        {
            var depObj = (DependencyObject)sender;
            var autoSelectionScroll = GetAutoScrollToSelected(depObj);
            var autoLastScroll = GetAutoScrollToLast(depObj);
            var scrollToItem = GetScrollToItem(depObj);

            switch (sender)
            {
                case ListBox listbox:
                    Scroll(listbox, scrollToItem, autoLastScroll, autoSelectionScroll);
                    break;
                case DataGrid dataGrid:
                    Scroll(dataGrid, scrollToItem, autoLastScroll, autoSelectionScroll);
                    break;
            }
        }

        private void Scroll(ListBox listbox, object scrollToItem, bool autoLastScroll, bool autoSelectionScroll)
        {
            ScrollToItem(listbox, scrollToItem);
            if (autoLastScroll)
            {
                if (listbox.ItemsSource is INotifyCollectionChanged collection)
                    collection.CollectionChanged += (s, o) => { ScrollToLast(listbox); };
                ScrollToLast(listbox);
            }

            if (autoSelectionScroll)
                listbox.SelectionChanged += ListBox_SelectionChanged;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listbox = (ListBox)sender;
            if (listbox.SelectedItem != null)
                listbox.ScrollIntoView(listbox.SelectedItem);
        }

        private void ScrollToItem(ListBox listBox, object item)
        {
            if (item != null)
                listBox.ScrollIntoView(item);
        }

        private void ScrollToLast(ListBox listBox)
        {
            if (listBox.Items.Count > 0)
                listBox.ScrollIntoView(listBox.Items[listBox.Items.Count - 1]);
        }

        private void Scroll(DataGrid dataGrid, object scrollToItem, bool autoLastScroll, bool autoSelectionScroll)
        {
            ScrollToItem(dataGrid, scrollToItem);
            if (autoLastScroll)
            {
                if (dataGrid.ItemsSource is INotifyCollectionChanged collection)
                    collection.CollectionChanged += (s, o) => { ScrollToLast(dataGrid); };
                ScrollToLast(dataGrid);
            }

            if (autoSelectionScroll)
                dataGrid.SelectionChanged += DataGrid_SelectionChanged;
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var dataGrid = (DataGrid)sender;
            if (dataGrid.SelectedItem != null)
                dataGrid.ScrollIntoView(dataGrid.SelectedItem);
        }

        private void ScrollToItem(DataGrid dataGrid, object item)
        {
            if (item != null)
                dataGrid.ScrollIntoView(item);
        }

        private void ScrollToLast(DataGrid dataGrid)
        {
            if (dataGrid.Items.Count > 0)
                dataGrid.ScrollIntoView(dataGrid.Items[dataGrid.Items.Count - 1]);
        }

        private static void OnDisableChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is ItemsControl control))
                throw new InvalidOperationException("ScrollBehavior.Disable can be attached to ItemsControls only.");

            if ((bool)e.OldValue)
            {
                ScrollViewer.SetPanningMode(control, PanningMode.Both);
                control.PreviewMouseWheel -= ControlOnPreviewMouseWheel;
            }

            if ((bool)e.NewValue)
            {
                ScrollViewer.SetPanningMode(control, PanningMode.None);
                control.PreviewMouseWheel += ControlOnPreviewMouseWheel;
            }
        }

        private static void ControlOnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var parent = VisualTreeHelper.GetParent((DependencyObject)sender);
            if (!(parent is UIElement element))
                return;

            var eventArgs = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta)
            {
                RoutedEvent = UIElement.MouseWheelEvent,
                Source = sender
            };

            element.RaiseEvent(eventArgs);
            e.Handled = true;
        }
    }
}