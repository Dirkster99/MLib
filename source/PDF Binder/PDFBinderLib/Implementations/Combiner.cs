namespace PDFBinderLib
{

    using Implementations;
    using iTextSharp.text;
    using iTextSharp.text.pdf;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    internal class Combiner : IBinder , IDisposable
    {
        #region fields
        Document _document = null;
        PdfCopy _pdfCopy = null;
        #endregion fields

        #region constructor
        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="outputFilePath"></param>
        public Combiner()
        {
        }
        #endregion constructors

        #region methods
        public void CreateTargetPDF(string outputFilePath)
        {
            var outputStream = File.Create(outputFilePath);

            _document = new Document();
            _pdfCopy = new PdfCopy(_document, outputStream);
            _document.Open();
        }

        /// <summary>
        /// Test a PDF file to see whether it is OK, Unreadable or protected.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public PDFTestResult TestSourceFile(string fileName)
        {
            try
            {
                PdfReader reader = new PdfReader(fileName);
                bool ok = !reader.IsEncrypted() ||
                    (reader.Permissions & PdfWriter.AllowAssembly) == PdfWriter.AllowAssembly;
                reader.Close();

                return ok ? PDFTestResult.OK : PDFTestResult.Protected;
            }
            catch
            {
                return PDFTestResult.Unreadable;
            }
        }

        /// <summary>
        /// Add another PDF file for binding into the existiing file
        /// that was created when the <seealso cref="IBinder"/> has
        /// been created.
        /// </summary>
        /// <param name="fileName"></param>
        public void AddFile(string fileName)
        {
            var reader = new PdfReader(fileName);

            for (var i = 1; i <= reader.NumberOfPages; i++)
            {
                var size = reader.GetPageSizeWithRotation(i);
                _document.SetPageSize(size);
                _document.NewPage();

                var page = _pdfCopy.GetImportedPage(reader, i);
                _pdfCopy.AddPage(page);
            }

            reader.Close();
        }

        public void Dispose()
        {
            try
            {
                _document.Close();
            }
            catch
            {
            }
        }

        /// <summary>
        /// Returns a newly started task that combines all source PDF files into one target file.
        /// </summary>
        /// <param name="sourceFiles"></param>
        /// <param name="targetFile"></param>
        /// <returns></returns>
        public async Task<string> BindPDFAsync(IList<IPDFStateFile> sourceFiles
            , IPDFFile targetFile
            , IProgress progress)
        {
            try
            {
                return await Task.Factory.StartNew<string>(() =>
                {
                    progress.Reset(0, (sourceFiles.Count * 2) - 1, 0);
                    progress.IsVisible = true;

                    bool IsOK = true;

                    progress.Start(0);

                    // Testing all source files
                    for (int i = 0; i < sourceFiles.Count; i++)
                    {
                        sourceFiles[i].State = TestSourceFile(sourceFiles[i].FileName);
                        progress.Value++;

                        if (sourceFiles[i].State != PDFTestResult.OK)
                            IsOK = false;
                    }

                    if (IsOK == false)
                        return string.Empty;

                    CreateTargetPDF(targetFile.FileName);

                    // Binding source files
                    for (int i = 0; i < sourceFiles.Count; i++)
                    {
                        AddFile((string)sourceFiles[i].FileName);
                        progress.Value++;
                    }

                    return targetFile.FileName;
                });
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }
        #endregion methods
    }
}
