namespace MLib.Themes
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
        /// Gets a the applicable foreground Brush key that should be used for coloring text.
        /// </summary>
        public static readonly ComponentResourceKey ControlTextBrushKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlTextBrushKey");

        /// <summary>
        /// Gets the key of the preferred width of a (window) close button.
        /// </summary>
        public static readonly ComponentResourceKey ControlCloseButtonWidthKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlCloseButtonWidthKey");

        /// <summary>
        /// Gets the key of the preferred height of a (window) close button.
        /// </summary>
        public static readonly ComponentResourceKey ControlSystemButtonHeightKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlSystemButtonHeightKey");

        /// <summary>
        /// Gets the key of the preferred width of a button.
        /// </summary>
        public static readonly ComponentResourceKey ControlSystemButtonWidthKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlSystemButtonWidthKey");

        /// <summary>
        /// Gets the Brush key of the foreground color for a button.
        /// </summary>
        public static readonly ComponentResourceKey ControlButtonTextKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlButtonTextKey");

        /// <summary>
        /// Gets the Brush key of the background color of a system button when a user hovers the mouse over it.
        /// </summary>
        public static readonly ComponentResourceKey ControlSystemButtonBackgroundOnMoseOverKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlSystemButtonBackgroundOnMoseOverKey");

        /// <summary>
        /// Gets the Brush key of the foreground color of a system button when a user hovers the mouse over it.
        /// </summary>
        public static readonly ComponentResourceKey ControlSystemButtonForegroundOnMoseOverKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlSystemButtonForegroundOnMoseOverKey");

        /// <summary>
        /// Gets the Brush key of the background color of a button when it is in Pressed state.
        /// </summary>
        public static readonly ComponentResourceKey ControlSystemButtonBackgroundIsPressedKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlSystemButtonBackgroundIsPressedKey");

        /// <summary>
        /// Gets the Brush key of the foreground color of a button when it is in Pressed state.
        /// </summary>
        public static readonly ComponentResourceKey ControlSystemButtonForegroundIsPressedKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlSystemButtonForegroundIsPressedKey");

        /// <summary>
        /// Gets the Brush key of the background color of a control.
        /// </summary>
        public static readonly ComponentResourceKey ControlButtonBackgroundKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlButtonBackgroundKey");

        /// <summary>
        /// Gets the Brush key of the border color of a control.
        /// </summary>
        public static readonly ComponentResourceKey ControlButtonBorderKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlButtonBorderKey");

        /// <summary>
        /// Gets the Brush key of a button's text (or foreground) color when it is in disabled state.
        /// </summary>
        public static readonly ComponentResourceKey ControlButtonTextDisabledKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlButtonTextDisabledKey");

        /// <summary>
        /// Gets the Brush key of a control's background color when it is in disabled state.
        /// </summary>
        public static readonly ComponentResourceKey ControlDisabledBackgroundKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlDisabledBackgroundKey");

        /// <summary>
        /// Gets the Brush key of a control's border color when it is in disabled state.
        /// </summary>
        public static readonly ComponentResourceKey ControlDisabledBorderKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlDisabledBorderKey");

        /// <summary>
        /// Gets the mouse over Brush key of a control's background color when the user hovers the mouse over it.
        /// </summary>
        public static readonly ComponentResourceKey ControlButtonBackgroundHoverKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlButtonBackgroundHoverKey");

        /// <summary>
        /// Gets Brush key of a button's background color when the button is pressed.
        /// </summary>
        public static readonly ComponentResourceKey ControlButtonBackgroundPressedKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlButtonBackgroundPressedKey");

        /// <summary>
        /// Gets Brush key of a button's border color when the mouse is hovering over it.
        /// </summary>
        public static readonly ComponentResourceKey ControlButtonBorderHoverKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlButtonBorderHoverKey");

        /// <summary>
        /// Gets Brush key of a button's text or foreground color when the mouse is hovering over it.
        /// </summary>
        public static readonly ComponentResourceKey ControlButtonTextHoverKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlButtonTextHoverKey");

        /// <summary>
        /// Gets Brush key of a button's text or foreground color when the button is pressed.
        /// </summary>
        public static readonly ComponentResourceKey ControlButtonTextPressedKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlButtonTextPressedKey");

        /// <summary>
        /// Gets Brush key of a button's border color when the button is pressed.
        /// </summary>
        public static readonly ComponentResourceKey ControlButtonBorderPressedKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlButtonBorderPressedKey");

        /// <summary>
        /// Gets Brush key of a button's border when it is the default button within a dialog.
        /// </summary>
        public static readonly ComponentResourceKey ControlButtonIsDefaultBorderBrushKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlButtonIsDefaultBorderBrushKey");

        /// <summary>
        /// Gets Brush key of a button's border when it is the default button within a dialog and
        /// has currently no input focus.
        /// </summary>
        public static readonly ComponentResourceKey ControlButtonIsUnfocusedDefaultBrushKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlButtonIsUnfocusedDefaultBrushKey");

        /// <summary>
        /// Gets text or foreground Brush key of an unspecified input control that is in disabled state
        /// (e.g.: ItemsControl, TreeView, ListBox ...).
        /// </summary>
        public static readonly ComponentResourceKey ControlInputTextDisabledKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlInputTextDisabledKey");

        // Color definitions for ItemsControl based controls (Listview, Combobox, listbox etc...)
        #region ItemsControl Item
        /// <summary>
        /// Gets text or foreground Brush key of an item in an ItemsControl (TreeView, ListBox, ListView).
        /// </summary>
        public static readonly ComponentResourceKey ControlItemTextKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlItemTextKey");

        /// <summary>
        /// Gets background Brush key of an item in an ItemsControl (TreeView, ListBox, ListView)
        /// where the mouse is hovering over.
        /// </summary>
        public static readonly ComponentResourceKey ControlItemBackgroundHoverKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlItemBackgroundHoverKey");

        /// <summary>
        /// Gets text or foreground Brush key of an item in an ItemsControl (TreeView, ListBox, ListView)
        /// where the mouse is hovering over.
        /// </summary>
        public static readonly ComponentResourceKey ControlItemTextHoverKey    = new ComponentResourceKey(typeof(ResourceKeys), "ControlItemTextHoverKey");

        /// <summary>
        /// Gets text or foreground Brush key of a selected item in an ItemsControl (TreeView, ListBox, ListView).
        /// </summary>
        public static readonly ComponentResourceKey ControlItemTextSelectedKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlItemTextSelectedKey");

        /// <summary>
        /// Gets text or foreground Brush key of a disabled item in an ItemsControl (TreeView, ListBox, ListView).
        /// </summary>
        public static readonly ComponentResourceKey ControlItemTextDisabledKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlItemTextDisabledKey");

        /// <summary>
        /// Gets a background Brush key of a selected item in an ItemsControl (TreeView, ListBox, ListView).
        /// </summary>
        public static readonly ComponentResourceKey ControlItemBackgroundSelectedKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlItemBackgroundSelectedKey");

        /// <summary>
        /// Gets a border Brush key of a selected item in an ItemsControl (TreeView, ListBox, ListView).
        /// </summary>
        public static readonly ComponentResourceKey ControlItemBorderSelectedKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlItemBorderSelectedKey");

        /// <summary>
        /// Gets a background Brush key of a selected item in an ItemsControl (TreeView, ListBox, ListView)
        /// that has currently no input focus.
        /// </summary>
        public static readonly ComponentResourceKey ControlItemUnfocusedBackgroundSelectedKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlItemUnfocusedBackgroundSelectedKey");

        /// <summary>
        /// Gets a border Brush key of a selected item in an ItemsControl (TreeView, ListBox, ListView)
        /// that has currently no input focus.
        /// </summary>
        public static readonly ComponentResourceKey ControlItemUnfocusedBorderSelectedKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlItemUnfocusedBorderSelectedKey");

        /// <summary>
        /// Gets a border Brush key of a selected and pressed item in an ItemsControl (TreeView, ListBox, ListView).
        /// </summary>
        public static readonly ComponentResourceKey ControlItemPressedBorderBrush = new ComponentResourceKey(typeof(ResourceKeys), "ControlItemPressedBorderBrush");

        /// <summary>
        /// Gets a background Brush key of a disabled  item in an ItemsControl (ListBox).
        /// </summary>
        public static readonly ComponentResourceKey DisabledVisualElement         = new ComponentResourceKey(typeof(ResourceKeys), "DisabledVisualElement");

        /// <summary>
        /// Gets a background Brush key of a shadow on a validation ToolTip in a ListBox.
        /// </summary>
        public static readonly ComponentResourceKey ValidationToolTipTemplateShadowBrush = new ComponentResourceKey(typeof(ResourceKeys), "ValidationToolTipTemplateShadowBrush");

        /// <summary>
        /// Gets a background Brush key of a validation error element in a ListBox.
        /// </summary>
        public static readonly ComponentResourceKey ValidationErrorElement = new ComponentResourceKey(typeof(ResourceKeys), "ValidationErrorElement");
        #endregion ItemsControl Item

        /// <summary>
        /// Gets the Alternate Row 1 Background Color key of a row in a listview.
        /// </summary>
        public static readonly ComponentResourceKey AlternateRow1BackgroundColorKey = new ComponentResourceKey(typeof(ResourceKeys), "AlternateRow1BackgroundColorKey");

        /// <summary>
        /// Gets the Alternate Row 2 Background Color key of a row in a listview.
        /// </summary>
        public static readonly ComponentResourceKey AlternateRow2BackgroundColorKey = new ComponentResourceKey(typeof(ResourceKeys), "AlternateRow2BackgroundColorKey");

        /// <summary>
        /// Gets the Alternate Row 1 Background Brush key of a row in a listview.
        /// </summary>
        public static readonly ComponentResourceKey AlternateRow1BackgroundBrushKey = new ComponentResourceKey(typeof(ResourceKeys), "AlternateRow1BackgroundBrushKey");

        /// <summary>
        /// Gets the Alternate Row 2 Background Brush key of a row in a listview.
        /// </summary>
        public static readonly ComponentResourceKey AlternateRow2BackgroundBrushKey = new ComponentResourceKey(typeof(ResourceKeys), "AlternateRow2BackgroundBrushKey");
        
        // Color definitions for ScrollBar specific items
        #region ScrollBar Item
        /// <summary>
        /// Gets the background Brush key of the thumb that is used to drive a scrollbar.
        /// </summary>
        public static readonly ComponentResourceKey ControlScrollBarThumbBackgroundKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlScrollBarThumbBackgroundKey");

        /// <summary>
        /// Gets the border Brush key of the thumb that is used to drive a scrollbar.
        /// </summary>
        public static readonly ComponentResourceKey ControlScrollBarThumbBorderKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlScrollBarThumbBorderKey");

        /// <summary>
        /// Gets the foreground Brush key of the thumb that is used to drive a scrollbar.
        /// </summary>
        public static readonly ComponentResourceKey ControlScrollBarThumbForegroundKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlScrollBarThumbForegroundKey");

        /// <summary>
        /// Gets the background Brush key of the thumb that is used to drive a scrollbar
        /// when the mouse is hovered over it.
        /// </summary>
        public static readonly ComponentResourceKey ControlScrollBarThumbBackgroundHoverKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlScrollBarThumbBackgroundHoverKey");

        /// <summary>
        /// Gets the foreground Brush key of the thumb that is used to drive a scrollbar
        /// when the mouse is hovered over it.
        /// </summary>
        public static readonly ComponentResourceKey ControlScrollBarThumbForegroundHoverKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlScrollBarThumbForegroundHoverKey");

        /// <summary>
        /// Gets the background Brush key of the thumb that is used to drive a scrollbar
        /// when the user is dragging it.
        /// </summary>
        public static readonly ComponentResourceKey ControlScrollBarThumbBackgroundDraggingKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlScrollBarThumbBackgroundDraggingKey");

        /// <summary>
        /// Gets the foreground Brush key of the thumb that is used to drive a scrollbar
        /// when the user is dragging it.
        /// </summary>
        public static readonly ComponentResourceKey ControlScrollBarThumbForegroundDraggingKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlScrollBarThumbForegroundDraggingKey");

        /// <summary>
        /// Gets the background Brush key of a scrollbar.
        /// </summary>
        public static readonly ComponentResourceKey ControlScrollBarBackgroundKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlScrollBarBackgroundKey");
        #endregion ScrollBar Item

        /// <summary>
        /// Gets a border Brush key of a control (Combobox, Tool Tip).
        /// </summary>
        public static readonly ComponentResourceKey ControlWindowBorderActiveKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlWindowBorderActiveKey");

        /// <summary>
        /// Gets a background Brush key of a control (Combobox, Tool Tip).
        /// </summary>
        public static readonly ComponentResourceKey ControlWindowBackgroundKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlWindowBackgroundKey");

        /// <summary>
        /// Gets a text or foreground Brush key of a control (Combobox, Tool Tip).
        /// </summary>
        public static readonly ComponentResourceKey ControlWindowTextKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlWindowTextKey");

        /// <summary>
        /// Gets a background Brush key of an input control when the mouse is hovering over it.
        /// </summary>
        public static readonly ComponentResourceKey ControlInputBackgroundHoverKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlInputBackgroundHoverKey");

        /// <summary>
        /// Gets a border Brush key of an input control when the mouse is hovering over it.
        /// </summary>
        public static readonly ComponentResourceKey ControlInputBorderHoverKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlInputBorderHoverKey");

        /// <summary>
        /// Gets a text or foreground Brush key of an input control.
        /// </summary>
        public static readonly ComponentResourceKey ControlInputTextKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlInputTextKey");

        /// <summary>
        /// Gets a background Brush key of an input control.
        /// </summary>
        public static readonly ComponentResourceKey ControlInputBackgroundKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlInputBackgroundKey");

        /// <summary>
        /// Gets a border Brush key of an input control.
        /// </summary>
        public static readonly ComponentResourceKey ControlInputBorderKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlInputBorderKey");

        #region Pop-Up
        /// <summary>
        /// Gets a background Color key of a Pop-Up control.
        /// </summary>
        public static readonly ComponentResourceKey ControlPopupBackgroundKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlPopupBackgroundKey");

        /// <summary>
        /// Gets a background Brush key of a Pop-Up control.
        /// </summary>
        public static readonly ComponentResourceKey ControlPopupBackgroundBrushKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlPopupBackgroundBrushKey");
        #endregion Pop-Up

        /// <summary>
        /// Gets a background Brush key of a Progress control.
        /// </summary>
        public static readonly ComponentResourceKey ProgressBackgroundBrushKey = new ComponentResourceKey(typeof(ResourceKeys), "ProgressBackgroundBrushKey");

        /// <summary>
        /// Gets the foreground Brush key for a validation error display (e.g.: TextBlock.Foreground in PasswordBox).
        /// </summary>
        public static readonly ComponentResourceKey ControlsValidationBrushKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlsValidationBrushKey");

        /// <summary>
        /// Gets a general black color key usually used for foreground colors that should always
        /// be visible against the background. This color can, for example, be White for a dark theme.
        /// </summary>
        public static readonly ComponentResourceKey BlackColorKey = new ComponentResourceKey(typeof(ResourceKeys), "BlackColorKey");

        /// <summary>
        /// Gets a general black brush key usually used for foreground colors that should always
        /// be visible against the background. This color can, for example, be White for a dark theme.
        /// </summary>
        public static readonly ComponentResourceKey BlackBrushKey = new ComponentResourceKey(typeof(ResourceKeys), "BlackBrushKey");

        #region General Gray
        /// <summary>
        /// Gets a general gray color key applicable for controls in normal state.
        /// </summary>
        public static readonly ComponentResourceKey GrayNormalKey = new ComponentResourceKey(typeof(ResourceKeys), "GrayNormalKey");

        /// <summary>
        /// Gets a general gray color key applicable for controls in mouse over state.
        /// </summary>
        public static readonly ComponentResourceKey GrayHoverKey = new ComponentResourceKey(typeof(ResourceKeys), "GrayHoverKey");

        /// <summary>
        /// Gets a general gray brush key applicable for controls in normal state.
        /// </summary>
        public static readonly ComponentResourceKey GrayNormalBrushKey = new ComponentResourceKey(typeof(ResourceKeys), "GrayNormalBrushKey");

        /// <summary>
        /// Gets a general gray brush key applicable for controls in mouse over state.
        /// </summary>
        public static readonly ComponentResourceKey GrayHoverBrushKey = new ComponentResourceKey(typeof(ResourceKeys), "GrayHoverBrushKey");

        /// <summary>
        /// Gets a general gray color key in a gray range from 1 (almost black) to 10 (almost white).
        /// </summary>
        public static readonly ComponentResourceKey Gray1ColorKey = new ComponentResourceKey(typeof(ResourceKeys), "Gray1ColorKey");

        /// <summary>
        /// Gets a general gray color key in a gray range from 1 (almost black) to 10 (almost white).
        /// </summary>
        public static readonly ComponentResourceKey Gray2ColorKey = new ComponentResourceKey(typeof(ResourceKeys), "Gray2ColorKey");

        /// <summary>
        /// Gets a general gray color key in a gray range from 1 (almost black) to 10 (almost white).
        /// </summary>
        public static readonly ComponentResourceKey Gray5ColorKey = new ComponentResourceKey(typeof(ResourceKeys), "Gray5ColorKey");

        /// <summary>
        /// Gets a general gray color key in a gray range from 1 (almost black) to 10 (almost white).
        /// </summary>
        public static readonly ComponentResourceKey Gray6ColorKey = new ComponentResourceKey(typeof(ResourceKeys), "Gray6ColorKey");

        /// <summary>
        /// Gets a general gray color key in a gray range from 1 (almost black) to 10 (almost white).
        /// </summary>
        public static readonly ComponentResourceKey Gray7ColorKey = new ComponentResourceKey(typeof(ResourceKeys), "Gray7ColorKey");

        /// <summary>
        /// Gets a general gray color key in a gray range from 1 (almost black) to 10 (almost white).
        /// </summary>
        public static readonly ComponentResourceKey Gray8ColorKey = new ComponentResourceKey(typeof(ResourceKeys), "Gray8ColorKey");

        /// <summary>
        /// Gets a general gray color key in a gray range from 1 (almost black) to 10 (almost white).
        /// </summary>
        public static readonly ComponentResourceKey Gray10ColorKey = new ComponentResourceKey(typeof(ResourceKeys), "Gray10ColorKey");

        /// <summary>
        /// Gets a general gray Brush key in a gray range from 1 (almost black) to 10 (almost white).
        /// </summary>
        public static readonly ComponentResourceKey Gray1BrushKey = new ComponentResourceKey(typeof(ResourceKeys), "Gray1BrushKey");

        /// <summary>
        /// Gets a general gray Brush key in a gray range from 1 (almost black) to 10 (almost white).
        /// </summary>
        public static readonly ComponentResourceKey Gray2BrushKey = new ComponentResourceKey(typeof(ResourceKeys), "Gray2BrushKey");

        /// <summary>
        /// Gets a general gray Brush key in a gray range from 1 (almost black) to 10 (almost white).
        /// </summary>
        public static readonly ComponentResourceKey Gray5BrushKey = new ComponentResourceKey(typeof(ResourceKeys), "Gray5BrushKey");

        /// <summary>
        /// Gets a general gray Brush key in a gray range from 1 (almost black) to 10 (almost white).
        /// </summary>
        public static readonly ComponentResourceKey Gray6BrushKey = new ComponentResourceKey(typeof(ResourceKeys), "Gray6BrushKey");

        /// <summary>
        /// Gets a general gray Brush key in a gray range from 1 (almost black) to 10 (almost white).
        /// </summary>
        public static readonly ComponentResourceKey Gray7BrushKey = new ComponentResourceKey(typeof(ResourceKeys), "Gray7BrushKey");

        /// <summary>
        /// Gets a general gray Brush key in a gray range from 1 (almost black) to 10 (almost white).
        /// </summary>
        public static readonly ComponentResourceKey Gray8BrushKey = new ComponentResourceKey(typeof(ResourceKeys), "Gray8BrushKey");

        /// <summary>
        /// Gets a general gray Brush key in a gray range from 1 (almost black) to 10 (almost white).
        /// </summary>
        public static readonly ComponentResourceKey Gray10BrushKey = new ComponentResourceKey(typeof(ResourceKeys), "Gray10BrushKey");
        #endregion General Gray

        /// <summary>
        /// Gets a foreground Brush key that can be used for highlighting certain elements
        /// (e.g. signal checked state if applicable).
        /// </summary>
        public static readonly ComponentResourceKey HighlightBrushKey = new ComponentResourceKey(typeof(ResourceKeys), "HighlightBrushKey");

        /// <summary>
        /// Gets a border Brush key applicable on a checkbox or radio button control.
        /// </summary>
        public static readonly ComponentResourceKey CheckBoxBrushKey = new ComponentResourceKey(typeof(ResourceKeys), "CheckBoxBrushKey");

        /// <summary>
        /// Gets a transparent background Brush key that can be used to overlay an item and
        /// signal its enabled state on a checkbox or radio button control.
        /// </summary>
        public static readonly ComponentResourceKey TransparentWhiteBrushKey = new ComponentResourceKey(typeof(ResourceKeys), "TransparentWhiteBrushKey");

        /// <summary>
        /// Gets a half transparent background Brush key that can be used to overlay an item and
        /// signal its disabled state on a checkbox or radio button control.
        /// </summary>
        public static readonly ComponentResourceKey SemiTransparentWhiteBrushKey = new ComponentResourceKey(typeof(ResourceKeys), "SemiTransparentWhiteBrushKey");

        /// <summary>
        /// Gets the Brush key applicable for Glyph elements (e.g.: down chevron on combobox).
        /// </summary>
        public static readonly ComponentResourceKey GlyphBrushKey = new ComponentResourceKey(typeof(ResourceKeys), "GlyphBrushKey");

        #region GroupBox Colors
        /// <summary>
        /// Gets a foreground Brush key of a group box control.
        /// </summary>
        public static readonly ComponentResourceKey GroupBoxForeground = new ComponentResourceKey(typeof(ResourceKeys), "GroupBoxForeground");

        /// <summary>
        /// Gets a background Brush key of a group box control.
        /// </summary>
        public static readonly ComponentResourceKey GroupBoxBackground = new ComponentResourceKey(typeof(ResourceKeys), "GroupBoxBackground");

        /// <summary>
        /// Gets a border Brush key of a group box control.
        /// </summary>
        public static readonly ComponentResourceKey GroupBoxBorderBrush = new ComponentResourceKey(typeof(ResourceKeys), "GroupBoxBorderBrush");

        /// <summary>
        /// Gets a background Brush key for the content portion of a group box control.
        /// </summary>
        public static readonly ComponentResourceKey GroupBoxContentBackground = new ComponentResourceKey(typeof(ResourceKeys), "GroupBoxContentBackground");
        #endregion GroupBox Colors
        
        #region Slider Colors
        /// <summary>
        /// Gets the normal background Brush key applicable for the thumb of a Slider control.
        /// </summary>
        public static readonly ComponentResourceKey SliderThumbBackground = new ComponentResourceKey(typeof(ResourceKeys), "SliderThumbBackground");

        /// <summary>
        /// Gets the normal border Brush key applicable for the thumb of a Slider control.
        /// </summary>
        public static readonly ComponentResourceKey SliderThumbBorder = new ComponentResourceKey(typeof(ResourceKeys), "SliderThumbBorder");

        /// <summary>
        /// Gets the background Brush key applicable for the thumb of a Slider control
        /// when the mouse is hovered over it.
        /// </summary>
        public static readonly ComponentResourceKey SliderThumbBackgroundHover = new ComponentResourceKey(typeof(ResourceKeys), "SliderThumbBackgroundHover");

        /// <summary>
        /// Gets the border Brush key applicable for the thumb of a Slider control
        /// when the mouse is hovered over it.
        /// </summary>
        public static readonly ComponentResourceKey SliderThumbBorderHover = new ComponentResourceKey(typeof(ResourceKeys), "SliderThumbBorderHover");

        /// <summary>
        /// Gets the background Brush key applicable for the thumb of a Slider control
        /// when the thumb is being dragged.
        /// </summary>
        public static readonly ComponentResourceKey SliderThumbBackgroundDragging = new ComponentResourceKey(typeof(ResourceKeys), "SliderThumbBackgroundDragging");

        /// <summary>
        /// Gets the border Brush key applicable for the thumb of a Slider control
        /// when the thumb is being dragged.
        /// </summary>
        public static readonly ComponentResourceKey SliderThumbBorderDragging = new ComponentResourceKey(typeof(ResourceKeys), "SliderThumbBorderDragging");

        /// <summary>
        /// Gets the background Brush key applicable for the thumb of a Slider control
        /// when the slider is disabled
        /// </summary>
        public static readonly ComponentResourceKey SliderThumbBackgroundDisabled = new ComponentResourceKey(typeof(ResourceKeys), "SliderThumbBackgroundDisabled");

        /// <summary>
        /// Gets the border Brush key applicable for the thumb of a Slider control
        /// when the slider is disabled
        /// </summary>
        public static readonly ComponentResourceKey SliderThumbBorderDisabled = new ComponentResourceKey(typeof(ResourceKeys), "SliderThumbBorderDisabled");

        /// <summary>
        /// Gets the Brush key applicable to draw Tick marks of a Slider control.
        /// </summary>
        public static readonly ComponentResourceKey SliderTick = new ComponentResourceKey(typeof(ResourceKeys), "SliderTick");

        /// <summary>
        /// Gets the border Brush key applicable draw the bowser of the Tick marks part of a Slider control.
        /// </summary>
        public static readonly ComponentResourceKey SliderTrackBorder = new ComponentResourceKey(typeof(ResourceKeys), "SliderTrackBorder");

        /// <summary>
        /// Gets the background Brush key applicable draw the bowser of the Tick marks part of a Slider control.
        /// </summary>
        public static readonly ComponentResourceKey SliderTrackBackground = new ComponentResourceKey(typeof(ResourceKeys), "SliderTrackBackground");

        /// <summary>
        /// Gets the background Brush key applicable draw the selected Tick marks of a Slider control.
        /// </summary>
        public static readonly ComponentResourceKey SliderSelectionBackground = new ComponentResourceKey(typeof(ResourceKeys), "SliderSelectionBackground");

        /// <summary>
        /// Gets the border Brush key applicable draw the selected Tick marks of a Slider control.
        /// </summary>
        public static readonly ComponentResourceKey SliderSelectionBorder = new ComponentResourceKey(typeof(ResourceKeys), "SliderSelectionBorder");

        /// <summary>
        /// Gets the foreground Brush key applicable draw the Tick marks of a Slider control
        /// when the control is in disabled state.
        /// </summary>
        public static readonly ComponentResourceKey SliderTickDisabled = new ComponentResourceKey(typeof(ResourceKeys), "SliderTickDisabled");
        #endregion Slider Colors

        /// <summary>
        /// Gets the background Brush key applicable for GridSplitter and StatusBar controls.
        /// </summary>
        public static readonly ComponentResourceKey SeparatorBackground = new ComponentResourceKey(typeof(ResourceKeys), "SeparatorBackground");

        /// <summary>
        /// Gets the foreground Brush key of a Seperator in a StatusBar control or GridSplitter.
        /// </summary>
        public static readonly ComponentResourceKey SeperatorForeground = new ComponentResourceKey(typeof(ResourceKeys), "SeperatorForeground");

        #region StatusBar
        /// <summary>
        /// Gets the foreground Brush key applicable for a StatusBar control.
        /// </summary>
        public static readonly ComponentResourceKey StatusBarForegroundBrushKey = new ComponentResourceKey(typeof(ResourceKeys), "StatusBarForegroundBrushKey");

        /// <summary>
        /// Gets the background Brush key applicable for a StatusBar control.
        /// </summary>
        public static readonly ComponentResourceKey StatusBarBackgroundBrushKey = new ComponentResourceKey(typeof(ResourceKeys), "StatusBarBackgroundBrushKey");

        /// <summary>
        /// Gets the foreground Brush key applicable for a StatusBar control in disabled state.
        /// </summary>
        public static readonly ComponentResourceKey StatusDisabledForegroundBrushKey = new ComponentResourceKey(typeof(ResourceKeys), "StatusDisabledForegroundBrushKey");
        #endregion StatusBar
        
        #region Toolbar Colors
        /// <summary>
        /// Gets the background Brush key of a toolbaritem when the mouse is hovered over it.
        /// </summary>
        public static readonly ComponentResourceKey ToolBarItemHoverBackgroundBrushKey = new ComponentResourceKey(typeof(ResourceKeys), "ToolBarItemHoverBackgroundBrushKey");

        /// <summary>
        /// Gets the border Brush key of a toolbaritem when the mouse is hovered over it.
        /// </summary>
        public static readonly ComponentResourceKey ToolBarItemHoverBorderBrushKey = new ComponentResourceKey(typeof(ResourceKeys), "ToolBarItemHoverBorderBrushKey");

        /// <summary>
        /// Gets the background Brush key of a toolbaritem in disabled state.
        /// </summary>
        public static readonly ComponentResourceKey ToolBarItemDisabledBrushKey = new ComponentResourceKey(typeof(ResourceKeys), "ToolBarItemDisabledBrushKey");

        /// <summary>
        /// Gets the background Brush key of a toolbaritem in pressed state.
        /// </summary>
        public static readonly ComponentResourceKey ToolBarButtonPressed = new ComponentResourceKey(typeof(ResourceKeys), "ToolBarButtonPressed");

        /// <summary>
        /// Gets the normal background Brush key of a toolbar.
        /// </summary>
        public static readonly ComponentResourceKey ToolBarBackground = new ComponentResourceKey(typeof(ResourceKeys), "ToolBarBackground");

        /// <summary>
        /// Gets the normal background Brush key of a ToolBarTray.
        /// </summary>
        public static readonly ComponentResourceKey ToolBarTrayBackground = new ComponentResourceKey(typeof(ResourceKeys), "ToolBarTrayBackground");
        #endregion Toolbar Colors

        #region ProgressBar
        /// <summary>
        /// MLib has 2 ways of enabling Metro ProgressBars (Infinit ProgressBar with 4 dots sliding through)
        /// A MetroProgressBar control lives in <see cref="MLib.Controls.Metro.MetroProgressBar"/>
        /// and the other one is activated by default.
        /// 
        /// A non-metro progress bar with a moving rectangle only can be activated with this style key.
        /// </summary>
        public static readonly ComponentResourceKey ProgressStyleKey = new ComponentResourceKey(typeof(ResourceKeys), "ProgressStyleKey");
        #endregion ProgressBar
    }
}
