using System;
using System.ComponentModel.Composition;
using System.IO;
using LayerCake.UI.Controls;
using Microsoft.Win32;
using Xceed.Wpf.Toolkit;

namespace LayerCake.UI.Services
{
    /// <summary>
    /// Реализация <see cref="IDialogService"/> по умолчанию.
    /// </summary>
    [Export(typeof(IDialogService))]
    public sealed class DialogService : IDialogService
    {
        #region Поля

        [Import]
        private IPresentationService presentationService = null;

        #endregion

        #region Private-методы

        private void InitializeFileDialog(FileDialogSettings dialogSettings, FileDialog dialog)
        {
            dialog.Title = dialogSettings.Title;
            dialog.DefaultExt = dialogSettings.DefaultExtension;
            dialog.Filter = dialogSettings.Filter;
            dialog.CheckPathExists = true;
            dialog.AddExtension = true;
            dialog.FileName = dialogSettings.FileName;
            dialog.InitialDirectory = String.IsNullOrEmpty(dialogSettings.FileName) ?
                dialogSettings.InitialDirectory : Path.GetDirectoryName(dialogSettings.FileName);

            if (String.IsNullOrEmpty(dialog.InitialDirectory))
                dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        }

        private bool ShowFileDialog(FileDialogSettings dialogSettings, FileDialog dialog)
        {
            var dialogResult = dialog.ShowDialog(presentationService.TopmostWindow);

            if (dialogResult.HasValue && dialogResult.Value)
            {
                dialogSettings.FileName = dialog.FileName;
            }

            return dialogResult.HasValue && dialogResult.Value;
        }

        #endregion

        #region Реализация IDialogService

        /// <summary>
        /// Показывает окно сообщений.
        /// </summary>
        /// <param name="dialogSettings">
        /// Параметры отображения окна сообщений.
        /// </param>
        /// <returns>
        /// Результат отображения окна сообщений из множества <see cref="System.Windows.MessageBoxResult"/>.
        /// </returns>
        public System.Windows.MessageBoxResult ShowMessageBox(MessageBoxSettings dialogSettings)
        {
            return MessageBox.Show(dialogSettings.Text, dialogSettings.Title,
                dialogSettings.Button, dialogSettings.Image, dialogSettings.DefaultResult);
        }

        /// <summary>
        /// Показывает диалог открытия файла.
        /// </summary>
        /// <param name="dialogSettings">
        /// Параметры отображения диалога.
        /// </param>
        /// <returns>
        /// <see langword="true"/>, если диалог закрыт по кнопке "ОК";
        /// <see langword="false"/>, если диалог закрыт по кнопке "Отмена", либо выполнение диалога прервано иным способом.
        /// </returns>
        public bool ShowOpenFileDialog(FileDialogSettings dialogSettings)
        {
            var dialog = new OpenFileDialog
            {
                CheckFileExists = true
            };

            InitializeFileDialog(dialogSettings, dialog);

            return ShowFileDialog(dialogSettings, dialog);
        }

        /// <summary>
        /// Показать диалог сохранения файла
        /// </summary>
        /// <param name="dialogSettings">
        /// Параметры отображения диалога.
        /// </param>
        /// <returns>
        /// <see langword="true"/>, если диалог закрыт по кнопке "ОК";
        /// <see langword="false"/>, если диалог закрыт по кнопке "Отмена", либо выполнение диалога прервано иным способом.
        /// </returns>
        public bool ShowSaveFileDialog(FileDialogSettings dialogSettings)
        {
            var dialog = new SaveFileDialog
            {
                OverwritePrompt = true
            };

            InitializeFileDialog(dialogSettings, dialog);

            return ShowFileDialog(dialogSettings, dialog);
        }

        /// <summary>
        /// Показывает диалог "О программе".
        /// </summary>
        /// <param name="viewModel">
        /// Модель представления, которая инициировала показ диалога.
        /// </param>
        /// <param name="dialogSettings">
        /// Параметры отображения диалога.
        /// </param>
        public void ShowAboutBox(AboutBoxSettings dialogSettings)
        {
            var aboutBox = new AboutBox
            {
                Owner = presentationService.TopmostWindow,
                DataContext = dialogSettings
            };

            aboutBox.ShowDialog();
        }

        #endregion
    }
}
