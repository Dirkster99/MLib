namespace MDemo.Demos.ViewModels
{
    using MDemo.ViewModels.Base;
    using MWindowInterfacesLib.Interfaces;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Tasks;

    /// <summary>
    /// Viewmodel drives a sample dialog that is really
    /// based on a CustomDialog that accepts a custom view and
    /// viewmodel and implements some standard behaviour
    /// <seealso cref="IBaseMetroDialogFrameViewModel"/>.
    /// </summary>
    public class ProgressDialogViewModel : MsgDemoViewModel
    {
        #region fields
        private string _CancelButtonText = string.Empty;
        private bool _IsCancelButtonVisible = true;

        private ICommand _CancelCommand;
        private ICommand _CloseCommand;
        private bool _IsEnabledClose = true;
        private bool _IsCancelable = false;

        private ProgressViewModel _Progress;
        private string _CloseButtonText = string.Empty;

        private CancellationTokenSource _CancelTokenSource = null;
        private CancellationToken _CancelToken = CancellationToken.None;
        private bool _CloseDialogOnProgressFinished = false;
        private bool? _StopButtonIsFocused = null;
        #endregion fieldsk

        #region constructors
        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="closeDialogOnProgressFinished"></param>
        public ProgressDialogViewModel(ProgressSettings settings)
          : this()
        {
            ResetSettings(settings);
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public ProgressDialogViewModel()
        {
            ResetSettings(new ProgressSettings());

            _CancelTokenSource = null;
            _CancelToken = CancellationToken.None;
        }
        #endregion constructors

        #region properties
        /// <summary>
        /// Gets the property with the main progress information shown in the UI.
        /// The view should bind to this item and ovserve updates here...
        /// </summary>
        public ProgressViewModel Progress
        {
            get
            {
                return _Progress;
            }
        }

        public string CancelButtonText
        {
            get { return _CancelButtonText; }
            set
            {
                if (_CancelButtonText != value)
                {
                    _CancelButtonText = value;
                    RaisePropertyChanged(() => this.CancelButtonText);
                }
            }
        }

        public bool? StopButtonIsFocused
        {
            get { return _StopButtonIsFocused; }
            set
            {
                if (_StopButtonIsFocused != value)
                {
                    _StopButtonIsFocused = value;
                    RaisePropertyChanged(() => this.StopButtonIsFocused);
                }
            }
        }

        public string CloseButtonText
        {
            get { return _CloseButtonText; }
            set
            {
                if (_CloseButtonText != value)
                {
                    _CloseButtonText = value;
                    RaisePropertyChanged(() => this.CloseButtonText);
                }
            }
        }

        public bool IsCancelable
        {
            get { return _IsCancelable; }
            protected set
            {
                if (_IsCancelable != value)
                {
                    _IsCancelable = value;
                    RaisePropertyChanged(() => this.IsCancelable);
                }
            }
        }

        public bool IsCancelButtonVisible
        {
            get { return _IsCancelButtonVisible; }
            protected set
            {
                if (_IsCancelButtonVisible != value)
                {
                    _IsCancelButtonVisible = value;
                    RaisePropertyChanged(() => this.IsCancelButtonVisible);

                    if (value == true)
                        StopButtonIsFocused = true;
                }
            }
        }

        /// <summary>
        /// Gets the OK command that is invoked to close this dialog.
        /// The OK command is invoked when the user clicks the OK button.
        /// </summary>
        public virtual ICommand CancelCommand
        {
            get
            {
                if (this._CancelCommand == null)
                {
                    this._CancelCommand = new RelayCommand<object>((p) =>
                    {
                        this.Cancel();
                    }
////// THE UI BEHAVES STRANGE IF WE DO THIS and so we don't
////// http://stackoverflow.com/questions/2331622/weird-problem-where-button-does-not-get-re-enabled-unless-the-mouse-is-clicked
////// Lets instead let IsEnabled handle this situation and the UI is looking good :-)
////                    ,       
////                    (p) =>
////                    {
////                        if (Progress != null)
////                        {
////                            if (IsCancelable == false)
////                                return false;
////                        }
////
////                        return true;
////                    }
                    );
                }

                return this._CancelCommand;
            }
        }

        /// <summary>
        /// Gets the OK command that is invoked to close this dialog.
        /// The OK command is invoked when the user clicks the OK button.
        /// </summary>
        public override ICommand CloseCommand
        {
            get
            {
                if (this._CloseCommand == null)
                {
                    this._CloseCommand = new RelayCommand<object>((p) =>
                    {
                        OnExecuteCloseDialog();
                    });
                }

                return this._CloseCommand;
            }
        }

        public bool IsEnabledClose
        {
            get { return _IsEnabledClose; }
            protected set
            {
                if (_IsEnabledClose != value)
                {
                    _IsEnabledClose = value;
                    RaisePropertyChanged(() => this.IsEnabledClose);
                }
            }
        }

        public bool CloseDialogOnProgressFinished
        {
            get { return _CloseDialogOnProgressFinished; }
            protected set
            {
                if (_CloseDialogOnProgressFinished != value)
                {
                    _CloseDialogOnProgressFinished = value;
                    RaisePropertyChanged(() => this.CloseDialogOnProgressFinished);
                }
            }
        }
        #endregion properties

        #region methods
        /// <summary>
        /// Cancel Asynchronous processing (if there is any right now)
        /// </summary>
        public void Cancel()
        {
            if (_CancelTokenSource != null && IsCancelable == true)
            {
                _CancelTokenSource.Cancel();
                SetCancelable(false);
            }
        }

        /// <summary>
        /// Executes an action (a set of .Net statements that get a <see cref="CancellationToken"/>
        /// (to reacte on a cancel request and cancel a process) and a <seealso cref="IProgress"/>
        /// interface to display the progress of the executed actions.
        /// </summary>
        /// <param name="settings"></param>
        internal void StartProcess(ProgressSettings[] settings)
        {
            _CancelTokenSource = new CancellationTokenSource();
            _CancelToken = _CancelTokenSource.Token;
            Progress.Aborted(false, false);
            IsEnabledClose = false;
            SetProgressing(true);

            Task taskToProcess = Task.Factory.StartNew(stateObj =>
            {
                try
                {
                    foreach (var item in settings)
                    {
                        this.ResetSettings(item);
                        _CancelToken.ThrowIfCancellationRequested();

                        item.ExecAction(_CancelToken, Progress);
                    }
                }
                catch (OperationCanceledException)
                {
                    Progress.Aborted(true, true);
                }
                catch (Exception)
                {
                }
                finally
                {
                    SetProgressing(false);
                    IsEnabledClose = true;

                    // Close this dialog if we are done progressing here...
                    // Or leave it open if progressing was cancelled or we ran into an error
                    // This approach always requires a close button to be available at the end
                    // of the progress because we otherwise leave a dialog open that cannot be
                    // closed by the user :-(
                    if (CloseDialogOnProgressFinished == true &&
                        Progress.AbortedWithCancel == false && Progress.AbortedWithError == false)
                    {
                        OnExecuteCloseDialog();
                    }
                }
            },
            _CancelToken).ContinueWith(ant =>
            {
            });
        }

        /// <summary>
        /// Reset all viewmodel states in accordance with the
        /// given <paramref name="settings"/> object. This is
        /// useful when presenting a sequence of different
        /// progress indicators (eg. infinite first and finite later)
        /// in 1 dialog.
        /// </summary>
        /// <param name="settings"></param>
        protected void ResetSettings(ProgressSettings settings)
        {
            if (_Progress == null)
                _Progress = new ProgressViewModel(settings);

            _Progress.ResetSettings(settings);

            IsCancelable = IsCancelButtonVisible = settings.IsCancelable;
            CloseDialogOnProgressFinished = settings.CloseViewOnProgressFinished;

            Title = settings.Title;
            Message = settings.Message;

            CancelButtonText = settings.CancelButtonText;
            CloseButtonText = settings.CloseButtonText;

            DefaultResult = settings.DefaultResult;
            DefaultCloseResult = settings.DefaultCloseResult;
        }

        /// <summary>
        /// Method executes when the user clicked on the close command OR
        /// when the progress has finished without error and dialog was
        /// configured to close automatically.
        /// </summary>
        private void OnExecuteCloseDialog()
        {
            if (Progress != null)
            {
                if (Progress.IsProgressing == true)
                {
                    return;
                }
            }

            base.Result = DialogIntResults.CANCEL; // CANCEL Button

            base.SendDialogStateChangedEvent();

            base.DialogCloseResult = true;
        }

        /// <summary>
        /// Performs the viewmodel logic to Show the Stop button
        /// only when the process can be cancelled.
        /// </summary>
        /// <param name="isCancelable"></param>
        private void SetCancelable(bool isCancelable)
        {
            if (IsCancelable == isCancelable)
                return;

            IsCancelable = isCancelable;

            if (this.Progress.IsProgressing == false)
            {
                this.Progress.SetShowCloseButton(true);
                IsCancelButtonVisible = false;
            }
            else
            {
                this.Progress.SetShowCloseButton(false);
                IsCancelButtonVisible = IsCancelable;
            }
        }

        private void SetProgressing(bool inProgress)
        {
            if (Progress.IsProgressing != inProgress)
            {
                Progress.SetProgressing(inProgress);

                if (inProgress == false)              // Not progressing any longer
                {                                    // Processing is complete or has been cancelled
                    DialogCanCloseViaChrome = true; // Support ESC or Enter behaviour when not progressing
                    IsCancelButtonVisible = false;
                }
                else
                {
                    DialogCanCloseViaChrome = false; // Do not support ESC or Enter behaviour when progressing
                    IsCancelButtonVisible = IsCancelable;
                }
            }
        }
        #endregion methods
    }
}
