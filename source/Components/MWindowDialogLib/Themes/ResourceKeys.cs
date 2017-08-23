namespace MWindowDialogLib.Themes
{
    using System.Windows;

    public static class ResourceKeys
    {
        #region Accent Keys
        // Accent Color Key and Accent Brush Key
        // These keys are used to accent elements in the UI
        // (e.g.: Color of Activated Normal Window Frame, ResizeGrip, Focus or MouseOver input elements)
        public static readonly ComponentResourceKey ControlAccentColorKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlAccentColorKey");
        public static readonly ComponentResourceKey ControlAccentBrushKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlAccentBrushKey");
        #endregion Accent Keys

        #region Normal Control Foreground and Background Keys
        // Color Keys
        public static readonly ComponentResourceKey ControlNormalForegroundKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlNormalForegroundKey");
        public static readonly ComponentResourceKey ControlNormalBackgroundKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlNormalBackgroundKey");

        // Brush Keys for colors defined above
        public static readonly ComponentResourceKey ControlNormalForegroundBrushKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlNormalForegroundBrushKey");
        public static readonly ComponentResourceKey ControlNormalBackgroundBrushKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlNormalBackgroundBrushKey");
        #endregion Normal Control Foreground and Background Keys

        public static readonly ComponentResourceKey DialogFrameBrushKey = new ComponentResourceKey(typeof(ResourceKeys), "DialogFrameBrushKey");

        public static readonly ComponentResourceKey MsgBoxMessageColorBrushKey = new ComponentResourceKey(typeof(ResourceKeys), "MsgBoxMessageColorBrushKey");

        // Black & White Color Definition
        public static readonly ComponentResourceKey BlackColorKey = new ComponentResourceKey(typeof(ResourceKeys), "BlackColorKey");
        public static readonly ComponentResourceKey BlackBrushKey = new ComponentResourceKey(typeof(ResourceKeys), "BlackBrushKey");

        public static readonly ComponentResourceKey WhiteColorKey = new ComponentResourceKey(typeof(ResourceKeys), "WhiteColorKey");
        public static readonly ComponentResourceKey WhiteBrushKey = new ComponentResourceKey(typeof(ResourceKeys), "WhiteBrushKey");

        #region MouseOver Keys
        public static readonly ComponentResourceKey ControlMouseOverBackgroundKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlMouseOverBackgroundKey");
        public static readonly ComponentResourceKey ControlMouseOverBackgroundBrushKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlMouseOverBackgroundBrushKey");
        #endregion

        // Non-Color Keys
        public static readonly ComponentResourceKey WindowButtonStyleKey = new ComponentResourceKey(typeof(ResourceKeys), "WindowButtonStyleKey");
    }
}
