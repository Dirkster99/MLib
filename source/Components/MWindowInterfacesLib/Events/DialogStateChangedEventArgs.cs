namespace MWindowInterfacesLib.Events
{
    using System;

    /// <summary>
    /// Implements an event class that can be used to tell listeners when a dialog
    /// is closed, opened and so forth.
    /// </summary>
    public class DialogStateChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Class constructor.
        /// </summary>
        public DialogStateChangedEventArgs()
        {
        }
    }
}
