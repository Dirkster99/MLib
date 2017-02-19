namespace MWindowDialogLib.ViewModels
{
    using Base;
    using MWindowInterfacesLib.MsgBox.Enums;
    using System;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    ////using UserNotification.Events;
    ////using UserNotification.Interfaces;

    /***
     * 
     * Window Close Esc Key                           -> Set default result via CloseCommand CommandParameter binding
     * Window Close F4                                -> Set default result via MessageBox_Closing method
     * Window Close (X) Button (System window chrome) -> Set default result via MessageBox_Closing method
     * 
     * Behavior is same when setting default result but what do we do if
     * this.DialogCanCloseWithF4 == true
     * 
     * and the user hits escape?
     */

    /// <summary>
    /// Source:
    /// http://blogsprajeesh.blogspot.de/2009/12/wpf-messagebox-custom-control-updated.html
    /// http://prajeeshprathap.codeplex.com/sourcecontrol/list/patches?ProjectName=prajeeshprathap
    /// 
    /// A viewmodel that drives an advanced message box dialog window through its life cycle.
    /// This message box supports:
    /// - Custom images
    /// - Help Link Navigation for advanced research in online resources (by the user)
    /// - (Expander) section with more textual/technical details
    /// </summary>
    internal class MsgBoxViewModel : DialogResultViewModel<MsgBoxResult>  ////, INotifyableViewModel
        , IMsgBoxViewModel<MsgBoxResult>
    {
        #region fields
        private string mTitle;
        private string mMessage;
        private string mInnerMessageDetails;

        private readonly MsgBoxButtons mButtonOption;  // Store button configuration that is shown in dialog
        private bool mYesNoVisibility;
        private bool mCancelVisibility;
        private bool mOKVisibility;
        private bool mCloseVisibility;
        private bool mShowDetails;

        private RelayCommand<object> mYesCommand;
        private RelayCommand<object> mNoCommand;
        private RelayCommand<object> mCancelCommand;
        private RelayCommand<object> mCloseCommand;
        private RelayCommand<object> mOKCommand;
        private RelayCommand<string> mNavigateToUri;
        private RelayCommand<string> mCopyText;

        private bool mEnableCopyFunction;

        private ImageSource mCopyImageSource;

        private MsgBoxResult mIsDefaultButton;

        private bool mDialogCanCloseViaChrome;
        private bool? mDialogCloseResult;

        private string mHyperlinkLabel = string.Empty;
        private object mHelpLink = null;
        private string mHelpLinkTitle;
        private Func<object, bool> mNavigateHyperlinkMethod = MsgBoxViewModel.NavigateToUniversalResourceIndicator;
        #endregion fields

        #region constructor
        /// <summary>
        /// Class constructor from parameters.
        /// </summary>
        /// <param name="caption"></param>
        /// <param name="messageBoxText"></param>
        /// <param name="innerMessage"></param>
        /// <param name="buttonOption"></param>
        /// <param name="image"></param>
        /// <param name="defaultButton"></param>
        /// <param name="helpLink"></param>
        /// <param name="helpLinkTitle"></param>
        /// <param name="navigateHelplinkMethod"></param>
        /// <param name="enableCopyFunction"></param>
        /// <param name="defaultCloseResult">Determines the result if user closes a dialog with Esc, F4, or Window close button (X)</param>
        /// <param name="dialogCanCloseViaChrome">Determines whether user can close dialog via Esc, F4, or Window close button (X)</param>
        internal MsgBoxViewModel(string messageBoxText,
                                string caption,
                                string innerMessage,
                                MsgBoxButtons buttonOption,
                                MsgBoxImage image,
                                MsgBoxResult defaultButton = MsgBoxResult.None,
                                object helpLink = null,
                                string helpLinkTitle = "",
                                Func<object, bool> navigateHelplinkMethod = null,
                                bool enableCopyFunction = false,
                                MsgBoxResult defaultCloseResult = MsgBoxResult.None,
                                bool dialogCanCloseViaChrome = true)
        {
            this.mButtonOption = buttonOption;
            this.Title = caption;
            this.Message = messageBoxText;
            this.InnerMessageDetails = innerMessage;

            // Enable Copy should be set before button options since button options should
            // be able to over rule the enableCopyFunction parameter
            this.EnableCopyFunction = enableCopyFunction;
            this.SetButtonVisibility(buttonOption);

            this.IsDefaultButton = this.SetupDefaultButton(buttonOption, defaultButton);

            this.TypeOfImage = image;

            this.mHelpLink = helpLink;
            this.HelpLinkTitle = helpLinkTitle;

            this.Result = MsgBoxResult.None;
            this.DefaultCloseResult = defaultCloseResult;
            this.DialogCanCloseViaChrome = dialogCanCloseViaChrome;

            this.mDialogCloseResult = null;

            if (navigateHelplinkMethod != null)
                this.mNavigateHyperlinkMethod = navigateHelplinkMethod;
        }
        #endregion constructor

        #region events
        /// <summary>
        /// Expose an event that is triggered when the viewmodel tells its view:
        /// Here is another notification message please show it to the user.
        /// </summary>
////        public event UserNotification.Events.ShowNotificationEventHandler ShowNotificationMessage;
        #endregion events

        #region properties
        /// <summary>
        /// Title of message shown to the user (this is usally the Window title)
        /// </summary>
        public string Title
        {
            get
            {
                return this.mTitle;
            }

            set
            {
                if (this.mTitle != value)
                {
                    this.mTitle = value;
                    this.RaisePropertyChanged(() => this.Title);
                }
            }
        }

        /// <summary>
        /// Message content that tells the user what the problem is
        /// (why is it a problem, how can it be fixed,
        ///  and clicking which button will do what resolution [if any] etc...).
        /// </summary>
        public string Message
        {
            get
            {
                return this.mMessage;
            }

            set
            {
                if (this.mMessage != value)
                {
                    this.mMessage = value;
                    this.RaisePropertyChanged(() => this.Message);
                }
            }
        }

        /// <summary>
        /// More message details displayed in an expander (this can, for example,
        /// by a stacktrace or other technical information that can be shown for
        /// trouble shooting advanced scenarious via copy button - CSC etc...).
        /// </summary>
        public string InnerMessageDetails
        {
            get
            {
                return this.mInnerMessageDetails;
            }

            set
            {
                if (this.mInnerMessageDetails != value)
                {
                    this.mInnerMessageDetails = value;
                    this.RaisePropertyChanged(() => this.InnerMessageDetails);
                }
            }
        }

        /// <summary>
        /// Get/set property to determine a image in the copy message button
        /// of the dialog.
        /// 
        /// This property represents the actual IMAGE not the enumeration.
        /// </summary>
        public ImageSource CopyImageSource
        {
            get
            {
                return this.mCopyImageSource;
            }

            set
            {
                this.mCopyImageSource = value;
                this.RaisePropertyChanged(() => this.CopyImageSource);
            }
        }

        /// <summary>
        /// Get/set visibility of Yes/No buttons
        /// </summary>
        public bool YesNoVisibility
        {
            get
            {
                return this.mYesNoVisibility;
            }

            set
            {
                if (this.mYesNoVisibility != value)
                {
                    this.mYesNoVisibility = value;
                    this.RaisePropertyChanged(() => this.YesNoVisibility);
                }
            }
        }

        /// <summary>
        /// Get/set visibility of Cancel button
        /// </summary>
        public bool CancelVisibility
        {
            get
            {
                return this.mCancelVisibility;
            }

            set
            {
                if (this.mCancelVisibility != value)
                {
                    this.mCancelVisibility = value;
                    this.RaisePropertyChanged(() => this.CancelVisibility);
                }
            }
        }

        /// <summary>
        /// Get/set visibility of OK button
        /// </summary>
        public bool OkVisibility
        {
            get
            {
                return this.mOKVisibility;
            }

            set
            {
                if (this.mOKVisibility != value)
                {
                    this.mOKVisibility = value;
                    this.RaisePropertyChanged(() => this.OkVisibility);
                }
            }
        }

        /// <summary>
        /// Get/set visibility of Close button
        /// </summary>
        public bool CloseVisibility
        {
            get
            {
                return this.mCloseVisibility;
            }

            set
            {
                if (this.mCloseVisibility != value)
                {
                    this.mCloseVisibility = value;
                    this.RaisePropertyChanged(() => this.CloseVisibility);
                }
            }
        }

        /// <summary>
        /// Get/set visibility of Show Details section in dialog
        /// </summary>
        public bool ShowDetails
        {
            get
            {
                return this.mShowDetails;
            }

            set
            {
                if (this.mShowDetails != value)
                {
                    this.mShowDetails = value;
                    this.RaisePropertyChanged(() => this.ShowDetails);
                }
            }
        }

        /// <summary>
        /// Get property to determine the default button (if any)
        /// to be used in the dialog (user can hit ENTER key to execute that function).
        /// </summary>
        public MsgBoxResult IsDefaultButton
        {
            get
            {
                return this.mIsDefaultButton;
            }

            set
            {
                if (this.mIsDefaultButton != value)
                {
                    this.mIsDefaultButton = value;
                    this.RaisePropertyChanged(() => this.mIsDefaultButton);
                }
            }
        }

        /// <summary>
        /// Get/set property to determine whether the copy message
        /// function is available to the user or not (default: available).
        /// </summary>
        public bool EnableCopyFunction
        {
            get
            {
                return this.mEnableCopyFunction;
            }

            set
            {
                if (this.mEnableCopyFunction != value)
                {
                    this.mEnableCopyFunction = value;
                    this.RaisePropertyChanged(() => this.EnableCopyFunction);
                }
            }
        }

        /// <summary>
        /// Get property to determine whether the dialog can be closed with
        /// the corresponding result or not. This property is typically used
        /// with an attached behaviour (<seealso cref="DialogCloseResult"/>) in the Views's XAML.
        /// </summary>
        public bool? DialogCloseResult
        {
            get
            {
                return this.mDialogCloseResult;
            }

            private set
            {
                if (this.mDialogCloseResult != value)
                {
                    this.mDialogCloseResult = value;
                    this.RaisePropertyChanged(() => this.DialogCloseResult);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool DialogCanCloseViaChrome
        {
            get
            {
                return this.mDialogCanCloseViaChrome;
            }

            private set
            {
                if (this.mDialogCanCloseViaChrome != value)
                {
                    this.mDialogCanCloseViaChrome = value;
                    this.RaisePropertyChanged(() => this.DialogCanCloseViaChrome);
                }
            }
        }

        public bool CloseWindowButtonVisibility { get { return true; } }

        #region Help Hyperlink
        /// <summary>
        /// Gets/sets a string that is displayed to label a hyperlink.
        /// </summary>
        public string HyperlinkLabel
        {
            get
            {
                return (this.mHyperlinkLabel == null ? string.Empty : this.mHyperlinkLabel);
            }

            set
            {
                if (this.mHyperlinkLabel != value)
                {
                    this.mHyperlinkLabel = value;
                    this.RaisePropertyChanged(() => this.HyperlinkLabel);
                }
            }
        }

        /// <summary>
        /// Get/set property to determine the address to browsed to when displaying a help link.
        /// </summary>
        public string HelpLink
        {
            get
            {
                try
                {
                    if (this.mHelpLink != null)
                        return this.mHelpLink.ToString();
                }
                catch
                {
                }

                return string.Empty;
            }

            set
            {
                if (object.Equals(this.mHelpLink, value) != true)
                {
                    this.mHelpLink = value;
                    this.RaisePropertyChanged(() => this.HelpLink);
                    this.RaisePropertyChanged(() => this.AllToString);
                }
            }
        }

        /// <summary>
        /// Get/set property to determine the text for displaying a help link.
        /// By default the text is the toString content of the <seealso cref="HelpLink"/>
        /// but it can also be a different text if that text is set in the constructor.
        /// </summary>
        public string HelpLinkTitle
        {
            get
            {
                if ((this.mHelpLinkTitle == null ? string.Empty : this.mHelpLinkTitle).Length <= 0)
                    return this.HelpLink;

                return this.mHelpLinkTitle;
            }

            set
            {
                if (this.mHelpLinkTitle != value)
                {
                    this.mHelpLinkTitle = value;
                    this.RaisePropertyChanged(() => this.HelpLinkTitle);
                    this.RaisePropertyChanged(() => this.AllToString);
                }
            }
        }

        /// <summary>
        /// Get property to determine whether a helplink should be display or not.
        /// A helplink should not be displayed if there is no HelpLink information
        /// available, and it can be dispalyed otherwise.
        /// </summary>
        public bool DisplayHelpLink
        {
            get
            {
                if (this.mHelpLink == null)
                    return false;

                if (this.mHelpLink is string)
                {
                    if ((this.mHelpLink as string).Length == 0)
                        return false;
                }


                return true;
            }
        }
        #endregion Help Hyperlink

        /// <summary>
        /// Get property to get all textual information in one text block.
        /// This property is typically used to copy all text (even details)
        /// to the clipboard so users can paste it into their email and send
        /// the problem description off to those who care and know...
        /// </summary>
        public string AllToString
        {
            get
            {
                return string.Format("Title: {0}\n Message: {1}\n Help Link: {2}\nHelp Link Url: {3}\nMore Details: {4}\n",
                                                            this.Title, this.Message, this.HelpLinkTitle, this.HelpLink, this.InnerMessageDetails);
            }
        }

        /// <summary>
        /// Get property to determine type pf image to be shown to the user
        /// based on <seealso cref="MsgBoxImage"/> enumeration.
        /// </summary>
        public MsgBoxImage TypeOfImage
        {
            get;
            private set;
        }

        #region Commanding
        /// <summary>
        /// Get the command that is executed when the user clicked the 'Yes' button.
        /// </summary>
        public ICommand YesCommand
        {
            get
            {
                if (this.mYesCommand == null)
                    this.mYesCommand = new RelayCommand<object>((p) =>
                    {
                        this.Result = MsgBoxResult.Yes;
                        this.DialogCloseResult = true;
                    });

                return this.mYesCommand;
            }
        }

        /// <summary>
        /// Gets a command that is executed when the user clicked the 'No' button.
        /// </summary>
        public ICommand NoCommand
        {
            get
            {
                if (this.mNoCommand == null)
                    this.mNoCommand = new RelayCommand<object>((p) =>
                    {
                        this.Result = MsgBoxResult.No;
                        this.DialogCloseResult = true;
                    });
                return this.mNoCommand;
            }
        }

        /// <summary>
        /// Get the command that is executed when the user clicked the 'Cancel' button.
        /// </summary>
        public ICommand CancelCommand
        {
            get
            {
                if (this.mCancelCommand == null)
                    this.mCancelCommand = new RelayCommand<object>((p) =>
                    {
                        this.Result = MsgBoxResult.Cancel;
                        this.DialogCloseResult = true;
                    });

                return this.mCancelCommand;
            }
        }

        /// <summary>
        /// Get the command that is executed when the user clicked the 'Close' button
        /// or attempted to close the dialog via ESC-Key binding.
        /// </summary>
        public ICommand CloseCommand
        {
            get
            {
                if (this.mCloseCommand == null)
                {
                    this.mCloseCommand = new RelayCommand<object>((p) =>
                    {
                        bool bComparam = false;
                        MsgBoxResult ComParam = MsgBoxResult.None;

                        // The ESC-Key binding will close this with a parameter
                        if (p != null)
                        {
                            if (p is MsgBoxResult)
                            {
                                // Close via ESC key (or close via other than labeled buttons) is disabled
                                if (this.DialogCanCloseViaChrome == false)
                                {
                                    ////                                    this.ShowLegalCloseOptionsNotification();
                                    ////                                    return;
                                }

                                ComParam = (MsgBoxResult)p;
                                bComparam = true;
                            }
                        }

                        if (bComparam == true)
                        {
                            // Interpret close window (ESC Key) as Cancel
                            this.Result = ComParam;
                        }
                        else
                        {
                            // Close with (Close) button does not occur with a parameter
                            this.Result = MsgBoxResult.Close;
                        }

                        this.DialogCloseResult = true;
                    },
                    (p) => 
                    {
                        return this.DialogCanCloseViaChrome;
                    });
                }

                return this.mCloseCommand;
            }
        }

        /// <summary>
        /// Gets a command that is executed when the user clicked the 'OK' button.
        /// </summary>
        public ICommand OkCommand
        {
            get
            {
                if (this.mOKCommand == null)
                    this.mOKCommand = new RelayCommand<object>((p) =>
                    {
                        this.Result = MsgBoxResult.OK;
                        this.DialogCloseResult = true;
                    });

                return this.mOKCommand;
            }
        }

        /// <summary>
        /// Gets a command that starts a new (browser)
        /// process to navigate to this (web) target
        /// </summary>
        public ICommand NavigateToUri
        {
            get
            {
                if (this.mNavigateToUri == null)
                    this.mNavigateToUri = new RelayCommand<string>((param) => this.mNavigateHyperlinkMethod((string)param));

                return this.mNavigateToUri;
            }
        }

        /// <summary>
        /// Execute a command to copy the text string supplied
        /// as parameter into the clipboard
        /// </summary>
        public ICommand CopyText
        {
            get
            {
                if (this.mCopyText == null)
                    this.mCopyText = new RelayCommand<string>((param) => MsgBoxViewModel.CopyTextToClipboard((string)param));

                return this.mCopyText;
            }
        }
        #endregion Commanding
        #endregion properties

        #region methods
        /// <summary>
        /// Extract a textual tree of inner exceptions from an exception and return its representation as string.
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="textMessage"></param>
        /// <param name="details"></param>
        /// <returns></returns>
        public static string GetExceptionDetails(Exception exp,
                                                                                         string textMessage,
                                                                                         out string details)
        {
            details = string.Empty;
            string messageBoxText = string.Empty;

            try
            {
                // Write Message tree of inner exception into textual representation
                messageBoxText = exp.Message;

                Exception innerEx = exp.InnerException;

                for (int i = 0; innerEx != null; i++, innerEx = innerEx.InnerException)
                {
                    string spaces = string.Empty;

                    for (int j = 0; j < i; j++)
                        spaces += "  ";

                    messageBoxText += "\n" + spaces + "└─>" + innerEx.Message;
                }

                // Label message tree with meaningful info: "Error while reading file X."
                if (textMessage != null)
                {
                    if (textMessage.Length > 0)
                    {
                        messageBoxText = string.Format("{0}\n\n{1}", textMessage, messageBoxText);
                    }
                }

                // Write complete stack trace info into details section
                details = exp.ToString();
            }
            catch
            {
            }

            return messageBoxText;
        }

        /// <summary>
        /// Message box result is not set if dialog view is closed via F4, Window X button (standard chrome)
        /// so we determine whether the dialog is allowed to close via these 'hidden' mechanics and cancel it if not.
        /// 
        /// Otherwise, we set the result to default result here.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void MessageBox_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.DialogCanCloseViaChrome == false)
            {
                ////                if (this.Result == MsgBoxResult.None)
                ////                {
                ////                    // Revoke close event since this is not allowed here
                ////                    e.Cancel = true;
                ////
                ////                    this.ShowLegalCloseOptionsNotification();
                ////
                ////                    return;
                ////                }
            }

            // Just set default close result and continue closing the dialog
            if (this.Result == MsgBoxResult.None)
                this.Result = this.DefaultCloseResult;
        }

        /// <summary>
        /// Show a notification that users should use the labelled choice buttons to close a dialog
        /// instead of trying escape, ALT-F4, or Window Close (X) ... window chrome accessibilies...
        /// </summary>
        ////        private void ShowLegalCloseOptionsNotification()
        ////        {
        ////            if (this.ShowNotificationMessage != null)
        ////            {
        ////                this.ShowNotificationMessage(this, new ShowNotificationEvent
        ////                 (
        ////                    Local.Strings.Notificaation_Usage_Title,
        ////                    string.Format(Local.Strings.Notificaation_Usage_Description,
        ////                                                this.GetVisibleButtonDescription(this.mButtonOption)),
        ////
        ////                    this.GetApplicationResource("MsgBox_Usage_Notification",
        ////                                                                            "pack://application:,,,/MsgBox;component/Images/MsgBoxImages/48px-Dialog-error-round.svg.png")
        ////                 ));
        ////            }
        ////        }

        /// <summary>
        /// Attempt to locate a dynamic (<seealso cref="BitmapImage"/>) resource
        /// and return it or return null if the resource could not be located.
        /// </summary>
        /// <param name="resourceKey"></param>
        /// <param name="fallbackUri"></param>
        /// <returns></returns>
        private BitmapImage GetApplicationResource(string resourceKey,
                                                                                             string fallbackUri)
        {
            try
            {
                if (Application.Current.Resources[resourceKey] != null)
                {
                    if (Application.Current.Resources[resourceKey] is BitmapImage)
                    {
                        return Application.Current.Resources[resourceKey] as BitmapImage;
                    }
                }
            }
            catch
            {
            }

            // Try to locate fallback option if resource was not found in dictionary
            try
            {
                return new BitmapImage(new Uri(fallbackUri));
            }
            catch
            {
            }

            return null;
        }

        /// <summary>
        /// Write the supplied string into the Wiindows Clipboard such that
        /// users can past it into their favourite text editor
        /// </summary>
        /// <param name="textToCopy"></param>
        private static void CopyTextToClipboard(string textToCopy)
        {
            if (textToCopy == null) return;

            System.Windows.Clipboard.SetText(textToCopy);
        }

        /// <summary>
        /// Default method for navigating the hyperlink. A different method can
        /// be invoked if the corresponding constructor was used and
        /// <seealso cref="mNavigateHyperlinkMethod"/> was set
        /// (this method is ignorred in this case).
        /// </summary>
        /// <param name="uriTarget"></param>
        /// <returns></returns>
        private static bool NavigateToUniversalResourceIndicator(object uriTarget)
        {
            string uriTargetString = uriTarget as string;

            try
            {
                if (uriTargetString != null)
                {
                    Process.Start(new ProcessStartInfo(uriTargetString));
                    return true;
                }
            }
            catch
            {
            }

            return false;
        }

        /// <summary>
        /// Determine the visibility of each button based on the given <paramref name="buttonOption"/> parameter.
        /// </summary>
        /// <param name="buttonOption"></param>
        private void SetButtonVisibility(MsgBoxButtons buttonOption)
        {
            switch (buttonOption)
            {
                case MsgBoxButtons.OKCancel:
                    this.YesNoVisibility = false;
                    this.CancelVisibility = true;
                    this.OkVisibility = true;
                    this.CloseVisibility = false;
                    break;

                case MsgBoxButtons.OKCancelCopy:
                    this.EnableCopyFunction = true;
                    this.YesNoVisibility = false;
                    this.CancelVisibility = true;
                    this.OkVisibility = true;
                    this.CloseVisibility = false;
                    break;

                case MsgBoxButtons.YesNo:
                    this.YesNoVisibility = true;
                    this.CancelVisibility = false;
                    this.OkVisibility = false;
                    this.CloseVisibility = false;
                    break;

                case MsgBoxButtons.YesNoCopy:
                    this.EnableCopyFunction = true;
                    this.EnableCopyFunction = true;
                    this.YesNoVisibility = true;
                    this.CancelVisibility = false;
                    this.OkVisibility = false;
                    this.CloseVisibility = false;
                    break;

                case MsgBoxButtons.YesNoCancel:
                    this.YesNoVisibility = true;
                    this.CancelVisibility = true;
                    this.OkVisibility = false;
                    this.CloseVisibility = false;
                    break;

                case MsgBoxButtons.YesNoCancelCopy:
                    this.EnableCopyFunction = true;
                    this.YesNoVisibility = true;
                    this.CancelVisibility = true;
                    this.OkVisibility = false;
                    this.CloseVisibility = false;
                    break;

                case MsgBoxButtons.OK:
                    this.YesNoVisibility = false;
                    this.CancelVisibility = false;
                    this.OkVisibility = true;
                    this.CloseVisibility = false;
                    break;

                case MsgBoxButtons.OKCopy:
                    this.EnableCopyFunction = true;
                    this.YesNoVisibility = false;
                    this.CancelVisibility = false;
                    this.OkVisibility = true;
                    this.CloseVisibility = false;
                    break;

                case MsgBoxButtons.OKClose:
                    this.YesNoVisibility = false;
                    this.CancelVisibility = false;
                    this.OkVisibility = true;
                    this.CloseVisibility = true;
                    break;

                case MsgBoxButtons.OKCloseCopy:
                    this.EnableCopyFunction = true;
                    this.YesNoVisibility = false;
                    this.CancelVisibility = false;
                    this.OkVisibility = true;
                    this.CloseVisibility = true;
                    break;

                case MsgBoxButtons.Close:
                    this.YesNoVisibility = false;
                    this.CancelVisibility = false;
                    this.OkVisibility = false;
                    this.CloseVisibility = true;
                    break;

                case MsgBoxButtons.CloseCopy:
                    this.EnableCopyFunction = true;
                    this.YesNoVisibility = false;
                    this.CancelVisibility = false;
                    this.OkVisibility = false;
                    this.CloseVisibility = true;
                    break;

                default:
                    this.YesNoVisibility = false;
                    this.CancelVisibility = false;
                    this.OkVisibility = true;
                    this.CloseVisibility = true;
                    break;
            }

            if (string.IsNullOrEmpty(this.InnerMessageDetails))
                this.ShowDetails = false;
            else
                this.ShowDetails = true;
        }

        /// <summary>
        /// Gets the localized names of all buttons that are currently visible in the GUI
        /// as comma seperated list.
        /// </summary>
        /// <param name="buttonOption"></param>
        /// <returns></returns>
        private string GetVisibleButtonDescription(MsgBoxButtons buttonOption)
        {
            switch (buttonOption)
            {
                case MsgBoxButtons.OKCancel:
                case MsgBoxButtons.OKCancelCopy:
                    return String.Format("{0}, {1}", MsgBox.Local.Strings.OK, MsgBox.Local.Strings.Cancel);

                case MsgBoxButtons.YesNo:
                case MsgBoxButtons.YesNoCopy:
                    return String.Format("{0}, {1}", MsgBox.Local.Strings.Yes, MsgBox.Local.Strings.No);

                case MsgBoxButtons.YesNoCancel:
                case MsgBoxButtons.YesNoCancelCopy:
                    return String.Format("{0}, {1}, {2}", MsgBox.Local.Strings.Yes, MsgBox.Local.Strings.No, MsgBox.Local.Strings.Cancel);

                case MsgBoxButtons.Close:
                case MsgBoxButtons.CloseCopy:
                    return String.Format("{0}", MsgBox.Local.Strings.Close);

                case MsgBoxButtons.OKClose:
                case MsgBoxButtons.OKCloseCopy:
                    return String.Format("{0}, {1}", MsgBox.Local.Strings.OK, MsgBox.Local.Strings.Close);

                case MsgBoxButtons.OK:
                case MsgBoxButtons.OKCopy:
                default:
                    return String.Format("{0}", MsgBox.Local.Strings.OK);
            }
        }

        /// <summary>
        /// Determine a default button (such as OK or Yes) to be executed when the user hits the ENTER key.
        /// </summary>
        /// <param name="buttonOption"></param>
        /// <param name="defaultButton"></param>
        private MsgBoxResult SetupDefaultButton(MsgBoxButtons buttonOption,
                                                MsgBoxResult defaultButton)
        {
            MsgBoxResult ret = defaultButton;

            // Lets define a useful default button (can be executed with ENTER)
            // if caller did not define a button or
            // if did not explicitly told the sub-system to not define a default
            // button via MsgBoxResult.NoDefaultButton
            if (defaultButton == MsgBoxResult.None)
            {
                switch (buttonOption)
                {
                    case MsgBoxButtons.Close:
                    case MsgBoxButtons.CloseCopy:
                        ret = MsgBoxResult.Close;
                        break;

                    case MsgBoxButtons.OK:
                    case MsgBoxButtons.OKCancel:
                    case MsgBoxButtons.OKClose:
                    case MsgBoxButtons.OKCopy:
                    case MsgBoxButtons.OKCancelCopy:
                    case MsgBoxButtons.OKCloseCopy:
                        ret = MsgBoxResult.OK;
                        break;

                    case MsgBoxButtons.YesNo:
                    case MsgBoxButtons.YesNoCancel:
                    case MsgBoxButtons.YesNoCopy:
                    case MsgBoxButtons.YesNoCancelCopy:
                        ret = MsgBoxResult.Yes;
                        break;
                }
            }

            return ret;
        }
        #endregion methods
    }
}
