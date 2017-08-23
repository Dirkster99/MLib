namespace UserNotification.View
{
  using System;
  using System.Windows;
  using UserNotification.Interfaces;
  using UserNotification.View;
  using UserNotification.ViewModel;

  /// <summary>
  /// Interaction logic for NotifyableWindow.xaml
  /// 
  /// Based on: 
  /// </summary>
  public partial class NotifyableWindow : Window
  {
    #region fields
    private const double TopOffset = 21;
    private const double LeftOffset = 0;

    // Is a window derived visual control class that shows the actual notification view
    readonly NotificationWindow notificationWindow = null;

    // Is a viewmodel which keeps the command binding and event triggering to base the notifications on
    // Using this setup tests the real world scenario where notifications are triggered through
    // complex conditions in the viewmodel. These worklflows are not necessarily triggered by a button in a view.
    private INotifyableViewModel mViewModel;

    private bool mAttachedNotificationEvent;
    #endregion fields

    #region constructors
    /// <summary>
    /// Class Constructor
    /// </summary>
    public NotifyableWindow()
            : base()
    {
      this.mAttachedNotificationEvent = false;
      this.notificationWindow = new NotificationWindow();
      this.DataContextChanged += MsgBoxView_DataContextChanged;
      this.Loaded += OnMsgBoxView_Loaded;
      this.Unloaded += NotifyableWindow_Unloaded;
    }
    #endregion constructors

    #region methods
    /// <summary>
    /// Method is invoked when the datacontext is changed.
    /// This requires changing event hook-up on attached viewmodel to enable
    /// notification event conversion from viewmodel into view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void MsgBoxView_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
    {
      if (this.mAttachedNotificationEvent == true)
        this.mViewModel.ShowNotificationMessage -= ViewModel_ShowNotificationMessage;

      this.mViewModel = e.NewValue as INotifyableViewModel;

      if (this.mViewModel != null)
      {
        this.mViewModel.ShowNotificationMessage += ViewModel_ShowNotificationMessage;
        this.mAttachedNotificationEvent = true;
      }
      else
        this.mAttachedNotificationEvent = false;
    }

    /// <summary>
    /// Method is invoked when the viewmodel tells the view: Show another notification to the user.
    /// (override this method if you want to use a different viewmodel and custom (re-styled) notification view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ViewModel_ShowNotificationMessage(object sender, UserNotification.Events.ShowNotificationEvent e)
    {
      NotificationViewModel vm = new UserNotification.ViewModel.NotificationViewModel
      {
        Title = e.Title,
        ImageIcon = e.ImageIcon,
        Message = e.Message
      };

      this.SetNextNotificationPosition(vm);
      this.notificationWindow.ShowNotification(vm);
    }

    /// <summary>
    /// Method is invoked to re-position the notification view such that it is close to the owning view.
    /// </summary>
    protected void SetNextNotificationPosition(NotificationViewModel vm)
    {
      double height = 125, width = 400;

      if (vm != null)
      {
        height = vm.ViewHeight;
        width = vm.ViewWidth;
      }

      // Attempt to position notification below close window (x) button
      this.notificationWindow.Top = this.Top + NotifyableWindow.TopOffset;
      this.notificationWindow.Left = this.Left + NotifyableWindow.LeftOffset +
                                     Math.Abs(this.Width - width);

      // Re-position notifiaction window if it appears to be outside of the visual screen
      // This works on primary screen [1 screen scenario] not sure if it works on 2 or more screens (?)
      if (this.notificationWindow.Top + height > SystemParameters.VirtualScreenHeight)
        this.notificationWindow.Top = SystemParameters.VirtualScreenHeight - height;

      if (this.notificationWindow.Left + width > SystemParameters.VirtualScreenWidth)
        this.notificationWindow.Left = SystemParameters.VirtualScreenWidth - width;

      if (this.notificationWindow.Top < SystemParameters.VirtualScreenTop)
        this.notificationWindow.Top = SystemParameters.VirtualScreenTop + 10;

      if (this.notificationWindow.Left < SystemParameters.VirtualScreenLeft)
        this.notificationWindow.Left = SystemParameters.VirtualScreenLeft + 10;
    }

    /// <summary>
    /// Method is invoked when the window is Loaded to set the notification owner property.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void OnMsgBoxView_Loaded(object sender, RoutedEventArgs e)
    {
      //this will make minimize restore of notifications too
      this.notificationWindow.Owner = this;
    }

    /// <summary>
    /// Free notification resources when parent window is being closed.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void NotifyableWindow_Unloaded(object sender, EventArgs e)
    {
      // Close notification window for good
      this.notificationWindow.CloseInvokedByParent();

      // Free event hook-up bewteen view and viewmodel
      if (this.mAttachedNotificationEvent == true)
        this.mViewModel.ShowNotificationMessage -= ViewModel_ShowNotificationMessage;
    }
    #endregion methods
  }
}
