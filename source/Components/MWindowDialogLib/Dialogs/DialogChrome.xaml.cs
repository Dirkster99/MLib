namespace MWindowDialogLib.Dialogs
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for DialogChromexaml.xaml
    /// </summary>
    public partial class DialogChrome : UserControl
    {
        public DialogChrome()
        {
            InitializeComponent();
        }

        public object ChromeContent
        {
            get { return (object)GetValue(ChromeContentProperty); }
            set { SetValue(ChromeContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ChromeContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ChromeContentProperty =
            DependencyProperty.Register("ChromeContent", typeof(object), typeof(DialogChrome), new PropertyMetadata(null));


    }
}
