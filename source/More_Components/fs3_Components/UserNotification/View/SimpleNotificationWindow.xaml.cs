namespace UserNotification.View
{
  using System;
  using System.Windows;
  using System.Windows.Controls;
  using System.Windows.Media;
  using UserNotification.ViewModel;

  /// <summary>
  /// This class is used to view the content of a notification window.
  /// 
  /// Based on: http://www.codeproject.com/Articles/499241/Growl-Alike-WPF-Notifications
  /// </summary>
  public partial class SimpleNotificationWindow : Window
  {
    #region constructor
    /// <summary>
    /// Class constructor
    /// </summary>
    public SimpleNotificationWindow()
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
    /// Method is invoked to re-position the notification view such that it is close to the owning view.
    /// </summary>
    protected void SetNextNotificationPosition(Visual view)
    {
      double height = 125, width = 400;
      const double topOffset = 21;
      const double leftOffset = 0;

      Point position = view.PointToScreen(new Point(0, 0));

      // Attempt to position notification below visual given to this function
      this.Top = position.Y + topOffset;
      this.Left = position.X + leftOffset;

      // Re-position notifiaction window if it appears to be outside of the visual screen
      // This works on primary screen [1 screen scenario] not sure if it works on 2 or more screens (?)
      //
      // Position above textbox instead of on lower corner of screen
      if (this.Top + height > SystemParameters.VirtualScreenHeight)
        this.Top = Math.Abs(position.Y - height);

      if (this.Left + width > SystemParameters.VirtualScreenWidth)
        this.Left = SystemParameters.VirtualScreenWidth - width;

      // Case should never occur since default position is below textblock
      if (this.Top < SystemParameters.VirtualScreenTop)
        this.Top = position.Y + topOffset;

      if (this.Left < SystemParameters.VirtualScreenLeft)
        this.Left = SystemParameters.VirtualScreenLeft + 10;
    }

    /// <summary>
    /// Show a new notification to the user.
    /// </summary>
    /// <param name="notification"></param>
    public void ShowNotification(NotificationViewModel notification, Visual view)
    {
      ////Visually Debug this window with the color
      ////this.Background = new SolidColorBrush(Color.FromArgb(255,255,255,255));

      // Bind Window parameters (Width, Height, and TopMost)
      this.DataContext = notification;

      // Set content on control template
      this.NotificationsControl.Content = notification;
      this.SetNextNotificationPosition(view);

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
