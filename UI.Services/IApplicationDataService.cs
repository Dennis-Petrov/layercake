using System.Diagnostics.Contracts;
using LayerCake.UI.Services.CodeContracts;

namespace LayerCake.UI.Services
{
    /// <summary>
    /// Определяет интерфейс сервиса данных уровня приложения.
    /// </summary>
    /// <typeparam name="TModel">
    /// Тип доменной модели приложения.
    /// </typeparam>
    [ContractClass(typeof(ApplicationDataServiceContract<>))]
    public interface IApplicationDataService<TModel>
        where TModel : class
    {
        /// <summary>
        /// Выполняет загрузку доменной модели приложения.
        /// </summary>
        /// <returns>
        /// Экземпляр доменной модели приложения.
        /// </returns>
        TModel LoadApplicationModel();

        /// <summary>
        /// Выполняет загрузку параметров отображения главного окна приложения.
        /// </summary>
        /// <returns>
        /// Параметры отображения главного окна приложения.
        /// </returns>
        WindowSettings LoadMainWindowSettings();

        /// <summary>
        /// Выполняет сохранение доменной модели приложения.
        /// </summary>
        /// <param name="instance">
        /// Экземпляр доменной модели приложения.
        /// </param>
        void SaveApplicationModel(TModel instance);

        /// <summary>
        /// Выполняет сохранение параметров отображения главного окна приложения.
        /// </summary>
        /// <param name="instance">
        /// Экземпляр параметров отображения главного окна приложения.
        /// </param>
        void SaveMainWindowSettings(WindowSettings instance);
    }
}
