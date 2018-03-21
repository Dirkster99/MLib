namespace MWindowDialogLib.Dialogs
{
    using MWindowInterfacesLib;
    using MWindowInterfacesLib.Interfaces;
    using System;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media.Animation;

    /// <summary>
    /// The base class for dialogs.
    ///
    /// You probably don't want to use this class, if you want to add arbitrary content
    /// to your dialog, use the CustomDialog class.
    /// </summary>
    public partial class DialogFrame : UserControl, IBaseMetroDialogFrame
    {
        #region constructors
        /// <summary>
        /// Static constructor
        /// </summary>
        static DialogFrame()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DialogFrame)
                , new FrameworkPropertyMetadata(typeof(DialogFrame)));
        }

        /// <summary>
        /// Initializes a new DialogFrame object.
        /// </summary>
        /// <param name="owningWindow">The window that is the parent of the dialog.</param>
        /// <param name="settings">The settings for the message dialog.</param>
        protected DialogFrame(IMetroWindow owningWindow
                            , IMetroDialogFrameSettings settings)
            : this()
        {
            DialogSettings = settings ?? new MetroDialogFrameSettings();

            OwningWindow = owningWindow;
        }

        /// <summary>
        /// Constructs a new BaseMetroDialogFrame.
        /// </summary>
        public DialogFrame()
        {
            DialogSettings = new MWindowInterfacesLib.MetroDialogFrameSettings();

            ////InitializeComponent();
            Initialize();
        }
        #endregion constructors

        #region events
        /// <summary>
        /// Gets/sets an event handler that is invoked when the <seealso cref="IMetroWindow"/>
        /// has chnaged its size. The event coupling is necessary to have the content dialog
        /// change its size accordingly.
        /// </summary>
        public SizeChangedEventHandler SizeChangedHandler { get; set; }

        /// <summary>
        /// This event invoked is to signal subscribers (e.g. viewmodel)
        /// when the dialog should be closed.
        /// </summary>
        public event EventHandler DialogCloseResultEvent;
        #endregion events

        #region properties

        #region DialogCloseAnimation
        /// <summary>
        /// Gets/sets the storyboard that is shown when
        /// Dialog hide is configured to be animated.
        /// </summary>
        public Storyboard DialogCloseAnimation
        {
            get { return (Storyboard)GetValue(DialogCloseAnimationProperty); }
            set { SetValue(DialogCloseAnimationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DialogClose.  This enables animation, styling, binding, etc...
        private static readonly DependencyProperty DialogCloseAnimationProperty =
            DependencyProperty.Register("DialogCloseAnimation"
                                       , typeof(Storyboard)
                                       , typeof(DialogFrame), new PropertyMetadata(null));
        #endregion DialogCloseAnimation

        #region DialogCanCloseViaChrome
        /// <summary>
        /// Gets/Sets the DialogCanCloseViaChrome dependency property.
        ///
        /// Bind this property between view and viemodel to have the viewmodel tell
        /// the view whether it is OK to close without picking a choice (eg. yes) or not.
        /// </summary>
        public bool DialogCanCloseViaChrome
        {
            get { return (bool)GetValue(DialogCanCloseViaChromeProperty); }
            set { SetValue(DialogCanCloseViaChromeProperty, value); }
        }

        /// <summary>
        /// Backing store field of the DialogCanCloseViaChrome dependency property.
        ///
        /// Bind this property between view and viemodel to have the viewmodel tell
        /// the view whether it is OK to close without picking a choice (eg. yes) or not.
        /// </summary>
        public static readonly DependencyProperty DialogCanCloseViaChromeProperty =
            DependencyProperty.Register("DialogCanCloseViaChrome"
                , typeof(bool)
                , typeof(DialogFrame)
                , new PropertyMetadata(true));
        #endregion DialogCanCloseViaChrome

        #region DialogCloseResult
        /// <summary>
        /// Bind this property between view and viemodel to have the viewmodel tell
        /// the view that it is time to disappear (eg. user has clicked a choice button).
        /// </summary>
        public bool? DialogCloseResult
        {
            get { return (bool?)GetValue(DialogCloseResultProperty); }
            set { SetValue(DialogCloseResultProperty, value); }
        }

        /// <summary>
        /// Backing store field of the DialogCloseResult dependency property.
        ///
        /// Bind this property between view and viemodel to have the viewmodel tell
        /// the view that it is time to disappear (eg. user has clicked a choice button).
        /// </summary>
        public static readonly DependencyProperty DialogCloseResultProperty =
            DependencyProperty.Register("DialogCloseResult"
                , typeof(bool?)
                , typeof(DialogFrame)
                , new PropertyMetadata(null, OnChangedCallback));

        private static void OnChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is bool?)
            {
                bool? bvalue = (bool?)e.NewValue;

                if (bvalue == null)
                    return;

                if (bvalue == true) // Dialog can be closed now
                {
                    var dialog = d as DialogFrame;

                    // Generate Event to close dialog in
                    // MessageBoxServiceImpl.Show Method
                    if (dialog.DialogCloseResultEvent != null)
                    {
                        dialog.Dispatcher.BeginInvoke(new Action(() => dialog.DialogCloseResultEvent(dialog, new EventArgs())));
                    }
                }
            }
        }
        #endregion DialogCloseResult

        #region Title
        /// <summary>
        /// Gets/sets the dialog's title dependency property.
        /// </summary>
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        /// <summary>
        /// Gets/sets the dialog's title dependency property.
        /// </summary>
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(DialogFrame), new PropertyMetadata(default(string)));
        #endregion Title

        /// <summary>
        /// Gets the standard dialog settings for this dialog.
        /// </summary>
        public IMetroDialogFrameSettings DialogSettings { get; private set; }

        /// <summary>
        /// Gets the window that owns the current Dialog
        /// IF AND ONLY IF the dialog is shown externally.
        /// </summary>
        protected internal Window ParentDialogWindow { get; internal set; }

        /// <summary>
        /// Gets the window that owns the current Dialog
        /// IF AND ONLY IF the dialog is shown inside of a window.
        /// </summary>
        public IMetroWindow OwningWindow { get; internal set; }
        #endregion properties

        #region methods
        /// <summary>
        /// Waits for the dialog to become ready for interaction.
        /// </summary>
        /// <returns>A task that represents the operation and it's status.</returns>
        public Task WaitForLoadAsync()
        {
            Dispatcher.VerifyAccess();

            if (this.IsLoaded) return new Task(() => { });

            if (!DialogSettings.AnimateShow)
                this.Opacity = 1.0; //skip the animation

            TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();

            RoutedEventHandler handler = null;
            handler = (sender, args) =>
            {
                this.Loaded -= handler;

                this.Focus();

                tcs.TrySetResult(null);
            };

            this.Loaded += handler;

            return tcs.Task;
        }

        /// <summary>
        /// Waits until this dialog gets unloaded.
        /// </summary>
        /// <returns></returns>
        public Task WaitUntilUnloadedAsync()
        {
            TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();

            Unloaded += (s, e) =>
            {
                tcs.TrySetResult(null);
            };

            return tcs.Task;
        }

        /// <summary>
        /// Waits until a dialog is closed.
        /// </summary>
        /// <returns></returns>
        public Task _WaitForCloseAsync()
        {
            TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();

            if (DialogSettings.AnimateHide)
            {
                Storyboard closingStoryboard = this.DialogCloseAnimation;
                ////Storyboard closingStoryboard = this.Resources["DialogCloseStoryboard"] as Storyboard;
                ////
                ////if (closingStoryboard == null)
                ////    closingStoryboard = Application.Current.Resources["DialogCloseStoryboard"] as Storyboard;

                if (closingStoryboard == null)
                    throw new InvalidOperationException("Unable to find the dialog closing storyboard. Did you forget to set the DialogCloseAnimation dependency property?");

                EventHandler handler = null;
                handler = (sender, args) =>
                {
                    closingStoryboard.Completed -= handler;

                    tcs.TrySetResult(null);
                };

                closingStoryboard = closingStoryboard.Clone();

                closingStoryboard.Completed += handler;

                closingStoryboard.Begin(this);
            }
            else
            {
                this.Opacity = 0.0;
                tcs.TrySetResult(null); //skip the animation
            }

            return tcs.Task;
        }

        /// <summary>
        /// Set the ZIndex value for this <seealso cref="DialogFrame"/>.
        /// This method can make sure that a given dialog is visible when more
        /// than one dialog is open.
        /// </summary>
        /// <param name="newPanelIndex"></param>
        public void SetZIndex(int newPanelIndex)
        {
            this.SetValue(Panel.ZIndexProperty, newPanelIndex);
        }

        /// <summary>
        /// This is called in the loaded event.
        /// </summary>
        protected virtual void OnLoaded()
        {
            // nothing here
        }

        /// <summary>
        /// Method can be customized to handle dialog specific code that should
        /// run when the dialog is displayed.
        /// </summary>
        public virtual void OnShown() { }

        /// <summary>
        /// Method can be customized to handle dialog specific code that should
        /// run when the dialog is removed from the display.
        /// </summary>
        public virtual void OnClose()
        {
            // this is only set when a dialog is shown (externally) in it's OWN window.
            if (ParentDialogWindow != null)
                ParentDialogWindow.Close();
        }

        /// <summary>
        /// A last chance virtual method
        /// for stopping an external dialog from closing.
        /// </summary>
        /// <returns></returns>
        internal protected virtual bool OnRequestClose()
        {
            return true; //allow the dialog to close.
        }

        private void Initialize()
        {
            this.Loaded += (sender, args) =>
            {
                OnLoaded();
                HandleTheme();
            };

            this.Unloaded += BaseMetroDialogFrame_Unloaded;
        }

        private void BaseMetroDialogFrame_Unloaded(object sender, RoutedEventArgs e)
        {
            this.Unloaded -= BaseMetroDialogFrame_Unloaded;
        }

        private void HandleTheme()
        {
            if (this.ParentDialogWindow != null)
            {
                this.ParentDialogWindow.SetValue(BackgroundProperty, this.Background);
            }
        }
        #endregion methods
    }
}
