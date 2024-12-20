using FunkyInventoryWPF.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FunkyInventoryWPF.UserControls
{
    public partial class TitleControl : UserControl
    {
        #region RoutedCommands
        #region HomeCommand
        private static readonly RoutedCommand homeCommand = new();
        public static RoutedCommand HomeCommand = homeCommand;
        private void CanExecuteHomeCommand(object sender, CanExecuteRoutedEventArgs e)
            => e.CanExecute = e.Source is Control && SelectedControl != typeof(SplashControl);
        private void ExecutedHomeCommand(object sender, ExecutedRoutedEventArgs e)
            => SwapContent(((App)Application.Current).SplashControl);
        #endregion

        #region MovieCommand
        private static readonly RoutedCommand movieCommand = new();
        public static RoutedCommand MovieCommand = movieCommand;
        private void CanExecuteMovieCommand(object sender, CanExecuteRoutedEventArgs e)
            => e.CanExecute = e.Source is Control;
        private void ExecutedMovieCommand(object sender, ExecutedRoutedEventArgs e)
        {
            //SwapContent(((App)Application.Current).MovieControl);
        }
        #endregion

        #region MusicCommand
        private static readonly RoutedCommand musicCommand = new();
        public static RoutedCommand MusicCommand = musicCommand;
        private void CanExecuteMusicCommand(object sender, CanExecuteRoutedEventArgs e)
            => e.CanExecute = e.Source is Control;
        private void ExecutedMusicCommand(object sender, ExecutedRoutedEventArgs e)
        {
            //SwapContent(((App)Application.Current).MusicControl);
        }
        #endregion

        #region RecipeCommand
        private static readonly RoutedCommand recipeCommand = new();
        public static RoutedCommand RecipeCommand = recipeCommand;
        private void CanExecuteRecipeCommand(object sender, CanExecuteRoutedEventArgs e)
            => e.CanExecute = e.Source is Control && SelectedControl != typeof(RecipeControl);
        private void ExecutedRecipeCommand(object sender, ExecutedRoutedEventArgs e)
            => SwapContent(((App)Application.Current).RecipeControl);
        #endregion

        #region UserAdminCommand
        private static readonly RoutedCommand iserAdminCommand = new();
        public static RoutedCommand UserAdminCommand = iserAdminCommand;
        private void CanExecuteUserAdminCommand(object sender, CanExecuteRoutedEventArgs e)
            => e.CanExecute = e.Source is Control && SelectedControl != typeof(UserAdministrationControl);
        private void ExecutedUserAdminCommand(object sender, ExecutedRoutedEventArgs e)
            => SwapContent(((App)Application.Current).UserAdministrationControl);
        #endregion
        #endregion

        #region Properties
        private CancellationToken cancellationToken = default;
        private TitleControlViewModel VM;
        private readonly MainWindow inventoryWindow;
        private bool _suspendChangeHandlers = false;
        private Type SelectedControl { get; set; }
        #endregion

        public TitleControl(MainWindow parent, TitleControlViewModel vm)
        {
            InitializeComponent();

            inventoryWindow = parent;
            DataContext = vm;
            VM = vm;
        }

        #region Events
        private void OnLoad(object sender, RoutedEventArgs e)
        {
            if (DataContext is TitleControlViewModel vm)
                vm.IsAdmin = inventoryWindow?.LoggedInUser?.Role?.RoleName == "Admin";
            SwapContent(((App)Application.Current).SplashControl);
        }
        #endregion

        #region Functions
        private void SwapContent(Control control)
        {
            dpContent.Children.Clear();
            dpContent.Children.Add(control);
            SelectedControl = control.GetType();
        }
        #endregion
    }
}
