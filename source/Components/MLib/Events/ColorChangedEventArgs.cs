namespace MLib.Events
{
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    /// Represents the method that will handle the
    /// <see cref="ColorChangedEventArgs"/> routed event.
    /// </summary>
    /// <param name="sender">The object where the event handler is attached.</param>
    /// <param name="e">The event data (new color).</param>
    public delegate void ColorChangedEventHandler(object sender, ColorChangedEventArgs e);

    /// <summary>
    /// Implements the <see cref="ColorChangedEventArgs"/> routed event.
    /// 
    /// This event is used by the AppearanceManagerImpl
    /// to tell any listener when the AccentColor has changed via the
    /// AccentColorChanged event.
    /// </summary>
    public class ColorChangedEventArgs : RoutedEventArgs
    {
        #region constructors
        /// <summary>
        /// Class constructor from Color parameter.
        /// </summary>
        public ColorChangedEventArgs(Color newColor)
        {
            this.NewColor = Color.FromRgb(newColor.R, newColor.G, newColor.B);
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        protected ColorChangedEventArgs()
        {
        }
        #endregion constructors

        #region properties
        /// <summary>
        /// Gets the value of the new color to which the system has changed.
        /// </summary>
        public Color NewColor { get; private set; }
        #endregion properties
    }
}
