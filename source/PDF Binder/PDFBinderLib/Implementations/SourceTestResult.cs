namespace PDFBinderLib
{
    /// <summary>
    /// Enumeration to indicate PDF File states as indicated by the core PDF library.
    /// </summary>
    public enum PDFTestResult
    {
        Unknown = -1
      , OK = 1
      , Unreadable = 0
      , Protected = 2
    }
}
