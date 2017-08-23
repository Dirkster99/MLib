namespace Doc.DocManager.Interfaces
{
    public interface IFileItem
    {
        string Path { get; }

        string FileName { get; }

        string PathAndFileName { get; }
    }
}
