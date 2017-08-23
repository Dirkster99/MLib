namespace Doc
{
    using DocManager.Interfaces;
    using System.Collections.Generic;

    internal class DocumentTypeItem : IDocumentTypeItem
    {
        #region fields
        #endregion fields

        #region constructors
        /// <summary>
        /// Class constructor
        /// </summary>
        public DocumentTypeItem(string description, List<string> extensions, int sortPriority = 0)
        {
            this.Description = description;
            this.DocFileTypeExtensions = extensions;
            this.SortPriority = sortPriority;
        }
        #endregion constructors

        #region properties
        public List<string> DocFileTypeExtensions { get; private set; }

        public string Description { get; private set; }

        public int SortPriority { get; private set; }
        #endregion properties

        #region methods
        #endregion methods
    }
}
