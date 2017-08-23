namespace MWindowDialogLib.Internal
{
    using MWindowInterfacesLib;
    using MWindowInterfacesLib.Enums;
    using MWindowInterfacesLib.Events;
    using MWindowInterfacesLib.Interfaces;
    using MWindowInterfacesLib.MsgBox.Enums;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using Util;

    internal class DialogManager : IDialogManager
    {
        /// <summary>
        /// Gets the default instance of the dialog manager.
        /// </summary>
        public static readonly IDialogManager Instance = new DialogManager();

        private readonly IMetroWindowService _metroWindowService = new MetroWindowServiceImpl();

        #region constructors
        /// <summary>
        /// Class Constructor
        /// </summary>
        public DialogManager()
        {
        }
        #endregion constructors

        #region events
        public event EventHandler<DialogStateChangedEventArgs> DialogOpened;
        public event EventHandler<DialogStateChangedEventArgs> DialogClosed;
        #endregion events

        #region methods
        /// <summary>
        /// Creates a MsgBoxDialog inside of the current window.
        /// </summary>
        /// <param name="window">The MetroWindow</param>
        /// <param name="title">The title of the MessageDialog.</param>
        /// <param name="message">The message contained within the MessageDialog.</param>
        /// <param name="style">The type of buttons to use.</param>
        /// <param name="settings">Optional settings that override the global metro dialog settings.</param>
        /// <returns>A task promising the result of which button was pressed.</returns>
        public Task<MsgBoxResult> ShowMsgBoxAsync(
              IMetroWindow metroWindow
            , IMsgBoxDialogFrame<MsgBoxResult> dialog
            , IMetroDialogFrameSettings settings = null)
        {
            metroWindow.Dispatcher.VerifyAccess();
            return this.HandleOverlayOnShow(metroWindow, settings).ContinueWith(z =>
            {
                return (Task<MsgBoxResult>)metroWindow.Dispatcher.Invoke(new Func<Task<MsgBoxResult>>(() =>
                {
                    settings = settings ?? new MetroDialogFrameSettings();

                    SizeChangedEventHandler sizeHandler = this.SetupAndOpenDialog(metroWindow, dialog);
                    dialog.SizeChangedHandler = sizeHandler;

                    // Call this method in the dialog to wait until the dialog is closing ...
                    return dialog.WaitForLoadAsync().ContinueWith(x =>
                    {
                        if (DialogOpened != null)
                        {
                            metroWindow.Dispatcher.BeginInvoke(new Action(() => DialogOpened(this, new DialogStateChangedEventArgs())));
                        }

                        return dialog.WaitForButtonPressAsync().ContinueWith(y =>
                        {
                            //once a button as been clicked, begin removing the dialog.

                            dialog.OnClose();

                            if (DialogClosed != null)
                            {
                                metroWindow.Dispatcher.BeginInvoke(new Action(() => DialogClosed(this, new DialogStateChangedEventArgs())));
                            }

                            Task closingTask = (Task)metroWindow.Dispatcher.Invoke(new Func<Task>(() => dialog._WaitForCloseAsync()));
                            return closingTask.ContinueWith(a =>
                            {
                                return ((Task)metroWindow.Dispatcher.Invoke(new Func<Task>(() =>
                                {
                                    metroWindow.SizeChanged -= sizeHandler;

                                    this.RemoveDialog(metroWindow, dialog);

                                    return this.HandleOverlayOnHide(metroWindow, settings);
                                }))).ContinueWith(y3 => y).Unwrap();
                            });
                        }).Unwrap();
                    }).Unwrap().Unwrap();
                }));
            }).Unwrap();
        }

        /// <summary>
        /// Hides a visible Metro Dialog instance.
        /// </summary>
        /// <param name="window">The window with the dialog that is visible.</param>
        /// <param name="dialog">The dialog instance to hide.</param>
        /// <param name="settings">An optional pre-defined settings instance.</param>
        /// <returns>A task representing the operation.</returns>
        /// <exception cref="InvalidOperationException">
        /// The <paramref name="dialog"/> is not visible in the window.
        /// This happens if <see cref="ShowMetroDialogAsync"/> hasn't been called before.
        /// </exception>
        public Task HideMetroDialogAsync(
              IMetroWindow metroWindow
            , IBaseMetroDialogFrame dialog
            , IMetroDialogFrameSettings settings = null)
        {
            metroWindow.Dispatcher.VerifyAccess();
            if (!metroWindow.MetroActiveDialogContainer.Children.Contains(dialog as UIElement) && !metroWindow.MetroInactiveDialogContainer.Children.Contains(dialog as UIElement))
                throw new InvalidOperationException("The provided dialog is not visible in the specified window.");

            metroWindow.SizeChanged -= dialog.SizeChangedHandler;

            dialog.OnClose();

            Task closingTask = (Task)metroWindow.Dispatcher.Invoke(new Func<Task>(dialog._WaitForCloseAsync));
            return closingTask.ContinueWith(a =>
            {
                if (DialogClosed != null)
                {
                    metroWindow.Dispatcher.BeginInvoke(new Action(() => DialogClosed(this, new DialogStateChangedEventArgs())));
                }

                return (Task)metroWindow.Dispatcher.Invoke(new Func<Task>(() =>
                {
                    this.RemoveDialog(metroWindow, dialog);

                    return this.HandleOverlayOnHide(metroWindow, settings);
                }));
            }).Unwrap();
        }

        /// <summary>
        /// Adds a Metro Dialog instance to the specified window and makes it visible asynchronously.
        /// If you want to wait until the user has closed the dialog, use <see cref="ShowMetroDialogAsyncAwaitable"/>
        /// <para>You have to close the resulting dialog yourself with <see cref="HideMetroDialogAsync"/>.</para>
        /// </summary>
        /// <param name="window">The owning window of the dialog.</param>
        /// <param name="dialog">The dialog instance itself.</param>
        /// <param name="settings">An optional pre-defined settings instance.</param>
        /// <returns>A task representing the operation.</returns>
        /// <exception cref="InvalidOperationException">The <paramref name="dialog"/> is already visible in the window.</exception>
        public Task ShowMetroDialogAsync(
              IMetroWindow metroWindow
            , IBaseMetroDialogFrame dialog
            , IMetroDialogFrameSettings settings = null)
        {
            metroWindow.Dispatcher.VerifyAccess();
            if (metroWindow.MetroActiveDialogContainer.Children.Contains(dialog as UIElement) || metroWindow.MetroInactiveDialogContainer.Children.Contains(dialog as UIElement))
                throw new InvalidOperationException("The provided dialog is already visible in the specified window.");

            return this.HandleOverlayOnShow(metroWindow, settings).ContinueWith(z =>
            {
                return (Task)metroWindow.Dispatcher.Invoke(new Func<Task>(() =>
                {
                    settings = settings ?? new MetroDialogFrameSettings();

                    SizeChangedEventHandler sizeHandler = this.SetupAndOpenDialog(metroWindow, dialog);
                    dialog.SizeChangedHandler = sizeHandler;

                    return dialog.WaitForLoadAsync().ContinueWith(x =>
                    {
                        dialog.OnShown();

                        if (DialogOpened != null)
                        {
                            metroWindow.Dispatcher.BeginInvoke(new Action(() => DialogOpened(this, new DialogStateChangedEventArgs())));
                        }
                    });
                }));
            }).Unwrap();
        }

        public Task<int> ShowMetroDialogAsync(IMetroWindow metroWindow
                                        , IMsgBoxDialogFrame<int> dialog
                                        , IMetroDialogFrameSettings settings = null)
        {
            metroWindow.Dispatcher.VerifyAccess();
            return this.HandleOverlayOnShow(metroWindow, settings).ContinueWith(z =>
            {
                return (Task<int>)metroWindow.Dispatcher.Invoke(new Func<Task<int>>(() =>
                {
                    settings = settings ?? new MetroDialogFrameSettings();

                    SizeChangedEventHandler sizeHandler = SetupAndOpenDialog(metroWindow, dialog);
                    dialog.SizeChangedHandler = sizeHandler;

                    return dialog.WaitForLoadAsync().ContinueWith(x =>
                    {
                        if (DialogOpened != null)
                        {
                            metroWindow.Dispatcher.BeginInvoke(new Action(() => DialogOpened(metroWindow, new DialogStateChangedEventArgs())));
                        }

                        return dialog.WaitForButtonPressAsync().ContinueWith(y =>
                        {
                            //once a button as been clicked, begin removing the dialog.

                            dialog.OnClose();

                            if (DialogClosed != null)
                            {
                                metroWindow.Dispatcher.BeginInvoke(new Action(() => DialogClosed(metroWindow, new DialogStateChangedEventArgs())));
                            }

                            Task closingTask = (Task)metroWindow.Dispatcher.Invoke(new Func<Task>(() => dialog._WaitForCloseAsync()));
                            return closingTask.ContinueWith(a =>
                            {
                                return ((Task)metroWindow.Dispatcher.Invoke(new Func<Task>(() =>
                                {
                                    metroWindow.SizeChanged -= sizeHandler;

                                    this.RemoveDialog(metroWindow, dialog);

                                    return this.HandleOverlayOnHide(metroWindow, settings);
                                }))).ContinueWith(y3 => y).Unwrap();
                            });
                        }).Unwrap();
                    }).Unwrap().Unwrap();
                }));
            }).Unwrap();
        }

        #region Modal External dialog
        /// <summary>
        /// Creates a InputDialog outside of the current window.
        /// </summary>
        /// <param name="window">The MetroWindow</param>
        /// <param name="title">The title of the MessageDialog.</param>
        /// <param name="message">The message contained within the MessageDialog.</param>
        /// <param name="settings">Optional settings that override the global metro dialog settings.</param>
        /// <returns>The text that was entered or null (Nothing in Visual Basic) if the user cancelled the operation.</returns>
        ////        public string ShowModalInputExternal(
        ////            IMetroWindow metroWindow
        ////          , string title
        ////          , string message
        ////          , Window win
        ////          , IMetroDialogFrameSettings settings = null)
        ////        {
        ////            win = CreateModalExternalWindow(metroWindow, win);
        ////
        ////            settings = settings ?? new MetroDialogFrameSettings();
        ////
        ////            //create the dialog control
        ////            var dialog = new InputDialog(metroWindow, settings)
        ////            {
        ////                Message = message,
        ////                Title = title,
        ////                Input = settings.DefaultText
        ////            };
        ////
        ////            ////SetDialogFontSizes(settings, dialog);
        ////
        ////            win.Content = dialog;
        ////
        ////            string result = null;
        ////            dialog.WaitForButtonPressAsync().ContinueWith(task =>
        ////            {
        ////                result = task.Result;
        ////                win.Invoke(win.Close);
        ////            });
        ////
        ////            HandleOverlayOnShow(metroWindow, settings);
        ////            win.ShowDialog();
        ////            HandleOverlayOnHide(metroWindow, settings);
        ////            return result;
        ////        }

        /// <summary>
        /// Creates a MessageDialog ouside of the current window.
        /// </summary>
        /// <param name="window">The MetroWindow</param>
        /// <param name="title">The title of the MessageDialog.</param>
        /// <param name="message">The message contained within the MessageDialog.</param>
        /// <param name="style">The type of buttons to use.</param>
        /// <param name="settings">Optional settings that override the global metro dialog settings.</param>
        /// <returns>A task promising the result of which button was pressed.</returns>
        ////        public MessageDialogResult ShowModalMessageExternal(
        ////              IMetroWindow metroWindow
        ////            , string title
        ////            , string message
        ////            , Window dialogWindow
        ////            , MessageDialogStyle style = MessageDialogStyle.Affirmative
        ////            , IMetroDialogFrameSettings settings = null)
        ////        {
        ////            dialogWindow = CreateModalExternalWindow(metroWindow, dialogWindow);
        ////
        ////            settings = settings ?? new MetroDialogFrameSettings();
        ////
        ////            //create the dialog control
        ////            var dialog = new MessageDialog(metroWindow, settings)
        ////            {
        ////                Message = message,
        ////                Title = title,
        ////                ButtonStyle = style
        ////            };
        ////
        ////            ////SetDialogFontSizes(settings, dialog);
        ////
        ////            dialogWindow.Content = dialog;
        ////
        ////            MessageDialogResult result = MessageDialogResult.Affirmative;
        ////            dialog.WaitForButtonPressAsync().ContinueWith(task =>
        ////            {
        ////                result = task.Result;
        ////                dialogWindow.Invoke(dialogWindow.Close);
        ////            });
        ////
        ////            HandleOverlayOnShow(metroWindow, settings);
        ////            dialogWindow.ShowDialog();
        ////            HandleOverlayOnHide(metroWindow, settings);
        ////            return result;
        ////        }

        /// <summary>
        /// Creates a LoginDialog outside of the current window.
        /// </summary>
        /// <param name="metroWindow">The window that is the parent of the dialog.</param>
        /// <param name="title">The title of the LoginDialog.</param>
        /// <param name="message">The message contained within the LoginDialog.</param>
        /// <param name="dialogWindow"></param>
        /// <param name="settings">Optional settings that override the global metro dialog settings.</param>
        /// <returns>The text that was entered or null (Nothing in Visual Basic) if the user cancelled the operation.</returns>
        ////        public ILoginDialogData ShowModalLoginExternal(
        ////              IMetroWindow metroWindow
        ////            , string title
        ////            , string message
        ////            , Window dialogWindow
        ////            , ILoginDialogSettings settings = null)
        ////        {
        ////            dialogWindow = CreateModalExternalWindow(metroWindow, dialogWindow);
        ////
        ////            settings = settings ?? new LoginDialogSettings();
        ////
        ////            //create the dialog control
        ////            LoginDialog dialog = new LoginDialog(metroWindow, settings)
        ////            {
        ////                Title = title,
        ////                Message = message
        ////            };
        ////
        ////            ////SetDialogFontSizes(settings, dialog);
        ////
        ////            dialogWindow.Content = dialog;
        ////
        ////            ILoginDialogData result = null;
        ////            dialog.WaitForButtonPressAsync().ContinueWith(task =>
        ////            {
        ////                result = task.Result;
        ////                dialogWindow.Invoke(dialogWindow.Close);
        ////            });
        ////
        ////            HandleOverlayOnShow(metroWindow, settings);
        ////            dialogWindow.ShowDialog();
        ////            HandleOverlayOnHide(metroWindow, settings);
        ////
        ////            return result;
        ////        }
        #endregion Modal External dialog

        private void AddDialog(IMetroWindow metroWindow
                             , IBaseMetroDialogFrame dialog)
        {
            metroWindow.StoreFocus();

            // if there's already an active dialog, move to the background
            var activeDialog = metroWindow.MetroActiveDialogContainer.Children.Cast<UIElement>().SingleOrDefault();
            if (activeDialog != null)
            {
                metroWindow.MetroActiveDialogContainer.Children.Remove(activeDialog);
                metroWindow.MetroInactiveDialogContainer.Children.Add(activeDialog);
            }

            // add the dialog to the container}
            metroWindow.MetroActiveDialogContainer.Children.Add(dialog as UIElement);
        }

        private void RemoveDialog(IMetroWindow metroWindow
                                , IBaseMetroDialogFrame dialog)
        {
            if (metroWindow.MetroActiveDialogContainer.Children.Contains(dialog as UIElement))
            {
                // remove the dialog from the container
                metroWindow.MetroActiveDialogContainer.Children.Remove(dialog as UIElement);

                // if there's an inactive dialog, bring it to the front
                var dlg = metroWindow.MetroInactiveDialogContainer.Children.Cast<UIElement>().LastOrDefault();
                if (dlg != null)
                {
                    metroWindow.MetroInactiveDialogContainer.Children.Remove(dlg);
                    metroWindow.MetroActiveDialogContainer.Children.Add(dlg);
                }
            }
            else
            {
                metroWindow.MetroInactiveDialogContainer.Children.Remove(dialog as UIElement);
            }

            if (metroWindow.MetroActiveDialogContainer.Children.Count == 0)
            {
                metroWindow.RestoreFocus();
            }
        }

        private Task HandleOverlayOnHide(IMetroWindow metroWindow
                                       , IMetroDialogFrameSettings settings)
        {
            if (!metroWindow.MetroActiveDialogContainer.Children.OfType<IBaseMetroDialogFrame>().Any())
            {
                return (settings == null || settings.AnimateHide ? metroWindow.HideOverlayAsync() : Task.Factory.StartNew(() => metroWindow.Dispatcher.Invoke(new Action(metroWindow.HideOverlay))));
            }
            else
            {
                var tcs = new System.Threading.Tasks.TaskCompletionSource<object>();
                tcs.SetResult(null);
                return tcs.Task;
            }
        }

        private Task HandleOverlayOnShow(IMetroWindow metroWindow
                                       , IMetroDialogFrameSettings settings)
        {
            if (!metroWindow.MetroActiveDialogContainer.Children.OfType<IBaseMetroDialogFrame>().Any())
            {
                return (settings == null || settings.AnimateShow ? metroWindow.ShowOverlayAsync() : Task.Factory.StartNew(() => metroWindow.Dispatcher.Invoke(new Action(metroWindow.ShowOverlay))));
            }
            else
            {
                var tcs = new System.Threading.Tasks.TaskCompletionSource<object>();
                tcs.SetResult(null);
                return tcs.Task;
            }
        }

        private SizeChangedEventHandler SetupAndOpenDialog(
              IMetroWindow metroWindow
            , IBaseMetroDialogFrame dialog)
        {
            dialog.SetZIndex((int)metroWindow.OverlayBox.GetValue(Panel.ZIndexProperty) + 1);

            dialog.MinHeight = metroWindow.ActualHeight / 4.0;
            dialog.MaxHeight = metroWindow.ActualHeight;

            SizeChangedEventHandler sizeHandler = (sender, args) =>
            {
                dialog.MinHeight = metroWindow.ActualHeight / 4.0;
                dialog.MaxHeight = metroWindow.ActualHeight;
            };

            metroWindow.SizeChanged += sizeHandler;

            this.AddDialog(metroWindow, dialog);

            dialog.OnShown();

            return sizeHandler;
        }

        #region Modal Outside Window Methods

        /// <summary>
        /// Creates a modal dialog outside of the current main window.
        /// </summary>
        /// <param name="metroWindow">The MetroWindow</param>
        /// <param name="dlgControl">The outside modal window to be owned by a given <seealso cref="IMetroWindow"/></param>
        /// <param name="settings">Optional settings that override the global metro dialog settings.</param>
        /// <returns>The result that was entered or 0 if the user escape keyed the dialog...</returns>
        public int ShowModalDialogExternal(
              IMetroWindow metroWindow
            , IMsgBoxDialogFrame<int> dlgControl
            , IMetroDialogFrameSettings settings = null)
        {
            settings = settings ?? new MetroDialogFrameSettings();

            // Create the outter dialog window that hosts the dialog control
            var dlgWindow = _metroWindowService.CreateExternalWindow();

            // The window is visible on top of the mainWindow
            dlgWindow = CreateModalExternalWindow(metroWindow, dlgWindow);

            // Release the dialog opened event if there are any subscribers
            if (this.DialogOpened != null)
                metroWindow.Dispatcher.BeginInvoke(new Action(() => DialogOpened(this, new DialogStateChangedEventArgs())));

            if (settings.MsgBoxMode == StaticMsgBoxModes.ExternalMoveable)
            {
                // Relay drag event from thumb to outer window to let user drag the dialog
                if (dlgControl.DialogThumb != null && dlgWindow is IMetroWindow)
                    ((IMetroWindow)dlgWindow).SetWindowEvents(dlgControl.DialogThumb);
            }

            dlgWindow.Content = dlgControl;

            int result = 0;
            dlgControl.WaitForButtonPressAsync().ContinueWith(task =>
            {
                result = task.Result;
                dlgWindow.Invoke(dlgWindow.Close);
            });

            HandleOverlayOnShow(metroWindow, settings);
            dlgWindow.ShowDialog();

            // Release the dialog closed event if there are any subscribers
            if (this.DialogClosed != null)
                metroWindow.Dispatcher.BeginInvoke(new Action(() => DialogClosed(this, new DialogStateChangedEventArgs())));

            HandleOverlayOnHide(metroWindow, settings);

            return result;
        }

        /// <summary>
        /// Creates an External MsgBox dialog outside of the main window.
        /// </summary>
        /// <param name="metroWindow"></param>
        /// <param name="dlgControl"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public MsgBoxResult ShowModalDialogExternal(
              IMetroWindow metroWindow
            , IMsgBoxDialogFrame<MsgBoxResult> dlgControl
            , IMetroDialogFrameSettings settings = null)
        {
            settings = settings ?? new MetroDialogFrameSettings();

            // Create the outter dialog window that hosts the dialog control
            var dlgWindow = _metroWindowService.CreateExternalWindow();

            // The window is visible on top of the mainWindow
            dlgWindow = CreateModalExternalWindow(metroWindow, dlgWindow);

            if (settings.MsgBoxMode == StaticMsgBoxModes.ExternalMoveable)
            {
                // Relay drag event from thumb to outer window to let user drag the dialog
                if (dlgControl.DialogThumb != null && dlgWindow is IMetroWindow)
                    ((IMetroWindow)dlgWindow).SetWindowEvents(dlgControl.DialogThumb);
            }

            dlgWindow.Content = dlgControl;

            MsgBoxResult result = MsgBoxResult.None;
            dlgControl.WaitForButtonPressAsync().ContinueWith(task =>
            {
                result = task.Result;
                dlgWindow.Invoke(dlgWindow.Close);
            });

            HandleOverlayOnShow(metroWindow, settings);
            dlgWindow.ShowDialog();
            HandleOverlayOnHide(metroWindow, settings);

            return result;
        }

        /// <summary>
        /// Attempts to attach a given dialog window to a given
        /// owner main window and returns the dialog window instance.
        /// </summary>
        /// <param name="metroWindow"></param>
        /// <param name="dialogWindow"></param>
        /// <returns></returns>
        private Window CreateModalExternalWindow(
              IMetroWindow metroWindow
            , Window dialogWindow)
        {
            if (metroWindow == null)
                return dialogWindow;

            if (dialogWindow != null &&
                metroWindow != null &&
                dialogWindow != metroWindow)
            {
                dialogWindow.Owner = metroWindow as Window;
            }

            // It is not necessary here because the owner is set above
            dialogWindow.Topmost = false;

            // WindowStartupLocation should be CenterOwner
            dialogWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            // Set Width and Height maximum according to Owner window
            dialogWindow.Width = metroWindow.ActualWidth;
            dialogWindow.MaxHeight = metroWindow.ActualHeight;
            dialogWindow.SizeToContent = SizeToContent.Height;

            return dialogWindow;
        }
        #endregion Modal Outside Window Methods
        #endregion methods
    }
}
