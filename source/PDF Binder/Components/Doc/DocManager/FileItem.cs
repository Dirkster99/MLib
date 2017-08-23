namespace Doc.DocManager
{
    using Interfaces;

    internal class FileItem : IFileItem
    {
        string _path = string.Empty;
        string _filename = string.Empty;

        public FileItem(string path, string filename)
        {
            _path = path;
            _filename = filename;
        }

        public string FileName
        {
            get
            {
                return _filename;
            }
        }

        public string Path
        {
            get
            {
                return _path;
            }
        }

        public string PathAndFileName
        {
            get
            {
                return System.IO.Path.Combine( _path, _filename);
            }
        }
    }
}
