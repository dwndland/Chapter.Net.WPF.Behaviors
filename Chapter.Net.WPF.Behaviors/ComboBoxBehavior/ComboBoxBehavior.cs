// -----------------------------------------------------------------------------------------------------------------
// <copyright file="ComboBoxBehavior.cs" company="dwndland">
//     Copyright (c) David Wendland. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Controls;

// ReSharper disable once CheckNamespace

namespace Chapter.Net.WPF.Behaviors;

/// <summary>
///     Brings the definition of max text length of the combobox if it is editable.
/// </summary>
public class ComboBoxBehavior
{
    /// <summary>
    ///     Defines the MaxLength attached dependency property.
    /// </summary>
    public static readonly DependencyProperty MaxLengthProperty =
        DependencyProperty.RegisterAttached("MaxLength", typeof(int), typeof(ComboBoxBehavior), new UIPropertyMetadata(OnMaxLengthChanged));

    /// <summary>
    ///     Gets the maximum length.
    /// </summary>
    /// <param name="obj">The object.</param>
    /// <returns>The max length value.</returns>
    public static int GetMaxLength(DependencyObject obj)
    {
        return (int)obj.GetValue(MaxLengthProperty);
    }

    /// <summary>
    ///     Sets the maximum length.
    /// </summary>
    /// <param name="obj">The object.</param>
    /// <param name="value">The value.</param>
    public static void SetMaxLength(DependencyObject obj, int value)
    {
        obj.SetValue(MaxLengthProperty, value);
    }

    private static void OnMaxLengthChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
    {
        if (!(obj is ComboBox comboBox))
            throw new InvalidOperationException("The ComboBoxBehavior.MaxLength only can be attached on a ComboBox control");

        comboBox.Loaded +=
            (s, e) =>
            {
                var textBox = comboBox.Template.FindName("PART_EditableTextBox", comboBox) as TextBox;
                textBox?.SetValue(TextBox.MaxLengthProperty, args.NewValue);
            };
    }
}