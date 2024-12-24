using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FunkyInventoryWPF.UserControls;

public partial class DirectionListViewItemControl : UserControl
{
    #region Dependency Properties
    //#region DeleteDirectionCommand
    //public static readonly DependencyProperty DeleteDirectionCommandProperty = DependencyProperty.Register("DeleteDirectionCommand", typeof(ICommand), typeof(DirectionListViewItemControl), new UIPropertyMetadata(null));
    //public ICommand DeleteDirectionCommand
    //{
    //    get { return (ICommand)GetValue(DeleteDirectionCommandProperty); }
    //    set { SetValue(DeleteDirectionCommandProperty, value); }
    //}
    //#endregion

    #region DeleteVisibility
    public static DependencyProperty DeleteVisibilityProperty = DependencyProperty.Register("DeleteVisibility", typeof(Visibility), typeof(DirectionListViewItemControl),
        new FrameworkPropertyMetadata(Visibility.Visible, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, (obj, args) => ((DirectionListViewItemControl)obj).OnDeleteVisibilityChanged(args)));
    public Visibility DeleteVisibility
    {
        get => (Visibility)GetValue(DeleteVisibilityProperty);
        set => SetValue(DeleteVisibilityProperty, value);
    }
    private void OnDeleteVisibilityChanged(DependencyPropertyChangedEventArgs args)
    {
    }
    #endregion

    #region Direction
    public static DependencyProperty DirectionProperty = DependencyProperty.Register("Direction", typeof(string), typeof(DirectionListViewItemControl),
        new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, (obj, args) => ((DirectionListViewItemControl)obj).OnDirectionChanged(args)));
    public string? Direction
    {
        get => (string)GetValue(DirectionProperty);
        set => SetValue(DirectionProperty, value);
    }
    private void OnDirectionChanged(DependencyPropertyChangedEventArgs args)
    {
    }
    #endregion

    #region Order
    public static DependencyProperty OrderProperty = DependencyProperty.Register("Order", typeof(string), typeof(DirectionListViewItemControl),
        new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, (obj, args) => ((DirectionListViewItemControl)obj).OnOrderChanged(args)));
    public string? Order
    {
        get => (string)GetValue(OrderProperty);
        set => SetValue(OrderProperty, value);
    }
    private void OnOrderChanged(DependencyPropertyChangedEventArgs args)
    {
    }
    #endregion
    #endregion

    #region Routed Event Handlers
    public event RoutedEventHandler DeleteDirection
    {
        add { lock (objectLock) { btnDeleteDirection.Click += value; } }
        remove { lock (objectLock) { btnDeleteDirection.Click -= value; } }
    }
    #endregion

    #region Routed Commands
    #region DeleteDirectionCommand
    private static readonly RoutedCommand deleteDirectionCommand = new();
    public static RoutedCommand DeleteDirectionCommand = deleteDirectionCommand;
    private void CanExecuteDeleteDirectionCommand(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = true;
    }
    private void ExecutedDeleteDirectionCommand(object sender, ExecutedRoutedEventArgs e)
    {
    }
    #endregion
    #endregion

    public readonly object objectLock = new();

    public DirectionListViewItemControl()
    {
        InitializeComponent();
    }
}
