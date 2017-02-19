namespace MWindowDialogLib.Internal
{
    using System.Windows;

    internal interface IContextRegistration
    {
        /// <summary>
        /// Register the associated object (typically a bound viewmodel) with
        /// the <seealso cref="DependencyObject"/> (typically a window).
        /// </summary>
        /// <param name="associatedObject"></param>
        /// <param name="dependencyObject"></param>
        void AddContext(object associatedObject, DependencyObject dependencyObject);

        /// <summary>
        /// Remove the associated object (typically a bound viewmodel) and its
        /// context <seealso cref="DependencyObject"/> (typically a window).
        /// </summary>
        /// <param name="associatedObject"></param>
        void RemoveContext(object associatedObject);

        /// <summary>
        /// Determines whether a given context is registered or not.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        bool IsRegistered(object context);

        /// <summary>
        /// Gets the associated/registered object for a given  (registered) context.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        DependencyObject GetAssociation(object context);

        /// <summary>
        /// Determines if a given context object is registered or not.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        bool ContainsKey(object context);
    }
}