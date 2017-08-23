namespace MWindowDialogLib.Internal
{
    using System.Windows;

    public class Find
    {
        /// <summary>
        /// Attempts to find a suitable owner window by searching in
        /// 1) The registered context object associations <seealso cref="ContextRegistration"/> and
        /// 2) The standard collection of .Net window objects.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dialog"></param>
        /// <returns>The suitable or null if none was found.</returns>
        public static Window OwnerWindow(
                object context
              , Window dialog = null)
        {
            // Just return what we got if this is already a window
            if (context is Window)
                return context as Window;

            // Start to search for a Window reference
            Window mainWindow = context as Window;
            Window dialogOwner = null;

            if (mainWindow == null)
            {
                // Lets see if this context is registered
                if (context != null)
                {
                    mainWindow = ContextRegistration.Instance.GetAssociation(context) as Window;

                    if (mainWindow != null)
                        dialogOwner = mainWindow;
                }

                // Context is not registered - lets try and find a suitable window anyway
                if (mainWindow == null)
                {
                    if (Application.Current != null)
                    {
                        if (dialog != null)
                        {
                            if (dialog != Application.Current.MainWindow)
                                dialogOwner = Application.Current.MainWindow;
                            else
                                dialogOwner = GetOwnerWindow();
                        }
                        else // dialog == null
                        {
                            if (Application.Current.MainWindow != null)
                                dialogOwner = Application.Current.MainWindow;
                            else
                                dialogOwner = GetOwnerWindow();
                        }
                    }
                }
            }
            else
            {
                if (dialog != null)
                {
                    if (dialog != mainWindow)
                        dialogOwner = mainWindow;
                }
            }

            if (dialog != null)
            {
                // Last chance check to make sure window can open without main window
                // (eg.: in start-up or after shut-down)
                if (dialogOwner == dialog)
                    dialogOwner = null;
            }

            return dialogOwner;
        }

        /// <summary>
        /// Attempt to find the owner window for a message box
        /// </summary>
        /// <returns>Owner Window</returns>
        private static Window GetOwnerWindow()
        {
            Window owner = null;

            if (Application.Current != null)
            {
                foreach (Window w in Application.Current.Windows)
                {
                    if (w != null)
                    {
                        if (w.IsActive)
                        {
                            owner = w;
                            break;
                        }
                    }
                }
            }

            return owner;
        }
    }
}
