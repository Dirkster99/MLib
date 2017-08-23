namespace PDFBinderLib
{
    using Implementations;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Define an interface to contain all emthods necessary to combine PDF files.
    /// </summary>
    public interface IBinder : IDisposable
    {
        /// <summary>
        /// Returns a newly started task that combines all source PDF files into one target file.
        /// </summary>
        /// <param name="sourceFiles"></param>
        /// <param name="targetFile"></param>
        /// <returns></returns>
        Task<string> BindPDFAsync(IList<IPDFStateFile> sources, IPDFFile target, IProgress progress);

        /// <summary>
        /// Creates the target PDF file into wihch each
        /// source PDF file can be added.
        /// </summary>
        /// <param name="outputFilePath"></param>
        void CreateTargetPDF(string outputFilePath);

        /// <summary>
        /// Add another PDF file for binding into the existiing file
        /// that was created when the <seealso cref="IBinder"/> has
        /// been created.
        /// </summary>
        /// <param name="fileName"></param>
        void AddFile(string fileName);

        /// <summary>
        /// Test a PDF file to see whether it is OK, Unreadable or protected.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        PDFTestResult TestSourceFile(string fileName);
    }
}
