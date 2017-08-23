namespace MDemo.Demos.ViewModels.Tasks
{
    /// <summary>
    /// This interface gives backend code from long running tasks
    /// access to core properties that are used to update the user
    /// on the progress of a long running task.
    /// </summary>
    public interface IProgress
    {
        #region properties
        /// <summary>
        /// Gets/Sets Maximum value for a finite progress display.
        /// </summary>
        double ProgressMaximum { get; set; }

        /// <summary>
        /// Gets/Sets Current value for a finite progress display.
        /// </summary>
        double ProgressValue { get; set; }

        /// <summary>
        /// Gets/Sets Minimum value for a finite progress display.
        /// </summary>
        double ProgressMinimum { get; set; }

        /// <summary>
        /// Gets/Sets whether the progress display should currently be visible
        /// or not (progress may not be displayed if processing has already finished)
        /// </summary>
        bool ProgressIsVisible { get; set; }

        /// <summary>
        /// Gets whether the process is in progress (currently progressing) or not.
        /// </summary>
        bool IsProgressing { get; }

        /// <summary>
        /// Gets/Sets Whether the bound progress display is finite (usually
        /// progressing from minimum to maximum) or infinite.
        /// </summary>
        bool ProgressIsFinite { get; set; }

        /// <summary>
        /// Displays a status text to describe the current
        /// progress step in a textual way...
        /// </summary>
        string ProgressText { get; set; }

        /// <summary>
        /// Gets/sets an object that can be set by the task that is
        /// executing inside the progress viewmodel. Most likely this
        /// object represents the result of the waiting for this process...
        /// </summary>
        object ProcessResult { get; set; }
        #endregion properties
    }
}
