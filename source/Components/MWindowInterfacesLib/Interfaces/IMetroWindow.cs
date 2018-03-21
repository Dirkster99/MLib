namespace MWindowInterfacesLib.Interfaces
{
    using System.ComponentModel;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Threading;

    /// <summary>
    /// Defines an interface that must be implemented by the window that
    /// shows the content dialog as part of its content.
    /// </summary>
    public interface IMetroWindow
    {
        #region properties
        /// <summary>
        /// Gets the rendered height of this element.
        /// </summary>
        /// <returns>
        /// The element's height, as a value in device-independent units (1/96th inch per
        /// unit). The default value is 0 (zero).
        ///</returns>
        double ActualHeight { get; }

        /// <summary>
        /// Gets the rendered width of this element.
        /// </summary>
        /// <returns>
        /// The element's width, as a value in device-independent units (1/96th inch per
        /// unit). The default value is 0 (zero).
        /// </returns>
        double ActualWidth { get; }

        /// <summary>
        /// Gets the System.Windows.Threading.Dispatcher this System.Windows.Threading.DispatcherObject
        /// is associated with.
        /// </summary>
        /// <returns>The dispatcher.</returns>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        Dispatcher Dispatcher { get; }

        /// <summary>
        /// Gets or sets the System.Windows.Window that owns this System.Windows.Window.
        ///
        /// Exceptions:
        ///   T:System.ArgumentException:
        ///     A window tries to own itself-or-Two windows try to own each other.
        ///
        ///   T:System.InvalidOperationException:
        ///     The System.Windows.Window.Owner property is set on a visible window shown using
        ///     System.Windows.Window.ShowDialog-or-The System.Windows.Window.Owner property
        ///     is set with a window that has not been previously shown.
        /// </summary>
        /// <returns>A System.Windows.Window object that represents the owner of this System.Windows.Window.</returns>
        [DefaultValue(null)]
        Window Owner { get; set; }

////        IMessageDialogSettings MetroDialogOptions { get; set; }

        /// <summary>
        /// Gets the overlay box that is used to display modal dialogs inside the main window.
        /// </summary>
        Grid OverlayBox { get; }

        /// <summary>
        /// Gets a collection of Active modal dialogs that are displayed inside the main window.
        /// </summary>
        Grid MetroActiveDialogContainer { get; }

        /// <summary>
        /// Gets a collection of InActive modal dialogs that can be displayed inside the main window.
        /// </summary>
        Grid MetroInactiveDialogContainer { get; }

        /// <summary>
        /// Determines if there is currently a ContentDialog visible or not.
        /// </summary>
        bool IsContentDialogVisible { get; }
        #endregion properties

        #region events
        /// <summary>
        ///     Occurs when either the System.Windows.FrameworkElement.ActualHeight or the System.Windows.FrameworkElement.ActualWidth
        ///     properties change value on this element.
        /// </summary>
        event SizeChangedEventHandler SizeChanged;
        #endregion events

        #region methods
        /// <summary>
        /// Ensure visibility of the OverlayBox.
        /// </summary>
        void ShowOverlay();

        /// <summary>
        /// Begins to show the MetroWindow's overlay effect.
        /// </summary>
        /// <returns>A task representing the process.</returns>
        System.Threading.Tasks.Task ShowOverlayAsync();

        /// <summary>
        /// Ensure non-visibility of the OverlayBox.
        /// </summary>
        void HideOverlay();

        /// <summary>
        /// Begins to hide the MetroWindow's overlay effect.
        /// </summary>
        /// <returns>A task representing the process.</returns>
        System.Threading.Tasks.Task HideOverlayAsync();

        /// <summary>
        /// Stores the given element, or the last focused element via FocusManager, for restoring the focus after closing a dialog.
        /// See also RestoreFocus, ResetStoredFocus method.
        /// </summary>
        /// <param name="thisElement">The element which will be focused again.</param>
        void StoreFocus(IInputElement thisElement = null);

        /// <summary>
        /// Restores a previously saved keyboard focus back to the last focused element.
        /// See also StoreFocus, ResetStoredFocus method.
        /// </summary>
        void RestoreFocus();

        /// <summary>
        /// Gets a task object for the currently active content dialog.
        /// </summary>
        Task<TDialog> GetCurrentDialogAsync<TDialog>() where TDialog : IBaseMetroDialogFrame;

        /// <summary>
        /// Method connects the <see cref="Thumb"/> object on the window chrome
        /// with the correct drag events to let user drag the window on the screen.
        /// </summary>
        /// <param name="windowTitleThumb"></param>
        void SetWindowEvents(Thumb windowTitleThumb);
        #endregion methods
    }
}
