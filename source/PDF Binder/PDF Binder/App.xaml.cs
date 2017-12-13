namespace PDF_Binder
{
    using Doc.DocManager.Interfaces;
    using log4net;
    using log4net.Config;
    using MLib.Interfaces;
    using Models;
    using Settings.Interfaces;
    using Settings.UserProfile;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading;
    using System.Windows;
    using ViewModels;
    using ViewModels.VMManagement;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region fields
        protected static log4net.ILog Logger;

        private MainWindow _mainWindow = null;
        #endregion fields

        #region constructors
        static App()
        {
            XmlConfigurator.Configure();
            Logger = LogManager.GetLogger("default");

            // Create service model to ensure available services
            ServiceInjector.InjectServices();
        }
        #endregion constructors

        #region methods
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            ViewModels.AppViewModel appVM = null;
            try
            {
                // Set shutdown mode here (and reset further below) to enable showing custom dialogs (messageboxes)
                // durring start-up without shutting down application when the custom dialogs (messagebox) closes
                ShutdownMode = System.Windows.ShutdownMode.OnExplicitShutdown;
            }
            catch
            {
            }

            var settings = GetService<ISettingsManager>(); // add the default themes
            var appearance = GetService<IAppearanceManager>();
            AppLifeCycleViewModel lifeCycle = null;

            try
            {
                lifeCycle = new AppLifeCycleViewModel();
                lifeCycle.LoadConfigOnAppStartup(settings, appearance);

                appearance.SetTheme(settings.Themes
                                    , settings.Options.GetOptionValue<string>("Appearance", "ThemeDisplayName")
                                    , ThemeViewModel.GetCurrentAccentColor(settings));

                // Construct Application ViewMOdel and mainWindow
                appVM = new ViewModels.AppViewModel(lifeCycle);
                appVM.SetSessionData(settings.SessionData);

                var vmManager = GetService<IVMManager>();
                vmManager.AddVMItem(VMItemKeys.ApplicationViewModel, "ApplicationViewModel", appVM);
                appVM.SetCurrentViewModel(VMItemKeys.ApplicationViewModel);

////                // Customize services specific items for this application
////                // Program message box service for Modern UI (Metro Light and Dark)
////                var msgBox = GetService<IMessageBoxService>();
////                msgBox.Style = MsgBoxStyle.WPFThemed;
            }
            catch
            {
            }

            try
            {
                var selectedLanguage = settings.Options.GetOptionValue<string>("Options", "LanguageSelected");

                Thread.CurrentThread.CurrentCulture = new CultureInfo(selectedLanguage);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(selectedLanguage);
            }
            catch
            {
            }

            // Create the optional appearance viewmodel and apply
            // current settings to start-up with correct colors etc...
            ////var appearSettings = new AppearanceViewModel(settings.Themes);
            ////appearSettings.ApplyOptionsFromModel(settings.Options);

            // Initialize WPF theming and friends ...
            appVM.InitForMainWindow(GetService<IAppearanceManager>()
                                , settings.Options.GetOptionValue<string>("Appearance", "ThemeDisplayName"));

            Application.Current.MainWindow = _mainWindow = new MainWindow();
            MainWindow.DataContext = appVM;

            AppCore.CreateAppDataFolder();

            if (MainWindow != null && appVM != null)
            {
                // and show it to the user ...
                MainWindow.Loaded += MainWindow_Loaded;
                MainWindow.Closing += OnClosing;

                // When the ViewModel asks to be closed, close the window.
                // Source: http://msdn.microsoft.com/en-us/magazine/dd419663.aspx
                MainWindow.Closed += MainWindow_Closed;

                ConstructMainWindowSession(appVM, _mainWindow);
                MainWindow.Show();
            }
        }

        /// <summary>
        /// Method is invoked when the MainWindow fires the Closed event.
        /// 
        /// Pre-requisite: Close event is previously processed and was not revoked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_Closed(object sender, EventArgs e)
        {
            var vmManager = GetService<IVMManager>();

            vmManager.UnsetCurrentViewModel();

            var appVM = GetAppViewModel();

            // Save session data and close application
            OnClosed(appVM, _mainWindow);

            DestroyAppViewModel();

            appVM = null;
            _mainWindow = null;
        }

        /// <summary>
        /// Method is invoked when the mainwindow is loaded and visble to the user.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ShutdownMode = ShutdownMode.OnLastWindowClose;

                var service = GetService<IFileManager>();

                RegisterDocumentTypes(service);
            }
            catch (Exception exp)
            {
                Logger.Error(exp);
            }

            /***
                        try
                        {
                            Application.Current.MainWindow = mMainWin = new MainWindow();
                            ShutdownMode = System.Windows.ShutdownMode.OnLastWindowClose;

                            AppCore.CreateAppDataFolder();

                            if (mMainWin != null && app != null)
                            {
                                mMainWin.Closing += OnClosing;


                                ConstructMainWindowSession(app, mMainWin);
                                mMainWin.Show();
                            }
                        }
                        catch (Exception exp)
                        {
                            Logger.Error(exp);
                        }
            ***/
        }

        /// <summary>
        /// Register document types and file extensions
        /// to be handled in this application.
        /// </summary>
        /// <param name="service"></param>
        private void RegisterDocumentTypes(IFileManager service)
        {
            var docType = service.RegisterDocumentType("Select.PDFFile"
                                                     , "PDF Files"
                                                     , "PDF Files"
                                                     , "pdf"
                                                     , null
                                                     , 0);

            if (docType != null)
            {
                // PDF Files (*.pdf)|*.pdf
                var t = docType.CreateItem("All Files", new List<string>() { "*" }, 10);
                docType.RegisterFileTypeItem(t);
            }
        }

        private AppViewModel GetAppViewModel()
        {
            var vmManager = GetService<IVMManager>();

            return (AppViewModel)vmManager.GetVMItem(VMItemKeys.ApplicationViewModel).Instance;
        }

        /// <summary>
        /// Removes the <seealso cref="AppViewModel"/> instance from the viewmodel managers
        /// collection and destroys it for good (at end of live time).
        /// </summary>
        private void DestroyAppViewModel()
        {
            var vmManager = GetService<IVMManager>();

            _mainWindow.DataContext = null;
            var appVM = (AppViewModel)vmManager.RemoveVMItem(VMItemKeys.ApplicationViewModel).Instance;

            if (appVM != null)
            {
                var dispose = appVM as IDisposable;
                if (dispose != null)
                    dispose.Dispose();
            }
        }

        /// <summary>
        /// COnstruct MainWindow an attach datacontext to it.
        /// </summary>
        /// <param name="workSpace"></param>
        /// <param name="win"></param>
        private void ConstructMainWindowSession(AppViewModel workSpace, IViewSize win)
        {
            try
            {
                var settings = GetService<ISettingsManager>();

                // Establish command binding to accept user input via commanding framework
                // workSpace.InitCommandBinding(win);

                ViewPosSizeModel viewSz;
                settings.SessionData.WindowPosSz.TryGetValue(settings.SessionData.MainWindowName
                                                           , out viewSz);

                viewSz.SetWindowsState(win);

                string lastActiveFile = settings.SessionData.LastActiveSolution;

                MainWindow mainWin = win as MainWindow;
            }
            catch (Exception exp)
            {
                Logger.Error(exp);
            }
        }

        /// <summary>
        /// Save session data on closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                AppViewModel wsVM = base.MainWindow.DataContext as AppViewModel;

                if (wsVM != null)
                {
                    // Close all open files and check whether application is ready to close
                    if (wsVM.AppLifeCycle.Exit_CheckConditions(wsVM) == true)
                    {
                        // (other than exception and error handling)
                        wsVM.AppLifeCycle.OnRequestClose(true);

                        e.Cancel = false;
                    }
                    else
                    {
                        wsVM.AppLifeCycle.CancelShutDown();
                        e.Cancel = true;
                    }
                }
            }
            catch (Exception exp)
            {
                Logger.Error(exp);
            }
        }

        /// <summary>
        /// Execute closing function and persist session data to be reloaded on next restart
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClosed(AppViewModel appVM, IViewSize win)
        {
            try
            {
                var settings = GetService<ISettingsManager>();

                ViewPosSizeModel viewSz;
                settings.SessionData.WindowPosSz.TryGetValue(settings.SessionData.MainWindowName
                                                           , out viewSz);
                viewSz.GetWindowsState(win);

                appVM.GetSessionData(settings.SessionData);

                // Save/initialize program options that determine global programm behaviour
                appVM.AppLifeCycle.SaveConfigOnAppClosed(win);
            }
            catch (Exception exp)
            {
                Logger.Error(exp);

////                var msg = GetService<IMessageBoxService>();
////
////                msg.Show(exp.ToString(), "Unexpected Error",
////                                MsgBox.MsgBoxButtons.OK, MsgBox.MsgBoxImage.Error);
            }
        }

        /// <summary>
        /// This method gets the service locator instance
        /// that is  used in turn to get an application specific service instance.
        /// </summary>
        /// <typeparam name="TServiceContract"></typeparam>
        /// <returns></returns>
        private TServiceContract GetService<TServiceContract>() where TServiceContract : class
        {
            return ServiceLocator.ServiceContainer.Instance.GetService<TServiceContract>();
        }
        #endregion methods
    }
}
