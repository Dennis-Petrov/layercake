using System;

namespace LayerCake.Communication
{
    /// <summary>
    /// Представляет базовый класс для сервисов, работающих на стороне клиента, и взаимодействующих с WCF-сервисами.
    /// </summary>
    /// <typeparam name="T">
    /// Тип контракта сервиса.
    /// </typeparam>
    public abstract class ClientSideService<T>
        where T : class
    {
        private readonly Client<T> client;

        /// <summary>
        /// Инициализирует экземпляр <see cref="ClientSideService{T}"/>.
        /// </summary>
        /// <param name="endpointConfigurationName">
        /// Имя конфигурации конечной точки.
        /// </param>
        protected ClientSideService(string endpointConfigurationName)
        {
            this.client = new Client<T>(endpointConfigurationName);
        }

        /// <summary>
        /// Выполняет метод, который возвращает значение.
        /// </summary>
        /// <typeparam name="TResult">
        /// Тип возвращаемого значения.
        /// </typeparam>
        /// <param name="method">
        /// Метод, который нужно выполнить. 
        /// </param>
        /// <returns>
        /// Результат работы метода.
        /// </returns>
        protected TResult Execute<TResult>(Func<T, TResult> method)
        {
            return client.Execute<TResult>(method);
        }

        /// <summary>
        /// Выполняет метод, который не возвращает значение.
        /// </summary>
        /// <param name="method">
        /// Метод, который нужно выполнить.
        /// </param>
        protected void Execute(Action<T> method)
        {
            client.Execute(method);
        }
    }
}
