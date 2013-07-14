using System;
using System.Diagnostics.Contracts;
using System.Windows;

namespace LayerCake.UI.Services.CodeContracts
{
    [ContractClassFor(typeof(IDialogService))]
    internal abstract class DialogServiceContract : IDialogService
    {
        #region Реализация IDialogService

        public MessageBoxResult ShowMessageBox(MessageBoxSettings dialogSettings)
        {
            Contract.Requires<ArgumentNullException>(dialogSettings != null);

            throw new NotImplementedException();
        }

        public bool ShowOpenFileDialog(FileDialogSettings dialogSettings)
        {
            Contract.Requires<ArgumentNullException>(dialogSettings != null);

            throw new NotImplementedException();
        }

        public bool ShowSaveFileDialog(FileDialogSettings dialogSettings)
        {
            Contract.Requires<ArgumentNullException>(dialogSettings != null);

            throw new NotImplementedException();
        }

        public void ShowAboutBox(AboutBoxSettings dialogSettings)
        {
            Contract.Requires<ArgumentNullException>(dialogSettings != null);
        }

        #endregion
    }
}
