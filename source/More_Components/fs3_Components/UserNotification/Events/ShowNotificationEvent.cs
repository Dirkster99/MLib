namespace UserNotification.Events
{
  using System;
  using System.Windows;
  using System.Windows.Controls.Primitives;
  using System.Windows.Media.Imaging;

  /// <summary>
  /// Event handler delegation method to be used when handling <seealso cref="ShowNotificationEvent"/> events.
  /// </summary>
  /// <param name="sender"></param>
  /// <param name="e"></param>
  public delegate void ShowNotificationEventHandler(object sender, ShowNotificationEvent e);

  /// <summary>
  /// This class is used to message the fact that the sub-system would like to show another notification
  /// to the user.
  /// 
  /// Expectation: The connected view is processing the event and shows a (pop-up) message to the user.
  /// </summary>
  public class ShowNotificationEvent : EventArgs
  {
    #region constructor
    /// <summary>
    ///    Initializes a new instance of the ShowNotificationEvent class.
    /// </summary>
    /// <param name="imageIcon"></param>
    /// <param name="message"></param>
    /// <param name="title"></param>
    public ShowNotificationEvent(string title,
                                 string message,
                                 BitmapImage imageIcon)
    {
      this.Title = title;
      this.Message = message;
      this.ImageIcon = imageIcon;
    }
    #endregion constructor

    #region properties
    /// <summary>
    /// Get the title string of notification.
    /// </summary>
    public string Title    { get; protected set; }
    
    /// <summary>
    /// Get message of notification.
    /// </summary>
    public string Message  { get; protected set; }
    
    /// <summary>
    /// Get url string to an image resource that represents this type of notification.
    /// </summary>
    public BitmapImage ImageIcon { get; protected set; }
    #endregion properties
  }
}
