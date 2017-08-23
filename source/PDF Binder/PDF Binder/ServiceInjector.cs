namespace PDF_Binder
{
    using Doc;
    using Doc.DocManager.Interfaces;
    using ExplorerLib;
    using MLib;
    using MWindowDialogLib;
    using MWindowInterfacesLib.Interfaces;
    using ServiceLocator;
    using Settings;
    using Settings.Interfaces;
    using ViewModels;
    using ViewModels.VMManagement;

    /// <summary>
    /// Creates and initializes all services.
    /// </summary>
    public static class ServiceInjector
    {
        /// <summary>
        /// Loads service objects into the ServiceContainer on startup of application.
        /// </summary>
        /// <returns>Returns the current <seealso cref="ServiceContainer"/> instance
        /// to let caller work with service container items right after creation.</returns>
        public static ServiceContainer InjectServices()
        {
            ServiceContainer.Instance.AddService<IContentDialogService>(ContentDialogService.Instance);
            ServiceContainer.Instance.AddService<IVMManager>(VMManagerService.Instance);
            ServiceContainer.Instance.AddService<IFileManager>(FileManagerService.Instance);
            ServiceContainer.Instance.AddService<ISettingsManager>(SettingsManager.Instance);
            ServiceContainer.Instance.AddService<IAppearanceManager>(AppearanceManager.Instance);
            ServiceContainer.Instance.AddService<IExplorer>(new Explorer());

            return ServiceContainer.Instance;
        }
    }
}
