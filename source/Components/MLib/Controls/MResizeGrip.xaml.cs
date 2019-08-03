namespace MLib.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;
    using System.Windows.Media;

    /// <summary>
    /// Interaction logic for MResizeGrip.xaml
    /// </summary>
    public class MResizeGrip : Thumb
    {
        #region fields
        private Cursor _cursor;
        private Window _window;
        #endregion fields

        #region ctors
        /// <summary>
        /// Static class constructor
        /// </summary>
        static MResizeGrip()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MResizeGrip),
                                                     new FrameworkPropertyMetadata(typeof(MResizeGrip)));
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public MResizeGrip()
        {
            _cursor = default(Cursor);

            Loaded += MResizeGrip_Loaded;
            Unloaded += MResizeGrip_Unloaded;
        }
        #endregion ctors

        #region methods
        /// <summary>
        /// Executes when control is loaded - life time is ending.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MResizeGrip_Unloaded(object sender, RoutedEventArgs e)
        {
            // Clean this up in case window lives longer than control...
            _window = null;
            _cursor = default(Cursor);
        }

        /// <summary>
        /// Executes when control is loaded - life time starts a new.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MResizeGrip_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= MResizeGrip_Loaded;

            var control = sender as Controls.MResizeGrip;

            if (sender == null)
                return;

            _window = control.FindVisualAncestorOfType<Window>(this);

            if (_window == null)
                return;

            if (_window.ResizeMode == ResizeMode.NoResize)
                return;

            // Attach drag events to make Window resizable via ResizeGrip
            control.DragDelta += OnResizeThumbDragDelta;
            control.DragStarted += OnResizeThumbDragStarted;
            control.DragCompleted += OnResizeThumbDragCompleted;
        }

        private void OnResizeThumbDragStarted(object sender, DragStartedEventArgs e)
        {
            _cursor = _window.Cursor;
            _window.Cursor = Cursors.SizeNWSE;
        }

        private void OnResizeThumbDragCompleted(object sender, DragCompletedEventArgs e)
        {
            _window.Cursor = _cursor;
        }

        private void OnResizeThumbDragDelta(object sender, DragDeltaEventArgs e)
        {
            double yAdjust = _window.Height + e.VerticalChange;
            double xAdjust = _window.Width + e.HorizontalChange;

            //make sure not to resize to negative width or heigth            
            xAdjust = Math.Max(Math.Min(xAdjust, _window.MaxWidth) , _window.MinWidth);
            yAdjust = Math.Max(Math.Min(yAdjust, _window.MaxHeight), _window.MinHeight);

            _window.Width = xAdjust;
            _window.Height = yAdjust;

            e.Handled = true;
        }

        private Window FindWindow(object sender)
        {
            var control = sender as Controls.MResizeGrip;

            if (sender == null)
                return null;

            return control.FindVisualAncestorOfType<Window>(this);
        }

        /// <summary>
        /// Find a control element (eg. Window) based on its type in the VisualTree of another element (eg. TextBox).
        /// 
        /// https://stackoverflow.com/questions/636383/how-can-i-find-wpf-controls-by-name-or-type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Elt"></param>
        /// <returns></returns>
        private T FindVisualAncestorOfType<T>(DependencyObject Elt) where T : DependencyObject
        {
            for (DependencyObject parent = VisualTreeHelper.GetParent(Elt);
                parent != null; parent = VisualTreeHelper.GetParent(parent))
            {
                T result = parent as T;

                if (result != null)
                    return result;
            }

            return null;
        }
        #endregion methods
    }
}
