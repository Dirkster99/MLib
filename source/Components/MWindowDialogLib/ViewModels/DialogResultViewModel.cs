namespace MWindowDialogLib.ViewModels
{
    /// <summary>
    /// Implements a class template for a dialog driven view model in which
    /// it is important to lock the result (OK or Cancel button clicked) in a flexible fashion.
    ///
    /// This template can be used to drive a dialog based view through its life-cycle.
    /// A dialog based view can be very similar to a classic message box dialog but can also
    /// a drop-down content element, or something else.
    ///
    /// The difference here is that we use an int or enum or ... to return a value indicating
    /// the element that a user had selected in a custom dialog view.
    /// </summary>
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
