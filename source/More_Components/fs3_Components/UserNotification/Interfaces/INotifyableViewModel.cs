namespace UserNotification.Interfaces
{
  /// <summary>
  /// This interface can be used to connect viewmodel with view
  /// when showing notifications that can pop-up over a window
  /// or over all currently visible windows (IsTopMost = true in notification viewmodel)
  /// </summary>
  public interface INotifyableViewModel
  {
    /// <summary>
    /// Expose an event that is triggered when the viewmodel tells its view:
    /// Here is another notification message please show it to the user.
    /// </summary>
    event UserNotification.Events.ShowNotificationEventHandler ShowNotificationMessage;
  }
}
