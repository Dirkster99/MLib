namespace HistoryControlLib.Themes
{
    using System.Windows;

    /// <summary>
    /// Implements a static theming keys that are used for styling and theming in this library.
    /// </summary>
    public static class ResourceKeys
    {
        #region Accent Keys
        /// <summary>
        /// Accent Color Key and Accent Brush Key
        /// These keys are used to accent elements in the UI
        /// (e.g.: Color of Activated Normal Window Frame, ResizeGrip, Focus or MouseOver input elements)
        /// </summary>
        public static readonly ComponentResourceKey ControlAccentColorKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlAccentColorKey");

        /// <summary>
        /// Accent Brush Key - This Brush key is used to accent elements in the UI
        /// (e.g.: Color of Activated Normal Window Frame, ResizeGrip, Focus or MouseOver input elements)
        /// </summary>
        public static readonly ComponentResourceKey ControlAccentBrushKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlAccentBrushKey");
        #endregion Accent Keys

        #region Normal Control Foreground and Background Keys
        /// <summary>
        /// Unspecific normal foreground <see cref="System.Windows.Media.Color"/> key.
        /// </summary>
        public static readonly ComponentResourceKey ControlNormalForegroundKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlNormalForegroundKey");

        /// <summary>
        /// Unspecific normal background <see cref="System.Windows.Media.Color"/> key.
        /// </summary>
        public static readonly ComponentResourceKey ControlNormalBackgroundKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlNormalBackgroundKey");

        /// <summary>
        /// Unspecific normal foreground <see cref="System.Windows.Media.Brush"/> key.
        /// </summary>
        public static readonly ComponentResourceKey ControlNormalForegroundBrushKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlNormalForegroundBrushKey");

        /// <summary>
        /// Unspecific normal background <see cref="System.Windows.Media.Brush"/> key.
        /// </summary>
        public static readonly ComponentResourceKey ControlNormalBackgroundBrushKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlNormalBackgroundBrushKey");

        /// <summary>
        /// Unspecific normal border <see cref="System.Windows.Media.Brush"/> key.
        /// </summary>
        public static readonly ComponentResourceKey ControlNormalBorderKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlNormalBorderKey");
        #endregion Normal Control Foreground and Background Keys

        #region MouseOver Keys
        /// <summary>
        /// Unspecific normal background <see cref="System.Windows.Media.Color"/> key for mouse over effects.
        /// </summary>
        public static readonly ComponentResourceKey ControlMouseOverBackgroundKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlMouseOverBackgroundKey");

        /// <summary>
        /// Unspecific normal background <see cref="System.Windows.Media.Brush"/> key for mouse over effects.
        /// </summary>
        public static readonly ComponentResourceKey ControlMouseOverBackgroundBrushKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlMouseOverBackgroundBrushKey");
////    public static readonly ComponentResourceKey ControlMouseOverBorderKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlMouseOverBorderKey");
        #endregion

        /// <summary>
        /// Normal Item background <see cref="System.Windows.Media.Brush"/> key for item in ItemsControls, such as, treeviews, listbox etc.
        /// </summary>
        public static readonly ComponentResourceKey ControlItemBorderSelectedKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlItemBorderSelectedKey");

        /// <summary>
        /// Item background <see cref="System.Windows.Media.Brush"/> key for selected items in ItemsControls, such as, treeviews, listbox etc.
        /// </summary>
        public static readonly ComponentResourceKey ControlItemBackgroundSelectedKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlItemBackgroundSelectedKey");

        /// <summary>
        /// Item foreground <see cref="System.Windows.Media.Brush"/> key for selected items in ItemsControls, such as, treeviews, listbox etc.
        /// </summary>
        public static readonly ComponentResourceKey ControlItemTextSelectedKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlItemTextSelectedKey");

        #region Disabled Control Colors
        /// <summary>
        /// Item background <see cref="System.Windows.Media.Brush"/> key for disabled items in ItemsControls, such as, treeviews, listbox etc.
        /// </summary>
        public static readonly ComponentResourceKey ControlDisabledBackgroundKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlDisabledBackgroundKey");

        /// <summary>
        /// Item foreground <see cref="System.Windows.Media.Brush"/> key for disabled items in ItemsControls, such as, treeviews, listbox etc.
        /// </summary>
        public static readonly ComponentResourceKey ControlDisabledForegroundKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlDisabledForegroundKey");

        /// <summary>
        /// Item border <see cref="System.Windows.Media.Brush"/> key for disabled items in ItemsControls, such as, treeviews, listbox etc.
        /// </summary>
        public static readonly ComponentResourceKey ControlDisabledBorderKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlDisabledBorderKey");
        #endregion Disabled Control Colors

        #region Pop-Up controls
        /// <summary>
        /// PopUp control background <see cref="System.Windows.Media.Brush"/> key for disabled items in ItemsControls, such as, treeviews, listbox etc.
        /// </summary>
        public static readonly ComponentResourceKey PopUpControlNormalBackgroundKey = new ComponentResourceKey(typeof(ResourceKeys), "PopUpControlNormalBackgroundKey");

        /// <summary>
        /// PopUp control border <see cref="System.Windows.Media.Brush"/> key for disabled items in ItemsControls, such as, treeviews, listbox etc.
        /// </summary>
        public static readonly ComponentResourceKey PopUpControlNormalBorderKey = new ComponentResourceKey(typeof(ResourceKeys), "PopUpControlNormalBorderKey");
        #endregion Pop-Up controls

        /// <summary>
        /// Determines the style of the history (forward, backward, up) buttons in BrowseHistory display.
        /// </summary>
        public static readonly ComponentResourceKey HistoryButtonStyleKey = new ComponentResourceKey(typeof(ResourceKeys), "HistoryButtonStyleKey");

        /// <summary>
        /// Determines the style of the toggle drop down button in BrowseHistory display.
        /// This button is usually a clickable down chevron symbol with a pop-up list of recent
        /// locations underneath.
        /// </summary>
        public static readonly ComponentResourceKey HistoryToggleButtonStyleKey = new ComponentResourceKey(typeof(ResourceKeys), "HistoryToggleButtonStyleKey");

        /// <summary>
        /// Defines an arraow Geometry that can be used in a Path object to style
        /// forward, backward, and Up buttons.
        /// </summary>
        public static readonly ComponentResourceKey ArrowGeometryKey = new ComponentResourceKey(typeof(ResourceKeys), "ArrowGeometryKey");
    }
}
