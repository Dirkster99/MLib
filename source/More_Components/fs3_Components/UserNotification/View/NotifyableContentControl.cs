namespace UserNotification.View
{
  using System;
  using System.ComponentModel;
  using System.Windows;
  using System.Windows.Controls;
  using System.Windows.Media;
  using UserNotification.Interfaces;
  using UserNotification.ViewModel;

  /// <summary>
  /// This class implements a control that can be used to extend any standard control
  /// (eg: ListBox, TreeView) with a notification functionality. A notification is a
  /// small pop-up window that gives users feedback as to why somethings not working
  /// or what a user might be able to do to use a function correctly.
  ///
  /// This control is based on <seealso cref="ContentControl"/>. Application developers
  /// can use this fact to contain any other standard control (say TextBox) inside this
  /// control. The Notification dependency property can then be used to show notifications
  /// in the vicinity of the contained control.
  ///
  /// This allows application developers to extend any control (eg: ListBox, TreeView)
  /// with a notification facility since application developers can invoke the
  /// ShowNotification event in the INotifyableViewModel interface  to show a
  /// short pop-up message to the user. The pop-up message is shown in the
  /// vicinity of the content control that contains the real control (eg: ListBox)
  /// to which this notfication is related to.
  /// </summary>
  public class NotifyableContentControl : ContentControl
  {
    #region fields
    /// <summary>
    /// Gets/sets the notification dependency property that can be used to show
    /// user notification as pop-up object over the normal UI.
    /// </summary>
    public static readonly DependencyProperty NotificationProperty =
        DependencyProperty.Register("Notification",
                                     typeof(INotifyableViewModel),
                                     typeof(NotifyableContentControl),
                                     new FrameworkPropertyMetadata(null,
                                     new PropertyChangedCallback(OnNotificationChangedCallback)));

    private SimpleNotificationWindow mTip;
    private bool mDestroyNotificationOnFocusChange = false;
    private object mlockObject = new object();

    /// <summary>
    /// Is a viewmodel which keeps the command binding and event triggering to base the notifications on
    /// Using this setup tests the real world scenario where notifications are triggered through
    /// complex conditions in the viewmodel. These worklflows are not necessarily triggered by a button in a view.
    /// </summary>
    private INotifyableViewModel mViewModel;
    #endregion fields
 
    #region constructors
    /// <summary>
    /// Class constructor
    /// </summary>
    public NotifyableContentControl()
    {
    }
    #endregion constructors

    #region properties
    /// <summary>
    /// Gets/sets the notification dependency property that can be used to show
    /// user notification as pop-up object over the normal UI.
    /// </summary>
    [Category("Notification")]
    public INotifyableViewModel Notification
    {
      get { return (INotifyableViewModel)GetValue(NotificationProperty); }
      set { SetValue(NotificationProperty, value); }
    }
    #endregion properties

    #region methods
    private static void OnNotificationChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
 	    var obj = d as NotifyableContentControl;

      if (obj != null)
      {
        obj.OnNofitivationChange(e.OldValue as INotifyableViewModel,
                                 e.NewValue as INotifyableViewModel);
      }
    }

    /// <summary>
    /// Method is invoked when the nofitivation dependency property is changed.
    /// This requires changing event hook-up on attached viewmodel to enable
    /// notification event conversion from viewmodel into view.
    /// </summary>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    private void OnNofitivationChange(INotifyableViewModel oldValue, INotifyableViewModel newValue)
    {
      if (this.mViewModel != null)
      {
        // Remove old link for showing notification pop-up message event
        this.mViewModel.ShowNotificationMessage -= ViewModel_ShowNotificationMessage;
      }

      this.mViewModel = newValue;

      if (this.mViewModel != null)
      {
        // Establish new link to show notification pop-up message event
        this.mViewModel.ShowNotificationMessage += ViewModel_ShowNotificationMessage;
      }
    }

    /// <summary>
    /// Method is invoked when the viewmodel tells the view: Show another notification to the user.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ViewModel_ShowNotificationMessage(object sender, UserNotification.Events.ShowNotificationEvent e)
    {
      this.ShowNotification(e.Title, e.Message, true);
    }

    /// <summary>
    /// Shows a notification warning to the user to clarify the current application behavior.
    /// </summary>
    /// <param name="title"></param>
    /// <param name="message"></param>
    /// <param name="notifiedFromViewmodel"></param>
    private void ShowNotification(string title, string message, bool notifiedFromViewmodel)
    {
      lock (this.mlockObject)
      {
        this.mDestroyNotificationOnFocusChange = notifiedFromViewmodel;

        if (this.mTip == null)
        {
          // Construct the notification window object to be shown to the user
          this.mTip = new SimpleNotificationWindow();

          var ownerWindow = this.GetDpObjectFromVisualTree(this, typeof(Window)) as Window;
          this.mTip.Owner = ownerWindow;
        }

        NotificationViewModel vm = new NotificationViewModel()
        {
          Title = title,
          Message = message,
          IsTopmost = false
        };

        this.mTip.ShowNotification(vm, this);
      }
    }

    /// <summary>
    /// Walk visual tree to find the first DependencyObject of a specific type.
    /// (This method works for finding a ScrollViewer within a TreeView).
    /// </summary>
    private DependencyObject GetDpObjectFromVisualTree(DependencyObject startObject, Type type)
    {
      // Walk the visual tree to get the parent(ItemsControl)
      // of this control
      DependencyObject parent = startObject;
      while (parent != null)
      {
        if (type.IsInstanceOfType(parent))
          break;
        else
          parent = VisualTreeHelper.GetParent(parent);
      }

      return parent;
    }
    #endregion methods
  }
}
