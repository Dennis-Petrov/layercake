using System;
using System.Windows;
using System.Diagnostics.Contracts;

namespace LayerCake.UI.Services.CodeContracts
{
    [ContractClassFor(typeof(IPresentationService))]
    internal abstract class PresentationServiceContract : IPresentationService
    {
        #region Реализация IPresentationService

        public abstract bool IsMainWindowShown { get; }
        public abstract Window TopmostWindow { get; }

        public void ShowInMainWindow<T>(string templateId, T viewModel, WindowSettings windowSettings)
            where T : ViewModel
        {
            Contract.Requires<ArgumentException>(!String.IsNullOrEmpty(templateId));
            Contract.Requires<ArgumentNullException>(viewModel != null);
            Contract.Requires<ArgumentNullException>(windowSettings != null);
            Contract.Requires<InvalidOperationException>(!IsMainWindowShown);
        }

        public void ShowInMainWindow<T>(T viewModel, WindowSettings windowSettings) 
            where T : ViewModel
        {
            Contract.Requires<ArgumentNullException>(viewModel != null);
            Contract.Requires<ArgumentNullException>(windowSettings != null);
            Contract.Requires<InvalidOperationException>(!IsMainWindowShown);
        }

        public bool ShowInDialog<T>(string templateId, T viewModel, WindowSettings windowSettings)
            where T : ViewModel
        {
            Contract.Requires<ArgumentException>(!String.IsNullOrEmpty(templateId));
            Contract.Requires<ArgumentNullException>(viewModel != null);
            Contract.Requires<ArgumentNullException>(windowSettings != null);
            Contract.Requires<InvalidOperationException>(IsMainWindowShown);

            throw new NotImplementedException();
        }

        public bool ShowInDialog<T>(T viewModel, WindowSettings windowSettings) 
            where T : ViewModel
        {
            Contract.Requires<ArgumentNullException>(viewModel != null);
            Contract.Requires<ArgumentNullException>(windowSettings != null);
            Contract.Requires<InvalidOperationException>(IsMainWindowShown);

            throw new NotImplementedException();
        }

        #endregion
    }
}
