namespace PDFBinderLib
{
    public interface IPDFFile
    {
        string FileName { get; }
    }

    public interface IPDFStateFile : IPDFFile
    {
        PDFTestResult State { get; set; }
    }
    
}
