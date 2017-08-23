namespace WatermarkControlsDemo.ViewModels
{
    using Base;
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Input;
    using Themes.Selector;

    public class AppViewModel : Base.ViewModelBase
    {
        #region private fields
        private List<ThemeDefinition> _ListOfThemes = null;
        private ICommand _SelectionChanged = null;
        private bool _IsThemeSelectionEnabled = false;
        #endregion private fields

        #region constructors
        public AppViewModel()
        {
            _ListOfThemes = new List<ThemeDefinition>();

            _ListOfThemes.Add(new ThemeDefinition("Light", "/WatermarkControlsDemo;component/Themes/LightTheme.xaml"));
            _ListOfThemes.Add(new ThemeDefinition("Dark", "/WatermarkControlsDemo;component/Themes/DarkTheme.xaml"));
        }
        #endregion constructors

        #region properties
        /// <summary>
        /// Returns a list of theme definitons.
        /// </summary>
        public List<ThemeDefinition> ListOfThemes
        {
            get
            {
                return this._ListOfThemes;
            }
        }

        /// <summary>
        /// Command executes when the user has selected
        /// a different UI theme to display.
        /// </summary>
        public ICommand SelectionChanged
        {
            get
            {
                if (_SelectionChanged == null)
                {
                    _SelectionChanged = new RelayCommand<object>((p) =>
                    {
                        object[] paramets = p as object[];

                        if (paramets != null)
                        {
                            ThemeDefinition ts = paramets[0] as ThemeDefinition;

                            if (ts != null)
                            {
                                IsThemeSelectionEnabled = false;
                                try
                                {
                                    ThemeSelector.SetCurrentThemeDictionary(Application.Current.MainWindow
                                                                           , new Uri(ts.Source, UriKind.RelativeOrAbsolute));
                                }
                                catch (Exception)
                                {
                                }
                                finally
                                {
                                    IsThemeSelectionEnabled = true;
                                }
                            }
                        }
                    });
                }

                return _SelectionChanged;
            }
        }

        /// <summary>
        /// Gets whether a different theme can be selected right now or not.
        /// This property should be bound to the UI that selects a different
        /// theme to avoid the case in which a user could select a theme and
        /// select a different theme while the first theme change request is
        /// still processed.
        /// </summary>
        public bool IsThemeSelectionEnabled
        {
            get { return _IsThemeSelectionEnabled; }

            private set
            {
                if (this._IsThemeSelectionEnabled != value)
                {
                    _IsThemeSelectionEnabled = value;
                    RaisePropertyChanged(() => this._IsThemeSelectionEnabled);
                }
            }
        }
        #endregion properties
    }
}
