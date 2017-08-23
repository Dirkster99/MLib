namespace MDemo.Demos.ViewModels
{
    using MDemo.ViewModels.Base;
    using MWindowInterfacesLib.Interfaces;
    using System.Security;
    using System.Windows.Input;

    /// <summary>
    /// Viewmodel drives a sample input dialog that is really
    /// based on a CustomDialog that accepts a custom view and
    /// viewmodel and implements some standard behaviour
    /// <seealso cref="IBaseMetroDialogFrameViewModel"/>.
    /// </summary>
    public class LoginDialogViewModel : MsgDemoViewModel
    {
        #region fields
        private string _Username = string.Empty;
        private string _AffirmativeButtonText = string.Empty;
        private string _NegativeButtonText = string.Empty;

        private ICommand _OKCommand;
        private ICommand _CloseCommand;

        private string _RememberCheckBoxText = string.Empty;
        private bool _RememberCheckBoxChecked = false;
        private bool _IsRememberCheckBoxVisible = false;

        private bool _IsNegativeButtonButtonVisible = true;

        private bool _IsDirty = false;

        private SecureString _Password = new SecureString();
        private bool _IsUserNameVisible = true;
        #endregion fields

        #region properties
        public bool IsUserNameVisible
        {
            get
            {
                return _IsUserNameVisible;
            }

            internal set
            {
                if (_IsUserNameVisible != value)
                {
                    _IsUserNameVisible = value;
                    RaisePropertyChanged(() => this.IsUserNameVisible);
                }
            }
        }

        public string Username
        {
            get { return _Username; }
            set
            {
                if (_Username != value)
                {
                    _Username = value;
                    RaisePropertyChanged(() => this.Username);
                }
            }
        }

        public SecureString Password
        {
            get
            {
                return _Password;
            }

            set
            {
                if (_Password != value)
                {
                    _Password = value;
                    IsDirty = true;
                }
            }
        }

        /// <summary>
        /// Get/set whether the settings stored in this instance have been
        /// changed and need to be saved when program exits (at the latest).
        /// </summary>
        public bool IsDirty
        {
            get
            {
                return _IsDirty;
            }

            set
            {
                if (_IsDirty != value)
                    _IsDirty = value;
            }
        }

        #region Remember Checkbox Properties
        public string RememberCheckBoxText
        {
            get { return _RememberCheckBoxText; }
            set
            {
                if (_RememberCheckBoxText != value)
                {
                    _RememberCheckBoxText = value;
                    RaisePropertyChanged(() => this.RememberCheckBoxText);
                }
            }
        }

        public bool RememberCheckBoxChecked
        {
            get { return _RememberCheckBoxChecked; }
            set
            {
                if (_RememberCheckBoxChecked != value)
                {
                    _RememberCheckBoxChecked = value;
                    RaisePropertyChanged(() => this.RememberCheckBoxChecked);
                }
            }
        }

        public bool IsRememberCheckBoxVisible
        {
            get { return _IsRememberCheckBoxVisible; }
            set
            {
                if (_IsRememberCheckBoxVisible != value)
                {
                    _IsRememberCheckBoxVisible = value;
                    RaisePropertyChanged(() => this.IsRememberCheckBoxVisible);
                }
            }
        }
        #endregion

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

        public bool IsNegativeButtonButtonVisible
        {
            get { return _IsNegativeButtonButtonVisible; }
            set
            {
                if (_IsNegativeButtonButtonVisible != value)
                {
                    _IsNegativeButtonButtonVisible = value;
                    RaisePropertyChanged(() => this.IsNegativeButtonButtonVisible);
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
