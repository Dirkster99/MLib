/*************************************************************************************

   Extended WPF Toolkit

   Copyright (C) 2007-2013 Xceed Software Inc.

   This program is provided to you under the terms of the Microsoft Public
   License (Ms-PL) as published at http://wpftoolkit.codeplex.com/license 

   For more features, controls, and fast professional support,
   pick up the Plus Edition at http://xceed.com/wpf_toolkit

   Stay informed: follow @datagrid on Twitter or Like http://facebook.com/datagrids

  ***********************************************************************************/
namespace DropDownButtonLib.Controls
{
  using System;
  using System.ComponentModel;
  using System.Windows;
  using System.Windows.Controls;
  using System.Windows.Controls.Primitives;
  using System.Windows.Input;
  using DropDownButtonLib.Utilities;

  /// <summary>
  /// Implements a look-less WPF DropDownButton control.
  /// </summary>
  [TemplatePart(Name = DropDownButton.PART_DropDownButton, Type = typeof(ToggleButton))]
  [TemplatePart(Name = DropDownButton.PART_ContentPresenter, Type = typeof(ContentPresenter))]
  [TemplatePart(Name = DropDownButton.PART_Popup, Type = typeof(Popup))]
  public class DropDownButton : ContentControl, ICommandSource
  {
    #region fields
    /// <summary>
    /// Const string name of the required drop down button element in the <see cref="DropDownButton"/> control.
    /// </summary>
    public const string PART_DropDownButton = "PART_DropDownButton";

    /// <summary>
    /// Const string name of the required ContentPresenter element in the <see cref="DropDownButton"/> control.
    /// </summary>
    public const string PART_ContentPresenter = "PART_ContentPresenter";

    /// <summary>
    /// Const string name of the required PopUp element in the <see cref="DropDownButton"/> control.
    /// </summary>
    public const string PART_Popup = "PART_Popup";

    #region dependency properties
    /// <summary>
    /// Backing store of the DropDownContent dependency property.
    /// </summary>
    public static readonly DependencyProperty DropDownContentProperty = DependencyProperty.Register("DropDownContent", typeof(object), typeof(DropDownButton), new UIPropertyMetadata(null, OnDropDownContentChanged));

    /// <summary>
    /// Backing store of the IsOpen dependency property.
    /// </summary>
    public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register("IsOpen", typeof(bool), typeof(DropDownButton), new UIPropertyMetadata(false, OnIsOpenChanged));

    /// <summary>
    /// Backing store of the Command dependency property.
    /// </summary>
    public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(DropDownButton), new PropertyMetadata((ICommand)null, OnCommandChanged));

    /// <summary>
    /// Backing store of the CommandParameter dependency property.
    /// </summary>
    public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register("CommandParameter", typeof(object), typeof(DropDownButton), new PropertyMetadata(null));

    /// <summary>
    /// Backing store of the CommandTarget dependency property.
    /// </summary>
    public static readonly DependencyProperty CommandTargetProperty = DependencyProperty.Register("CommandTarget", typeof(IInputElement), typeof(DropDownButton), new PropertyMetadata(null));

    #region events
    /// <summary>
    /// Backing store of the Click Event dependency property.
    /// </summary>
    public static readonly RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(DropDownButton));

    /// <summary>
    /// Backing store of the Opened Event dependency property.
    /// </summary>
    public static readonly RoutedEvent OpenedEvent = EventManager.RegisterRoutedEvent("Opened", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(DropDownButton));

    /// <summary>
    /// Backing store of the Closed Event dependency property.
    /// </summary>
    public static readonly RoutedEvent ClosedEvent = EventManager.RegisterRoutedEvent("Closed", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(DropDownButton));
    #endregion events
    #endregion dependency properties

    private System.Windows.Controls.Primitives.ButtonBase mButton;
    private ContentPresenter mContentPresenter;
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
    static DropDownButton()
    {
      DefaultStyleKeyProperty.OverrideMetadata(typeof(DropDownButton),
                                               new FrameworkPropertyMetadata(typeof(DropDownButton)));
    }

    /// <summary>
    /// Default class constructor
    /// </summary>
    public DropDownButton()
    {
      Keyboard.AddKeyDownHandler(this, this.OnKeyDown);
      Mouse.AddPreviewMouseDownOutsideCapturedElementHandler(this, this.OnMouseDownOutsideCapturedElement);
    }
    #endregion constructors

    #region events
    /// <summary>
    /// Adds/removes a reference to a click event handler.
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
    /// Adds/removes a reference to a Opened event handler.
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
    /// Adds/removes a reference to a Closed event handler.
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
    /// Gets/sets part of the DropDownContent dependency property of this class.
    /// </summary>
    public object DropDownContent
    {
      get
      {
        return (object)GetValue(DropDownContentProperty);
      }

      set
      {
        this.SetValue(DropDownContentProperty, value);
      }
    }

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
    /// Gets/sets CommandParameter dependency property.
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
    /// Gets/sets CommandTarget dependency property.
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

      this.mContentPresenter = GetTemplateChild(PART_ContentPresenter) as ContentPresenter;

      if (this.mPopup != null)
        this.mPopup.Opened -= this.Popup_Opened;

      this.mPopup = GetTemplateChild(PART_Popup) as Popup;

      if (this.mPopup != null)
        this.mPopup.Opened += this.Popup_Opened;
    }

    /// <summary>
    /// Method is invoked when the DropDownContent dependency property is changed.
    /// </summary>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    protected virtual void OnDropDownContentChanged(object oldValue, object newValue)
    {
      // TODO: Add your property changed side-effects. Descendants can override as well.
    }

    /// <summary>
    /// Method is invoked when the IsOpen dependency property is changed.
    /// </summary>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    protected virtual void OnIsOpenChanged(bool oldValue, bool newValue)
    {
      if (newValue == true)
        this.RaiseRoutedEvent(DropDownButton.OpenedEvent);
      else
        this.RaiseRoutedEvent(DropDownButton.ClosedEvent);
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
    /// The method opens the drop-down/pop-up element.
    /// </summary>
    protected virtual void OnClick()
    {
      this.RaiseRoutedEvent(DropDownButton.ClickEvent);
      this.RaiseCommand();
    }

    /// <summary>
    /// Method is invoked when the DropDownContent dependency property is changed.
    /// </summary>
    /// <param name="o"></param>
    /// <param name="e"></param>
    private static void OnDropDownContentChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
    {
      var dropDownButton = o as DropDownButton;

      if (dropDownButton != null)
        dropDownButton.OnDropDownContentChanged((object)e.OldValue, (object)e.NewValue);
    }

    /// <summary>
    /// Method is invoked when the IsOpen dependency property is changed.
    /// </summary>
    /// <param name="o"></param>
    /// <param name="e"></param>
    private static void OnIsOpenChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
    {
      var dropDownButton = o as DropDownButton;

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
      var dropDownButton = d as DropDownButton;

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

          // ContentPresenter items will get focus in Popup_Opened().
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
      // Set the focus on the content of the ContentPresenter.
      if (this.mContentPresenter != null)
        this.mContentPresenter.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
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
