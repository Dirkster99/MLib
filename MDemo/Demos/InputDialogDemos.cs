namespace MDemo.Demos
{
    using MWindowDialogLib.Dialogs;
    using MWindowInterfacesLib.Interfaces;

    public class InputDialogDemos : MDemo.ViewModels.Base.ModelBase
    {
        internal async void ShowDialogFromVM(object context)
        {
            var viewModel = new Demos.ViewModels.InputDialogViewModel()
            {
                Title = "From a VM",
                Message = "This dialog was shown from a VM, without knowledge of Window",
                AffirmativeButtonText = "OK",
                DefaultResult = DialogIntResults.OK  // Return Key => OK Clicked
            };

            var customDialog = new MWindowDialogLib.Dialogs.CustomDialog(new Demos.Views.InputView(), viewModel);

            var coord = GetService<IContentDialogService>().Coordinator;

            var result = await coord.ShowMetroDialogAsync(context, customDialog);
        }

        internal async void ShowDialog(IMetroWindow parentWindow)
        {
            var viewModel = new Demos.ViewModels.InputDialogViewModel()
            {
                Title = "Hello!",
                Message = "What is your name?",
                AffirmativeButtonText = "OK",
                NegativeButtonText = "Cancel",
                DefaultResult = DialogIntResults.OK  // Return Key => OK Clicked
            };

            var customDialog = new MWindowDialogLib.Dialogs.CustomDialog(new Demos.Views.InputView(), viewModel);

            var dlg = GetService<IContentDialogService>();
            var manager = dlg.Manager;

            var result = await manager.ShowMetroDialogAsync(parentWindow, customDialog);

            // user pressed cancel, press ESC or closed via (x) button
            if (result == DialogIntResults.CANCEL)
                return;

            var message = string.Format("Hello " + viewModel.Input + "! (result: {0})", result);

            await dlg.MsgBox.ShowAsync(parentWindow, message, "Hello");
        }

        /// <summary>
        /// Shows the dialog in a seperate window displayed over the current main window.
        /// </summary>
        /// <param name="parentWindow"></param>
        internal void ShowDialogOutside(IMetroWindow parentWindow)
        {
            var viewModel = new Demos.ViewModels.InputDialogViewModel()
            {
                Title = "Hello!",
                Message = "What is your name?",
                AffirmativeButtonText = "OK",
                NegativeButtonText = "Cancel",
                DefaultResult = DialogIntResults.OK  // Return Key => OK Clicked
            };

            var customDialog = new MWindowDialogLib.Dialogs.CustomDialog(new Demos.Views.InputView(), viewModel);

            var dlg = GetService<IContentDialogService>();

            var result = dlg.Manager.ShowModalDialogExternal(parentWindow, customDialog
                                                                         , dlg.DialogSettings);

            if (result != 2) // user pressed cancel or not OK
                return;

            // user pressed cancel, press ESC or closed via (x) button
            if (result == DialogIntResults.CANCEL)
                return;

            var message = string.Format("Hello " + viewModel.Input + "! (result: {0})", result);

            dlg.MsgBox.Show(parentWindow, message, "Hello");
        }
    }
}
