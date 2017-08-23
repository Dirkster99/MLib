namespace MDemo.Demos
{
    using MWindowInterfacesLib.Events;
    using MWindowInterfacesLib.Interfaces;
    using System;
    using System.Threading.Tasks;
    using System.Windows;

    /// <summary>
    /// Creates a class that contains all sample demo code
    /// for all custom dialogs implemented in this demo application.
    /// </summary>
    public class CustomDialogDemos : MDemo.ViewModels.Base.ModelBase
    {
        /// <summary>
        /// Shows a custom dialog with text input controls driven through
        /// a custom viewmodel class.
        /// </summary>
        internal async void RunCustomFromVm(object context)
        {
            var coord = GetService<IContentDialogService>().Coordinator;

            var customDialog = new MWindowDialogLib.Dialogs.CustomDialog(new Views.CustomDialogView());

            var customDialogViewModel = new ViewModels.CustomDialogViewModel(instance =>
            {
                coord.HideMetroDialogAsync(context, customDialog);

                System.Diagnostics.Debug.WriteLine("Custom Dialog -" + instance.Title + "- VM Result: ");
                System.Diagnostics.Debug.WriteLine("FirstName: " + instance.FirstName);
                System.Diagnostics.Debug.WriteLine(" LastName: " + instance.LastName);
            })
            { Title = "Custom Dialog" };

            customDialog.DataContext = customDialogViewModel;

            await coord.ShowMetroDialogAsync(context, customDialog);
        }

        /// <summary>
        /// This method demos a custom dialog that requires no buttons
        /// but will be closed after an ellapsed interval of time.
        /// </summary>
        /// <param name="parentWindow"></param>
        internal async void ShowCustomDialog(IMetroWindow parentWindow)
        {
            var dlg = GetService<IContentDialogService>();
            var manager = dlg.Manager;

            object dlgContent = Application.Current.Resources["CustomDialogTest"];

            var viewModel = new Demos.ViewModels.MsgDemoViewModel()
            {
                DialogCanCloseViaChrome = false
              , CloseWindowButtonVisibility = false
              , Title = "Custom Dialog"
            };

            var customDialogView = new MWindowDialogLib.Dialogs.CustomDialog(
                parentWindow
                , dlgContent
                , viewModel);

            #pragma warning disable CS4014
            // Dialog is not awaited to allow next message box to be displayed on top of it.
            manager.ShowMetroDialogAsync(parentWindow, customDialogView);
            #pragma warning restore CS4014

            viewModel.Message = "A message box will appear in 5 seconds.";

            await Delay(5000);

            await dlg.MsgBox.ShowAsync(parentWindow, "This message is shown on top of another.", "Secondary dialog");

            viewModel.Message = "The dialog will close in 2 seconds.";
            await Delay(2000);

            await manager.HideMetroDialogAsync(parentWindow, customDialogView);
        }

        /// <summary>
        /// This method demos a custom dialog that can envoke an external
        /// close event via the dialogs viewmodel.
        /// 
        /// 1) The DialogClosed event of the viewmodel is raised via
        ///    the CloseCommand and executes the CloseCustomDialog method below.
        ///    The CloseCustomDialog method executes the manager's
        ///    HideMetroDialogAsync method which in turn raises
        ///    
        /// 2) The manager's DialogClosed event which in turn
        /// 
        /// 3) Shows another dialog via the inline bound event...
        /// </summary>
        /// <param name="parentWindow"></param>
        internal async void ShowAwaitCustomDialog(IMetroWindow parentWindow)
        {
            var dlg = GetService<IContentDialogService>();
            var manager = dlg.Manager;

            EventHandler<DialogStateChangedEventArgs> dialogManagerOnDialogOpened = null;
            dialogManagerOnDialogOpened = (o, args) => {
                manager.DialogOpened -= dialogManagerOnDialogOpened;
                Console.WriteLine("Custom Dialog opened!");
            };
            manager.DialogOpened += dialogManagerOnDialogOpened;

            EventHandler<DialogStateChangedEventArgs> dialogManagerOnDialogClosed = null;
            dialogManagerOnDialogClosed = (o, args) => {
                manager.DialogClosed -= dialogManagerOnDialogClosed;
                Console.WriteLine("Custom Dialog closed!");

                dlg.MsgBox.Show(parentWindow, "Dialog gone", "The custom dialog has closed");
            };

            manager.DialogClosed += dialogManagerOnDialogClosed;

            // Construct a viewmodel with the interaction logic
            var viewModel = new Demos.ViewModels.MsgDemoViewModel();
            viewModel.DialogClosed += CloseCustomDialog;

            // Construct a view with the content to be displayed
            var dlgContent = Application.Current.Resources["CustomCloseDialogTest"];

            var customDialogView = new MWindowDialogLib.Dialogs.CustomDialog(
                parentWindow
               , dlgContent
               , viewModel);

            _dialog = customDialogView;
            _parentWindow = parentWindow;

            await manager.ShowMetroDialogAsync(parentWindow, customDialogView);

            // Waits until the either close button is clicked to invoke the CloseCommand in the viewModel
            await _dialog.WaitUntilUnloadedAsync();
        }

        // These fields are just used to store some references while the custom dialog is open
        private IBaseMetroDialogFrame _dialog = null;
        private IMetroWindow _parentWindow = null;

        /// <summary>
        /// Method executes when Close (x) window or demo button is clicked
        /// 
        /// 1> This invokes the bound ICommand CloseCommand which in turn
        ///    2> will invoke the event that is bound to this method...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void CloseCustomDialog(object sender, DialogStateChangedEventArgs e)
        {
            var dlg = GetService<IContentDialogService>();
            var manager = dlg.Manager;

            await manager.HideMetroDialogAsync(_parentWindow, _dialog);

            _dialog = null;
            _parentWindow = null;
        }

        private Task Delay(int dueTime)
        {
            return Task.Delay(dueTime);
        }
    }
}
