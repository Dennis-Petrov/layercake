using System;
using System.Collections.Concurrent;
using System.Diagnostics.Contracts;
using System.ServiceModel;

namespace LayerCake.Communication
{
    /// <summary>
    /// Предоставляет методы для подключения к WCF-сервису.
    /// </summary>
    /// <typeparam name="T">
    /// Тип контракта WCF-сервиса.
    /// </typeparam>
    public sealed class Client<T>
        where T : class
    {
        #region Статические члены

        private static readonly ConcurrentDictionary<string, ChannelFactory<T>> channelFactories = 
            new ConcurrentDictionary<string, ChannelFactory<T>>();

        #endregion

        #region Поля

        private readonly string endpointConfigurationName;

        #endregion

        #region Конструктор

        /// <summary>
        /// Инициализирует экземпляр <see cref="Client{T}"/>.
        /// </summary>
        /// <param name="endpointConfigurationName">
        /// Имя конфигурации конечной точки.
        /// </param>
        public Client(string endpointConfigurationName)
        {
            Contract.Requires<ArgumentException>(!String.IsNullOrEmpty(endpointConfigurationName));

            this.endpointConfigurationName = endpointConfigurationName;
        }

        #endregion

        #region Public-методы

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
        public TResult Execute<TResult>(Func<T, TResult> method)
        {
            Contract.Requires<ArgumentNullException>(method != null);

            var channelFactory = channelFactories.GetOrAdd(endpointConfigurationName,
                configurationName => new ChannelFactory<T>(configurationName));

            T channel = null;
            try
            {
                channel = channelFactory.CreateChannel();

                return method(channel);
            }
            finally
            {
                if (channel != null)
                {
                    (channel as IDisposable).DisposeIfNotNull();
                }
            }
        }

        /// <summary>
        /// Выполняет метод, который не возвращает значение.
        /// </summary>
        /// <param name="method">
        /// Метод, который нужно выполнить.
        /// </param>
        public void Execute(Action<T> method)
        {
            Contract.Requires<ArgumentNullException>(method != null);

            Execute<object>(channel =>
            {
                method(channel);
                return null;
            });
        }

        #endregion
    }
}
