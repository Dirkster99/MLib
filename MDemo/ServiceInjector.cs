namespace MDemo
{
    using MLib;
    using MWindowDialogLib;
    using MWindowInterfacesLib.Interfaces;
    using MWindowLib;
    using ServiceLocator;
    using Settings;
    using Settings.Interfaces;

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
            ServiceContainer.Instance.AddService<IContentDialogService>(ContentDialogService.GetInstance(MetroWindowService.Instance));

            ServiceContainer.Instance.AddService<ISettingsManager>(SettingsManager.Instance);
            ServiceContainer.Instance.AddService<IAppearanceManager>(AppearanceManager.Instance);

            return ServiceContainer.Instance;
        }
    }
}
