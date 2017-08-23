namespace WatermarkControlsDemo
{
    using System.Windows;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Class Constructor
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();

            this.DataContext = new ViewModels.AppViewModel();
        }
    }
}
