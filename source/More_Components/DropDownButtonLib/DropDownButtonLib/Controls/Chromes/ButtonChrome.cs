/*************************************************************************************

   Extended WPF Toolkit

   Copyright (C) 2007-2013 Xceed Software Inc.

   This program is provided to you under the terms of the Microsoft Public
   License (Ms-PL) as published at http://wpftoolkit.codeplex.com/license 

   For more features, controls, and fast professional support,
   pick up the Plus Edition at http://xceed.com/wpf_toolkit

   Stay informed: follow @datagrid on Twitter or Like http://facebook.com/datagrids

  ***********************************************************************************/
namespace DropDownButtonLib.Controls.Chromes
{
  using System;
  using System.Windows;
  using System.Windows.Controls;

  /// <summary>
  /// Implements a ButtonChrome which is a kind of frame that
  /// determines the look and feel of a button.
  /// </summary>
  public class ButtonChrome : ContentControl
  {
    #region fields
    /// <summary>
    /// Backing store for corner radius dependency property of button chrome.
    /// </summary>
    public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(ButtonChrome), new UIPropertyMetadata(default(CornerRadius), new PropertyChangedCallback(OnCornerRadiusChanged)));

    /// <summary>
    /// Backing store for inner corner radius dependency property of button chrome.
    /// </summary>
    public static readonly DependencyProperty InnerCornerRadiusProperty = DependencyProperty.Register("InnerCornerRadius", typeof(CornerRadius), typeof(ButtonChrome), new UIPropertyMetadata(default(CornerRadius), new PropertyChangedCallback(OnInnerCornerRadiusChanged)));
    
    /// <summary>
    /// Backing store for render checked dependency property of button chrome.
    /// </summary>
    public static readonly DependencyProperty RenderCheckedProperty = DependencyProperty.Register("RenderChecked", typeof(bool), typeof(ButtonChrome), new UIPropertyMetadata(false, OnRenderCheckedChanged));

    /// <summary>
    /// Backing store for render enabled dependency property of button chrome.
    /// </summary>
    public static readonly DependencyProperty RenderEnabledProperty = DependencyProperty.Register("RenderEnabled", typeof(bool), typeof(ButtonChrome), new UIPropertyMetadata(true, OnRenderEnabledChanged));

    /// <summary>
    /// Backing store for render focused dependency property of button chrome.
    /// </summary>
    public static readonly DependencyProperty RenderFocusedProperty = DependencyProperty.Register("RenderFocused", typeof(bool), typeof(ButtonChrome), new UIPropertyMetadata(false, OnRenderFocusedChanged));

    /// <summary>
    /// Backing store for render mouse over dependency property of button chrome.
    /// </summary>
    public static readonly DependencyProperty RenderMouseOverProperty = DependencyProperty.Register("RenderMouseOver", typeof(bool), typeof(ButtonChrome), new UIPropertyMetadata(false, OnRenderMouseOverChanged));

    /// <summary>
    /// Backing store for render normal dependency property of button chrome.
    /// </summary>
    public static readonly DependencyProperty RenderNormalProperty = DependencyProperty.Register("RenderNormal", typeof(bool), typeof(ButtonChrome), new UIPropertyMetadata(true, OnRenderNormalChanged));

    /// <summary>
    /// Backing store for render pressed dependency property of button chrome.
    /// </summary>
    public static readonly DependencyProperty RenderPressedProperty = DependencyProperty.Register("RenderPressed", typeof(bool), typeof(ButtonChrome), new UIPropertyMetadata(false, OnRenderPressedChanged));
    #endregion fields

    #region constructors
    /// <summary>
    /// Static class constructor
    /// </summary>
    static ButtonChrome()
    {
      DefaultStyleKeyProperty.OverrideMetadata(typeof(ButtonChrome), new FrameworkPropertyMetadata(typeof(ButtonChrome)));
    }
    #endregion constructors

    #region properties
    #region dependency properties
    /// <summary>
    /// Gets/sets corner radius dependency property of button chrome.
    /// </summary>
    public CornerRadius CornerRadius
    {
      get
      {
        return (CornerRadius)GetValue(CornerRadiusProperty);
      }

      set
      {
        this.SetValue(CornerRadiusProperty, value);
      }
    }

    /// <summary>
    /// Gets/sets inner corner radius dependency property of button chrome.
    /// </summary>
    public CornerRadius InnerCornerRadius
    {
      get
      {
        return (CornerRadius)GetValue(InnerCornerRadiusProperty);
      }

      set
      {
        this.SetValue(InnerCornerRadiusProperty, value);
      }
    }

    /// <summary>
    /// Gets/sets render checked dependency property of button chrome.
    /// </summary>
    public bool RenderChecked
    {
      get
      {
        return (bool)GetValue(RenderCheckedProperty);
      }
 
      set
      {
        this.SetValue(RenderCheckedProperty, value);
      }
    }

    /// <summary>
    /// Gets/sets render enabled dependency property of button chrome.
    /// </summary>
    public bool RenderEnabled
    {
      get
      {
        return (bool)GetValue(RenderEnabledProperty);
      }
 
      set
      {
        this.SetValue(RenderEnabledProperty, value);
      }
    }

    /// <summary>
    /// Gets/sets render focused dependency property of button chrome.
    /// </summary>
    public bool RenderFocused
    {
      get
      {
        return (bool)GetValue(RenderFocusedProperty);
      }
  
      set
      {
        this.SetValue(RenderFocusedProperty, value);
      }
    }

    /// <summary>
    /// Gets/sets render mouseover dependency property of button chrome.
    /// </summary>
    public bool RenderMouseOver
    {
      get
      {
        return (bool)GetValue(RenderMouseOverProperty);
      }
 
      set
      {
        this.SetValue(RenderMouseOverProperty, value);
      }
    }

    /// <summary>
    /// Gets/sets render normal dependency property of button chrome.
    /// </summary>
    public bool RenderNormal
    {
      get
      {
        return (bool)GetValue(RenderNormalProperty);
      }
     
      set
      {
        this.SetValue(RenderNormalProperty, value);
      }
    }

    /// <summary>
    /// Gets/sets render pressed dependency property of button chrome.
    /// </summary>
    public bool RenderPressed
    {
      get
      {
        return (bool)GetValue(RenderPressedProperty);
      }
    
      set
      {
        this.SetValue(RenderPressedProperty, value);
      }
    }
    #endregion dependency properties
    #endregion properties

    #region methods
    /// <summary>
    /// Method executes when the corner radius dependency property is changed.
    /// It re-computes the inner corner radius since it always needs to be 1
    /// less than the outer radius.
    /// </summary>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    protected virtual void OnCornerRadiusChanged(CornerRadius oldValue, CornerRadius newValue)
    {
      // we always want the InnerBorderRadius to be one less than the CornerRadius
      CornerRadius newInnerCornerRadius = new CornerRadius(Math.Max(0, newValue.TopLeft - 1),
                                                           Math.Max(0, newValue.TopRight - 1),
                                                           Math.Max(0, newValue.BottomRight - 1),
                                                           Math.Max(0, newValue.BottomLeft - 1));

      this.InnerCornerRadius = newInnerCornerRadius;
    }

    /// <summary>
    /// Method executes when the inner corner radius dependency property is changed.
    /// 
    /// TODO: Add your property changed side-effects. Descendants can override as well.
    /// </summary>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    protected virtual void OnInnerCornerRadiusChanged(CornerRadius oldValue, CornerRadius newValue)
    {
      
    }

    /// <summary>
    /// Method executes when the render checked dependency property is changed.
    /// 
    /// TODO: Add your property changed side-effects. Descendants can override as well.
    /// </summary>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    protected virtual void OnRenderCheckedChanged(bool oldValue, bool newValue)
    {
    }

    /// <summary>
    /// Method executes when the render enabled dependency property is changed.
    /// 
    /// TODO: Add your property changed side-effects. Descendants can override as well.
    /// </summary>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    protected virtual void OnRenderEnabledChanged(bool oldValue, bool newValue)
    {
    }

    /// <summary>
    /// Method executes when the render focused dependency property is changed.
    /// 
    /// TODO: Add your property changed side-effects. Descendants can override as well.
    /// </summary>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    protected virtual void OnRenderFocusedChanged(bool oldValue, bool newValue)
    {
    }

    /// <summary>
    /// Method executes when the render mouseover dependency property is changed.
    /// 
    /// TODO: Add your property changed side-effects. Descendants can override as well.
    /// </summary>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    protected virtual void OnRenderMouseOverChanged(bool oldValue, bool newValue)
    {
    }

    /// <summary>
    /// Method executes when the render normal dependency property is changed.
    /// 
    /// TODO: Add your property changed side-effects. Descendants can override as well.
    /// </summary>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    protected virtual void OnRenderNormalChanged(bool oldValue, bool newValue)
    {
    }

    /// <summary>
    /// Method executes when the render pressed dependency property is changed.
    /// 
    /// TODO: Add your property changed side-effects. Descendants can override as well.
    /// </summary>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    protected virtual void OnRenderPressedChanged(bool oldValue, bool newValue)
    {
    }

    /// <summary>
    /// Method executes when the corner radius dependency property is changed.
    /// </summary>
    /// <param name="o"></param>
    /// <param name="e"></param>
    private static void OnCornerRadiusChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
    {
      ButtonChrome buttonChrome = o as ButtonChrome;
      
      if (buttonChrome != null)
        buttonChrome.OnCornerRadiusChanged((CornerRadius)e.OldValue, (CornerRadius)e.NewValue);
    }

    /// <summary>
    /// Method executes when the inner corner radius dependency property is changed.
    /// </summary>
    /// <param name="o"></param>
    /// <param name="e"></param>
    private static void OnInnerCornerRadiusChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
    {
      ButtonChrome buttonChrome = o as ButtonChrome;
      if (buttonChrome != null)
        buttonChrome.OnInnerCornerRadiusChanged((CornerRadius)e.OldValue, (CornerRadius)e.NewValue);
    }

    /// <summary>
    /// Method executes when the render checked dependency property is changed.
    /// </summary>
    /// <param name="o"></param>
    /// <param name="e"></param>
    private static void OnRenderCheckedChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
    {
      ButtonChrome buttonChrome = o as ButtonChrome;
      if (buttonChrome != null)
        buttonChrome.OnRenderCheckedChanged((bool)e.OldValue, (bool)e.NewValue);
    }

    /// <summary>
    /// Method executes when the render enabled dependency property is changed.
    /// </summary>
    /// <param name="o"></param>
    /// <param name="e"></param>
    private static void OnRenderEnabledChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
    {
      ButtonChrome buttonChrome = o as ButtonChrome;
      if (buttonChrome != null)
        buttonChrome.OnRenderEnabledChanged((bool)e.OldValue, (bool)e.NewValue);
    }

    /// <summary>
    /// Method executes when the render focused dependency property is changed.
    /// </summary>
    /// <param name="o"></param>
    /// <param name="e"></param>
    private static void OnRenderFocusedChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
    {
      ButtonChrome buttonChrome = o as ButtonChrome;
      if (buttonChrome != null)
        buttonChrome.OnRenderFocusedChanged((bool)e.OldValue, (bool)e.NewValue);
    }

    /// <summary>
    /// Method executes when the render mouseover dependency property is changed.
    /// </summary>
    /// <param name="o"></param>
    /// <param name="e"></param>
    private static void OnRenderMouseOverChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
    {
      ButtonChrome buttonChrome = o as ButtonChrome;
      if (buttonChrome != null)
        buttonChrome.OnRenderMouseOverChanged((bool)e.OldValue, (bool)e.NewValue);
    }

    /// <summary>
    /// Method executes when the render normal dependency property is changed.
    /// </summary>
    /// <param name="o"></param>
    /// <param name="e"></param>
    private static void OnRenderNormalChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
    {
      ButtonChrome buttonChrome = o as ButtonChrome;
      if (buttonChrome != null)
        buttonChrome.OnRenderNormalChanged((bool)e.OldValue, (bool)e.NewValue);
    }

    /// <summary>
    /// Method executes when the render pressed dependency property is changed.
    /// </summary>
    /// <param name="o"></param>
    /// <param name="e"></param>
    private static void OnRenderPressedChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
    {
      ButtonChrome buttonChrome = o as ButtonChrome;
      if (buttonChrome != null)
        buttonChrome.OnRenderPressedChanged((bool)e.OldValue, (bool)e.NewValue);
    }
    #endregion methods
  }
}
