// -----------------------------------------------------------------------------------------------------------------
// <copyright file="ColumnVisibilityBehavior.cs" company="dwndland">
//     Copyright (c) David Wendland. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;

// ReSharper disable CheckNamespace

namespace Chapter.Net.WPF.Behaviors;

/// <summary>
///     Brings the feature to controls with a <see cref="System.Windows.Controls.GridViewColumnHeader" /> to show or hide
///     columns dynamically.
/// </summary>
public sealed class ColumnVisibilityBehavior
{
    /// <summary>
    ///     Defines the VisibleColumns attached dependency property.
    /// </summary>
    public static readonly DependencyProperty VisibleColumnsProperty =
        DependencyProperty.RegisterAttached("VisibleColumns", typeof(IList), typeof(ColumnVisibilityBehavior), new UIPropertyMetadata(OnVisibleColumnsChanged));

    /// <summary>
    ///     Defines the Name attached dependency property.
    /// </summary>
    public static readonly DependencyProperty NameProperty =
        DependencyProperty.RegisterAttached("Name", typeof(object), typeof(ColumnVisibilityBehavior), new UIPropertyMetadata(null));

    /// <summary>
    ///     Defines the Position attached dependency property.
    /// </summary>
    private static readonly DependencyProperty PositionProperty =
        DependencyProperty.RegisterAttached("Position", typeof(int), typeof(ColumnVisibilityBehavior), new UIPropertyMetadata(0));

    private static readonly DependencyProperty ColumnVisibilityBehaviorProperty =
        DependencyProperty.RegisterAttached("ColumnVisibilityBehavior", typeof(ColumnVisibilityBehavior), typeof(ColumnVisibilityBehavior), new UIPropertyMetadata(null));

    private readonly List<GridViewColumn> _filteredColumns;
    private GridViewColumnCollection _columns;
    private bool _isCatchedAlready;
    private DependencyObject _owner;

    private ColumnVisibilityBehavior()
    {
        _filteredColumns = new List<GridViewColumn>();
    }

    /// <summary>
    ///     Gets a list of visible columns by their name.
    /// </summary>
    /// <param name="obj">The element from which the property value is read.</param>
    /// <returns>The ColumnVisibilityBehavior.VisibleColumns property value for the element.</returns>
    public static IList GetVisibleColumns(DependencyObject obj)
    {
        return (IList)obj.GetValue(VisibleColumnsProperty);
    }

    /// <summary>
    ///     Attaches a list of visible columns by their name.
    /// </summary>
    /// <param name="obj">The element to which the attached property is written.</param>
    /// <param name="value">The needed ColumnVisibilityBehavior.VisibleColumns value.</param>
    public static void SetVisibleColumns(DependencyObject obj, IList value)
    {
        obj.SetValue(VisibleColumnsProperty, value);
    }

    private static void OnVisibleColumnsChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
        if (!(sender is ListView element))
            throw new InvalidOperationException("The ColumnVisibilityBehavior can be attached to a ListView only.");

        if (e.OldValue != null)
            element.Loaded -= Eement_Loaded;
        if (e.NewValue != null)
            element.Loaded += Eement_Loaded;
    }

    /// <summary>
    ///     Gets the name of the element.
    /// </summary>
    /// <param name="obj">The element from which the property value is read.</param>
    /// <returns>The ColumnVisibilityBehavior.Name property value for the element.</returns>
    public static object GetName(DependencyObject obj)
    {
        return obj.GetValue(NameProperty);
    }

    /// <summary>
    ///     Attaches the name.
    /// </summary>
    /// <param name="obj">The element to which the attached property is written.</param>
    /// <param name="value">The needed ColumnVisibilityBehavior.Name value.</param>
    public static void SetName(DependencyObject obj, object value)
    {
        obj.SetValue(NameProperty, value);
    }

    private static ColumnVisibilityBehavior GetColumnVisibilityBehavior(DependencyObject obj)
    {
        return (ColumnVisibilityBehavior)obj.GetValue(ColumnVisibilityBehaviorProperty);
    }

    private static void SetColumnVisibilityBehavior(DependencyObject obj, ColumnVisibilityBehavior value)
    {
        obj.SetValue(ColumnVisibilityBehaviorProperty, value);
    }

    private static ColumnVisibilityBehavior GetOrSetBehavior(DependencyObject sender)
    {
        var behavior = GetColumnVisibilityBehavior(sender);
        if (behavior != null)
            return behavior;

        behavior = new ColumnVisibilityBehavior();
        SetColumnVisibilityBehavior(sender, behavior);
        return behavior;
    }

    private static int GetPosition(DependencyObject obj)
    {
        return (int)obj.GetValue(PositionProperty);
    }

    private static void SetPosition(DependencyObject obj, int value)
    {
        obj.SetValue(PositionProperty, value);
    }

    private static void Eement_Loaded(object sender, RoutedEventArgs e)
    {
        var element = (DependencyObject)sender;
        var behavior = GetOrSetBehavior(element);
        if (behavior._isCatchedAlready)
            return;

        var presenter = VisualTreeAssist.FindChild<GridViewHeaderRowPresenter>(element);
        if (presenter != null)
            behavior.Handle(element, presenter.Columns);
    }

    private void Handle(DependencyObject sender, GridViewColumnCollection columns)
    {
        _isCatchedAlready = true;

        _owner = sender;
        _columns = columns;
        NumerizeColumns();

        var visibleColumns = GetVisibleColumns(sender);
        if (visibleColumns is INotifyCollectionChanged notifyCollectionChanged)
            notifyCollectionChanged.CollectionChanged += (a, b) => Refresh();

        Refresh();
    }

    private void NumerizeColumns()
    {
        for (var i = 0; i < _columns.Count; ++i)
            SetPosition(_columns[i], i);
    }

    private void Refresh()
    {
        ResetOldFiltered();
        FilterOut();
    }

    private void ResetOldFiltered()
    {
        while (_columns.Count > 0)
        {
            _filteredColumns.Add(_columns[0]);
            _columns.RemoveAt(0);
        }

        _filteredColumns.Sort(SortByPosition);

        foreach (var column in _filteredColumns)
            _columns.Add(column);
        _filteredColumns.Clear();
    }

    private static int SortByPosition(GridViewColumn first, GridViewColumn second)
    {
        return GetPosition(first).CompareTo(GetPosition(second));
    }

    private void FilterOut()
    {
        var visibleColumns = GetVisibleColumns(_owner);
        for (var i = 0; i < _columns.Count; ++i)
        {
            var name = GetName(_columns[i]);
            if (name == null)
                continue;

            if (visibleColumns.Contains(name))
                continue;

            _filteredColumns.Add(_columns[i]);
            _columns.RemoveAt(i);
            --i;
        }
    }
}