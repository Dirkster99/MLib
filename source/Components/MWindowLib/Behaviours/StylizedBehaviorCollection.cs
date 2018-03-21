namespace MWindowLib.Behaviours
{
    using System.Windows;
    using System.Windows.Interactivity;

    /// <summary>
    /// Class implements a freezable collection to keep track of behaviors
    /// in the <seealso cref="StylizedBehaviors"/> attached property.
    /// </summary>
    public class StylizedBehaviorCollection : FreezableCollection<Behavior>
    {
        /// <summary>
        /// Class constructor.
        /// </summary>
        protected override Freezable CreateInstanceCore()
        {
            return new StylizedBehaviorCollection();
        }
    }
}
