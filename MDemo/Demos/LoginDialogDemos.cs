namespace MDemo.Demos
{
    using Behaviours;
    using MWindowDialogLib.Dialogs;
    using MWindowInterfacesLib.Interfaces;
    using System;

    public class LoginDialogDemos : MDemo.ViewModels.Base.ModelBase
    {
        internal async void ShowDialogFromVM(object context
                                            , bool isNegativeButtonVisible = true
                                            , bool isUserNameVisible = true)
        {
            var customDialog = CreateLoginDialog(isNegativeButtonVisible, isUserNameVisible);

            var coord = GetService<IContentDialogService>().Coordinator;

            await coord.ShowMetroDialogAsync(context, customDialog).ContinueWith(t => Console.WriteLine(t.Result));
        }

        internal async void ShowDialog(IMetroWindow parentWindow
                                    , bool isNegativeButtonVisible = true
                                    , bool isUserNameVisible = true)
        {
            var customDialog = CreateLoginDialog(isNegativeButtonVisible, isUserNameVisible);
            var viewModel = customDialog.DataContext as Demos.ViewModels.LoginDialogViewModel;

            var dlg = GetService<IContentDialogService>();
            var manager = dlg.Manager;

            var result = await manager.ShowMetroDialogAsync(parentWindow, customDialog);

            if (result == DialogIntResults.OK)
            {
                var msgResult = await dlg.MsgBox.ShowAsync(parentWindow
                    , String.Format("Username: {0}\nPassword: {1}", viewModel.Username, PasswordBoxTextChanged.ConvertToUnsecureString(viewModel.Password))
                    , "Authentication Information");
            }
        }


        /// <summary>
        /// Shows the dialog in a seperate window displayed over the current main window.
        /// </summary>
        /// <param name="parentWindow"></param>
        internal void ShowDialogOutside(IMetroWindow parentWindow
                                    , bool isNegativeButtonVisible = true
                                    , bool isUserNameVisible = true)
        {
            var customDialog = CreateLoginDialog(isNegativeButtonVisible, isUserNameVisible);
            var viewModel = customDialog.DataContext as Demos.ViewModels.LoginDialogViewModel;

            var dlg = GetService<IContentDialogService>();

            var result = dlg.Manager.ShowModalDialogExternal(parentWindow, customDialog
                                                                         , dlg.DialogSettings);

            if (result != DialogIntResults.OK) // user pressed cancel or not OK
                return;

            // user pressed cancel, press ESC or closed via (x) button
            if (result == DialogIntResults.CANCEL)
                return;

            dlg.MsgBox.Show(parentWindow
                , String.Format("Username: {0}\nPassword: {1}"
                               , viewModel.Username
                               , PasswordBoxTextChanged.ConvertToUnsecureString(viewModel.Password))
                , "Authentication Information");
        }

        private CustomDialog CreateLoginDialog(bool isNegativeButtonVisible = true
                                             , bool isUserNameVisible = true)
        {
            var viewModel = new Demos.ViewModels.LoginDialogViewModel()
            {
                Title = "Authentication",
                Message = "Enter your credentials",

                IsUserNameVisible = isUserNameVisible,
                Username = "MLib",
                AffirmativeButtonText = "Login",

                IsNegativeButtonButtonVisible = isNegativeButtonVisible,
                NegativeButtonText = "Cancel",
                DefaultResult = DialogIntResults.OK  // Return Key => OK Clicked
            };

            return new MWindowDialogLib.Dialogs.CustomDialog(new Demos.Views.LoginView(), viewModel);
        }
    }
}
