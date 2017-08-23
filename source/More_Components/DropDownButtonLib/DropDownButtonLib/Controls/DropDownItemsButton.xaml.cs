namespace DropDownButtonLib.Controls
{
  using System;
  using System.Collections;
  using System.ComponentModel;
  using System.Windows;
  using System.Windows.Controls;
  using System.Windows.Controls.Primitives;
  using System.Windows.Input;
  using DropDownButtonLib.Utilities;

  /// <summary>
  /// Implements a look-less WPF DropDownItemsButton control.
  /// </summary>
  [TemplatePart(Name = PART_DropDownButton, Type = typeof(ToggleButton))]
  [TemplatePart(Name = PART_ItemsControl, Type = typeof(ItemsControl))]
  [TemplatePart(Name = PART_Popup, Type = typeof(Popup))]
  public class DropDownItemsButton : ContentControl, ICommandSource
  {
    #region fields
    /// <summary>
    /// Const string for required template element of this control.
    /// </summary>
    public const string PART_DropDownButton = "PART_DropDownButton";

    /// <summary>
    /// Const string for required template element of this control.
    /// </summary>
    public const string PART_ItemsControl = "PART_ItemsControl";

    /// <summary>
    /// Const string for required template element of this control.
    /// </summary>
    public const string PART_Popup = "PART_Popup";

    /// <summary>
    /// Backing store of IsOpen dependency property.
    /// </summary>
    public static readonly DependencyProperty IsOpenProperty =
           DependencyProperty.Register("IsOpen",
           typeof(bool), typeof(DropDownItemsButton),
           new UIPropertyMetadata(false, DropDownItemsButton.OnIsOpenChanged));

    /// <summary>
    /// Backing store of Command dependency property.
    /// </summary>
    public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(DropDownItemsButton), new PropertyMetadata((ICommand)null, OnCommandChanged));

    /// <summary>
    /// Backing store of Command Parameter dependency property.
    /// </summary>
    public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register("CommandParameter", typeof(object), typeof(DropDownItemsButton), new PropertyMetadata(null));

    /// <summary>
    /// Backing store of Command Target dependency property.
    /// </summary>
    public static readonly DependencyProperty CommandTargetProperty = DependencyProperty.Register("CommandTarget", typeof(IInputElement), typeof(DropDownItemsButton), new PropertyMetadata(null));

    #region DropDown ItemsControl dependencyproperties
    /// <summary>
    /// Backing store of dependency property implmentation
    /// - see corresponding property implementation for more details.
    /// </summary>
    public static readonly DependencyProperty ItemTemplateProperty =
        DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(DropDownItemsButton), new PropertyMetadata(null));

    /// <summary>
    /// Backing store of dependency property implmentation
    /// - see corresponding property implementation for more details.
    /// </summary>
    public static readonly DependencyProperty ItemsSourceProperty =
        DependencyProperty.Register("ItemsSource",
                                    typeof(IEnumerable),
                                    typeof(DropDownItemsButton),
                                    new PropertyMetadata(null));

    /// <summary>
    /// Backing store of dependency property implmentation
    /// - see corresponding property implementation for more details.
    /// </summary>
    public static readonly DependencyProperty ItemContainerStyleProperty =
        DependencyProperty.Register("ItemContainerStyle", typeof(Style), typeof(DropDownItemsButton), new PropertyMetadata(null));

    /// <summary>
    /// Backing store of dependency property implmentation
    /// - see corresponding property implementation for more details.
    /// </summary>
    public static readonly DependencyProperty ItemContainerStyleSelectorProperty =
        DependencyProperty.Register("ItemContainerStyleSelector",
                                    typeof(StyleSelector),
                                    typeof(DropDownItemsButton),
                                     new PropertyMetadata(null));

    /// <summary>
    /// Backing store of dependency property implmentation
    /// - see corresponding property implementation for more details.
    /// </summary>
    public static readonly DependencyProperty ItemsPanelProperty =
        DependencyProperty.Register("ItemsPanel",
                                   typeof(ItemsPanelTemplate),
                                   typeof(DropDownItemsButton),
                                   new PropertyMetadata(null));

    /// <summary>
    /// Backing store of dependency property implmentation
    /// - see corresponding property implementation for more details.
    /// </summary>
    public static readonly DependencyProperty TemplateOfItemControlProperty =
        DependencyProperty.Register("TemplateOfItemControl",
                                    typeof(ControlTemplate),
                                    typeof(DropDownItemsButton),
                                    new PropertyMetadata(null));
    #endregion DropDown ItemsControl dependencyproperties

    #region DropDown dependencyproperties
    /// <summary>
    /// Backing store of dependency property implmentation
    /// - see corresponding property implementation for more details.
    /// </summary>
    public static readonly DependencyProperty DropDownMaxHeightProperty =
        DependencyProperty.Register("DropDownMaxHeight", typeof(double), typeof(DropDownItemsButton), new PropertyMetadata(double.PositiveInfinity));

    /// <summary>
    /// Backing store of dependency property implmentation
    /// - see corresponding property implementation for more details.
    /// </summary>
    public static readonly DependencyProperty DropDownMaxWidthProperty =
        DependencyProperty.Register("DropDownMaxWidth", typeof(double), typeof(DropDownItemsButton), new PropertyMetadata(double.PositiveInfinity));
    #endregion DropDown dependencyproperties

    #region events
    /// <summary>
    /// Backing store of Click Event dependency property.
    /// </summary>
    public static readonly RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(DropDownItemsButton));

    /// <summary>
    /// Backing store of Opened Event dependency property.
    /// </summary>
    public static readonly RoutedEvent OpenedEvent = EventManager.RegisterRoutedEvent("Opened", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(DropDownItemsButton));

    /// <summary>
    /// Backing store of Closed Event dependency property.
    /// </summary>
    public static readonly RoutedEvent ClosedEvent = EventManager.RegisterRoutedEvent("Closed", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(DropDownItemsButton));
    #endregion events

    private System.Windows.Controls.Primitives.ButtonBase mButton;
    private ItemsControl mItemsControl;
    private Popup mPopup;

    /// <summary>
    /// Keeps a copy of the CanExecuteChanged handler so it doesn't get garbage collected.
    /// </summary>
    private EventHandler mCanExecuteChangedHandler;
    #endregion fields

    #region constructors
    /// <summary>
    /// Static class constructor
    /// </summary>
    static DropDownItemsButton()
    {
      DefaultStyleKeyProperty.OverrideMetadata(typeof(DropDownItemsButton),
                                               new FrameworkPropertyMetadata(typeof(DropDownItemsButton)));
    }

    /// <summary>
    /// Default class constructor
    /// </summary>
    public DropDownItemsButton()
    {
      Keyboard.AddKeyDownHandler(this, this.OnKeyDown);
      Mouse.AddPreviewMouseDownOutsideCapturedElementHandler(this, this.OnMouseDownOutsideCapturedElement);
    }
    #endregion constructors

    #region events
    /// <summary>
    /// Backing store of Click Event dependency property.
    /// </summary>
    public event RoutedEventHandler Click
    {
      add
      {
        this.AddHandler(ClickEvent, value);
      }

      remove
      {
        this.RemoveHandler(ClickEvent, value);
      }
    }

    /// <summary>
    /// Backing store of Opened Event dependency property.
    /// </summary>
    public event RoutedEventHandler Opened
    {
      add
      {
        this.AddHandler(OpenedEvent, value);
      }
 
      remove
      {
        this.RemoveHandler(OpenedEvent, value);
      }
    }

    /// <summary>
    /// Backing store of Closed Event dependency property.
    /// </summary>
    public event RoutedEventHandler Closed
    {
      add
      {
        this.AddHandler(ClosedEvent, value);
      }
 
      remove
      {
        this.RemoveHandler(ClosedEvent, value);
      }
    }
    #endregion events

    #region Properties
    /// <summary>
    /// Gets/sets whether the DwropDownContent is currently open or not.
    /// </summary>
    public bool IsOpen
    {
      get
      {
        return (bool)GetValue(IsOpenProperty);
      }

      set
      {
        this.SetValue(IsOpenProperty, value);
      }
    }

    #region DropDown ItemsControl dependencyproperties
    /// <summary>
    /// Gets/sets the ItemTemplate dependency property that
    /// represents the ItemTemplate property of the ItemsControl inside the
    /// Pop-up/DropDown element of the button.
    /// 
    /// See XAML for binding statement in PopUp.ItemsControl part of ControlTemplate.
    /// http://msdn.microsoft.com/en-us/library/system.windows.controls.itemscontrol.itemcontainerstyle%28v=vs.110%29.aspx
    /// </summary>
    public DataTemplate ItemTemplate
    {
      get { return (DataTemplate)this.GetValue(ItemTemplateProperty); }
      set { this.SetValue(ItemTemplateProperty, value); }
    }

    /// <summary>
    /// Gets/sets the ItemsSource dependency property that
    /// represents the ItemsSource property of the ItemsControl inside the
    /// Pop-up/DropDown element of the button.
    /// 
    /// See XAML for binding statement in PopUp.ItemsControl part of ControlTemplate.
    /// http://msdn.microsoft.com/en-us/library/system.windows.controls.itemscontrol.itemcontainerstyle%28v=vs.110%29.aspx
    /// </summary>
    public IEnumerable ItemsSource
    {
      get { return (IEnumerable)this.GetValue(ItemsSourceProperty); }
      set { this.SetValue(ItemsSourceProperty, value); }
    }

    /// <summary>
    /// Gets/sets the ItemContainerStyle dependency property that
    /// represents the ItemContainerStyle property 
    /// of the ItemsControl inside the Pop-up/DropDown element of the button.
    /// 
    /// See XAML for binding statement in PopUp.ItemsControl part of ControlTemplate.
    /// http://msdn.microsoft.com/en-us/library/system.windows.controls.itemscontrol.itemcontainerstyle%28v=vs.110%29.aspx
    /// </summary>
    public Style ItemContainerStyle
    {
      get { return (Style)this.GetValue(ItemContainerStyleProperty); }
      set { this.SetValue(ItemContainerStyleProperty, value); }
    }

    /// <summary>
    /// Gets/sets the ItemContainerStyleSelector dependency property that
    /// represents the ItemContainerStyleSelector property 
    /// of the ItemsControl inside the Pop-up/DropDown element of the button.
    /// 
    /// See XAML for binding statement in PopUp.ItemsControl part of ControlTemplate.
    /// http://msdn.microsoft.com/en-us/library/system.windows.controls.itemscontrol.itemcontainerstyle%28v=vs.110%29.aspx
    /// </summary>
    public StyleSelector ItemContainerStyleSelector
    {
      get { return (StyleSelector)this.GetValue(ItemContainerStyleSelectorProperty); }
      set { this.SetValue(ItemContainerStyleSelectorProperty, value); }
    }

    /// <summary>
    /// Gets/sets the ItemsPanel dependency property that represents the ItemsPanel property 
    /// of the ItemsControl inside the Pop-up/DropDown element of the button.
    /// 
    /// See XAML for binding statement in PopUp.ItemsControl part of ControlTemplate.
    /// http://msdn.microsoft.com/en-us/library/system.windows.controls.itemscontrol.itemcontainerstyle%28v=vs.110%29.aspx
    /// </summary>
    public ItemsPanelTemplate ItemsPanel
    {
      get { return (ItemsPanelTemplate)this.GetValue(ItemsPanelProperty); }
      set { this.SetValue(ItemsPanelProperty, value); }
    }

    /// <summary>
    /// Gets/sets the property value of the dependency property that represents the Template property 
    /// of the ItemsControl inside the Pop-up/DropDown element of the button. A default template is
    /// already setup with the default template of the <seealso cref="DropDownItemsButton"/> template
    /// but you can use this property to override it.
    /// 
    /// See XAML for binding statement in PopUp.ItemsControl part of ControlTemplate.
    /// http://msdn.microsoft.com/en-us/library/system.windows.controls.itemscontrol.itemcontainerstyle%28v=vs.110%29.aspx
    /// </summary>
    public ControlTemplate TemplateOfItemControl
    {
      get { return (ControlTemplate)this.GetValue(TemplateOfItemControlProperty); }
      set { this.SetValue(TemplateOfItemControlProperty, value); }
    }
    #endregion DropDown ItemsControl dependencyproperties

    #region DropDown dependencyproperties
    /// <summary>
    /// Gets/sets the maximum heigt of the Pop-up/DropDown element of the button.
    /// The default value is positive infiinity which means that the drop down will
    /// occupy the complite available vertical space if it is not restricted.
    /// </summary>
    [LocalizabilityAttribute(LocalizationCategory.None, Readability = Readability.Unreadable)]
    [TypeConverterAttribute(typeof(LengthConverter))]
    public double DropDownMaxHeight
    {
      get { return (double)this.GetValue(DropDownMaxHeightProperty); }
      set { this.SetValue(DropDownMaxHeightProperty, value); }
    }

    /// <summary>
    /// Gets/sets the maximum width of the Pop-up/DropDown element of the button.
    /// The default value is positive infiinity which means that the drop down will
    /// occupy the complite available horizontal space if it is not restricted.
    /// </summary>
    [LocalizabilityAttribute(LocalizationCategory.None, Readability = Readability.Unreadable)]
    [TypeConverterAttribute(typeof(LengthConverter))]
    public double DropDownMaxWidth
    {
      get { return (double)this.GetValue(DropDownMaxWidthProperty); }
      set { this.SetValue(DropDownMaxWidthProperty, value); }
    }
    #endregion DropDown dependencyproperties

    #region Commands
    /// <summary>
    /// Part of the Command dependency property.
    /// </summary>
    [TypeConverter(typeof(CommandConverter))]
    public ICommand Command
    {
      get
      {
        return (ICommand)GetValue(CommandProperty);
      }

      set
      {
        this.SetValue(CommandProperty, value);
      }
    }

    /// <summary>
    /// Part of the Command Parameter dependency property.
    /// </summary>
    public object CommandParameter
    {
      get
      {
        return this.GetValue(CommandParameterProperty);
      }

      set
      {
        this.SetValue(CommandParameterProperty, value);
      }
    }

    /// <summary>
    /// Part of the Command Target dependency property.
    /// </summary>
    public IInputElement CommandTarget
    {
      get
      {
        return (IInputElement)GetValue(CommandTargetProperty);
      }

      set
      {
        this.SetValue(CommandTargetProperty, value);
      }
    }
    #endregion Commands

    /// <summary>
    /// Gets/sets the templated PART_DropDownButton instance.
    /// </summary>
    protected System.Windows.Controls.Primitives.ButtonBase Button
    {
      get
      {
        return this.mButton;
      }

      set
      {
        if (this.mButton != value)
        {
          if (this.mButton != null)
            this.mButton.Click -= this.DropDownButton_Click;

          this.mButton = value;

          if (this.mButton != null)
            this.mButton.Click += this.DropDownButton_Click;
        }
      }
    }
    #endregion Properties

    #region methods
    /// <summary>
    /// <InheriteDoc/>
    /// </summary>
    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();
      this.Button = GetTemplateChild(PART_DropDownButton) as ToggleButton;

      this.mItemsControl = GetTemplateChild(PART_ItemsControl) as ItemsControl;

      if (this.mPopup != null)
        this.mPopup.Opened -= this.Popup_Opened;

      this.mPopup = GetTemplateChild(PART_Popup) as Popup;

      if (this.mPopup != null)
        this.mPopup.Opened += this.Popup_Opened;
    }

    /// <summary>
    /// Method is invoked when the IsOpen dependency property is changed.
    /// </summary>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    protected virtual void OnIsOpenChanged(bool oldValue, bool newValue)
    {
      if (newValue == true)
        this.RaiseRoutedEvent(DropDownItemsButton.OpenedEvent);
      else
        this.RaiseRoutedEvent(DropDownItemsButton.ClosedEvent);
    }

    /// <summary>
    /// Method is invoked when the Command dependency property is changed.
    /// </summary>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    protected virtual void OnCommandChanged(ICommand oldValue, ICommand newValue)
    {
      // If old command is not null, then we need to remove the handlers.
      if (oldValue != null)
        this.UnhookCommand(oldValue, newValue);

      this.HookUpCommand(oldValue, newValue);

      // May need to call this when changing the command parameter or target.
      this.CanExecuteChanged();
    }

    /// <summary>
    /// Method executes when the drop down button is clicked.
    /// </summary>
    protected virtual void OnClick()
    {
      this.RaiseRoutedEvent(DropDownItemsButton.ClickEvent);
      this.RaiseCommand();
    }

    /// <summary>
    /// Method is invoked when the IsOpen dependency property is changed.
    /// </summary>
    /// <param name="o"></param>
    /// <param name="e"></param>
    private static void OnIsOpenChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
    {
      var dropDownButton = o as DropDownItemsButton;

      if (dropDownButton != null)
        dropDownButton.OnIsOpenChanged((bool)e.OldValue, (bool)e.NewValue);
    }

    /// <summary>
    /// Method is invoked when the Command dependency property is changed.
    /// </summary>
    /// <param name="d"></param>
    /// <param name="e"></param>
    private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      var dropDownButton = d as DropDownItemsButton;

      if (dropDownButton != null)
        dropDownButton.OnCommandChanged((ICommand)e.OldValue, (ICommand)e.NewValue);
    }

    #region Event Handlers
    /// <summary>
    /// Implements a keyboard event handler for the
    /// System.Windows.Input.Keyboard.KeyDown attached event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnKeyDown(object sender, KeyEventArgs e)
    {
      if (this.IsOpen == false)
      {
        if (KeyboardUtilities.IsKeyModifyingPopupState(e))
        {
          this.IsOpen = true;

          // ItemsControl items will get focus in Popup_Opened().
          e.Handled = true;
        }
      }
      else
      {
        if (KeyboardUtilities.IsKeyModifyingPopupState(e))
        {
          this.CloseDropDown(true);
          e.Handled = true;
        }
        else if (e.Key == Key.Escape)
        {
          this.CloseDropDown(true);
          e.Handled = true;
        }
      }
    }

    /// <summary>
    /// Closes the drop-down content element when user clicks
    /// the mouse outside of the pop-up/drop down element.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnMouseDownOutsideCapturedElement(object sender, MouseButtonEventArgs e)
    {
      this.CloseDropDown(false);
    }

    /// <summary>
    /// Executes when the user clicks on the drop-down button.
    /// System opens the drop-down/pop-up element.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DropDownButton_Click(object sender, RoutedEventArgs e)
    {
      this.OnClick();
    }

    private void CanExecuteChanged(object sender, EventArgs e)
    {
      this.CanExecuteChanged();
    }

    private void Popup_Opened(object sender, EventArgs e)
    {
      // Set the focus on the content of the ItemsControl.
      if (this.mItemsControl != null)
        this.mItemsControl.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
    }
    #endregion Event Handlers

    private void CanExecuteChanged()
    {
      if (this.Command != null)
      {
        var command = this.Command as RoutedCommand;

        // If a RoutedCommand.
        if (command != null)
				{
					if (this.CommandTarget != null)
						this.IsEnabled = command.CanExecute(this.CommandParameter, this.CommandTarget) ? true : false;
					else
						this.IsEnabled = command.CanExecute(this.CommandParameter, this) ? true : false;
				}
        else
        {
          // If a not RoutedCommand.
          this.IsEnabled = this.Command.CanExecute(this.CommandParameter) ? true : false;
        }
      }
    }

    /// <summary>
    /// Closes the drop down.
    /// Is executed when the user closes the drop down via keyboard (ESC or ALT-Cursor Up).
    /// Is executed when the user clicks outside of the open drop down element.
    /// </summary>
    private void CloseDropDown(bool isFocusOnButton)
    {
      if (this.IsOpen == true)
        this.IsOpen = false;

      this.ReleaseMouseCapture();

      if (isFocusOnButton == true)
        this.Button.Focus();
    }

    /// <summary>
    /// Raises routed events.
    /// </summary>
    private void RaiseRoutedEvent(RoutedEvent routedEvent)
    {
      RoutedEventArgs args = new RoutedEventArgs(routedEvent, this);
      this.RaiseEvent(args);
    }

    /// <summary>
    /// Raises the command's Execute event.
    /// </summary>
    private void RaiseCommand()
    {
      if (this.Command != null)
      {
        RoutedCommand routedCommand = this.Command as RoutedCommand;

        if (routedCommand == null)
          ((ICommand)this.Command).Execute(this.CommandParameter);
        else
          routedCommand.Execute(this.CommandParameter, this.CommandTarget);
      }
    }

    /// <summary>
    /// Unhooks a command from the Command property.
    /// </summary>
    /// <param name="oldCommand">The old command.</param>
    /// <param name="newCommand">The new command.</param>
    private void UnhookCommand(ICommand oldCommand, ICommand newCommand)
    {
      EventHandler handler = this.CanExecuteChanged;
      oldCommand.CanExecuteChanged -= handler;
    }

    /// <summary>
    /// Hooks up a command to the CanExecuteChnaged event handler.
    /// </summary>
    /// <param name="oldCommand">The old command.</param>
    /// <param name="newCommand">The new command.</param>
    private void HookUpCommand(ICommand oldCommand, ICommand newCommand)
    {
      EventHandler handler = new EventHandler(this.CanExecuteChanged);
      this.mCanExecuteChangedHandler = handler;

      if (newCommand != null)
        newCommand.CanExecuteChanged += this.mCanExecuteChangedHandler;
    }
    #endregion methods
  }
}
