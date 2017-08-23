namespace PDF_Binder.ViewModels.VMManagement
{
    /// <summary>
    /// A service that shows message boxes.
    /// </summary>
    public class VMManagerService
    {
        #region properties
        /// <summary>
        /// Gets an instance of the MessageBox service component.
        /// This component displays message boxes, including stack traces,
        /// in an integrated WPF themed manner..
        /// </summary>
        public static IVMManager Instance
        {
            get
            {
                return new VMManager(null);
            }
        }
        #endregion properties
    }
}
