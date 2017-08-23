namespace UserNotification.View
{
  using System.Windows;
  using System.Windows.Controls;
  using UserNotification.ViewModel;

  /// <summary>
  /// This class is used to view the content of a notification window.
  /// 
  /// Based on: http://www.codeproject.com/Articles/499241/Growl-Alike-WPF-Notifications
  /// </summary>
  public partial class NotificationWindow : Window
  {
    #region constructor
    /// <summary>
    /// Class constructor
    /// </summary>
    public NotificationWindow()
    {
      this.CanClose = false;

      this.InitializeComponent();
    }
    #endregion constructor

    #region properties
    /// <summary>
    /// Determine whether window can be closed or whether closing
    /// via standard close functions should be cancelled in Closing override method.
    /// </summary>
    protected bool CanClose { get; set; }
    #endregion properties

    #region methods
    /// <summary>
    /// Is called by parent window to tell the window to close for good
    /// since parent view is also closing.
    /// </summary>
    public void CloseInvokedByParent()
    {
      this.CanClose = true;
      this.Close();
    }

    /// <summary>
    /// Overrides closing window behaviour to avoid that a user can close a notification
    /// via ALT+F4 or Esc key. Overiding this is required since a window that is closed
    /// cannot be shown via Show again - one would have to re-create the entire window
    /// with new - which is not part of the current concept.
    /// 
    /// Current concept is: use Show/hide durring livetime of hosting window and close
    /// when hosting window is closed.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
    {
      // Cancel closing notification window (since hidding should be used) until parent unloads
      // which will be notified via close through CloseInvokedByParent method
      if (this.CanClose == false)
      {
        e.Cancel = true;
        return;
      }

      base.OnClosing(e);
    }

    /// <summary>
    /// Show a new notification to the user.
    /// </summary>
    /// <param name="notification"></param>
    public void ShowNotification(NotificationViewModel notification)
    {
      ////Visually Debug this window with the color
      ////this.Background = new SolidColorBrush(Color.FromArgb(255,255,255,255));

      // Bind Window parameters (Width, Height, and TopMost)
      this.DataContext = notification;

      // Set content on control template
      this.NotificationsControl.Content = notification;

      if (!this.IsActive)
        this.Show();
    }

    /// <summary>
    /// Hide the notification window.
    /// </summary>
    public void HideNotification()
    {
      this.Hide();
    }

    /// <summary>
    /// This method is invoked via XAML code to tell the code behind
    /// that a notification is about to dissapper.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void NotificationWindowSizeChanged(object sender, SizeChangedEventArgs e)
    {
      if (e.NewSize.Height != 0.0)
        return;

      var element = sender as Grid;

      this.HideNotification();
    }
    #endregion methods
  }
}
