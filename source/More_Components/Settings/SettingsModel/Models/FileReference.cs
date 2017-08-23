namespace SettingsModel.Models
{
    using System.Xml.Serialization;

    /// <summary>
    /// Implement a simple file reverence model to allow XML persistence
    /// of a List<<seealso cref="FileReference"/>> via this class.
    /// </summary>
    public class FileReference
    {
        [XmlAttribute(AttributeName = "path")]
        public string path { get; set; }
    }
}
