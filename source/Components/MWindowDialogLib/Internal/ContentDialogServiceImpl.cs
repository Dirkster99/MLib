namespace MWindowDialogLib.Internal
{
    using MWindowInterfacesLib;
    using MWindowInterfacesLib.Interfaces;
    using MWindowInterfacesLib.MsgBox;

    /// <summary>
    /// Implements a service that shows content dialogs
    /// (MessageBoxes, LoginDialog etc) within the context
    /// of a given WPF Window.
    /// </summary>
    internal class ContentDialogServiceImpl : IContentDialogService
    {
        #region fields
        private readonly IDialogCoordinator _dialogCoordinator = null;
        private readonly IDialogManager _dialogManager = null;
        private readonly IMessageBoxService _MsgBox = null;

        private readonly IMetroDialogFrameSettings _DialogSettings = null;
        #endregion fields

        #region constructor
        /// <summary>
        /// Class constructor
        /// </summary>
        public ContentDialogServiceImpl()
        {
            _dialogManager = new DialogManager();
            _dialogCoordinator = new DialogCoordinator(_dialogManager);
            _MsgBox = new MessageBoxServiceImpl(this);

            _DialogSettings = new MetroDialogFrameSettings();
        }
        #endregion constructor

        #region properties
        /// <summary>
        /// Gets the default dialog settings that are applied when invoking
        /// a dialog from this service.
        /// 
        /// The message box service methodes take care of this property, automatically,
        /// the methodes in all other services, <seealso cref="DialogManager"/> and
        /// <seealso cref="DialogCoordinator"/> should be invoked with this property
        /// as parameter (or will be invoked with default settings).
        /// </summary>
        public IMetroDialogFrameSettings DialogSettings
        {
            get
            {
                return _DialogSettings;
            }
        }

        /// <summary>
        /// Gets an instance that implements the <seealso cref="IDialogCoordinator"/> interface.
        /// </summary>
        public IDialogCoordinator Coordinator
        {
            get
            {
                return _dialogCoordinator;
            }
        }

        /// <summary>
        /// Gets an instance that implments the <seealso cref="IDialogManager"/> interface.
        /// </summary>
        public IDialogManager Manager
        {
            get
            {
                return _dialogManager;
            }
        }

        /// <summary>
        /// Gets a message box service that can display message boxes
        /// in a variety of different configurations.
        /// </summary>
        public IMessageBoxService MsgBox
        {
            get
            {
                return _MsgBox;
            }
        }
        #endregion properties
    }
}
