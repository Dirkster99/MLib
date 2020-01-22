namespace MWindowLib.Behaviours
{
    using global::Microsoft.Xaml.Behaviors;
    using System.Windows;

    /// <summary>
    /// This class implements an attached property that can be attached
    /// to a window style to hold a collection of behaviors that in turn can manipulate
    /// the appearance of a window (e.g.: make it borderless).
    /// </summary>
    public class StylizedBehaviors
    {
        /// <summary>
        /// Backing store of the Behaviors dependency property which implements
        /// a collection of behaviors that can be applied to a style (of a window).
        /// </summary>
        public static readonly DependencyProperty BehaviorsProperty
            = DependencyProperty.RegisterAttached("Behaviors",
                                                  typeof(StylizedBehaviorCollection),
                                                  typeof(StylizedBehaviors),
                                                  new FrameworkPropertyMetadata(null,
                                                                                OnPropertyChanged));

        /// <summary>
        /// Gets the Behaviors dependency property which implements
        /// a collection of behaviors that can be applied to a style (of a window).
        /// </summary>
        /// <param name="uie"></param>
        /// <returns>the collection of style behaviors</returns>
        public static StylizedBehaviorCollection GetBehaviors(DependencyObject uie)
        {
            return (StylizedBehaviorCollection)uie.GetValue(BehaviorsProperty);
        }

        /// <summary>
        /// Sets the Behaviors dependency property which implements
        /// a collection of behaviors that can be applied to a style (of a window).
        /// </summary>
        /// <param name="uie"></param>
        /// <param name="value"></param>
        public static void SetBehaviors(DependencyObject uie, StylizedBehaviorCollection value)
        {
            uie.SetValue(BehaviorsProperty, value);
        }

        /// <summary>
        /// Method is invoked when the Behaviors dependency property is changed (on attach/detach).
        /// </summary>
        /// <param name="dpo">The FrameworkElement where this behavior is attached.</param>
        /// <param name="e">A collection of a <seealso cref="StylizedBehaviorCollection"/>
        /// to keep track of in this behaviors collection.</param>
        private static void OnPropertyChanged(DependencyObject dpo, DependencyPropertyChangedEventArgs e)
        {
            var uie = dpo as FrameworkElement;
            if (uie == null)
            {
                return;
            }

            var newBehaviors = e.NewValue as StylizedBehaviorCollection;
            var oldBehaviors = e.OldValue as StylizedBehaviorCollection;
            if (newBehaviors == oldBehaviors)
            {
                return;
            }

            BehaviorCollection itemBehaviors = Interaction.GetBehaviors(uie);

            uie.Unloaded -= FrameworkElementUnloaded;

            if (oldBehaviors != null)
            {
                foreach (var behavior in oldBehaviors)
                {
                    int index = GetIndexOf(itemBehaviors, behavior);
                    if (index >= 0)
                    {
                        itemBehaviors.RemoveAt(index);
                    }
                }
            }

            if (newBehaviors != null)
            {
                foreach (var behavior in newBehaviors)
                {
                    int index = GetIndexOf(itemBehaviors, behavior);
                    if (index < 0)
                    {
                        var clone = (Behavior)behavior.Clone();
                        SetOriginalBehavior(clone, behavior);
                        itemBehaviors.Add(clone);
                    }
                }
            }

            if (itemBehaviors.Count > 0)
            {
                uie.Unloaded += FrameworkElementUnloaded;
            }
            uie.Dispatcher.ShutdownStarted += Dispatcher_ShutdownStarted;
        }

        private static void Dispatcher_ShutdownStarted(object sender, System.EventArgs e)
        {
            ////var s = "";
        }

        private static void FrameworkElementUnloaded(object sender, RoutedEventArgs e)
        {
            // BehaviorCollection doesn't call Detach, so we do this
            var uie = sender as FrameworkElement;
            if (uie == null)
            {
                return;
            }
            BehaviorCollection itemBehaviors = Interaction.GetBehaviors(uie);
            foreach (var behavior in itemBehaviors)
            {
                behavior.Detach();
            }
            uie.Loaded += FrameworkElementLoaded;
        }

        private static void FrameworkElementLoaded(object sender, RoutedEventArgs e)
        {
            var uie = sender as FrameworkElement;
            if (uie == null)
            {
                return;
            }
            uie.Loaded -= FrameworkElementLoaded;
            BehaviorCollection itemBehaviors = Interaction.GetBehaviors(uie);
            foreach (var behavior in itemBehaviors)
            {
                behavior.Attach(uie);
            }
        }

        private static int GetIndexOf(BehaviorCollection itemBehaviors, Behavior behavior)
        {
            int index = -1;

            Behavior orignalBehavior = GetOriginalBehavior(behavior);

            for (int i = 0; i < itemBehaviors.Count; i++)
            {
                Behavior currentBehavior = itemBehaviors[i];
                if (currentBehavior == behavior || currentBehavior == orignalBehavior)
                {
                    index = i;
                    break;
                }

                Behavior currentOrignalBehavior = GetOriginalBehavior(currentBehavior);
                if (currentOrignalBehavior == behavior || currentOrignalBehavior == orignalBehavior)
                {
                    index = i;
                    break;
                }
            }

            return index;
        }

        private static readonly DependencyProperty OriginalBehaviorProperty
            = DependencyProperty.RegisterAttached("OriginalBehaviorInternal",
                                                  typeof(Behavior),
                                                  typeof(StylizedBehaviors),
                                                  new UIPropertyMetadata(null));

        private static Behavior GetOriginalBehavior(DependencyObject obj)
        {
            return obj.GetValue(OriginalBehaviorProperty) as Behavior;
        }

        private static void SetOriginalBehavior(DependencyObject obj, Behavior value)
        {
            obj.SetValue(OriginalBehaviorProperty, value);
        }
    }
}
