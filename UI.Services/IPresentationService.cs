using System.Diagnostics.Contracts;
using System.Windows;
using LayerCake.UI.Services.CodeContracts;

namespace LayerCake.UI.Services
{
    /// <summary>
    /// Определяет интерфейс сервиса отображения моделей представления.
    /// </summary>
    [ContractClass(typeof(PresentationServiceContract))]
    public interface IPresentationService
    {
        /// <summary>
        /// Возвращает <see langword="true"/>, если главное окно было показано;
        /// <see langword="false"/> в противном случае.
        /// </summary>
        bool IsMainWindowShown { get; }

        /// <summary>
        /// Возвращает окно верхнего уровня.
        /// </summary>
        Window TopmostWindow { get; }

        /// <summary>
        /// Отображает модель представления в главном окне.
        /// </summary>
        /// <typeparam name="T">
        /// Тип модели представления.
        /// </typeparam>
        /// <param name="templateId">
        /// Идентификатор шаблона данных.
        /// </param>
        /// <param name="viewModel">
        /// Модель представления, которую нужно отобразить в главном окне.
        /// </param>
        /// <param name="windowSettings">
        /// Параметры отображения главного окна.
        /// </param>
        void ShowInMainWindow<T>(string templateId, T viewModel, WindowSettings windowSettings)
            where T : ViewModel;

        /// <summary>
        /// Отображает модель представления в главном окне, используя шаблон данных по умолчанию.
        /// </summary>
        /// <typeparam name="T">
        /// Тип модели представления.
        /// </typeparam>
        /// <param name="viewModel">
        /// Модель представления, которую нужно отобразить в главном окне.
        /// </param>
        /// <param name="windowSettings">
        /// Параметры отображения главного окна.
        /// </param>
        void ShowInMainWindow<T>(T viewModel, WindowSettings windowSettings)
            where T : ViewModel;

        /// <summary>
        /// Отображает модель представления в диалоге.
        /// </summary>
        /// <typeparam name="T">
        /// Тип модели представления.
        /// </typeparam>
        /// <param name="templateId">
        /// Идентификатор шаблона данных.
        /// </param>
        /// <param name="viewModel">
        /// Модель представления, которую нужно отобразить в диалоге.
        /// </param>
        /// <param name="windowSettings">
        /// Параметры отображения диалога.
        /// </param>
        /// <returns>
        /// <see langword="true"/>, если диалог закрыт по кнопке подтверждения;
        /// <see langword="false"/> в противном случае.
        /// </returns>
        bool ShowInDialog<T>(string templateId, T viewModel, WindowSettings windowSettings)
            where T : ViewModel;

        /// <summary>
        /// Отображает модель представления в диалоге, используя шаблон данных по умолчанию.
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
        /// <see langword="false"/> в противном случае.
        /// </returns>
        bool ShowInDialog<T>(T viewModel, WindowSettings windowSettings)
            where T : ViewModel;
    }
}
