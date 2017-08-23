namespace WatermarkControlsDemo.ViewModels
{
    /// <summary>
    /// Defines a theme by its name, source etc...
    /// </summary>
    public class ThemeDefinition
    {
        /// <summary>
        /// Hidden standard constructor.
        /// </summary>
        private ThemeDefinition()
        {
        }

        /// <summary>
        /// Parameterized constructor.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="source"></param>
        public ThemeDefinition(string name, string source)
        {
            this.Name = (name != null ? name : string.Empty);
            this.Source = (source != null ? source : string.Empty);
        }

        /// <summary>
        /// Identifies a theme by a Name that can be used as a key.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Uri formatted source for this theme.
        /// </summary>
        public string Source { get; private set; }
    }
}