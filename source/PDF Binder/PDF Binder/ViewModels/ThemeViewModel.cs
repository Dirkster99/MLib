namespace PDF_Binder.ViewModels
{
    using MLib;
    using MLib.Themes;
    using Settings.Interfaces;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    /// ViewModel class that manages theme properties for binding and display in WPF UI.
    /// </summary>
    public class ThemeViewModel : Base.ViewModelBase
    {
        #region private fields
        private readonly ThemeDefinition  _DefaultTheme = null;
        private Dictionary<string, ThemeDefinition> _ListOfThemes = null;
        private ThemeDefinition _SelectedTheme = null;
        private bool _IsEnabled = true;
        #endregion private fields

        #region constructors
        /// <summary>
        /// Standard Constructor
        /// </summary>
        public ThemeViewModel()
        {
            var settings = GetService<ISettingsManager>(); // add the default themes

            _ListOfThemes = new Dictionary<string, ThemeDefinition>();

            foreach (var item in settings.Themes.GetThemeInfos())
            {
                var list = new List<string>();
                foreach (var subitem in item.ThemeSources)
                    list.Add(subitem.ToString());

                _ListOfThemes.Add(item.DisplayName, new ThemeDefinition(item.DisplayName, list));
            }

            // Lets make sure there is a default
            _ListOfThemes.TryGetValue(GetService<IAppearanceManager>().GetDefaultTheme().DisplayName, out _DefaultTheme);

            // and something sensible is selected
            _SelectedTheme = _DefaultTheme;
        }
        #endregion constructors

        #region properties
        /// <summary>
        /// Returns a default theme that should be applied when nothing else is available.
        /// </summary>
        public ThemeDefinition DefaultTheme
        {
            get
            {
                return _DefaultTheme;
            }
        }

        /// <summary>
        /// Returns a list of theme definitons.
        /// </summary>
        public List<ThemeDefinition> ListOfThemes
        {
            get
            {
                return _ListOfThemes.Select(it => it.Value).ToList();
            }
        }

        /// <summary>
        /// Gets the currently selected theme (or desfault on applaiction start-up)
        /// </summary>
        public ThemeDefinition SelectedTheme
        {
            get
            {
                return _SelectedTheme;
            }

            private set
            {
                if (_SelectedTheme != value)
                {
                    _SelectedTheme = value;
                    this.RaisePropertyChanged(() => this.SelectedTheme);
                }
            }
        }

        /// <summary>
        /// Gets whether a different theme can be selected right now or not.
        /// This property should be bound to the UI that selects a different
        /// theme to avoid the case in which a user could select a theme and
        /// select a different theme while the first theme change request is
        /// still processed.
        /// </summary>
        public bool IsEnabled
        {
            get { return _IsEnabled; }

            private set
            {
                if (_IsEnabled != value)
                {
                    _IsEnabled = value;
                    RaisePropertyChanged(() => IsEnabled);
                }
            }
        }
        #endregion properties

        #region methods
        /// <summary>
        /// Applies a new theme based on the changed selection in the input element.
        /// </summary>
        /// <param name="ts"></param>
        public void ApplyTheme(FrameworkElement fe, string themeName)
        {
            if (themeName != null)
            {
                IsEnabled = false;
                try
                {
                    var settings = GetService<ISettingsManager>(); // add the default themes

                    Color AccentColor = ThemeViewModel.GetCurrentAccentColor(settings);
                    GetService<IAppearanceManager>().SetTheme(settings.Themes, themeName, AccentColor);

                    ThemeDefinition o;
                    _ListOfThemes.TryGetValue(themeName, out o);
                    SelectedTheme = o;
                }
                catch
                {
                }
                finally
                {
                    IsEnabled = true;
                }
            }
        }

        public static Color GetCurrentAccentColor(ISettingsManager settings)
        {
            Color AccentColor;

            if (settings.Options.GetOptionValue<bool>("Appearance", "ApplyWindowsDefaultAccent"))
                AccentColor = SystemParameters.WindowGlassColor;
            else
                AccentColor = settings.Options.GetOptionValue<Color>("Appearance", "AccentColor");

            return AccentColor;
        }
        #endregion methods
    }
}
