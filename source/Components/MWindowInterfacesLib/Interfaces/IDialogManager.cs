namespace MWindowInterfacesLib.Interfaces
{
    using Events;
    using MsgBox.Enums;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines a service interface that supports coordination of content dialogs from within
    /// code behind of a view or code that holds references to a view.
    ///
    /// The methods specified here require a reference to a parent window.
    /// See also <seealso cref="IDialogCoordinator"/>
    /// </summary>
    public interface IDialogManager
    {
        #region events
        /// <summary>
        /// Implements an event source to tell listeners when a dialog is opened.
        /// </summary>
        event EventHandler<DialogStateChangedEventArgs> DialogOpened;

        /// <summary>
        /// Implements an event source to tell listeners when a dialog is closed.
        /// </summary>
        event EventHandler<DialogStateChangedEventArgs> DialogClosed;
        #endregion events

        #region methods
        /// <summary>
        /// Hides a visible Metro Dialog instance.
        /// </summary>
        /// <param name="metroWindow">The window with the dialog that is visible.</param>
        /// <param name="dialog">The dialog instance to hide.</param>
        /// <param name="settings">An optional pre-defined settings instance.</param>
        /// <returns>A task representing the operation.</returns>
        /// <exception cref="InvalidOperationException">
        /// The <paramref name="dialog"/> is not visible in the window.
        /// This happens if ShowMetroDialogAsync hasn't been called before.
        /// </exception>
        Task HideMetroDialogAsync(IMetroWindow metroWindow
                                , IBaseMetroDialogFrame dialog
                                , IMetroDialogFrameSettings settings = null);

        /// <summary>
        /// Creates a dialog inside of the current window.
        /// </summary>
        /// <param name="metroWindow">The MetroWindow</param>
        /// <param name="dialog">The dialog result interface of the dialog.</param>
        /// <param name="settings">Optional settings that override the global metro dialog settings.</param>
        /// <returns>A task promising the result of which button was pressed.</returns>
        Task<MsgBoxResult> ShowMsgBoxAsync(
              IMetroWindow metroWindow
            , IMsgBoxDialogFrame<MsgBoxResult> dialog
            , IMetroDialogFrameSettings settings = null);


        /// <summary>
        /// Adds a Metro Dialog instance to the specified window and makes it visible asynchronously.
        /// If you want to wait until the user has closed the dialog, use ShowMetroDialogAsyncAwaitable
        /// <para>You have to close the resulting dialog yourself with <see cref="HideMetroDialogAsync"/>.</para>
        /// </summary>
        /// <param name="metroWindow">The owning window of the dialog.</param>
        /// <param name="dialog">The dialog instance itself.</param>
        /// <param name="settings">An optional pre-defined settings instance.</param>
        /// <returns>A task representing the operation.</returns>
        /// <exception cref="InvalidOperationException">The <paramref name="dialog"/> is already visible in the window.</exception>
        Task ShowMetroDialogAsync(IMetroWindow metroWindow
                                , IBaseMetroDialogFrame dialog
                                , IMetroDialogFrameSettings settings = null);

        /// <summary>
        /// Creates a modal dialog inside of the current main window.
        /// </summary>
        /// <param name="metroWindow">The MetroWindow</param>
        /// <param name="dialog">The outside modal window to be owned by a given <seealso cref="IMetroWindow"/></param>
        /// <param name="settings">Optional settings that override the global metro dialog settings.</param>
        /// <returns>The result that was entered or 0 if the user escape keyed the dialog...</returns>
        Task<int> ShowMetroDialogAsync(IMetroWindow metroWindow
                                        , IMsgBoxDialogFrame<int> dialog
                                        , IMetroDialogFrameSettings settings = null);

        /// <summary>
        /// Creates a custom dialog outside of the current window.
        /// </summary>
        /// <param name="metroWindow">The MetroWindow</param>
        /// <param name="dialog">The outside modal window to be owned by a given MetroWindow</param>
        /// <param name="settings">Optional settings that override the global metro dialog settings.</param>
        /// <returns>The result event that was generated to close the dialog (button click).</returns>
        int ShowModalDialogExternal(
              IMetroWindow metroWindow
            , IMsgBoxDialogFrame<int> dialog
            , IMetroDialogFrameSettings settings = null);

        /// <summary>
        /// Creates an External MsgBox dialog outside of the current window.
        /// </summary>
        /// <param name="metroWindow">The MetroWindow</param>
        /// <param name="dialog">The outside modal window to be owned by a given MetroWindow</param>
        /// <param name="settings">Optional settings that override the global metro dialog settings.</param>
        /// <returns>The result event that was generated to close the dialog (button click).</returns>
        MsgBoxResult ShowModalDialogExternal(
              IMetroWindow metroWindow
            , IMsgBoxDialogFrame<MsgBoxResult> dialog
            , IMetroDialogFrameSettings settings = null);
        #endregion methods
    }
}