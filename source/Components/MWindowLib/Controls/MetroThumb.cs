namespace MWindowLib.Controls
{
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;

    /// <summary>
    /// Implements an extended thumb control for the metro framework.
    /// </summary>
    public class MetroThumb : Thumb, IMetroThumb
    {
        private TouchDevice currentDevice = null;

        /// <summary>
        /// Method is executed when thumb is dragged.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreviewTouchDown(TouchEventArgs e)
        {
            // Release any previous capture
            this.ReleaseCurrentDevice();

            // Capture the new touch
            this.CaptureCurrentDevice(e);
        }

        /// <summary>
        /// Method is executed when thumb is no longer dragged.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreviewTouchUp(TouchEventArgs e)
        {
            this.ReleaseCurrentDevice();
        }

        /// <summary>
        /// Method is executed when thumb is no longer touched.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLostTouchCapture(TouchEventArgs e)
        {
            // Only re-capture if the reference is not null
            // This way we avoid re-capturing after calling ReleaseCurrentDevice()
            if (this.currentDevice != null)
            {
                this.CaptureCurrentDevice(e);
            }
        }

        private void ReleaseCurrentDevice()
        {
            if (this.currentDevice != null)
            {
                // Set the reference to null so that we don't re-capture in the OnLostTouchCapture() method
                var temp = this.currentDevice;
                this.currentDevice = null;
                this.ReleaseTouchCapture(temp);
            }
        }

        private void CaptureCurrentDevice(TouchEventArgs e)
        {
            bool gotTouch = this.CaptureTouch(e.TouchDevice);
            if (gotTouch)
            {
                this.currentDevice = e.TouchDevice;
            }
        }
    }
}
