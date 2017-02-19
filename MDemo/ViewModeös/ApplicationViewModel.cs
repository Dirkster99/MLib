namespace PDF_Binder.ViewModels
{
    using Base;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using System.Windows.Input;

    public class ApplicationViewModel : ViewModels.Base.ViewModelBase
    {
        #region private members
        private ICommand _BindPDFCommand = null;

        private bool? _windowActivated = null;
        #endregion private members

        public ApplicationViewModel()
        {
        }

        public bool? WindowActivated
        {
            get
            {
                return _windowActivated;
            }

            private set
            {
                if (value == null && _windowActivated != null ||
                    value != null && _windowActivated == null ||
                    (value != null && _windowActivated != null && value != _windowActivated))
                {
                    _windowActivated = value;
                    this.RaisePropertyChanged(() => this.WindowActivated);
                }
            }
        }
        public ICommand BindPDFCommand
        {
            get
            {
                if (_BindPDFCommand == null)
                {
                    _BindPDFCommand = new RelayCommand(() =>
                    {
                        // BindPDF(this.SourceFiles, this.TargetFile);
                    });
                }

                return _BindPDFCommand;
            }
        }
    }
}
