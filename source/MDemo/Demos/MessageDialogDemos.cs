namespace MDemo.Demos
{
    using MWindowInterfacesLib.Interfaces;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using ViewModels;

    public class MessageDialogDemos : MDemo.ViewModels.Base.ModelBase
    {
        internal async void ShowDialog(IMetroWindow parentWindow)
        {
            var viewModel = new MessageDialogViewModel()
            {
                Title = "Hello!",
                Message = "Welcome to the world of metro!",

                DefaultResult = (int)ButtonList.NegativButtonValue
            };

            viewModel.SetCaption((int)ButtonList.AffirmativeButtonValue, "Hi");
            viewModel.SetCaption((int)ButtonList.NegativButtonValue, "Go away!");
            viewModel.SetCaption((int)ButtonList.FirstAuxilaryButtonValue, "Cancel");

            var customDialog = new MWindowDialogLib.Dialogs.CustomDialog(new Demos.Views.MessageView(), viewModel);

            var dlg = GetService<IContentDialogService>();
            var manager = dlg.Manager;

            var result = await manager.ShowMetroDialogAsync(parentWindow, customDialog);

            // user pressed cancel, press ESC or closed via (x) button
            if (result != (int)ButtonList.FirstAuxilaryButtonValue)
            {
                var answer = string.Format("You said: " + viewModel.ConvertResultToString(result));

                await dlg.MsgBox.ShowAsync(parentWindow, answer, "Result");
            }
        }

        internal async void ShowLimitedDialog(IMetroWindow parentWindow)
        {
            var viewModel = new MessageDialogViewModel()
            {
                Title = "Hello!",
                Message = "Welcome to the world of metro!" + string.Join(Environment.NewLine, "abc", "def", "ghi", "jkl", "mno", "pqr", "stu", "vwx", "yz"),

                DefaultResult = (int)ButtonList.NegativButtonValue,
                MaximumBodyHeight = 100
            };

            viewModel.SetCaption((int)ButtonList.AffirmativeButtonValue, "Hi");
            viewModel.SetCaption((int)ButtonList.NegativButtonValue, "Go away!");
            viewModel.SetCaption((int)ButtonList.FirstAuxilaryButtonValue, "Cancel");

            var customDialog = new MWindowDialogLib.Dialogs.CustomDialog(new Demos.Views.MessageView(), viewModel);

            var dlg = GetService<IContentDialogService>();
            var manager = dlg.Manager;

            var result = await manager.ShowMetroDialogAsync(parentWindow, customDialog);

            // user pressed cancel, press ESC or closed via (x) button
            if (result != (int)ButtonList.FirstAuxilaryButtonValue)
            {
                var answer = string.Format("You said: " + viewModel.ConvertResultToString(result));

                await dlg.MsgBox.ShowAsync(parentWindow, answer, "Result");
            }
        }

        /// <summary>
        /// Shows the dialog in a seperate window displayed over the current main window.
        /// </summary>
        /// <param name="parentWindow"></param>
        internal void ShowDialogOutside(IMetroWindow parentWindow)
        {
            var viewModel = new MessageDialogViewModel()
            {
                Title = "Hello!",
                Message = "Welcome to the world of metro!" + string.Join(Environment.NewLine, "abc", "def", "ghi", "jkl", "mno", "pqr", "stu", "vwx", "yz"),

                DefaultResult = (int)ButtonList.NegativButtonValue,
            };

            viewModel.SetCaption((int)ButtonList.AffirmativeButtonValue, "Hi");
            viewModel.SetCaption((int)ButtonList.NegativButtonValue, "Go away!");
            viewModel.SetCaption((int)ButtonList.FirstAuxilaryButtonValue, "Cancel");

            var customDialog = new MWindowDialogLib.Dialogs.CustomDialog(new Demos.Views.MessageView(), viewModel);

            var dlg = GetService<IContentDialogService>();
            var manager = dlg.Manager;

            var result = manager.ShowModalDialogExternal(parentWindow, customDialog
                                                                     , dlg.DialogSettings);

            // user pressed cancel, press ESC or closed via (x) button
            if (result != (int)ButtonList.FirstAuxilaryButtonValue)
            {
                var answer = string.Format("You said: " + viewModel.ConvertResultToString(result));

                dlg.MsgBox.Show(parentWindow, answer, "Result");
            }
        }

        #region Demo Dialogs Created by UI or BAckground Thread
        internal Action ShowDialogFromVM(object context, string startingThread)
        {
            return () =>
            {
                var message = $"MVVM based messages!\n\nThis dialog was created by {startingThread} Thread with ID=\"{Thread.CurrentThread.ManagedThreadId}\"\n" +
                              $"The current DISPATCHER_THREAD Thread has the ID=\"{Application.Current.Dispatcher.Thread.ManagedThreadId}\"";

                var viewModel = new MessageDialogViewModel()
                {
                    Title = $"Message from VM created by {startingThread}",
                    Message = message,

                    DefaultResult = (int)ButtonList.AffirmativeButtonValue,
                };
                viewModel.SetCaption((int)ButtonList.AffirmativeButtonValue, "OK");

                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    var customDialog = new MWindowDialogLib.Dialogs.CustomDialog(new Demos.Views.MessageView(), viewModel);

                    var coord = GetService<IContentDialogService>().Coordinator;

                    coord.ShowMetroDialogAsync(context, customDialog).ContinueWith(t => Console.WriteLine(t.Result));
                }));
            };
        }

        internal void PerformDialogCoordinatorAction(Action action, bool runInMainThread)
        {
            if (!runInMainThread)
            {
                Task.Factory.StartNew(action);
            }
            else
            {
                action();
            }
        }
        #endregion
    }
}
