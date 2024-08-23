// -----------------------------------------------------------------------------------------------------------------
// <copyright file="CutTooltipBehavior.cs" company="dwndland">
//     Copyright (c) David Wendland. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------------------------------------------------

using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

// ReSharper disable CheckNamespace

namespace Chapter.Net.WPF.Behaviors;

/// <summary>
///     Brings the functionality to the TextBlock and Label to show the text in the tooltip automatically when its cut.
/// </summary>
/// <remarks>
///     In the case of the Label the Content.ToString() will be used to get the text. If a tooltip is set already it
///     will be overwritten.
/// </remarks>
public sealed class CutTooltipBehavior
{
    /// <summary>
    ///     Defines the ShowTooltip attached dependency property.
    /// </summary>
    public static readonly DependencyProperty ShowTooltipProperty =
        DependencyProperty.RegisterAttached("ShowTooltip", typeof(CutTextKind), typeof(CutTooltipBehavior), new PropertyMetadata(OnShowTooltipChanged));

    private static readonly DependencyProperty BehaviorProperty =
        DependencyProperty.RegisterAttached("Behavior", typeof(CutTooltipBehavior), typeof(CutTooltipBehavior), new PropertyMetadata(null));

    private readonly FrameworkElement _owner;
    private CutTextKind _cutTextKind;

    private CutTooltipBehavior(FrameworkElement owner)
    {
        _owner = owner;
    }

    /// <summary>
    ///     Gets the value that defines when the tooltip should be shown.
    /// </summary>
    /// <param name="obj">The element from which the property value is read.</param>
    /// <returns>The CutTooltipBehavior.ShowTooltip property value for the element.</returns>
    public static CutTextKind GetShowTooltip(DependencyObject obj)
    {
        return (CutTextKind)obj.GetValue(ShowTooltipProperty);
    }

    /// <summary>
    ///     Sets the value that defines when the tooltip should be shown.
    /// </summary>
    /// <param name="obj">The element to which the attached property is written.</param>
    /// <param name="value">The needed CutTooltipBehavior.ShowTooltip value.</param>
    public static void SetShowTooltip(DependencyObject obj, CutTextKind value)
    {
        obj.SetValue(ShowTooltipProperty, value);
    }

    private static CutTooltipBehavior GetBehavior(DependencyObject obj)
    {
        return (CutTooltipBehavior)obj.GetValue(BehaviorProperty);
    }

    private static void SetBehavior(DependencyObject obj, CutTooltipBehavior value)
    {
        obj.SetValue(BehaviorProperty, value);
    }

    private static void OnShowTooltipChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (!(d is TextBlock) && !(d is ContentControl))
            throw new InvalidOperationException("The CutTooltipBehavior can be attached to a TextBlocks or ContentControls only.");

        var behavior = GetBehavior(d);
        if (behavior == null)
        {
            behavior = new CutTooltipBehavior(d as FrameworkElement);
            SetBehavior(d, behavior);
        }

        behavior.OnShowTooltipChanged((CutTextKind)e.OldValue, (CutTextKind)e.NewValue);
    }

    private void OnShowTooltipChanged(CutTextKind oldValue, CutTextKind newValue)
    {
        _cutTextKind = newValue;

        if (oldValue != CutTextKind.None)
        {
            _owner.IsVisibleChanged -= OnIsVisibleChanged;
            _owner.LayoutUpdated -= OnLayoutUpdated;
            _owner.SizeChanged -= OnSizeChanged;
        }

        if (newValue != CutTextKind.None)
        {
            _owner.IsVisibleChanged += OnIsVisibleChanged;
            _owner.LayoutUpdated += OnLayoutUpdated;
            _owner.SizeChanged += OnSizeChanged;
        }
    }

    private void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        CalculateTooltip();
    }

    private void OnLayoutUpdated(object sender, EventArgs e)
    {
        CalculateTooltip();
    }

    private void OnSizeChanged(object sender, SizeChangedEventArgs e)
    {
        CalculateTooltip();
    }

    private void CalculateTooltip()
    {
        var text = GetContentText();
        if (string.IsNullOrWhiteSpace(text))
            return;

        var fontInfo = GetFontInfo();
        if (fontInfo == null)
            return;

        var isWrapped = GetIsWrapped();

        Calculate(text, fontInfo, isWrapped);
    }

    private void Calculate(string text, FontInfo fontInfo, bool isWrapped)
    {
        var textLength = GetTextLength(fontInfo, text, isWrapped);

        bool showTooltip;
        switch (_cutTextKind)
        {
            case CutTextKind.Height:
                showTooltip = textLength.Height > _owner.DesiredSize.Height;
                break;
            case CutTextKind.Width:
                showTooltip = textLength.Width > _owner.DesiredSize.Width;
                break;
            case CutTextKind.WithAndHeight:
                showTooltip = textLength.Width > _owner.DesiredSize.Width || textLength.Height > _owner.DesiredSize.Height;
                break;
            default:
                showTooltip = false;
                break;
        }

        _owner.ToolTip = showTooltip ? text : null;
    }

    private Size GetTextLength(FontInfo fontInfo, string text, bool isWrapped)
    {
        var typeface = new Typeface(fontInfo.FontFamily, fontInfo.FontStyle, fontInfo.FontWeight, fontInfo.FontStretch);
        var formattedText = GetFormattedText(fontInfo, text, typeface);

        if (isWrapped)
            formattedText.MaxTextWidth = fontInfo.ActualWidth;

        var padding = GetPadding();
        return new Size(formattedText.Width + padding.Left + padding.Right, formattedText.Height + padding.Bottom + padding.Top);
    }

    private FormattedText GetFormattedText(FontInfo fontInfo, string text, Typeface typeface)
    {
#if NETFRAMEWORK
            using (var graphics = System.Drawing.Graphics.FromHwnd(IntPtr.Zero))
            {
                var fontSize = fontInfo.FontSize * (graphics.DpiX / 96f);
                return new FormattedText(
                    text,
                    Thread.CurrentThread.CurrentCulture,
                    fontInfo.FlowDirection,
                    typeface,
                    fontSize,
                    fontInfo.Foreground);
            }
#else
        return new FormattedText(
            text,
            Thread.CurrentThread.CurrentCulture,
            fontInfo.FlowDirection,
            typeface,
            fontInfo.FontSize,
            fontInfo.Foreground,
            VisualTreeHelper.GetDpi(_owner).PixelsPerDip);
#endif
    }

    private Thickness GetPadding()
    {
        switch (_owner)
        {
            case TextBlock textBlock:
                return textBlock.Padding;
            case Control control:
                return control.Padding;
            default:
                return new Thickness(0);
        }
    }

    private string GetContentText()
    {
        switch (_owner)
        {
            case TextBlock textBlock:
                return textBlock.Text;
            case ContentControl contentControl when contentControl.Content != null:
                return contentControl.Content.ToString();
            default:
                return string.Empty;
        }
    }

    private bool GetIsWrapped()
    {
        if (_owner is TextBlock textBlock)
            return textBlock.TextWrapping != TextWrapping.NoWrap;
        return false;
    }

    private FontInfo GetFontInfo()
    {
        switch (_owner)
        {
            case TextBlock textBlock:
                return GetFontInfo(textBlock);
            case Control control:
                return GetFontInfo(control);
            default:
                return null;
        }
    }

    private FontInfo GetFontInfo(TextBlock textBlock)
    {
        return new FontInfo
        {
            FontFamily = textBlock.FontFamily,
            FontStyle = textBlock.FontStyle,
            FontWeight = textBlock.FontWeight,
            FontStretch = textBlock.FontStretch,
            FlowDirection = textBlock.FlowDirection,
            FontSize = textBlock.FontSize,
            Foreground = textBlock.Foreground,
            ActualWidth = textBlock.DesiredSize.Width,
            ActualHeight = textBlock.DesiredSize.Height
        };
    }

    private FontInfo GetFontInfo(Control contentControl)
    {
        return new FontInfo
        {
            FontFamily = contentControl.FontFamily,
            FontStyle = contentControl.FontStyle,
            FontWeight = contentControl.FontWeight,
            FontStretch = contentControl.FontStretch,
            FlowDirection = contentControl.FlowDirection,
            FontSize = contentControl.FontSize,
            Foreground = contentControl.Foreground,
            ActualWidth = contentControl.ActualWidth,
            ActualHeight = contentControl.ActualHeight
        };
    }

    private class FontInfo
    {
        public FontFamily FontFamily { get; set; }
        public FontStyle FontStyle { get; set; }
        public FontWeight FontWeight { get; set; }
        public FontStretch FontStretch { get; set; }
        public FlowDirection FlowDirection { get; set; }
        public double FontSize { get; set; }
        public Brush Foreground { get; set; }
        public double ActualWidth { get; set; }
        public double ActualHeight { get; set; }
    }
}