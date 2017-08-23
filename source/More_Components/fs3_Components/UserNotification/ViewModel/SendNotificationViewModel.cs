namespace UserNotification.ViewModel
{
  using System.Windows.Media.Imaging;
  using UserNotification.Events;
  using UserNotification.Interfaces;

  /// <summary>
  /// Implements the viewmodel events and methods to send a notification to an attached view.
  /// </summary>
  public class SendNotificationViewModel : Base.ViewModelBase, INotifyableViewModel
  {

    #region events
    /// <summary>
    /// Standard notifiaction event of <seealso cref="INotifyableViewModel"/>
    /// </summary>
    public event UserNotification.Events.ShowNotificationEventHandler ShowNotificationMessage;
    #endregion events

    #region methods
    /// <summary>
    /// Sends a notification event to a view which will display a corresponding message, if any
    /// view has subscribed to the event in this object.
    /// </summary>
    /// <param name="title"></param>
    /// <param name="message"></param>
    /// <param name="imageIcon"></param>
    /// <returns>true if notifiaction was successfully send (view is attached to event)
    /// or , otherwise, false</returns>
    public bool ShowNotification(string title, string message, BitmapImage imageIcon = null)
    {
      // Invoke another notification event to tell the view: Lets display another notification.
      if (this.ShowNotificationMessage != null)
      {
        this.ShowNotificationMessage(this, new ShowNotificationEvent
         (
          title,
          message,
          imageIcon
         ));

        return true;
      }

      return false;
    }
    #endregion methods
  }
}
