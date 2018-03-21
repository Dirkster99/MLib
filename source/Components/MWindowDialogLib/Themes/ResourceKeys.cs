namespace MWindowDialogLib.Themes
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

        /// <summary>
        /// Gets the border Brush key for dialog frame.
        /// </summary>
        public static readonly ComponentResourceKey DialogFrameBrushKey = new ComponentResourceKey(typeof(ResourceKeys), "DialogFrameBrushKey");

        /// <summary>
        /// Gets the foreground Brush key for a message box dialog.
        /// </summary>
        public static readonly ComponentResourceKey MsgBoxMessageColorBrushKey = new ComponentResourceKey(typeof(ResourceKeys), "MsgBoxMessageColorBrushKey");

        /// <summary>
        /// Gets a general black Color key usually used for foreground colors that should always
        /// be visible against the background. This color can, for example, be White for a dark theme.
        /// </summary>
        public static readonly ComponentResourceKey BlackColorKey = new ComponentResourceKey(typeof(ResourceKeys), "BlackColorKey");

        /// <summary>
        /// Gets a general black Brush key usually used for foreground colors that should always
        /// be visible against the background. This color can, for example, be White for a dark theme.
        /// </summary>
        public static readonly ComponentResourceKey BlackBrushKey = new ComponentResourceKey(typeof(ResourceKeys), "BlackBrushKey");

        /// <summary>
        /// Gets a general white Color key usually used for background colors that should always
        /// be visible against the foreground. This color can, for example, be Black for a dark theme.
        /// </summary>
        public static readonly ComponentResourceKey WhiteColorKey = new ComponentResourceKey(typeof(ResourceKeys), "WhiteColorKey");

        /// <summary>
        /// Gets a general white Brush key usually used for background colors that should always
        /// be visible against the foreground. This color can, for example, be Black for a dark theme.
        /// </summary>
        public static readonly ComponentResourceKey WhiteBrushKey = new ComponentResourceKey(typeof(ResourceKeys), "WhiteBrushKey");

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
        /// Gets the applicable Style key for Windows buttons (close, restore, maximize etc...).
        /// </summary>
        public static readonly ComponentResourceKey WindowButtonStyleKey = new ComponentResourceKey(typeof(ResourceKeys), "WindowButtonStyleKey");
    }
}
