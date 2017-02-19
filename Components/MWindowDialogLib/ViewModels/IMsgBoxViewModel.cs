namespace MWindowDialogLib.ViewModels
{
    using MWindowInterfacesLib.Interfaces;
    using MWindowInterfacesLib.MsgBox.Enums;
    using System.ComponentModel;
    using System.Windows.Input;
    using System.Windows.Media;

    internal interface IMsgBoxViewModel<TResult> : IBaseMetroDialogFrameViewModel<TResult>
    {
        /// <summary>
        /// Get property to get all textual information in one text block.
        /// This property is typically used to copy all text (even details)
        /// to the clipboard so users can paste it into their email and send
        /// the problem description off to those who care and know...
        /// </summary>
        string AllToString { get; }

        /// <summary>
        /// Get/set visibility of Cancel button
        /// </summary>
        bool CancelVisibility { get; set; }

        /// <summary>
        /// Determines whether the button labelled 'Close' is shown or not.
        /// </summary>
        bool CloseVisibility { get; set; }

        /// <summary>
        /// Get/set property to determine a image in the copy message button
        /// of the dialog.
        /// 
        /// This property represents the actual IMAGE not the enumeration.
        /// </summary>
        ImageSource CopyImageSource { get; set; }

        #region commands
        /// <summary>
        /// Get the command that is executed when the user clicked the 'Cancel' button.
        /// </summary>
        ICommand CancelCommand { get; }

        /// <summary>
        /// Execute a command to copy the text string supplied
        /// as parameter into the clipboard
        /// </summary>
        ICommand CopyText { get; }

        /// <summary>
        /// Gets a command that starts a new (browser)
        /// process to navigate to this (web) target
        /// </summary>
        ICommand NavigateToUri { get; }

        ICommand NoCommand { get; }

        /// <summary>
        /// Gets a command that is executed when the user clicked the 'OK' button.
        /// </summary>
        ICommand OkCommand { get; }

        /// <summary>
        /// Gets a command that is executed when the user clicked the 'No' button.
        /// </summary>
        ICommand YesCommand { get; }
        #endregion commands

        /// <summary>
        /// Get property to determine the default button (if any)
        /// to be used in the dialog (user can hit ENTER key to execute that function).
        /// </summary>
        TResult IsDefaultButton { get; }

        /// <summary>
        /// Get property to determine whether a helplink should be display or not.
        /// A helplink should not be displayed if there is no HelpLink information
        /// available, and it can be dispalyed otherwise.
        /// </summary>
        bool DisplayHelpLink { get; }

        /// <summary>
        /// Get/set property to determine whether the copy message
        /// function is available to the user or not (default: available).
        /// </summary>
        bool EnableCopyFunction { get; set; }

        /// <summary>
        /// Get/set property to determine the address to browsed to when displaying a help link.
        /// </summary>
        string HelpLink { get; set; }

        /// <summary>
        /// Get/set property to determine the text for displaying a help link.
        /// By default the text is the toString content of the <seealso cref="HelpLink"/>
        /// but it can also be a different text if that text is set in the constructor.
        /// </summary>
        string HelpLinkTitle { get; set; }

        /// <summary>
        /// Gets/sets a string that is displayed to label a hyperlink.
        /// </summary>
        string HyperlinkLabel { get; set; }

        /// <summary>
        /// More message details displayed in an expander (this can, for example,
        /// by a stacktrace or other technical information that can be shown for
        /// trouble shooting advanced scenarious via copy button - CSC etc...).
        /// </summary>
        string InnerMessageDetails { get; set; }

        /// <summary>
        /// Message content that tells the user what the problem is
        /// (why is it a problem, how can it be fixed,
        ///  and clicking which button will do what resolution [if any] etc...).
        /// </summary>
        string Message { get; set; }

        /// <summary>
        /// Get/set visibility of OK button
        /// </summary>
        bool OkVisibility { get; set; }

        /// <summary>
        /// Get/set visibility of Show Details section in dialog
        /// </summary>
        bool ShowDetails { get; set; }

        /// <summary>
        /// Get property to determine type pf image to be shown to the user
        /// based on <seealso cref="MsgBoxImage"/> enumeration.
        /// </summary>
        MsgBoxImage TypeOfImage { get; }

        /// <summary>
        /// Get/set visibility of Yes/No buttons
        /// </summary>
        bool YesNoVisibility { get; set; }

        /// <summary>
        /// Message box result is not set if dialog view is closed via F4, Window X button (standard chrome)
        /// so we determine whether the dialog is allowed to close via these 'hidden' mechanics and cancel it if not.
        /// 
        /// Otherwise, we set the result to default result here.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MessageBox_Closing(object sender, CancelEventArgs e);
    }
}