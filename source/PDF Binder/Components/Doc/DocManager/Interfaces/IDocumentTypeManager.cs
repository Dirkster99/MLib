namespace Doc.DocManager.Interfaces
{
    using System;
    using System.Collections.ObjectModel;
/**
    /// <summary>
    /// Delegates the file open method to a method that can be registered in a module.
    /// The registered methid should return a viewmodel which in turn has registered a
    /// view and/or tool window viewmodels and views...
    /// </summary>
    /// <param name="fileModel"></param>
    /// <param name="settingsManager"></param>
    /// <returns></returns>
    public delegate IDocument FileOpenDelegate(IDocumentModel fileModel, object settingsManager);

    /// <summary>
    /// Delegates the file new method to a method that can be registered in a module.
    /// The registered method should return a viewmodel which in turn has registered a
    /// view for document display.
    /// 
    /// Create a new default document based on the given document model.
    /// </summary>
    /// <param name="documentModel"></param>
    /// <returns></returns>
    public delegate IDocument CreateNewDocumentDelegate(IDocumentModel documentModel);
***/
    /// <summary>
    /// Interface specification for the document management service that drives
    /// creation, loading and saving of documents in the low level backend.
    /// </summary>
    public interface IFileManager
    {
        #region properties
        ObservableCollection<IDocumentType> DocumentTypes { get; }
        #endregion properties

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="FileFilterName"></param>
        /// <param name="DefaultFilter"></param>
        /// <param name="FileOpenMethod">Is a static method that returns <seealso cref="FileBaseViewModel"/>
        /// and takes a string (path) and ISettingsManager as parameter.</param>
        /// <param name="t"></param>
        /// <returns></returns>
        IDocumentType RegisterDocumentType(string Key,
                                            string Name,
                                            string FileFilterName,
                                            string DefaultFilter,               // eg: 'log4j'
                                            ////FileOpenDelegate FileOpenMethod,
                                            ////CreateNewDocumentDelegate CreateDocumentMethod,
                                            Type t,
                                            int sortPriority = 0
                                            );

        /// <summary>
        /// Finds a document type that can handle a file
        /// with the given file extension eg ".txt" or "txt"
        /// when the original file name was "Readme.txt".
        /// 
        /// Always returns the 1st document type handler that matches the extension.
        /// </summary>
        /// <param name="fileExtension"></param>
        /// <param name="trimPeriod">Determines if an additional '.' character is removed
        /// from the given extension string or not.</param>
        /// <returns></returns>
        IDocumentType FindDocumentTypeByExtension(string fileExtension,
                                                  bool trimPeriod = false);

        IDocumentType FindDocumentTypeByKey(string typeOfDoc);

        /// <summary>
        /// Goes through all file/document type definitions and returns a filter string
        /// object that can be used in conjunction with FileOpen and FileSave dialog filters.
        /// </summary>
        /// <param name="key">Get entries for this viewmodel only,
        /// or all entries if key parameter is not set.</param>
        /// <returns></returns>
        IFileFilterEntries GetFileFilterEntries(string key = "");

        /// <summary>
        /// Expects a string containing a full path of a file to return an
        /// object with an explicitely seperated path and filename property.
        /// </summary>
        /// <param name="fileName">full path to a file.</param>
        /// <returns>An object with seperated path and file information.</returns>
        IFileItem GetFileItem(string fileName);
    }
}
