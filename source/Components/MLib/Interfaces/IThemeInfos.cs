namespace MLib.Interfaces
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Interface to a class that manages a set of theminfo entries.
    /// </summary>
    public interface IThemeInfos
    {
        /// <summary>
        /// Add another theme entry by its name and Uri source.
        /// </summary>
        /// <param name="name">The unique (display) name for this theme.</param>
        /// <param name="themeSources">List of Uri based resources to be loaded for this theme.</param>
        void AddThemeInfo(string name, List<Uri> themeSources);

        /// <summary>
        /// Add another theme entry by its name and Uri source.
        /// </summary>
        /// <param name="theme">The <see cref="IThemeInfo"/> based object instance containing
        /// the unique name definition and collection of Uri based resources to be loaded for
        /// this theme.</param>
        void AddThemeInfo(IThemeInfo theme);

        /// <summary>
        /// Retrieve an existing theme entry by its Uri source.
        /// Returns null if theme is not present.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IThemeInfo GetThemeInfo(string name);

        /// <summary>
        /// Remove an existing theme entry by its Uri source.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IThemeInfo RemoveThemeInfo(string name);

        /// <summary>
        /// Remove all existing theme entries.
        /// </summary>
        void RemoveAllThemeInfos();

        /// <summary>
        /// Enumerate through all themes
        /// </summary>
        /// <returns></returns>
        IEnumerable<IThemeInfo> GetThemeInfos();
    }
}
