namespace MWindowDialogLib.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;

    /// <summary>
    /// Implements a collection of associated objects (typically a bound viewmodel)
    /// and their context object <seealso cref="DependencyObject"/>
    /// (typically a window).
    /// 
    /// Both items should be registered (e.g. via DialogParticipation.Register in XAML).
    /// This registration enables viewmodel implementations to have a context which is
    /// required to show related messages in the correct content of a window.
    /// </summary>
    internal class ContextRegistration : IContextRegistration
    {
        #region fields
        private readonly static IContextRegistration _ContextRegistration = new ContextRegistration();

        private readonly IDictionary<object, DependencyObject> _RegistrationIndex = null;
        #endregion fields

        #region construstors
        public ContextRegistration()
        {
            _RegistrationIndex = new Dictionary<object, DependencyObject>();
        }
        #endregion construstors

        #region properties
        /// <summary>
        /// Gets a static instance of this class.
        /// </summary>
        public static IContextRegistration Instance
        {
            get { return ContextRegistration._ContextRegistration; }
        }

        #endregion properties

        #region methods
        /// <summary>
        /// Register the associated object (typically a bound viewmodel) with
        /// the <seealso cref="DependencyObject"/> (typically a window).
        /// </summary>
        /// <param name="associatedObject"></param>
        /// <param name="dependencyObject"></param>
        public void AddContext(object associatedObject
                             , DependencyObject dependencyObject)
        {
            _RegistrationIndex.Add(associatedObject, dependencyObject);
        }

        /// <summary>
        /// Remove the associated object (typically a bound viewmodel) and its
        /// context <seealso cref="DependencyObject"/> (typically a window).
        /// </summary>
        /// <param name="associatedObject"></param>
        public void RemoveContext(object associatedObject)
        {
            _RegistrationIndex.Remove(associatedObject);
        }

        /// <summary>
        /// Determines whether a given context is registered or not.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public bool IsRegistered(object context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            return _RegistrationIndex.ContainsKey(context);
        }

        /// <summary>
        /// Gets the associated/registered object for a given  (registered) context.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public DependencyObject GetAssociation(object context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            return _RegistrationIndex[context];
        }

        /// <summary>
        /// Determines if a given context object is registered or not.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public bool ContainsKey(object context)
        {
            if (context == null)
                return false;

            return _RegistrationIndex.ContainsKey(context);
        }
        #endregion methods
    }
}
