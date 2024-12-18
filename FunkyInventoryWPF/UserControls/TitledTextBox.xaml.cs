using System.Windows;
using System.Windows.Controls;

namespace FunkyInventoryWPF.UserControls;

public partial class TitledTextBox : UserControl
{
    #region Dependency Properties
    #region TextBlockForeground
    public static DependencyProperty TextBlockForegroundProperty = DependencyProperty.Register("TextBlockForeground", typeof(string), typeof(TitledTextBox),
        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
    public string TextBlockForeground
    {
        get => (string)GetValue(TextBlockForegroundProperty);
        set => SetValue(TextBlockForegroundProperty, value);
    }
    #endregion

    #region TextBlockFontSize
    public static DependencyProperty TextBlockFontSizeProperty = DependencyProperty.Register("TextBlockFontSize", typeof(string), typeof(TitledTextBox),
        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
    public string TextBlockFontSize
    {
        get => (string)GetValue(TextBlockFontSizeProperty);
        set => SetValue(TextBlockFontSizeProperty, value);
    }
    #endregion

    #region TextBlockFontWeight
    public static DependencyProperty TextBlockFontWeightProperty = DependencyProperty.Register("TextBlockFontWeight", typeof(string), typeof(TitledTextBox),
        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
    public string TextBlockFontWeight
    {
        get => (string)GetValue(TextBlockFontWeightProperty);
        set => SetValue(TextBlockFontWeightProperty, value);
    }
    #endregion

    #region TextBoxBorderBrush
    public static DependencyProperty TextBoxBorderBrushProperty = DependencyProperty.Register("TextBoxBorderBrush", typeof(string), typeof(TitledTextBox),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
    public string TextBoxBorderBrush
    {
        get => (string)GetValue(TextBoxBorderBrushProperty);
        set => SetValue(TextBoxBorderBrushProperty, value);
    }
    #endregion

    #region TextBoxBorderThickness
    public static DependencyProperty TextBoxBorderThicknessProperty = DependencyProperty.Register("TextBoxBorderThickness", typeof(string), typeof(TitledTextBox),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
    public string TextBoxBorderThickness
    {
        get => (string)GetValue(TextBoxBorderThicknessProperty);
        set => SetValue(TextBoxBorderThicknessProperty, value);
    }
    #endregion

    #region TextBoxCaretBrush
    public static DependencyProperty TextBoxCaretBrushProperty = DependencyProperty.Register("TextBoxCaretBrush", typeof(string), typeof(TitledTextBox),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
    public string TextBoxCaretBrush
    {
        get => (string)GetValue(TextBoxCaretBrushProperty);
        set => SetValue(TextBoxCaretBrushProperty, value);
    }
    #endregion

    #region TextBoxForeground
    public static DependencyProperty TextBoxForegroundProperty = DependencyProperty.Register("TextBoxForeground", typeof(string), typeof(TitledTextBox),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
    public string TextBoxForeground
    {
        get => (string)GetValue(TextBoxForegroundProperty);
        set => SetValue(TextBoxForegroundProperty, value);
    }
    #endregion

    #region TextBoxFontSize
    public static DependencyProperty TextBoxFontSizeProperty = DependencyProperty.Register("TextBoxFontSize", typeof(string), typeof(TitledTextBox),
        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
    public string TextBoxFontSize
    {
        get => (string)GetValue(TextBoxFontSizeProperty);
        set => SetValue(TextBoxFontSizeProperty, value);
    }
    #endregion

    #region TextBoxFontWeight
    public static DependencyProperty TextBoxFontWeightProperty = DependencyProperty.Register("TextBoxFontWeight", typeof(string), typeof(TitledTextBox),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
    public string TextBoxFontWeight
    {
        get => (string)GetValue(TextBoxFontWeightProperty);
        set => SetValue(TextBoxFontWeightProperty, value);
    }
    #endregion

    #region TextBoxTitle
    public static DependencyProperty TextBoxTitleProperty = DependencyProperty.Register("TextBoxTitle", typeof(string), typeof(TitledTextBox),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, (obj, args) => ((TitledTextBox)obj).OnTextBoxTitleChanged(args)));
    public string TextBoxTitle
    {
        get => (string)GetValue(TextBoxTitleProperty);
        set => SetValue(TextBoxTitleProperty, value);
    }
    private void OnTextBoxTitleChanged(DependencyPropertyChangedEventArgs args)
    {

    }
    #endregion

    #region Value
    public static DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(string), typeof(TitledTextBox),
        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, (obj, args) => ((TitledTextBox)obj).OnValueChanged(args)));
    public string Value
    {
        get => (string)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }
    private void OnValueChanged(DependencyPropertyChangedEventArgs args)
    {

    }
    #endregion
    #endregion

    public TitledTextBox()
    {
        InitializeComponent();
    }
}
