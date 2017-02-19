using System;

namespace MDemo.Demos.ViewModels.Tasks
{
    /// <summary>
    /// Viewmodel drives a sample dialog that is really
    /// based on a CustomDialog that accepts a custom view and
    /// viewmodel and implements some standard behaviour
    /// <seealso cref="IBaseMetroDialogFrameViewModel"/>.
    /// </summary>
    public class ProgressViewModel : MsgDemoViewModel, IProgress
    {
        #region fields
        private bool _ProgressIsVisible = true;
        private bool _ProgressIsFinite = true;
        private double _ProgressMinimum = 0;
        private double _ProgressMaximum = 1.0;
        private double _ProgressValue = 0.0;
        private string _ProgressText = string.Empty;

        private bool _IsProgressing = false;
        private bool _AbortedWithCancel = false;

        private bool _ShowCloseButton = false;
        private bool _AbortedWithError = false;
        #endregion fields

        #region constructors
        /// <summary>
        /// Class constructor
        /// </summary>
        public ProgressViewModel(ProgressSettings progSettings)
            : this()
        {
            ResetSettings(progSettings);
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public ProgressViewModel()
        {
            ResetProgress(0, 1, 0);
            ProcessResult = null;
        }
        #endregion constructors

        #region properties
        /// <summary>
        /// Gets/Sets Maximum value for a finite progress display.
        /// </summary>
        public double ProgressMaximum
        {
            get { return _ProgressMaximum; }
            set
            {
                if (_ProgressMaximum != value)
                {
                    _ProgressMaximum = value;
                    RaisePropertyChanged(() => this.ProgressMaximum);
                }
            }
        }

        /// <summary>
        /// Gets/Sets Current value for a finite progress display.
        /// </summary>
        public double ProgressValue
        {
            get
            {
                return _ProgressValue;
            }

            set
            {
                if (_ProgressValue != value)
                {
                    _ProgressValue = value;
                    base.RaisePropertyChanged(() => this.ProgressValue);
                }
            }
        }

        /// <summary>
        /// Gets/Sets Minimum value for a finite progress display.
        /// </summary>
        public double ProgressMinimum
        {
            get { return _ProgressMinimum; }
            set
            {
                if (_ProgressMinimum != value)
                {
                    _ProgressMinimum = value;
                    RaisePropertyChanged(() => this.ProgressMinimum);
                }
            }
        }

        /// <summary>
        /// Gets/Sets Whether the bound progress display is finite (usually
        /// progressing from minimum to maximum) or infinite.
        /// </summary>
        public bool ProgressIsFinite
        {
            get { return _ProgressIsFinite; }
            set
            {
                if (_ProgressIsFinite != value)
                {
                    _ProgressIsFinite = value;
                    RaisePropertyChanged(() => this.ProgressIsFinite);
                }
            }
        }

        /// <summary>
        /// Gets/Sets whether the progress display should currently be visible
        /// or not (progress may not be displayed if processing has already finished)
        /// </summary>
        public bool ProgressIsVisible
        {
            get { return _ProgressIsVisible; }
            set
            {
                if (_ProgressIsVisible != value)
                {
                    _ProgressIsVisible = value;
                    RaisePropertyChanged(() => this.ProgressIsVisible);
                }
            }
        }

        /// <summary>
        /// Displays a status text to describe the current
        /// progress step in a textual way...
        /// </summary>
        public string ProgressText
        {
            get { return _ProgressText; }
            set
            {
                if (_ProgressText != value)
                {
                    _ProgressText = value;
                    RaisePropertyChanged(() => this.ProgressText);
                }
            }
        }

        /// <summary>
        /// Gets whether a Close button should currently be shown or not.
        /// </summary>
        public bool ShowCloseButton
        {
            get { return _ShowCloseButton; }
            protected set
            {
                if (_ShowCloseButton != value)
                {
                    _ShowCloseButton = value;
                    RaisePropertyChanged(() => this.ShowCloseButton);
                }
            }
        }

        /// <summary>
        /// Gets whether the process was cencelled by the user clicking Cancel
        /// (or cancelled via an exception being thrown)
        /// or mot.
        /// </summary>
        public bool AbortedWithCancel
        {
            get { return _AbortedWithCancel; }
            protected set
            {
                if (_AbortedWithCancel != value)
                {
                    _AbortedWithCancel = value;
                    RaisePropertyChanged(() => this.AbortedWithCancel);
                }
            }
        }

        /// <summary>
        /// Gets whether the process was aborted with an error or not.
        /// </summary>
        public bool AbortedWithError
        {
            get { return _AbortedWithError; }
            protected set
            {
                if (_AbortedWithError != value)
                {
                    _AbortedWithError = value;
                    RaisePropertyChanged(() => this.AbortedWithError);
                }
            }
        }

        /// <summary>
        /// Gets whether the process is in progress (currently progressing) or not.
        /// </summary>
        public bool IsProgressing
        {
            get { return _IsProgressing; }
            protected set
            {
                if (_IsProgressing != value)
                {
                    _IsProgressing = value;
                    RaisePropertyChanged(() => this.IsProgressing);
                }
            }
        }

        /// <summary>
        /// Gets/sets an object that can be set by the task that is
        /// executing inside the progress viewmodel. Most likely this
        /// object represents the result of the waiting for this process...
        /// </summary>
        public object ProcessResult { get; set; }
        #endregion properties

        #region methods
        public void ResetProgress(double min, double max, double value)
        {
            ProgressMinimum = min;
            ProgressMaximum = max;
            ProgressValue = value;
            ProgressIsVisible = false;
        }

        internal void ResetSettings(ProgressSettings progSettings)
        {
            if (progSettings == null)
                return;

            ProgressIsFinite = progSettings.ProgressIsFinite;
            ProgressIsVisible = progSettings.ProgressIsVisible;
            ProgressMaximum = progSettings.ProgressMaximum;
            ProgressMinimum = progSettings.ProgressMinimum;
            ProgressText = progSettings.ProgressText;
            ProgressValue = progSettings.ProgressValue;
        }

        internal void SetProgressing(bool inProgress)
        {
            this.IsProgressing = inProgress;

            if (inProgress == false)          // Not progressing any longer
            {                                // Processing is complete or has been cancelled
                SetShowCloseButton(true);
                ProgressIsVisible = false;
            }
            else
            {
                SetShowCloseButton(false);
                ////Progress.ProgressIsVisible = true; // Progress may be configured to be invisible
            }
        }

        internal void Aborted(bool withCancel, bool withError = false)
        {
            AbortedWithCancel = withCancel;
            AbortedWithError = withError;
        }

        internal void SetShowCloseButton(bool isCloseButtonVisible)
        {
            ShowCloseButton = isCloseButtonVisible;
        }
        #endregion methods
    }
}
