namespace MDemo.Demos.ViewModels
{
    using MDemo.ViewModels.Base;
    using MWindowInterfacesLib.Interfaces;
    using System.Windows.Input;

    /// <summary>
    /// Viewmodel drives a sample input dialog that is really
    /// based on a CustomDialog that accepts a custom view and
    /// viewmodel and implements some standard behaviour
    /// <seealso cref="IBaseMetroDialogFrameViewModel"/>.
    /// </summary>
    public class InputDialogViewModel : MsgDemoViewModel
    {
        #region fields
        private string _Input;
        private string _AffirmativeButtonText;
        private string _NegativeButtonText;

        private ICommand _OKCommand;
        private ICommand _CloseCommand;
        #endregion fields

        #region properties
        public string Input
        {
            get { return _Input; }
            set
            {
                if (_Input != value)
                {
                    _Input = value;
                    RaisePropertyChanged(() => this.Input);
                }
            }
        }

        public string NegativeButtonText
        {
            get { return _NegativeButtonText; }
            set
            {
                if (_NegativeButtonText != value)
                {
                    _NegativeButtonText = value;
                    RaisePropertyChanged(() => this.NegativeButtonText);
                }
            }
        }
        public string AffirmativeButtonText
        {
            get { return _AffirmativeButtonText; }
            set
            {
                if (_AffirmativeButtonText != value)
                {
                    _AffirmativeButtonText = value;
                    RaisePropertyChanged(() => this.AffirmativeButtonText);
                }
            }
        }

        /// <summary>
        /// Gets the OK command that is invoked to close this dialog.
        /// The OK command is invoked when the user clicks the OK button.
        /// </summary>
        public virtual ICommand OKCommand
        {
            get
            {
                if (this._OKCommand == null)
                {
                    this._OKCommand = new RelayCommand<object>((p) =>
                    {
                        base.Result = DialogIntResults.OK; // OK Button

                        base.SendDialogStateChangedEvent();

                        base.DialogCloseResult = true;
                    });
                }

                return this._OKCommand;
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
                        base.Result = DialogIntResults.CANCEL; // CANCEL Button

                        base.SendDialogStateChangedEvent();

                        base.DialogCloseResult = true;
                    });
                }

                return this._CloseCommand;
            }
        }
        #endregion properties
    }
}
