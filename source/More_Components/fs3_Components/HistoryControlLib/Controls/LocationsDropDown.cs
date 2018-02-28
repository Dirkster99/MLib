namespace HistoryControlLib.Controls
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Implements a Locations Drop Down control which is basically a combobox
    /// without the display of a selected item (control has only a Chevron drop
    /// down button and a drop down list).
    /// 
    /// This control is useful if the selected element is displayed elsewhere in
    /// the application.
    /// </summary>
    public class LocationsDropDown : ComboBox
    {
        /// <summary>
        /// Static constructor.
        /// </summary>
        static LocationsDropDown()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LocationsDropDown), new FrameworkPropertyMetadata(typeof(LocationsDropDown)));
        }
    }
}
