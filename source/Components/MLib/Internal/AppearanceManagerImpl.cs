namespace MLib.Internal
{
    using Events;
    using MLib.Interfaces;
    using MLib.Internal.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Media;
    using Themes;

    /// <summary>
    /// The AppearanceManager class manages all WPF theme related items.
    /// Its location in the hierarchy is between the viewmodels and the
    /// themes settings service.
    /// </summary>
    internal class AppearanceManagerImpl : IAppearanceManager
    {
        #region fields
        /// <summary>
        /// Name and source of the defalt theme (e.g. dark or light).
        /// </summary>
        private IThemeInfo _defaultTheme = null;

        private IThemeInfo _currentTheme = null;
        private string _currentThemeName = null;
        #endregion fields

        #region constructors
        /// <summary>
        /// Standard class constructor.
        /// </summary>
        public AppearanceManagerImpl()
        {
            ThemeSources = new List<Uri>();
        }
        #endregion constructors

        #region properties
        /// <summary>
        /// Gets the name of the currently selected theme.
        /// </summary>
        public string ThemeName
        {
            get
            {
                return _currentThemeName;
            }
        }

        /// <summary>
        /// Gets the current theme source.
        /// </summary>
        public List<Uri> ThemeSources
        {

            get;
            private set;
        }

        /// <summary>
        /// Gets the current AccentColor from the merged resource dictionary.
        /// </summary>
        public Color AccentColor
        {
            get
            {
                try
                {
                    // Set accent color with windows 10 deafault accent colors
                    return (Color)Application.Current.Resources[ResourceKeys.ControlAccentColorKey];
                }
                catch (Exception)
                {
                    return Color.FromRgb(255, 255, 255);
                }
            }
        }
        #endregion properties

        #region events
        /// <summary>
        /// Relays an event to its subscribers whenever the accent color is changed.
        /// </summary>
        public event ColorChangedEventHandler AccentColorChanged;
        #endregion events

        #region methods
        /// <summary>
        /// Returns the default theme for the application
        /// </summary>
        /// <returns></returns>
        public IThemeInfo GetDefaultTheme()
        {
            return _defaultTheme;
        }

        /// <summary>
        /// Set the current theme as a selection of the settings service properties.
        /// </summary>
        /// <param name="Themes">Collections of themes to select the new theme from.</param>
        /// <param name="themeName">Name od the theme to be set (e.g.: Dark, Light)</param>
        /// <param name="AccentColor">Apply this accent color
        /// (can be Windows default or custom accent color).
        /// Accent Color in UI elements is invisible if this is null.</param>
        public void SetTheme(IThemeInfos Themes
                            , string themeName
                            , Color AccentColor)
        {
            var theme = Themes.GetThemeInfo(themeName);

            if (theme == null)
                theme = GetDefaultTheme();

            SetTheme(theme, AccentColor);
        }

        /// <summary>
        /// Resets the standard themes available through the theme settings interface.
        /// Method Adds Dark and Light theme infos from MLib - calling applications can
        /// use the AddThemeResources() method to add more resources.
        /// </summary>
        /// <param name="themes">Collection of themeinfos in which Dark and Light themes
        /// with MLib resources should be added.</param>
        /// <param name="removeAllThemeInfos">Determines whether existing collection
        /// of themeinfos is removed before addinng Dark and Light themes.</param>
        public void SetDefaultThemes(IThemeInfos themes,
                                     bool removeAllThemeInfos = true)
        {
            if (removeAllThemeInfos == true)
                themes.RemoveAllThemeInfos();

            // Add theming models
            themes.AddThemeInfo("Dark", new List<Uri>
            {
                new Uri("/Mlib;component/Themes/DarkTheme.xaml", UriKind.RelativeOrAbsolute)
            });

            themes.AddThemeInfo("Light", new List<Uri>
            {
                new Uri("/Mlib;component/Themes/LightTheme.xaml", UriKind.RelativeOrAbsolute)
            });

            SetDefaultTheme(themes, "Dark");
        }

        /// <summary>
        /// This function assumes that themes and their resources have
        /// been added, previously.
        /// 
        /// Use this method to define a default theme which can always be
        /// used as a backup whenever a certain theme is not defined etc...
        /// </summary>
        /// <param name="Themes"></param>
        /// <param name="defaultThemeName"></param>
        public void SetDefaultTheme(IThemeInfos Themes
                            , string defaultThemeName)
        {
            var theme = Themes.GetThemeInfo(defaultThemeName);

            if (theme == null)
                throw new System.ArgumentOutOfRangeException(defaultThemeName);

            _defaultTheme = theme;
        }


        /// <summary>
        /// Adds more resource files into the standard themes available
        /// through the theme settings interface.
        /// </summary>
        /// <param name="themeName"></param>
        /// <param name="additionalResource"></param>
        /// <param name="themes"></param>
        public void AddThemeResources(string themeName
                                    , List<Uri> additionalResource
                                    , IThemeInfos themes)
        {
            var theme = themes.GetThemeInfo(themeName);

            theme.AddResources(additionalResource);

            _defaultTheme = themes.GetThemeInfo("Dark");
        }

        /// <summary>
        /// Creates a new instance of an object that adheres to the
        /// <see cref="IThemeInfos"/> interface.
        /// </summary>
        /// <returns></returns>
        public IThemeInfos CreateThemeInfos()
        {
            return new ThemeInfos();
        }

        /// <summary>
        /// Resets the AccentColor without changing the theme and
        /// triggers a AccentColorChanged event to all listners.
        /// </summary>
        /// <param name="accentColor"></param>
        public void SetAccentColor(Color accentColor)
        {
            try
            {
                if (accentColor != null)
                {
                    bool bColorChanged = false;

                    bColorChanged = (Application.Current.Resources[ResourceKeys.ControlAccentColorKey] == null &&
                                    accentColor != null);

                    if (Application.Current.Resources[ResourceKeys.ControlAccentColorKey] != null)
                    {
                        bColorChanged = bColorChanged ||
                            ((Color)Application.Current.Resources[ResourceKeys.ControlAccentColorKey] != accentColor);
                    }

                    if (bColorChanged == true)
                    {
                        // Set accent color
                        Application.Current.Resources[ResourceKeys.ControlAccentColorKey] = accentColor;
                        Application.Current.Resources[ResourceKeys.ControlAccentBrushKey] = new SolidColorBrush(accentColor);

                        if (AccentColorChanged != null)
                        {
                            AccentColorChanged(this, new ColorChangedEventArgs(accentColor));
                        }
                    }
                }
            }
            catch { }
        }

        private void SetTheme(IThemeInfo theme
                            , Color AccentColor)
        {
            try
            {
                _currentTheme = theme;

                SetThemeSourceAndAccentColor(theme.ThemeSources, AccentColor);

                _currentThemeName = theme.DisplayName;
                ThemeSources = new List<Uri>(theme.ThemeSources);
            }
            catch
            {}
        }

        /// <summary>
        /// Is invoked whenever the application theme is changed
        /// and a new Accent Color is applied.
        /// 
        /// TODO XXX: Set AccentColor in other components (MsgBox) as well.
        /// </summary>
        /// <param name="sources"></param>
        /// <param name="accentColor"></param>
        private void SetThemeSourceAndAccentColor(List<Uri> sources
                                                , Color accentColor)
        {
            if (sources == null)
                throw new ArgumentNullException("source");

            Application.Current.Resources.Clear();

            foreach (var item in sources)
            {
                try
                {
                    var themeDict = new ResourceDictionary { Source = item };

                    // add new before removing old theme to avoid dynamicresource not found warnings
                    Application.Current.Resources.MergedDictionaries.Add(themeDict);
                }
                catch (Exception exp)
                {
                    Console.WriteLine(exp.Message);
                    Console.WriteLine(exp.StackTrace);
                }
            }

            SetAccentColor(accentColor);
        }
        #endregion methods
    }
}
