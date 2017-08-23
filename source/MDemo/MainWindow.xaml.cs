namespace MDemo
{
    using Settings.UserProfile;
    using System.Windows;
    using ViewModels;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MWindowLib.MetroWindow
                                     ,IViewSize  // Implements saving and loading/repositioning of Window
    {
        #region constructors
        public MainWindow()
        {
            this.InitializeComponent();
        }
        #endregion constructors

        #region Overlay Demo
        #region Message Dialogs
        private void ShowMessageDialog(object sender, RoutedEventArgs e)
        {
            var messageDemo = GetDemoViewModel().MessageDlgDemo;

            messageDemo.ShowDialog(this);
        }

        private void ShowLimitedMessageDialog(object sender, RoutedEventArgs e)
        {
            var messageDemo = GetDemoViewModel().MessageDlgDemo;

            messageDemo.ShowLimitedDialog(this);
        }
        #endregion Message Dialogs

        #region Input Dialog
        private void ShowInputDialog(object sender, RoutedEventArgs e)
        {
            var inputDlgDemo = GetDemoViewModel().InputDlgDemo;

            inputDlgDemo.ShowDialog(this);
        }
        #endregion Input Dialog

        #region LoginDialog
        private void ShowLoginDialog(object sender, RoutedEventArgs e)
        {
            GetDemoViewModel().LoginDlgDemo.ShowDialog(this);
        }

        private void ShowLoginDialogOnlyPassword(object sender, RoutedEventArgs e)
        {
            GetDemoViewModel().LoginDlgDemo.ShowDialog(this, false, false);
        }
        #endregion LoginDialog

        #region CustomCloseDialogTest
        private void ShowCustomDialog(object sender, RoutedEventArgs e)
        {
            GetDemoViewModel().CustomDlgDemo.ShowCustomDialog(this);
        }

        private void ShowAwaitCustomDialog(object sender, RoutedEventArgs e)
        {
            GetDemoViewModel().CustomDlgDemo.ShowAwaitCustomDialog(this);
        }
        #endregion CustomCloseDialogTest

        #region progress dialog tests
        /// <summary>
        /// Implements a progress dialog via viewmodel + custom dialog.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ShowProgressDialogDemo(object sender, RoutedEventArgs e)
        {
            var demo = GetDemoViewModel().ProgressDlgDemo;

            var result = await demo.ShowNoCancelProgressAsync(this, true, false, false);
        }

        private async void ShowFiniteProgressDialogDemo(object sender, RoutedEventArgs e)
        {
            var demo = GetDemoViewModel().ProgressDlgDemo;

            var result = await demo.ShowNoCancelProgressAsync(this, false, false, false);
        }

        /// <summary>
        /// Implements a progress dialog via viewmodel + custom dialog.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ShowInfiniteCancelableProgressDialogDemo(object sender, RoutedEventArgs e)
        {
            var demo = GetDemoViewModel().ProgressDlgDemo;

            var result = await demo.ShowCancelProgressAsync(this, true);
        }

        private async void ShowFiniteCancelableProgressDialogDemo(object sender, RoutedEventArgs e)
        {
            var demo = GetDemoViewModel().ProgressDlgDemo;

            var result = await demo.ShowCancelProgressAsync(this, false);
        }

        private async void ShowInfiniteCancelableProgressCloseDialogDemo(object sender, RoutedEventArgs e)
        {
            var demo = GetDemoViewModel().ProgressDlgDemo;

            var result = await demo.ShowCancelProgressAsync(this, true, true);
        }

        private async void ShowFiniteCancelableProgressCloseDialogDemo(object sender, RoutedEventArgs e)
        {
            var demo = GetDemoViewModel().ProgressDlgDemo;

            var result = await demo.ShowCancelProgressAsync(this, false, true);
        }

        private async void Show2CancelableProgressCloseDialogDemo(object sender, RoutedEventArgs e)
        {
            var demo = GetDemoViewModel().ProgressDlgDemo;

            var result = await demo.Show2CancelProgressAsync(this);
        }
        #endregion progress dialog tests

        #region Show OutsideDialogs
        private void ShowInputDialogOutside(object sender, RoutedEventArgs e)
        {
            var inputDlgDemo = GetDemoViewModel().InputDlgDemo;
            inputDlgDemo.ShowDialogOutside(this);
        }

        private void ShowLoginDialogOutside(object sender, RoutedEventArgs e)
        {
            var demo = GetDemoViewModel().LoginDlgDemo;
            demo.ShowDialogOutside(this);
        }

        private void ShowMessageDialogOutside(object sender, RoutedEventArgs e)
        {
            GetDemoViewModel().MessageDlgDemo.ShowDialogOutside(this);
        }

        private void ShowProgressDialogOutside(object sender, RoutedEventArgs e)
        {
            GetDemoViewModel().ProgressDlgDemo.ShowDialogOutside(this, false);
        }       
        #endregion Show OutsideDialogs

        /// <summary>
        /// Gets a reference to the DemoViewModel object via the DataContext of this Window.
        /// </summary>
        /// <returns></returns>
        private Demos.ViewModels.DemoViewModel GetDemoViewModel()
        {
            return (this.DataContext as AppViewModel).Demo;
        }
        #endregion Overlay Demo
    }
}
