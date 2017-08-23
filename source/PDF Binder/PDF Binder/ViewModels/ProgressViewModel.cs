namespace PDF_Binder.ViewModels
{
    using PDFBinderLib.Implementations;

    public class ProgressViewModel : Base.ViewModelBase, IProgress
    {
        #region fields
        private int _Min;
        private int _Value;
        private int _Max;
        private bool _IsVisible;
        #endregion fields

        #region constructors
        public ProgressViewModel()
        {
            Reset(0, 1, 0);
        }
        #endregion constructors

        #region properties
        public int Max
        {
            get
            {
                return _Max;
            }

            set
            {
                if (_Max != value)
                {
                    _Max = value;
                    base.RaisePropertyChanged(() => Max);
                }
            }
        }

        public int Min
        {
            get
            {
                return _Min;
            }

            set
            {
                if (_Min != value)
                {
                    _Min = value;
                    base.RaisePropertyChanged(() => Min);
                }
            }
        }

        public int Value
        {
            get
            {
                return _Value;
            }

            set
            {
                if (_Value != value)
                {
                    _Value = value;
                    base.RaisePropertyChanged(() => Value);
                }
            }
        }

        public bool IsVisible
        {
            get
            {
                return _IsVisible;
            }

            set
            {
                if (_IsVisible != value)
                {
                    _IsVisible = value;
                    base.RaisePropertyChanged(() => IsVisible);
                }
            }
        }
        #endregion properties

        #region methods
        public void Reset(int min, int max, int value)
        {
            Min = min;
            Max = max;
            Value = value;
            IsVisible = false;
        }

        public void Start(int value, bool isVisible = true)
        {
            Value = value;
            IsVisible = isVisible;
        }
        #endregion methods
    }
}
