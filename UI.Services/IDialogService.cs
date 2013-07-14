using System.Diagnostics.Contracts;
using System.Windows;
using LayerCake.UI.Services.CodeContracts;

namespace LayerCake.UI.Services
{
    /// <summary>
    /// Определяет интерфейс сервиса для отображения системных диалогов.
    /// </summary>
    [ContractClass(typeof(DialogServiceContract))]
    public interface IDialogService
    {
        /// <summary>
        /// Показывает окно сообщений.
        /// </summary>
        /// <param name="dialogSettings">
        /// Параметры отображения окна сообщений.
        /// </param>
        /// <returns>
        /// Результат отображения окна сообщений из множества <see cref="MessageBoxResult"/>.
        /// </returns>
        MessageBoxResult ShowMessageBox(MessageBoxSettings dialogSettings);

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
        bool ShowOpenFileDialog(FileDialogSettings dialogSettings);

        /// <summary>
        /// Показывает диалог сохранения файла.
        /// </summary>
        /// <param name="dialogSettings">
        /// Параметры отображения диалога.
        /// </param>
        /// <returns>
        /// <see langword="true"/>, если диалог закрыт по кнопке "ОК";
        /// <see langword="false"/>, если диалог закрыт по кнопке "Отмена", либо выполнение диалога прервано иным способом.
        /// </returns>
        bool ShowSaveFileDialog(FileDialogSettings dialogSettings);

        /// <summary>
        /// Показывает диалог "О программе".
        /// </summary>
        /// <param name="dialogSettings">
        /// Параметры отображения диалога.
        /// </param>
        void ShowAboutBox(AboutBoxSettings dialogSettings);
    }
}
