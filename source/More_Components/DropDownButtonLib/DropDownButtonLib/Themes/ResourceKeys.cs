namespace DropDownButtonLib.Themes
{
    using System.Windows;

    /// <summary>
    /// Resource key management class to keep track of all resources
    /// that can be re-styled in applications that make use of the implemented controls.
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

        #region Brush Keys
        /// <summary>
        /// Resource key of the controls normal background brush key.
        /// </summary>
        public static readonly ComponentResourceKey ControlNormalBackgroundKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlNormalBackgroundKey");

        /// <summary>
        /// Resource key of the controls disabled background brush key.
        /// </summary>
        public static readonly ComponentResourceKey ControlDisabledBackgroundKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlDisabledBackgroundKey");

        /// <summary>
        /// Resource key of the controls normal background brush key.
        /// </summary>
        public static readonly ComponentResourceKey ControlNormalBorderKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlNormalBorderKey");

        /// <summary>
        /// Resource key of the controls mouse over border brush key.
        /// </summary>
        public static readonly ComponentResourceKey ControlMouseOverBorderKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlMouseOverBorderKey");

        /// <summary>
        /// Resource key of the controls selected border brush key.
        /// </summary>
        public static readonly ComponentResourceKey ControlSelectedBorderKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlSelectedBorderKey");

        /// <summary>
        /// Resource key of the controls focused border brush key.
        /// </summary>
        public static readonly ComponentResourceKey ControlFocusedBorderKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlFocusedBorderKey");

        /// <summary>
        /// Resource key of the button's normal outer border brush key.
        /// </summary>
        public static readonly ComponentResourceKey ButtonNormalOuterBorderKey = new ComponentResourceKey(typeof(ResourceKeys), "ButtonNormalOuterBorderKey");

        /// <summary>
        /// Resource key of the button's normal inner border brush key.
        /// </summary>
        public static readonly ComponentResourceKey ButtonNormalInnerBorderKey = new ComponentResourceKey(typeof(ResourceKeys), "ButtonNormalInnerBorderKey");

        /// <summary>
        /// Resource key of the button's normal background brush key.
        /// </summary>
        public static readonly ComponentResourceKey ButtonNormalBackgroundKey = new ComponentResourceKey(typeof(ResourceKeys), "ButtonNormalBackgroundKey");

        /// <summary>
        /// Resource key of the button's mouse over background brush key.
        /// </summary>
        public static readonly ComponentResourceKey ButtonMouseOverBackgroundKey = new ComponentResourceKey(typeof(ResourceKeys), "ButtonMouseOverBackgroundKey");

        /// <summary>
        /// Resource key of the button's mouse over outer border brush key.
        /// </summary>
        public static readonly ComponentResourceKey ButtonMouseOverOuterBorderKey = new ComponentResourceKey(typeof(ResourceKeys), "ButtonMouseOverOuterBorderKey");

        /// <summary>
        /// Resource key of the button's mouse over inner border brush key.
        /// </summary>
        public static readonly ComponentResourceKey ButtonMouseOverInnerBorderKey = new ComponentResourceKey(typeof(ResourceKeys), "ButtonMouseOverInnerBorderKey");

        /// <summary>
        /// Resource key of the button's pressed outer border brush key.
        /// </summary>
        public static readonly ComponentResourceKey ButtonPressedOuterBorderKey = new ComponentResourceKey(typeof(ResourceKeys), "ButtonPressedOuterBorderKey");

        /// <summary>
        /// Resource key of the button's pressed inner border brush key.
        /// </summary>
        public static readonly ComponentResourceKey ButtonPressedInnerBorderKey = new ComponentResourceKey(typeof(ResourceKeys), "ButtonPressedInnerBorderKey");

        /// <summary>
        /// Resource key of the button's pressed background brush key.
        /// </summary>
        public static readonly ComponentResourceKey ButtonPressedBackgroundKey = new ComponentResourceKey(typeof(ResourceKeys), "ButtonPressedBackgroundKey");

        /// <summary>
        /// Resource key of the button's focused outer border brush key.
        /// </summary>
        public static readonly ComponentResourceKey ButtonFocusedOuterBorderKey = new ComponentResourceKey(typeof(ResourceKeys), "ButtonFocusedOuterBorderKey");

        /// <summary>
        /// Resource key of the button's focused inner border brush key.
        /// </summary>
        public static readonly ComponentResourceKey ButtonFocusedInnerBorderKey = new ComponentResourceKey(typeof(ResourceKeys), "ButtonFocusedInnerBorderKey");

        /// <summary>
        /// Resource key of the button's focused background brush key.
        /// </summary>
        public static readonly ComponentResourceKey ButtonFocusedBackgroundKey = new ComponentResourceKey(typeof(ResourceKeys), "ButtonFocusedBackgroundKey");

        /// <summary>
        /// Resource key of the button's disbled outer border brush key.
        /// </summary>
        public static readonly ComponentResourceKey ButtonDisabledOuterBorderKey = new ComponentResourceKey(typeof(ResourceKeys), "ButtonDisabledOuterBorderKey");

        /// <summary>
        /// Resource key of the button's disabled inner border brush key.
        /// </summary>
        public static readonly ComponentResourceKey ButtonInnerBorderDisabledKey = new ComponentResourceKey(typeof(ResourceKeys), "ButtonInnerBorderDisabledKey");

        /// <summary>
        /// Resource key of the drop down list item background key.
        /// </summary>
        public static readonly ComponentResourceKey DropDownList_MouseOverBackgroundKey = new ComponentResourceKey(typeof(ResourceKeys), "DropDownList_MouseOverBackgroundKey");

        /// <summary>
        /// Resource key of the drop down background key.
        /// </summary>
        public static readonly ComponentResourceKey DropDownList_BackgroundKey = new ComponentResourceKey(typeof(ResourceKeys), "DropDownList_BackgroundKey");

        /// <summary>
        /// Resource key of the drop down border key.
        /// </summary>
        public static readonly ComponentResourceKey DropDownList_BorderForegroundKey = new ComponentResourceKey(typeof(ResourceKeys), "DropDownList_BorderForegroundKey");
        #endregion Brush Keys

        /// <summary>
        /// Resource key object of the normal glyph color.
        /// </summary>
        public static readonly ComponentResourceKey GlyphNormalForegroundKey = new ComponentResourceKey(typeof(ResourceKeys), "GlyphNormalForegroundKey");

        /// <summary>
        /// Resource key object of the mouseover foreground glyph color.
        /// </summary>
        public static readonly ComponentResourceKey GlyphMouseOverForegroundKey = new ComponentResourceKey(typeof(ResourceKeys), "GlyphMouseOverForegroundKey");

        /// <summary>
        /// Resource key object of the disabled foreground glyph color.
        /// </summary>
        public static readonly ComponentResourceKey GlyphDisabledForegroundKey = new ComponentResourceKey(typeof(ResourceKeys), "GlyphDisabledForegroundKey");

        /// <summary>
        /// Resource key object of the corner radius property assigned in different UI elements (not just SpinButton).
        /// </summary>
        public static readonly ComponentResourceKey SpinButtonCornerRadiusKey = new ComponentResourceKey(typeof(ResourceKeys), "SpinButtonCornerRadiusKey");

        /// <summary>
        /// Resource style key of the repeat button.
        /// </summary>
        public static readonly ComponentResourceKey SpinnerButtonStyleKey = new ComponentResourceKey(typeof(ResourceKeys), "SpinnerButtonStyleKey");
    }
}
