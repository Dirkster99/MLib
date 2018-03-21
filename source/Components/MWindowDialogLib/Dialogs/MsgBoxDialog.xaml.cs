namespace MWindowDialogLib.Dialogs
{
    using MWindowInterfacesLib.Interfaces;
    using MWindowInterfacesLib.MsgBox.Enums;
    using System;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;
    using ViewModels;

    /// <summary>
    /// An internal control that represents a message dialog. Please use MetroWindow.ShowMessage instead!
    /// </summary>
    public partial class MsgBoxDialog : DialogFrame, IMsgBoxDialogFrame<MsgBoxResult>
    {
        #region constructors
        /// <summary>
        /// Class constructor.
        /// </summary>
        public MsgBoxDialog()
            : base()
        {
            InitializeComponent();
            this.DialogThumb = null;

            this.Loaded += MsgBoxDialog_Loaded;
        }

        /// <summary>
        /// Class constructor from parameters.
        /// </summary>
        /// <param name="parentWindow"></param>
        internal MsgBoxDialog(IMetroWindow parentWindow)
            : this(parentWindow, null)
        {
            InitializeComponent();
            this.DialogThumb = null;

            this.Loaded += MsgBoxDialog_Loaded;
        }

        /// <summary>
        /// Class constructor from parameters.
        /// </summary>
        /// <param name="parentWindow"></param>
        /// <param name="settings"></param>
        internal MsgBoxDialog(IMetroWindow parentWindow
                            , IMetroDialogFrameSettings settings)
            : base(parentWindow, settings)
        {
            InitializeComponent();
            this.DialogThumb = null;

            this.Loaded += MsgBoxDialog_Loaded;
        }

        /// <summary>
        /// Constructor from custom view and optional viewmodel.
        /// </summary>
        /// <param name="parentWindow"></param>
        /// <param name="content"></param>
        /// <param name="viewnodel"></param>
        /// <param name="settings"></param>
        internal MsgBoxDialog(IMetroWindow parentWindow
                            , FrameworkElement content
                            , object viewnodel = null
                            , IMetroDialogFrameSettings settings = null)
            : base(parentWindow, settings)
        {
            InitializeComponent();

            // Set the display view here ...
            this.PART_Msg_Content.ChromeContent = content;
            this.DialogThumb = this.PART_Msg_Content.PART_DialogTitleThumb;

            // Get a view and bind datacontext to it
            this.DataContext = viewnodel;

            this.Loaded += MsgBoxDialog_Loaded;
        }
        #endregion constructors

        /// <summary>
        /// Gets the dialog's thumb that is used to drag the dialog around
        /// when the user drags it.
        /// </summary>
        public Thumb DialogThumb { get; protected set; }

        #region methods
        /// <summary>
        /// Keeps the dialog open until a user or process has signalled
        /// that we can close this with a result...
        /// </summary>
        /// <returns></returns>
        public Task<MsgBoxResult> WaitForButtonPressAsync()
        {
            TaskCompletionSource<MsgBoxResult> tcs = new TaskCompletionSource<MsgBoxResult>();

            // List events that will be handled to exit the dialog with an enumerated result
            KeyEventHandler escapeKeyHandler = null;
            EventHandler dialgCloseResult = null;

            // This action should be invoked upon exiting a dialog
            Action cleanUpHandlers = null;

            // This action cleans-up all handlers added below and
            // should be invoked upon exiting the dialog
            cleanUpHandlers = () =>
            {
                KeyDown -= escapeKeyHandler;
                DialogCloseResultEvent -= dialgCloseResult;
            };

            // Handle keyboard events such as user presses enter or escape
            escapeKeyHandler = (sender, e) =>
            {
                var kay = e.Key;

                ////System.Console.WriteLine("Dialog Keyboard Handler: " + e.SystemKey + " ALT:" + Keyboard.Modifiers);

                if (e.Key == Key.Escape ||
                   (Keyboard.Modifiers == ModifierKeys.Alt && e.SystemKey == Key.F4))
                {
                    if (DialogCanCloseViaChrome == true)
                    {
                        cleanUpHandlers();

                        // Escape is same indication as Cancel
                        tcs.TrySetResult(MsgBoxResult.Cancel);
                    }
                }
                else if (e.Key == Key.Enter)
                {
                    if (DialogCanCloseViaChrome == true)
                    {
                        cleanUpHandlers();

                        // Enter key is same like clicking a button that has focus
                        // at the time (if there was any)
                        tcs.TrySetResult(GetResult());
                    }
                }
            };

            // Handle messagebox keyboard event (user clicked button in message box)
            dialgCloseResult = (sender, e) =>
            {
                cleanUpHandlers();

                tcs.TrySetResult(GetResult());
            };

            // Add this event handlers to exit dialog when one of these events occurs
            KeyDown += escapeKeyHandler;
            DialogCloseResultEvent += dialgCloseResult;

            return tcs.Task;
        }

        /// <summary>
        /// Try tp find the <seealso cref="MsgBoxResult"/> in the attached
        /// viewmodel if there is any of the expected type.
        /// </summary>
        /// <returns></returns>
        private MsgBoxResult GetResult()
        {
            var viewmodel = DataContext as IMsgBoxViewModel<MsgBoxResult>;

            if (viewmodel != null)
                return viewmodel.Result;

            return MsgBoxResult.None;
        }

        /// <summary>
        /// Method executes when the message box dialog is loaded and visible.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MsgBoxDialog_Loaded(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                bool bForceFocus = true;
                var vm = this.DataContext as IMsgBoxViewModel<MsgBoxResult>;

                if (vm != null)
                {
                    // Lets set a focus only if there is no default button, otherwise
                    // the button will be focused via binding and behaviour in xaml...
                    // But the focus should be gotten for sure since users can otherwise
                    // tab or cursor navigate the focus outside of the content dialog :-(
                    if (vm.DefaultCloseResult != MsgBoxResult.NoDefaultButton &&
                        vm.DefaultCloseResult != MsgBoxResult.None)
                    {
                        bForceFocus = false;
                    }
                }

                if (bForceFocus == true)
                {

                    this.Focus();

                    if (this.PART_Msg_Content != null)
                        this.PART_Msg_Content.Focus();
                }

            }));
        }
        #endregion methods
    }
}
