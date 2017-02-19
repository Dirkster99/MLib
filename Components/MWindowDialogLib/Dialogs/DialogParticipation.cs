namespace MWindowDialogLib.Dialogs
{
    using Internal;
    using System;
    using System.Collections.Generic;
    using System.Windows;

    /// <summary>
    /// Sample application in XAML Code:
    /// 
    /// xmlns:Dialog="clr-namespace:MWindowDialogLib.Dialogs;assembly=MWindowDialogLib"
    /// 
    /// Dialog:DialogParticipation.Register="{Binding Demo}"
    /// </summary>
    public static class DialogParticipation
    {
        #region Register dependency property
        public static readonly DependencyProperty RegisterProperty = DependencyProperty.RegisterAttached(
            "Register",
            typeof(object),
            typeof(DialogParticipation),
            new PropertyMetadata(default(object), RegisterPropertyChangedCallback));


        /// <summary>
        /// Register the associated object (typically a bound viewmodel) with
        /// the <seealso cref="DependencyObject"/> (typically a window).
        /// </summary>
        /// <param name="element"></param>
        /// <param name="context"></param>
        public static void SetRegister(DependencyObject element, object context)
        {
            element.SetValue(RegisterProperty, context);
        }

        /// <summary>
        /// Get the associated object (typically a bound viewmodel) with
        /// the <seealso cref="DependencyObject"/> (typically a window)
        /// as parameter.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="context"></param>
        public static object GetRegister(DependencyObject element)
        {
            return element.GetValue(RegisterProperty);
        }
        #endregion Register dependency property

        /// <summary>
        /// Method is invoked when the <seealso cref="RegisterProperty"/>
        /// changed event is being processed.
        /// </summary>
        /// <param name="dependencyObject"></param>
        /// <param name="dependencyPropertyChangedEventArgs"></param>
        private static void RegisterPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            // Remove the old value (if any)
            if (dependencyPropertyChangedEventArgs.OldValue != null)
                ContextRegistration.Instance.RemoveContext(dependencyPropertyChangedEventArgs.OldValue);

            // Add the new value (if any)
            if (dependencyPropertyChangedEventArgs.NewValue != null)
                ContextRegistration.Instance.AddContext(dependencyPropertyChangedEventArgs.NewValue
                                                        ,dependencyObject);
        }

        /// <summary>
        /// Determines whether a given context is registered or not.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        internal static bool IsRegistered(object context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            return ContextRegistration.Instance.ContainsKey(context);
        }

        /// <summary>
        /// Gets the associated/registered object for a given  (registered) context.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        internal static DependencyObject GetAssociation(object context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            return ContextRegistration.Instance.GetAssociation(context);
        }
    }
}
