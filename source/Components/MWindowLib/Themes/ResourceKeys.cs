namespace MWindowLib.Themes
{
    using System.Windows;

    /// <summary>
    /// Class implements static resource keys that should be referenced to configure
    /// colors, styles and other elements that are typically changed between themes.
    /// </summary>
    public static class ResourceKeys
    {
        #region Accent Keys
        /// <summary>
        /// Accent Color Key - This Color key is used to accent elements in the UI
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
        /// Gets the color key for the normal control enabled foreground color.
        /// </summary>
        public static readonly ComponentResourceKey ControlNormalForegroundKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlNormalForegroundKey");

        /// <summary>
        /// Gets the color key for the normal control enabled background color.
        /// </summary>
        public static readonly ComponentResourceKey ControlNormalBackgroundKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlNormalBackgroundKey");

        /// <summary>
        /// Gets the Brush key for the normal control enabled foreground color.
        /// </summary>
        public static readonly ComponentResourceKey ControlNormalForegroundBrushKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlNormalForegroundBrushKey");

        /// <summary>
        /// Gets the Brush key for the normal control enabled background color.
        /// </summary>
        public static readonly ComponentResourceKey ControlNormalBackgroundBrushKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlNormalBackgroundBrushKey");
        #endregion Normal Control Foreground and Background Keys

        #region MouseOver Keys        
        /// <summary>
        /// Gets the Color key that should be applied when the user hovers the mouse over a control.
        /// </summary>
        public static readonly ComponentResourceKey ControlMouseOverBackgroundKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlMouseOverBackgroundKey");
        
        /// <summary>
        /// Gets the applicable Brush key when the user hovers the mouse over a control.
        /// </summary>
        public static readonly ComponentResourceKey ControlMouseOverBackgroundBrushKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlMouseOverBackgroundBrushKey");
        #endregion

        /// <summary>
        /// Gets the background Color key of the overlay that is shown
        /// when a ContentDialog is visible within the main window.
        /// </summary>
        public static readonly ComponentResourceKey OverlayColorKey = new ComponentResourceKey(typeof(ResourceKeys), "OverlayColorKey");

        /// <summary>
        /// Gets the background Brush key of the overlay that is shown
        /// when a ContentDialog is visible within the main window.
        /// </summary>
        public static readonly ComponentResourceKey OverlayBrushKey = new ComponentResourceKey(typeof(ResourceKeys), "OverlayBrushKey");

        /// <summary>
        /// Gets the applicable Style key for Windows buttons (close, restore, maximize etc...).
        /// </summary>
        public static readonly ComponentResourceKey WindowButtonStyleKey = new ComponentResourceKey(typeof(ResourceKeys), "WindowButtonStyleKey");
    }
}
