namespace MWindowDialogLib.Internal
{
    using MWindowInterfacesLib.Interfaces;
    using System;
    using System.Threading.Tasks;
    using System.Windows;
    using Util;  //Extensions

    /// <summary>
    /// This class supports coordination of content dialogs from within
    /// a viewmodel that is attached to a window view.
    /// 
    /// The relevant methods contain a parameter called context to support
    /// this use case. The context is either:
    /// 
    /// 1) An implementation of <seealso cref="IMetroWindow"/> or
    /// 
    /// 2) A ViewModel that is bound to an <seealso cref="IMetroWindow"/> implementation
    ///    and registered via DialogParticipation.
    /// </summary>
    internal class DialogCoordinator : IDialogCoordinator
    {
        #region fields
        private readonly IDialogManager _dialogManager = null;
        #endregion fields

        public DialogCoordinator(IDialogManager dialogManager)
            : this()
        {
            _dialogManager = dialogManager;
        }

        protected DialogCoordinator()
        {

        }

        public Task<TDialog> GetCurrentDialogAsync<TDialog>(object context) where TDialog : IBaseMetroDialogFrame
        {
            var metroWindow = GetMetroWindow(context);
            return metroWindow.Dispatcher.Invoke(() => metroWindow.GetCurrentDialogAsync<TDialog>());
        }

        public Task ShowMetroDialogAsync(object context
                                       , IBaseMetroDialogFrame dialog
                                       , IMetroDialogFrameSettings settings = null)
        {
            var metroWindow = GetMetroWindow(context);
            return metroWindow.Dispatcher.Invoke(() => _dialogManager.ShowMetroDialogAsync(metroWindow, dialog, settings));
        }

        public async Task<int> ShowMetroDialogAsync(object context
                                       , IMsgBoxDialogFrame<int> dialog
                                       , IMetroDialogFrameSettings settings = null)
        {
            var metroWindow = GetMetroWindow(context);

            var result = await _dialogManager.ShowMetroDialogAsync(metroWindow, dialog);

            return result;
        }


        public Task HideMetroDialogAsync(object context
                                       , IBaseMetroDialogFrame dialog
                                       , IMetroDialogFrameSettings settings = null)
        {
            var metroWindow = GetMetroWindow(context);
            return metroWindow.Dispatcher.Invoke(() => _dialogManager.HideMetroDialogAsync(metroWindow, dialog, settings));
        }

        /// <summary>
        /// Attempts to find the MetroWindow that should show the ContentDialog
        /// by searching the context object in the DialogParticipation object.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public IMetroWindow GetMetroWindow(object context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (!ContextRegistration.Instance.IsRegistered(context))
                throw new InvalidOperationException("Context is not registered. Consider using static class DialogParticipation.Register in XAML to bind in the DataContext.");

            var association = ContextRegistration.Instance.GetAssociation(context);
            var metroWindow = association.Invoke(() => Window.GetWindow(association) as IMetroWindow);

            if (metroWindow == null)
                throw new InvalidOperationException("Context is not inside a MetroWindow.");

            return metroWindow;
        }
    }
}
