/*************************************************************************************

   Extended WPF Toolkit

   Copyright (C) 2007-2013 Xceed Software Inc.

   This program is provided to you under the terms of the Microsoft Public
   License (Ms-PL) as published at http://wpftoolkit.codeplex.com/license 

   For more features, controls, and fast professional support,
   pick up the Plus Edition at http://xceed.com/wpf_toolkit

   Stay informed: follow @datagrid on Twitter or Like http://facebook.com/datagrids

  ***********************************************************************************/

namespace WatermarkControlsLib.Themes
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

        #region Brush Keys
        public static readonly ComponentResourceKey ControlNormalForegroundKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlNormalForegroundKey");
        public static readonly ComponentResourceKey ControlNormalBackgroundKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlNormalBackgroundKey");
        public static readonly ComponentResourceKey ControlNormalBorderKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlNormalBorderKey");
        public static readonly ComponentResourceKey ControlMouseOverBackgroundKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlMouseOverBackgroundKey");

        // Mouse Over Colors
        public static readonly ComponentResourceKey ControlMouseOverBorderKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlMouseOverBorderKey");

        public static readonly ComponentResourceKey ControlSelectedBorderKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlSelectedBorderKey");
        public static readonly ComponentResourceKey ControlSelectedBackgroundKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlSelectedBackgroundKey");
        public static readonly ComponentResourceKey ControlSelectedForegroundKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlSelectedForegroundKey");

        // public static readonly ComponentResourceKey ControlFocusedBorderKey = new ComponentResourceKey( typeof( ResourceKeys ), "ControlFocusedBorderKey" );
        public static readonly ComponentResourceKey ControlPressedBackgroundKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlPressedBackgroundKey");

        // Watermark Colors
        public static readonly ComponentResourceKey ControlWatermarkForegroundKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlWatermarkForegroundKey");

        // Disabled Control Colors
        public static readonly ComponentResourceKey ControlDisabledBackgroundKey = new ComponentResourceKey( typeof( ResourceKeys ), "ControlDisabledBackgroundKey" );
        public static readonly ComponentResourceKey ControlDisabledForegroundKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlDisabledForegroundKey");
        public static readonly ComponentResourceKey ControlDisabledBorderKey = new ComponentResourceKey(typeof(ResourceKeys), "ControlDisabledBorderKey");

        // Popup Control Colors
        public static readonly ComponentResourceKey PopUpControlNormalBackgroundKey = new ComponentResourceKey(typeof(ResourceKeys), "PopUpControlNormalBackgroundKey");
        public static readonly ComponentResourceKey PopUpControlNormalBorderKey = new ComponentResourceKey(typeof(ResourceKeys), "PopUpControlNormalBorderKey");

        // public static readonly ComponentResourceKey ButtonNormalOuterBorderKey = new ComponentResourceKey( typeof( ResourceKeys ), "ButtonNormalOuterBorderKey" );
        // public static readonly ComponentResourceKey ButtonNormalInnerBorderKey = new ComponentResourceKey( typeof( ResourceKeys ), "ButtonNormalInnerBorderKey" );
        // public static readonly ComponentResourceKey ButtonNormalBackgroundKey = new ComponentResourceKey( typeof( ResourceKeys ), "ButtonNormalBackgroundKey" );

        // public static readonly ComponentResourceKey ButtonMouseOverBackgroundKey = new ComponentResourceKey( typeof( ResourceKeys ), "ButtonMouseOverBackgroundKey" );
        // public static readonly ComponentResourceKey ButtonMouseOverOuterBorderKey = new ComponentResourceKey( typeof( ResourceKeys ), "ButtonMouseOverOuterBorderKey" );
        // public static readonly ComponentResourceKey ButtonMouseOverInnerBorderKey = new ComponentResourceKey( typeof( ResourceKeys ), "ButtonMouseOverInnerBorderKey" );

        // public static readonly ComponentResourceKey ButtonPressedOuterBorderKey = new ComponentResourceKey( typeof( ResourceKeys ), "ButtonPressedOuterBorderKey" );
        // public static readonly ComponentResourceKey ButtonPressedInnerBorderKey = new ComponentResourceKey( typeof( ResourceKeys ), "ButtonPressedInnerBorderKey" );
        // public static readonly ComponentResourceKey ButtonPressedBackgroundKey = new ComponentResourceKey( typeof( ResourceKeys ), "ButtonPressedBackgroundKey" );

        // public static readonly ComponentResourceKey ButtonFocusedOuterBorderKey = new ComponentResourceKey( typeof( ResourceKeys ), "ButtonFocusedOuterBorderKey" );
        // public static readonly ComponentResourceKey ButtonFocusedInnerBorderKey = new ComponentResourceKey( typeof( ResourceKeys ), "ButtonFocusedInnerBorderKey" );
        // public static readonly ComponentResourceKey ButtonFocusedBackgroundKey = new ComponentResourceKey( typeof( ResourceKeys ), "ButtonFocusedBackgroundKey" );

        //public static readonly ComponentResourceKey ButtonDisabledOuterBorderKey = new ComponentResourceKey( typeof( ResourceKeys ), "ButtonDisabledOuterBorderKey" );
        // public static readonly ComponentResourceKey ButtonInnerBorderDisabledKey = new ComponentResourceKey( typeof( ResourceKeys ), "ButtonInnerBorderDisabledKey" );

        #endregion //Brush Keys
    }
}
