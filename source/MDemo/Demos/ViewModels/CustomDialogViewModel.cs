namespace MDemo.Demos.ViewModels
{
    using MDemo.ViewModels.Base;
    using System;
    using System.Windows.Input;

    public class CustomDialogViewModel : MsgDemoViewModel
    {
        #region fields
        private ICommand _closeCommand;
        private Action<CustomDialogViewModel> _closeHandler = null;

        private string _firstName = null;
        private string _lastName = null;
        #endregion fields

        public CustomDialogViewModel(Action<CustomDialogViewModel> closeHandler)
        {
            _closeHandler = closeHandler;
        }

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                RaisePropertyChanged(() => this.FirstName);
            }
        }

        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                RaisePropertyChanged(() => this.LastName);
            }
        }

        public override ICommand CloseCommand
        {
            get
            {
                if (_closeCommand == null)
                {
                    _closeCommand = new RelayCommand(() =>
                    {
                        _closeHandler(this);
                    });
                }
                return _closeCommand;
            }
        }
    }
}
