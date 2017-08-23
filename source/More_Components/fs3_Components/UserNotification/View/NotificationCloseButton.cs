namespace UserNotification.View
{
  using System.Windows;
  using System.Windows.Controls;

  /// <summary>
  /// Implements a look-less control that should function and look like a window (X) close button.
  /// </summary>
  public class NotificationCloseButton : Button
  {
    static NotificationCloseButton()
    {
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(NotificationCloseButton),
                                  new FrameworkPropertyMetadata(typeof(NotificationCloseButton)));
    }
  }
}
