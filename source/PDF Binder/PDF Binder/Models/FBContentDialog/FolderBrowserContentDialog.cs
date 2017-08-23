namespace PDF_Binder.Models.FBContentDialog
{
    using MWindowInterfacesLib.Events;
    using MWindowInterfacesLib.Interfaces;
    using System.Threading.Tasks;

    public class FolderBrowserContentDialog
    {
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
        internal async void ShowAwaitCustomDialog
        (
          IMetroWindow parentWindow
        , object contentDlgVM
        )
        {
            var dlg = ServiceLocator.ServiceContainer.Instance.GetService<IContentDialogService>();
            var manager = dlg.Manager;

            ////            EventHandler<DialogStateChangedEventArgs> dialogManagerOnDialogOpened = null;
            ////            dialogManagerOnDialogOpened = (o, args) => {
            ////                manager.DialogOpened -= dialogManagerOnDialogOpened;
            ////                Console.WriteLine("Custom Dialog opened!");
            ////            };
            ////            manager.DialogOpened += dialogManagerOnDialogOpened;
            ////
            ////            EventHandler<DialogStateChangedEventArgs> dialogManagerOnDialogClosed = null;
            ////            dialogManagerOnDialogClosed = (o, args) => {
            ////                manager.DialogClosed -= dialogManagerOnDialogClosed;
            ////                Console.WriteLine("Custom Dialog closed!");
            ////
            ////                dlg.MsgBox.Show(parentWindow, "Dialog gone", "The custom dialog has closed");
            ////            };

            ////            manager.DialogClosed += dialogManagerOnDialogClosed;

            // Construct a viewmodel with the interaction logic
            ////            var viewModel = new Demos.ViewModels.MsgDemoViewModel();
            ////            viewModel.DialogClosed += CloseCustomDialog;

            // Construct a view with the content to be displayed
            var dlgContent = new FBContentView();

            //Application.Current.Resources["CustomCloseDialogTest"];

            var customDialogView = new MWindowDialogLib.Dialogs.CustomDialog(
                parentWindow
               , dlgContent
               , contentDlgVM);

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
            var dlg = ServiceLocator.ServiceContainer.Instance.GetService<IContentDialogService>();
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
