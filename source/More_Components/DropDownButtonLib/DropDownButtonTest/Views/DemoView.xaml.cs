namespace DropDownButtonTest.Views
{
  using System.Windows;
  using System.Windows.Controls;

  /// <summary>
  /// Interaction logic for DemoView.xaml
  /// </summary>
  public partial class DemoView : UserControl
  {
    public DemoView()
    {
      this.InitializeComponent();
    }

    private void DropDownButton_Click(object sender, System.Windows.RoutedEventArgs e)
    {
    }

    private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
    {
      // this._dropDownButton.IsOpen = false;
    }
  }
}
