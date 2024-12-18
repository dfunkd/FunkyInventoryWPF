using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows;

namespace FunkyInventoryWPF.Core.Watermark;

public class WatermarkAdorner : Adorner
{
    #region Private Fields
    /// <summary>
    /// <see cref="ContentPresenter"/> that holds the watermark
    /// </summary>
    private readonly ContentPresenter contentPresenter;
    #endregion

    #region Constructor
    /// <summary>
    /// Initializes a new instance of the <see cref="WatermarkAdorner"/> class
    /// </summary>
    /// <param name="adornedElement"><see cref="UIElement"/> to be adorned</param>
    /// <param name="watermark">The watermark</param>
    public WatermarkAdorner(UIElement adornedElement, object watermark) :
       base(adornedElement)
    {
        IsHitTestVisible = false;

        contentPresenter = new()
        {
            Content = watermark,
            Opacity = 0.5,
            Margin = new Thickness(Control.Margin.Left + Control.Padding.Left, Control.Margin.Top + Control.Padding.Top, 0, 0)
        };

        if (Control is ItemsControl && Control is not ComboBox)
        {
            if (Control is ItemsControl)
                (Control as ItemsControl).Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF999999");
            if (Control is ComboBox)
                (Control as ComboBox).Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF999999");

            contentPresenter.VerticalAlignment = VerticalAlignment.Center;
            contentPresenter.HorizontalAlignment = HorizontalAlignment.Center;
        }

        // Hide the control adorner when the adorned element is hidden
        Binding binding = new("IsVisible")
        {
            Source = adornedElement,
            Converter = new BooleanToVisibilityConverter()
        };
        SetBinding(VisibilityProperty, binding);
    }
    #endregion

    #region Protected Properties
    /// <summary>
    /// Gets the number of children for the <see cref="ContainerVisual"/>.
    /// </summary>
    protected override int VisualChildrenCount
    {
        get => 1;
    }
    #endregion

    #region Private Properties
    /// <summary>
    /// Gets the control that is being adorned
    /// </summary>
    private Control Control
    {
        get => (Control)AdornedElement;
    }
    #endregion

    #region Protected Overrides
    /// <summary>
    /// Returns a specified child <see cref="Visual"/> for the parent <see cref="ContainerVisual"/>.
    /// </summary>
    /// <param name="index">A 32-bit signed integer that represents the index value of the child <see cref="Visual"/>. The value of index must be between 0 and <see cref="VisualChildrenCount"/> - 1.</param>
    /// <returns>The child <see cref="Visual"/>.</returns>
    protected override Visual GetVisualChild(int index)
        => contentPresenter;

    /// <summary>
    /// Implements any custom measuring behavior for the adorner.
    /// </summary>
    /// <param name="constraint">A size to constrain the adorner to.</param>
    /// <returns>A <see cref="Size"/> object representing the amount of layout space needed by the adorner.</returns>
    protected override Size MeasureOverride(Size constraint)
    {
        // Here's the secret to getting the adorner to cover the whole control
        contentPresenter.Measure(Control.RenderSize);
        return Control.RenderSize;
    }

    /// <summary>
    /// When overridden in a derived class, positions child elements and determines a size for a <see cref="FrameworkElement"/> derived class. 
    /// </summary>
    /// <param name="finalSize">The final area within the parent that this element should use to arrange itself and its children.</param>
    /// <returns>The actual size used.</returns>
    protected override Size ArrangeOverride(Size finalSize)
    {
        contentPresenter.Arrange(new Rect(finalSize));
        return finalSize;
    }
    #endregion
}
