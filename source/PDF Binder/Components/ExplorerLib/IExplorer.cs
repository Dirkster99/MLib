namespace ExplorerLib
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Implements an interface to an object that
    /// implements a set of standard functions for accessing the
    /// file system via file open and file save dialogs.
    /// </summary>
    public interface IExplorer
    {
        /// <summary>
        /// Let the user select a file to open
        /// -> return its path if file open was OK'ed
        ///    or return null on cancel.
        /// </summary>
        /// <param name="fileFilter"></param>
        /// <param name="lastFilePath"></param>
        /// <param name="myDocumentsUserDir"></param>
        /// <returns></returns>
        string FileOpen(string fileFilter, string lastFilePath, string myDocumentsUserDir = null);

        /// <summary>
        /// Method can be used to open mutlipe files via standard Windows Explorer
        /// File open dialog.
        /// </summary>
        /// <param name="fileFilter"></param>
        /// <param name="lastFilePath"></param>
        /// <param name="myDocumentsUserDir"></param>
        /// <returns></returns>
        IEnumerable<string> FileOpenMultipleFiles(string fileFilter,
                                                  string lastFilePath,
                                                  string myDocumentsUserDir = null);

        /// <summary>
        /// Save a file with a given path <paramref name="path"/> (that may be ommited -> results in SaveAs)
        /// using a given save function <paramref name="saveDocumentFunction"/> that takes a string parameter and returns bool on success.
        /// The <param name="saveAsFlag"/> can be set to true to indicate if whether SaveAs function is intended.
        /// The <param name="FileExtensionFilter"/> can be used to filter files when using a SaveAs dialog.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="saveDocumentFunction"></param>
        /// <param name="stringDiff"></param>
        /// <param name="saveAsFlag"></param>
        /// <param name="FileExtensionFilter"></param>
        /// <param name="lastFilePath"></param>
        /// <returns></returns>
        string GetDirectoryFromFilePath(string lastFilePath);

        /// <summary>
        /// Get a file path name reference and return the containing path (if any)
        /// </summary>
        /// <param name="lastFilePath"></param>
        /// <returns></returns>
        bool SaveDocumentFile(string path, Func<string, bool> saveDocumentFunction, string myDocumentsUserDir = null, bool saveAsFlag = false, string FileExtensionFilter = "");
    }
}
