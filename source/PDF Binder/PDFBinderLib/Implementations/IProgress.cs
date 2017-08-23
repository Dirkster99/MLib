namespace PDFBinderLib.Implementations
{
    public interface IProgress
    {
        #region properties
        int Min { get; set; }

        int Max { get; set; }

        int Value { get; set; }

        bool IsVisible { get; set; }
        #endregion properties

        #region methods
        void Reset(int min, int max, int value);

        void Start(int value, bool isVisible = true);
        #endregion methods
    }
}
