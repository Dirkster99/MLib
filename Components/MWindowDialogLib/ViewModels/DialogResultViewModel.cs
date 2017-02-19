namespace MWindowDialogLib.ViewModels
{
    public class DialogResultViewModel<T> : Base.BaseViewModel
    {
        #region fields
        private T mResult = default(T);
        private T mDefaultCloseResult = default(T);
        private T _DefaultResult = default(T);
        #endregion fields

        #region properties
        /// <summary>
        /// Get the resulting button (that has been clicked
        /// by the user) or result event when working with the dialog.
        /// </summary>
        public virtual T Result
        {
            get
            {
                return this.mResult;
            }

            protected set
            {
                if (this.mResult.Equals(value) == false)
                {
                    this.mResult = value;
                    this.RaisePropertyChanged(() => this.Result);
                }
            }
        }

        /// <summary>
        /// Gets the default value for the result datatype.
        /// </summary>
        public virtual T DefaultResult
        {
            get { return _DefaultResult; }

            set
            {
                if (this._DefaultResult.Equals(value) == false)
                {
                    this._DefaultResult = value;
                    this.RaisePropertyChanged(() => this.DefaultResult);
                }
            }
        }

        /// <summary>
        /// Gets property to determine dialog result when user closes it
        /// via F4 or Window Close (X) button when using window chrome.
        /// </summary>
        public virtual T DefaultCloseResult
        {
            get
            {
                return this.mDefaultCloseResult;
            }

            protected set
            {
                if (this.mDefaultCloseResult.Equals(value) == false)
                {
                    this.mDefaultCloseResult = value;
                    this.RaisePropertyChanged(() => this.DefaultCloseResult);
                }
            }
        }
        #endregion properties
    }
}
