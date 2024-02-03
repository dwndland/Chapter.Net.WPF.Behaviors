// -----------------------------------------------------------------------------------------------------------------
// <copyright file="TextBoxBehavior.cs" company="my-libraries">
//     Copyright (c) David Wendland. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

// ReSharper disable CheckNamespace

namespace Chapter.Net.WPF.Behaviors;

/// <summary>
///     Brings the features to text boxes to define its selection or bound the selection part.
/// </summary>
public sealed class TextBoxBehavior
{
    /// <summary>
    ///     Defines the SelectedText attached dependency property.
    /// </summary>
    public static readonly DependencyProperty SelectedTextProperty =
        DependencyProperty.RegisterAttached("SelectedText", typeof(string), typeof(TextBoxBehavior), new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedTextChanged));

    /// <summary>
    ///     Defines the SelectAllOnFocus attached dependency property.
    /// </summary>
    public static readonly DependencyProperty SelectAllOnFocusProperty =
        DependencyProperty.RegisterAttached("SelectAllOnFocus", typeof(bool), typeof(TextBoxBehavior), new UIPropertyMetadata(OnSelectAllOnFocusChanged));

    /// <summary>
    ///     Defines the RefreshBindingOnKey attached dependency property.
    /// </summary>
    public static readonly DependencyProperty RefreshBindingOnKeyProperty =
        DependencyProperty.RegisterAttached("RefreshBindingOnKey", typeof(Key), typeof(TextBoxBehavior), new UIPropertyMetadata(OnRefreshBindingOnKeyChanged));

    private static readonly DependencyProperty TextTextBoxBehaviorProperty =
        DependencyProperty.RegisterAttached("TextTextBoxBehavior", typeof(TextBoxBehavior), typeof(TextBoxBehavior), new UIPropertyMetadata(null));

    private static readonly DependencyProperty AllTextBoxBehaviorProperty =
        DependencyProperty.RegisterAttached("AllTextBoxBehavior", typeof(TextBoxBehavior), typeof(TextBoxBehavior), new UIPropertyMetadata(null));

    private bool _textSelecting;

    private TextBoxBehavior()
    {
    }

    /// <summary>
    ///     Gets the selected text in a text box.
    /// </summary>
    /// <param name="obj">The element from which the property value is read.</param>
    /// <returns>The TextBoxBehavior.SelectedText property value for the element.</returns>
    public static string GetSelectedText(DependencyObject obj)
    {
        return (string)obj.GetValue(SelectedTextProperty);
    }

    /// <summary>
    ///     Attaches the information which text has to be selected in a text box.
    /// </summary>
    /// <param name="obj">The element to which the attached property is written.</param>
    /// <param name="value">The needed TextBoxBehavior.SelectedText value.</param>
    public static void SetSelectedText(DependencyObject obj, string value)
    {
        obj.SetValue(SelectedTextProperty, value);
    }

    /// <summary>
    ///     Gets a value that indicates if everything has to be selected automatically when the text box got the focus.
    /// </summary>
    /// <param name="obj">The element from which the property value is read.</param>
    /// <returns>The TextBoxBehavior.SelectAllOnFocus property value for the element.</returns>
    public static bool GetSelectAllOnFocus(DependencyObject obj)
    {
        return (bool)obj.GetValue(SelectAllOnFocusProperty);
    }

    /// <summary>
    ///     Attaches a value that indicates if everything has to be selected automatically when the text box got the focus.
    /// </summary>
    /// <param name="obj">The element to which the attached property is written.</param>
    /// <param name="value">The needed TextBoxBehavior.SelectedText value.</param>
    public static void SetSelectAllOnFocus(DependencyObject obj, bool value)
    {
        obj.SetValue(SelectAllOnFocusProperty, value);
    }

    private static TextBoxBehavior GetTextTextBoxBehavior(DependencyObject obj)
    {
        return (TextBoxBehavior)obj.GetValue(TextTextBoxBehaviorProperty);
    }

    private static void SetTextTextBoxBehavior(DependencyObject obj, TextBoxBehavior value)
    {
        obj.SetValue(TextTextBoxBehaviorProperty, value);
    }

    private static TextBoxBehavior GetAllTextBoxBehavior(DependencyObject obj)
    {
        return (TextBoxBehavior)obj.GetValue(AllTextBoxBehaviorProperty);
    }

    private static void SetAllTextBoxBehavior(DependencyObject obj, TextBoxBehavior value)
    {
        obj.SetValue(AllTextBoxBehaviorProperty, value);
    }

    /// <summary>
    ///     Gets a value that indicates on which key the text binding has to be refreshed in a text box.
    /// </summary>
    /// <param name="obj">The element from which the property value is read.</param>
    /// <returns>The TextBoxBehavior.RefreshBindingOnKey property value for the element.</returns>
    public static Key GetRefreshBindingOnKey(DependencyObject obj)
    {
        return (Key)obj.GetValue(RefreshBindingOnKeyProperty);
    }

    /// <summary>
    ///     Attaches a value that indicates on which key the text binding has to be refreshed in a text box.
    /// </summary>
    /// <param name="obj">The element to which the attached property is written.</param>
    /// <param name="value">The needed TextBoxBehavior.RefreshBindingOnKey value.</param>
    public static void SetRefreshBindingOnKey(DependencyObject obj, Key value)
    {
        obj.SetValue(RefreshBindingOnKeyProperty, value);
    }

    private static void OnRefreshBindingOnKeyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
        var textBox = GetTextBox(sender);

        if (e.OldValue != null)
            textBox.KeyUp -= TextBox_KeyUp;
        if (e.NewValue != null)
            textBox.KeyUp += TextBox_KeyUp;
    }

    private static void TextBox_KeyUp(object sender, KeyEventArgs e)
    {
        var box = (TextBox)sender;
        if (e.Key == GetRefreshBindingOnKey(box))
        {
            var expression = box.GetBindingExpression(TextBox.TextProperty);
            expression?.UpdateSource();
        }
    }

    private static void OnSelectedTextChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
        var textBox = GetTextBox(sender);
        var behavior = GetTextTextBoxBehavior(sender);
        if (behavior == null)
        {
            behavior = new TextBoxBehavior();
            SetTextTextBoxBehavior(sender, behavior);
            textBox.SelectionChanged += behavior.TextBox_SelectionChanged;
        }

        behavior.RunSetSelectedText(textBox, e.NewValue);
    }

    private void RunSetSelectedText(TextBox textbox, object param)
    {
        if (!_textSelecting && param != null)
            SelectText(textbox, false, param.ToString());
    }

    private void TextBox_SelectionChanged(object sender, RoutedEventArgs e)
    {
        _textSelecting = true;
        var textBox = (TextBox)sender;
        SetSelectedText(textBox, textBox.SelectedText);
        _textSelecting = false;
    }

    private static void OnSelectAllOnFocusChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
        var textBox = GetTextBox(sender);
        var behavior = GetAllTextBoxBehavior(sender);
        if (behavior == null)
        {
            behavior = new TextBoxBehavior();
            SetAllTextBoxBehavior(sender, behavior);
            textBox.GotFocus += behavior.TextBox_GotFocus;
        }
    }

    private void TextBox_GotFocus(object sender, RoutedEventArgs e)
    {
        var textBox = (TextBox)sender;
        SelectText(textBox, true, null);
    }

    private static TextBox GetTextBox(DependencyObject sender)
    {
        if (sender is not TextBox textBox)
            throw new InvalidOperationException("The TextBoxBehavior.SelectionStart only can be attached on a TextBox control");
        return textBox;
    }

    private void SelectText(TextBox box, bool selectAll, string text)
    {
        ControlFocus.GiveFocus(box, delegate
        {
            if (selectAll)
            {
                box.SelectAll();
            }
            else if (!string.IsNullOrEmpty(text))
            {
                var pos = box.Text.IndexOf(text);
                if (pos > 0 && box.Text.Contains(text))
                    box.Select(pos, text.Length);
            }
        });
    }
}