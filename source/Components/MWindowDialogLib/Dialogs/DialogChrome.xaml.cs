namespace MWindowDialogLib.Dialogs
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for DialogChromexaml.xaml
    /// </summary>
    public partial class DialogChrome : UserControl
    {
        /// <summary>
        /// Class constructor.
        /// </summary>
        public DialogChrome()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets/sets the ChromeContent dependency property.
        /// </summary>
        public object ChromeContent
        {
            get { return (object)GetValue(ChromeContentProperty); }
            set { SetValue(ChromeContentProperty, value); }
        }

        /// <summary>
        /// Using a DependencyProperty as the backing store for ChromeContent.
        /// </summary>
        public static readonly DependencyProperty ChromeContentProperty =
            DependencyProperty.Register("ChromeContent", typeof(object), typeof(DialogChrome), new PropertyMetadata(null));
    }
}
