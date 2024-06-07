// -----------------------------------------------------------------------------------------------------------------
// <copyright file="ColumnResizeKind.cs" company="my-libraries">
//     Copyright (c) David Wendland. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------------------------------------------------

// ReSharper disable CheckNamespace

namespace Chapter.Net.WPF.Behaviors;

/// <summary>
///     Defines how the columns of a list view shall be resized by the <see cref="ColumnWidthBehavior" />.
/// </summary>
public enum ColumnResizeKind
{
    /// <summary>
    ///     The column widths stays unchanged.
    /// </summary>
    NoResize = -1,

    /// <summary>
    ///     The column widths are defined by the owner control width.
    /// </summary>
    ByControl = -2,

    /// <summary>
    ///     The column widths are defined by their shown content.
    /// </summary>
    ByContent = -3,

    /// <summary>
    ///     The column widths are calculated proportional by the owner control width.
    /// </summary>
    Proportional = -4
}