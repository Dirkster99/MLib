namespace MDemo.Demos.ViewModels.Tasks
{
    using MWindowInterfacesLib.Interfaces;
    using System;
    using System.Threading;

    /// <summary>
    /// Viewmodel drives a sample dialog that is really
    /// based on a CustomDialog that accepts a custom view and
    /// viewmodel and implements some standard behaviour
    /// <seealso cref="IBaseMetroDialogFrameViewModel"/>.
    /// </summary>
    public class ProgressSettings
    {
        #region constructors
        /// <summary>
        /// Progress is finite and
        /// can include text and
        /// can be canceled or not.
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="value"></param>
        /// <param name="progressText"></param>
        public ProgressSettings(
              double min
            , double max
            , double value
            , bool progressIsFinite = false         // Displaying a finite progress indicator
            , string progressText = default(string)
            , bool isCancelable = true
            , bool isVisible = true
            , bool closeDialogOnProgressFinished = false
            ) : this()
        {
            ProgressMinimum = min;
            ProgressMaximum = max;
            ProgressValue = value;
            ProgressIsFinite = progressIsFinite;
            ProgressText = progressText;
            IsCancelable = isCancelable;
            ProgressIsVisible = isVisible;
            CloseViewOnProgressFinished = closeDialogOnProgressFinished;
        }

        /// <summary>
        /// ProgressBar display is infinite or invisible and
        /// can include text and
        /// can be canceled or not.
        /// </summary>
        /// <param name="isVisible"></param>
        /// <param name="progressText"></param>
        public ProgressSettings(
              bool isVisible = true
            , string progressText = default(string)
            , bool isCancelable = true
            , bool closeDialogOnProgressFinished = false
            ) : this()                  // Displaying an infinite progress indicator via ctor
        {
            ProgressIsVisible = isVisible;
            ProgressText = progressText;
            IsCancelable = isCancelable;
            CloseViewOnProgressFinished = closeDialogOnProgressFinished;
        }

        /// <summary>
        /// Hidden standard constructor
        /// </summary>
        protected ProgressSettings()
        {
            ProgressIsVisible = true;
            ProgressIsFinite = true;
            ProgressMinimum = 0;
            ProgressMaximum = 1.0;
            ProgressValue = 0.0;
            IsCancelable = false;
            ProgressText = string.Empty;
            CloseViewOnProgressFinished = false;

            CancelButtonText = "Cancel";
            CloseButtonText = "Close";
            Title = Message = string.Empty;
            DefaultCloseResult = default(int);
            DefaultResult = DialogIntResults.OK;
        }
        #endregion constructors

        #region properties
        /// <summary>
        /// Gets the initial maximum value for a finite progress display.
        /// </summary>
        public double ProgressMaximum { get; protected set; }

        /// <summary>
        /// Gets the initial value for a finite progress display.
        /// </summary>
        public double ProgressValue { get; protected set; }

        /// <summary>
        /// Gets the initial minimum value for a finite progress display.
        /// </summary>
        public double ProgressMinimum { get; protected set; }

        /// <summary>
        /// Gets whether the progress display is configured to be infinite or not.
        /// </summary>
        public bool ProgressIsFinite { get; protected set; }

        /// <summary>
        /// Gets whether the progress indicator should be displayed or not.
        /// </summary>
        public bool ProgressIsVisible { get; protected set; }

        /// <summary>
        /// Displays a status text to describe the current
        /// progress step in a textual way...
        /// </summary>
        public string ProgressText { get; protected set; }

        /// <summary>
        /// Gets whether the progress can be canceled during processing or not.
        /// </summary>
        public bool IsCancelable { get; protected set; }

        /// <summary>
        /// Gets whether the dialog closes by itself when finished or whether it takes
        /// a user click to close the progress dialog.
        /// </summary>
        public bool CloseViewOnProgressFinished { get; protected set; }

        public string CancelButtonText { get; set; }

        public string CloseButtonText { get; set; }

        /// <summary>
        /// Title of message shown to the user (this is usally the Window title)
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Message content that tells the user what the problem is
        /// (why is it a problem, how can it be fixed,
        ///  and clicking which button will do what resolution [if any] etc...).
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets the default value for the result datatype.
        /// </summary>
        public int DefaultResult { get; set; }

        /// <summary>
        /// Gets property to determine dialog result when user closes it
        /// via F4 or Window Close (X) button when using window chrome.
        /// </summary>
        public int DefaultCloseResult { get; set; }

        public Action<CancellationToken, IProgress> ExecAction { get; set; }
        #endregion properties
    }
}
