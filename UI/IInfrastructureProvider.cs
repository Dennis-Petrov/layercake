using System;
using System.Diagnostics.Contracts;

namespace LayerCake.UI
{
    /// <summary>
    /// Определяет интерфейс провайдера инфраструктуры приложения.
    /// </summary>
    [ContractClass(typeof(InfrastructureProviderContract))]
    public interface IInfrastructureProvider
    {
        /// <summary>
        /// Возвращает или задает признак того, является ли этот объект занятым в настоящий момент времени.
        /// </summary>
        Boolean IsBusy { get; set; }

        /// <summary>
        /// Возвращает экземпляр <see cref="ViewModelStateMessagesContainer"/>, представляющий контейнер сообщений 
        /// о состоянии моделей представления.
        /// </summary>
        ViewModelStateMessagesContainer StateMessagesContainer { get; }
    }
}
