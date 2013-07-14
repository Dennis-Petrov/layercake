using System;
using System.Diagnostics.Contracts;
using System.Windows;
using LayerCake.UI.Services;

namespace LayerCake.UI
{
    /// <summary>
    /// Предоставляет набор методов-расширений для <see cref="ViewModel"/>.
    /// </summary>
    public static class ViewModelPresentationExtensions
    {
        #region Отображение моделей представления, системных диалогов и окон сообщений

        private static IDialogService GetDialogService(this ViewModel viewModel)
        {
            return viewModel.PartsContainer.GetExportedValue<IDialogService>();
        }

        /// <summary>
        /// Отображает модель представления в диалоге.
        /// </summary>
        /// <typeparam name="T">
        /// Тип модели представления.
        /// </typeparam>
        /// <param name="viewModel">
        /// Модель представления, которую нужно отобразить в диалоге.
        /// </param>
        /// <param name="windowSettings">
        /// Параметры отображения диалога.
        /// </param>
        /// <returns>
        /// <see langword="true"/>, если диалог закрыт по кнопке подтверждения;
        /// <see langword="false"/>, если диалог закрыт по кнопке отмены или через системное меню.
        /// </returns>
        public static bool ShowInDialog<T>(this T viewModel, WindowSettings windowSettings)
            where T : ViewModel
        {
            Contract.Requires<ArgumentNullException>(viewModel != null);

            return viewModel
                .PartsContainer
                .GetExportedValue<IPresentationService>()
                .ShowInDialog<T>(viewModel, windowSettings);
        }

        /// <summary>
        /// Показывает окно сообщений.
        /// </summary>
        /// <param name="viewModel">
        /// Модель представления, которая инициировала показ окна сообщений.
        /// </param>
        /// <param name="dialogSettings">
        /// Параметры показа окна сообщений.
        /// </param>
        /// <returns>
        /// Результат показа окна сообщений из множества <see cref="MessageBoxResult"/>.
        /// </returns>
        public static MessageBoxResult ShowMessageBox(this ViewModel viewModel, MessageBoxSettings dialogSettings)
        {
            Contract.Requires<ArgumentNullException>(viewModel != null);

            return viewModel
                .GetDialogService()
                .ShowMessageBox(dialogSettings);
        }

        /// <summary>
        /// Показывает диалог открытия файла.
        /// </summary>
        /// <param name="viewModel">
        /// Модель представления, которая инициировала показ диалога.
        /// </param>
        /// <param name="dialogSettings">
        /// Параметры отображения диалога.
        /// </param>
        /// <returns>
        /// <see langword="true"/>, если диалог закрыт по кнопке "ОК";
        /// <see langword="false"/>, если диалог закрыт по кнопке "Отмена", либо выполнение диалога прервано иным способом.
        /// </returns>
        public static bool ShowOpenFileDialog(this ViewModel viewModel, FileDialogSettings dialogSettings)
        {
            Contract.Requires<ArgumentNullException>(viewModel != null);

            return viewModel
                .GetDialogService()
                .ShowOpenFileDialog(dialogSettings);
        }

        /// <summary>
        /// Показать диалог сохранения файла
        /// </summary>
        /// <param name="viewModel">
        /// Модель представления, которая инициировала показ диалога.
        /// </param>
        /// <param name="dialogSettings">
        /// Параметры отображения диалога.
        /// </param>
        /// <returns>
        /// <see langword="true"/>, если диалог закрыт по кнопке "ОК";
        /// <see langword="false"/>, если диалог закрыт по кнопке "Отмена", либо выполнение диалога прервано иным способом.
        /// </returns>
        public static bool ShowSaveFileDialog(this ViewModel viewModel, FileDialogSettings dialogSettings)
        {
            Contract.Requires<ArgumentNullException>(viewModel != null);

            return viewModel
                .GetDialogService()
                .ShowSaveFileDialog(dialogSettings);
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
        public static void ShowAboutBox(this ViewModel viewModel, AboutBoxSettings dialogSettings)
        {
            Contract.Requires<ArgumentNullException>(viewModel != null);

            viewModel
                .GetDialogService()
                .ShowAboutBox(dialogSettings);
        }

        #endregion
    }
}
