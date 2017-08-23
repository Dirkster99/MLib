namespace MDemo.Behaviours
{
    using System;
    using System.Runtime.InteropServices;
    using System.Security;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
	/// Source:
	/// http://stackoverflow.com/questions/1034374/drag-and-drop-in-mvvm-with-scatterview
	/// http://social.msdn.microsoft.com/Forums/de-DE/wpf/thread/21bed380-c485-44fb-8741-f9245524d0ae
	/// 
	/// Attached behaviour to implement the text changed event via delegate command binding
    /// or routed commands on a PasswordBox.
	/// </summary>
    public static class PasswordBoxTextChanged
    {
        // Using a DependencyProperty as the backing store for EncryptedPassword.
        public static readonly DependencyProperty EncryptedPasswordProperty =
            DependencyProperty.RegisterAttached("EncryptedPassword",
                typeof(SecureString),
                typeof(PasswordBoxTextChanged),
                new PropertyMetadata(null, OnCommandChange));

        #region Helper Methods SecureString Conversion
        /// <summary>
        /// Source: http://blogs.msdn.com/b/fpintos/archive/2009/06/12/how-to-properly-convert-securestring-to-string.aspx
        /// </summary>
        public static string ConvertToUnsecureString(SecureString securePassword)
        {
            if (securePassword == null)
                throw new ArgumentNullException("securePassword");

            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(securePassword);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }

        public static SecureString ConvertToSecureString(string password)
        {
            if (password == null)
                throw new ArgumentNullException("password");

            var securePassword = new SecureString();
            foreach (char c in password)
                securePassword.AppendChar(c);

            securePassword.MakeReadOnly();
            return securePassword;
        }
        #endregion Helper Methods SecureString Conversion

        public static SecureString GetEncryptedPassword(DependencyObject obj)
        {
            return (SecureString)obj.GetValue(EncryptedPasswordProperty);
        }

        public static void SetEncryptedPassword(DependencyObject obj, SecureString value)
        {
            obj.SetValue(EncryptedPasswordProperty, value);
        }

        /// <summary>
        /// This method is hooked in the definition of the <seealso cref="DropCommandProperty"/>.
        /// It is called whenever the attached property changes - in our case the event of binding
        /// and unbinding the property to a sink is what we are looking for.
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnCommandChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var uiElement = d as PasswordBox;	                // Remove the handler if it exist to avoid memory leaks
            uiElement.PasswordChanged -= UiElement_PasswordChanged;

            if (e.NewValue != null)
            {
                var secureString = e.NewValue as SecureString;
                if (secureString != null)
                {
                    // Question: Is there any more secure way to init the passowrd box content with?
                    uiElement.Password = PasswordBoxTextChanged.ConvertToUnsecureString(secureString);
                }
            }

            // the property is attached so we attach the Drop event handler
            uiElement.PasswordChanged += UiElement_PasswordChanged;
        }

        /// <summary>
        /// This method is called when the registered event occurs. The sender should be the control
        /// on which this behaviour is attached - so we convert the sender into a <seealso cref="UIElement"/>
        /// and receive the Command through the <seealso cref="GetCommand"/> getter listed above.
        /// 
        /// The <paramref name="e"/> parameter contains the standard <seealso cref="EventArgs"/> data,
        /// which is unpacked and send upon the bound command.
        /// 
        /// This implementation supports binding of delegate commands and routed commands.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void UiElement_PasswordChanged(object sender, RoutedEventArgs e)
        {
            // Sender should be this class or a descendent of it
            var fwElement = sender as PasswordBox;

            // Sanity check just in case this was somehow send by something else
            if (fwElement == null)
                return;

            //Set bound dependency property in viewmodel
            SetEncryptedPassword(fwElement, fwElement.SecurePassword.Copy());
        }
    }
}
