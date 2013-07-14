using System.Diagnostics.Contracts;
using System.Windows;
using LayerCake.UI.Services.CodeContracts;

namespace LayerCake.UI.Services
{
    /// <summary>
    /// Определяет интерфейс сервиса для сопоставления шаблонов данных моделям представления.
    /// </summary>
    [ContractClass(typeof(DataTemplateRegistryServiceContract))]
    public interface IDataTemplateRegistryService
    {
        /// <summary>
        /// Ищет шаблон данных для заданного типа модели предстваления по идентификатору шаблона.
        /// </summary>
        /// <typeparam name="T">
        /// Тип модели представления.
        /// </typeparam>
        /// <param name="id">
        /// Идентификатор шаблона данных.
        /// </param>
        /// <returns>
        /// Экземпляр <see cref="DataTemplate"/>, представляющий шаблон данных для заданного типа.
        /// </returns>
        DataTemplate FindDataTemplate<T>(string id)
            where T : ViewModel;
    }
}
