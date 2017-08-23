namespace MDemo.Demos
{
    using MWindowDialogLib.Dialogs;
    using MWindowInterfacesLib.Events;
    using MWindowInterfacesLib.Interfaces;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using ViewModels.Tasks;

    public class ProgressDialogDemos : MDemo.ViewModels.Base.ModelBase
    {
        /// <summary>
        /// Shows a sample progress dialog that was invoked via a bound viewmodel.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="progressIsFinite"></param>
        /// <param name="closeDialogOnProgressFinished"></param>
        /// <param name="isCancelable"></param>
        internal async void ShowDialogFromVM(
              object context
            , bool progressIsFinite
            , bool closeDialogOnProgressFinished = false
            , bool isCancelable = true
            )
        {
            bool isVisible = true;
            string progressText = null;

            var progressColl = new ProgressSettings[1];

            // Configure a progress display with its basic settings
            progressColl[0] = new ProgressSettings(0, 1, 0, progressIsFinite
                                                  , progressText, isCancelable, isVisible
                                                  , closeDialogOnProgressFinished)
            {
                Title = "Progress from VM",
                Message = "Progressing all the things, wait a few seconds",
                ExecAction = GenCancelableSampleProcess()
            };

            var viewModel = new Demos.ViewModels.ProgressDialogViewModel();
            var customDialog = CreateProgressDialog(viewModel);

            var coord = GetService<IContentDialogService>().Coordinator;
            var manager = GetService<IContentDialogService>().Manager;

            EventHandler<DialogStateChangedEventArgs> OnViewOpenedEvent = (s, e) =>
            {
                // Start Task in ProgressViewModel and wait for result in Dialog below
                // But do not start before view is visible because task could otherwise
                // finish before view close request can be handled by the view ...
                viewModel.StartProcess(progressColl);
            };

            manager.DialogOpened += OnViewOpenedEvent;

            await coord.ShowMetroDialogAsync(context, customDialog).ContinueWith
            (
                t =>
                {
                    manager.DialogOpened -= OnViewOpenedEvent;
                    Console.WriteLine(t.Result);
                }
            );
        }

        internal void ShowDialogOutside(IMetroWindow parentWindow
            , bool progressIsFinite
            , bool closeDialogOnProgressFinished = false
            , bool isCancelable = true
            )
        {
            bool isVisible = true;
            string progressText = null;

            var progressColl = new ProgressSettings[1];

            // Configure a progress display with its basic settings
            progressColl[0] = new ProgressSettings(0, 1, 0, progressIsFinite
                                                , progressText, isCancelable, isVisible
                                                , closeDialogOnProgressFinished)
            {
                Title = "Progress Outside",
                Message = "This progress is shown in a seperate window above the main window",
                ExecAction = GenCancelableSampleProcess()
            };

            var viewModel = new Demos.ViewModels.ProgressDialogViewModel();
            var customDialog = CreateProgressDialog(viewModel);

            var dlg = GetService<IContentDialogService>();
            var manager = GetService<IContentDialogService>().Manager;

            EventHandler<DialogStateChangedEventArgs> OnViewOpenedEvent = (s, e) =>
            {
                // Start Task in ProgressViewModel and wait for result in Dialog below
                // But do not start before view is visible because task could otherwise
                // finish before view close request can be handled by the view ...
                viewModel.StartProcess(progressColl);
            };

            manager.DialogOpened += OnViewOpenedEvent;

            int result = -1;
            try
            {
                result = dlg.Manager.ShowModalDialogExternal(parentWindow, customDialog
                                                           , dlg.DialogSettings);
            }
            finally
            {
                manager.DialogOpened -= OnViewOpenedEvent;
            }

            Console.WriteLine("Process Result: '{0}'", viewModel.Progress.ProcessResult);
        }


        /// <summary>
        /// Method demos different sample methods for displaying a progress
        /// dialog with progress that cannot be cancelled because:
        /// 
        /// - UI shows no Cancel button
        /// - Backend process does not evaluate <seealso cref="CancellationToken"/> parameter
        /// </summary>
        /// <param name="parentWindow"></param>
        /// <param name="progressIsFinite"></param>
        /// <param name="closeDialogOnProgressFinished"></param>
        /// <param name="isCancelable"></param>
        /// <returns></returns>
        internal async Task<int> ShowNoCancelProgressAsync(
              IMetroWindow parentWindow
            , bool progressIsFinite
            , bool closeDialogOnProgressFinished = false
            , bool isCancelable = true
            )
        {
            bool isVisible = true;
            string progressText = null;

            var progressColl = new ProgressSettings[1];

            // Configure a progress display with its basic settings
            progressColl[0] = new ProgressSettings(0, 1, 0, progressIsFinite
                                                  , progressText, isCancelable, isVisible
                                                  , closeDialogOnProgressFinished)
            {
                Title = "Please wait...",
                Message = "We are baking some cupcakes!",
                ExecAction = GenSampleNonCancelableProocess()
            };

            var viewModel = new Demos.ViewModels.ProgressDialogViewModel();
            var customDialog = CreateProgressDialog(viewModel);

            var dlg = GetService<IContentDialogService>();
            var manager = dlg.Manager;

            EventHandler<DialogStateChangedEventArgs> OnViewOpenedEvent = (s, e) =>
            {
                // Start Task in ProgressViewModel and wait for result in Dialog below
                // But do not start before view is visible because task could otherwise
                // finish before view close request can be handled by the view ...
                viewModel.StartProcess(progressColl);
            };

            manager.DialogOpened += OnViewOpenedEvent;

            int result = -1;
            try
            {
                result = await manager.ShowMetroDialogAsync(parentWindow, customDialog);
            }
            finally
            {
                manager.DialogOpened -= OnViewOpenedEvent;
            }

            Console.WriteLine("Process Result: '{0}'", viewModel.Progress.ProcessResult);

            return result;
        }

        /// <summary>
        /// Method demos different sample methods for displaying a progress
        /// dialog with progress that can be cancelled because:
        /// 
        /// - UI shows a Cancel button
        /// - Backend process does evaluate <seealso cref="CancellationToken"/> parameter
        /// </summary>
        /// <param name="parentWindow"></param>
        /// <param name="progressIsFinite"></param>
        /// <param name="closeDialogOnProgressFinished"></param>
        /// <param name="isCancelable"></param>
        /// <returns></returns>
        internal async Task<int> ShowCancelProgressAsync(
            IMetroWindow parentWindow
            , bool progressIsFinite
            , bool closeDialogOnProgressFinished = false
            )
        {
            bool isVisible = true, isCancelable = true;
            string progressText = null;

            var progressColl = new ProgressSettings[1];

            // Configure a progress display with its basic settings
            progressColl[0] = new ProgressSettings(0, 1, 0, progressIsFinite
                                                  , progressText, isCancelable, isVisible
                                                  , closeDialogOnProgressFinished)
            {
                Title = "Please wait...",
                Message = "We are baking some cupcakes!",
                ExecAction = GenCancelableSampleProcess()
            };

            var viewModel = new Demos.ViewModels.ProgressDialogViewModel();
            var customDialog = CreateProgressDialog(viewModel);

            var dlg = GetService<IContentDialogService>();
            var manager = dlg.Manager;

            EventHandler<DialogStateChangedEventArgs> OnViewOpenedEvent = (s, e) =>
            {
                // Start Task in ProgressViewModel and wait for result in Dialog below
                // But do not start before view is visible because task could otherwise
                // finish before view close request can be handled by the view ...
                viewModel.StartProcess(progressColl);
            };

            manager.DialogOpened += OnViewOpenedEvent;

            int result = -1;
            try
            {
                result = await manager.ShowMetroDialogAsync(parentWindow, customDialog);
            }
            finally
            {
                manager.DialogOpened -= OnViewOpenedEvent;
            }

            Console.WriteLine("Process Result: '{0}'", viewModel.Progress.ProcessResult);

            return result;
        }

        /// <summary>
        /// Displays a first infinite progress that cannot be cancelled or closed.
        /// The display changes after a while into a finite progress display that
        /// can be cancelled or closed (when process is finished).
        /// </summary>
        /// <param name="parentWindow"></param>
        /// <param name="closeDialogOnProgressFinished"></param>
        /// <returns></returns>
        internal async Task<int> Show2CancelProgressAsync(IMetroWindow parentWindow
                                                        , bool closeDialogOnProgressFinished = false
                                                        )
        {
            bool isVisible = true;
            string progressText = null;

            var progressColl = new ProgressSettings[2];

            // Configure 1 progress display with its basic settings
            progressColl[0] = new ProgressSettings(0, 1, 0, true // IsInfinite
                                                  , progressText
                                                  , false        // isCancelable
                                                  , isVisible
                                                  , closeDialogOnProgressFinished)
            {
                Title = "Please wait...",
                Message = "We are baking some cupcakes!",
                ExecAction = GenCancelableSampleProcess()
            };

            // Configure 2nd progress display with its basic settings
            progressColl[1] = new ProgressSettings(0, 1, 0, false // IsInfinite
                                                  , progressText
                                                  , true        // isCancelable
                                                  , isVisible
                                                  , closeDialogOnProgressFinished)
            {
                Title = "Please wait... some more",
                Message = "We are baking some cupcakes!",
                ExecAction = GenCancelableSampleProcess()
            };

            var viewModel = new Demos.ViewModels.ProgressDialogViewModel();
            var customDialog = CreateProgressDialog(viewModel);

            var dlg = GetService<IContentDialogService>();
            var manager = dlg.Manager;

            EventHandler<DialogStateChangedEventArgs> OnViewOpenedEvent = (s, e) =>
            {
                // Start Task in ProgressViewModel and wait for result in Dialog below
                // But do not start before view is visible because task could otherwise
                // finish before view close request can be handled by the view ...
                viewModel.StartProcess(progressColl);
            };

            manager.DialogOpened += OnViewOpenedEvent;

            int result = -1;
            try
            {
                result = await manager.ShowMetroDialogAsync(parentWindow, customDialog);
            }
            finally
            {
                manager.DialogOpened -= OnViewOpenedEvent;
            }

            Console.WriteLine("Process Result: '{0}'", viewModel.Progress.ProcessResult);

            return result;
        }

        /// <summary>
        /// Generates a sample process that can take some time, is cancelable via
        /// <seealso cref="CancellationToken"/> and reports progress through the
        /// <seealso cref="IProgress"/> interface.
        /// </summary>
        /// <returns></returns>
        private Action<CancellationToken, IProgress> GenCancelableSampleProcess()
        {
            // Create an action that simulates a process that updates a progress view...
            Action<CancellationToken, IProgress> a =
                new Action<CancellationToken, IProgress>((ct, progress) =>
            {
                int iresult = 0;
                for (int i = 0; i < 10; i++)
                {
                    if(progress.ProgressIsFinite == false)
                        progress.ProgressText = string.Format("Step {0} of {1}...", i, 10-1);

                    Thread.Sleep(500);
                    progress.ProgressValue += 0.1;

                    iresult = i;

                    if (ct != null)
                        ct.ThrowIfCancellationRequested();
                }

                var printResult = string.Format("Backed {0} cup cakes.", iresult);

                if (progress.ProgressIsFinite == false)
                    progress.ProgressText = printResult;

                progress.ProcessResult = printResult;
                return;
            });

            return a;
        }

        /// <summary>
        /// Generates a sample process that can take some time, is not cancelable
        /// but reports progress through the <seealso cref="IProgress"/> interface.
        /// 
        /// Note: The generated action is the same as in the cancelable case (because
        /// a <seealso cref="CancellationToken"/> is always generated and supplied
        /// but the process generated here does not evaluate the <seealso cref="CancellationToken"/>
        /// makeing this process as a result non-cancelable.
        /// </summary>
        /// <returns></returns>
        private Action<CancellationToken, IProgress> GenSampleNonCancelableProocess()
        {
            // Create an action that simulates a process that updates a progress view...
            Action<CancellationToken, IProgress> a =
                new Action<CancellationToken, IProgress>((ct, progress) =>
            {
                int iresult = 0;
                for (int i = 0; i < 10; i++)
                {
                    if (progress.ProgressIsFinite == false)
                        progress.ProgressText = string.Format("Step {0} of {1}...", i, 10 - 1);

                    //progress.ProgressText = string.Format("Step {0} of {1}...", i, 10-1);
                    Thread.Sleep(500);
                    progress.ProgressValue += 0.1;
                    iresult = i;
                }

                var printResult = string.Format("Backed {0} cup cakes.", iresult);

                if (progress.ProgressIsFinite == false)
                    progress.ProgressText = printResult;

                progress.ProcessResult = printResult;
                return;
            });

            return a;
        }

        /// <summary>
        /// Creates a <seealso cref="MWindowDialogLib.Dialogs.CustomDialog"/> that contains a <seealso cref="ProgressView"/>
        /// in its content and has a <seealso cref="ProgressViewModel"/> attached to its datacontext,
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        private CustomDialog CreateProgressDialog(Demos.ViewModels.ProgressDialogViewModel viewModel)
        {
            return new MWindowDialogLib.Dialogs.CustomDialog(new Demos.Views.ProgressView(), viewModel);
        }
    }
}
