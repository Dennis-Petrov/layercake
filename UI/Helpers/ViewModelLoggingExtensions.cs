using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.ServiceModel;
using System.ServiceModel.Security;
using LayerCake.UI.Properties;
using NLog;

namespace LayerCake.UI
{
    /// <summary>
    /// Предоставляет набор методов-расширений для <see cref="ViewModel"/>.
    /// </summary>
    public static class ViewModelLoggingExtensions
    {
        #region Логирование исключений

        private static readonly Dictionary<Type, string> serviceExceptionMessages = new Dictionary<Type, string>
        {
            { typeof(MessageSecurityException), Resources.VME_MessageSecurityException },
            { typeof(SecurityAccessDeniedException), Resources.VME_AccessDeniedException },
            { typeof(EndpointNotFoundException), Resources.VME_EndpointNotFoundException }
        };

        private static void LogAggregateException(this ViewModel source, ViewModelStateMessagesContainer container,
            Logger logger, AggregateException aggregateException, string context, string message = null)
        {
            // пишем исключение в лог
            foreach (var exception in aggregateException.InnerExceptions)
            {
                logger.Debug(exception);

                string specialErrorMessage = null;

                if (serviceExceptionMessages.TryGetValue(exception.GetType(), out specialErrorMessage))
                {
                    // исключение из числа специально обрабатываемых
                    message = specialErrorMessage;
                    break;
                }
            }

            if (string.IsNullOrEmpty(message))
                message = aggregateException.InnerExceptions[0].Message;

            container.AddMessage(ViewModelStateMessageType.Error, source, context, message);
        }

        /// <summary>
        /// Регистрирует исключение, при логировании которого в коллекцию ошибок целевого объекта 
        /// добавляется предопределенное сообщение об ошибке.
        /// </summary>
        /// <typeparam name="TException">
        /// Тип исключения.
        /// </typeparam>
        /// <param name="message">
        /// Сообщение об ошибке.
        /// </param>
        public static void RegisterSpecialException<TException>(string message)
        {
            Contract.Requires<ArgumentException>(!String.IsNullOrEmpty(message));

            serviceExceptionMessages.Add(typeof(TException), message);
        }

        /// <summary>
        /// Выполняет логирование <see cref="AggregateException"/> и добавляет ошибку в коллекцию
        /// ошибок текущего <see cref="IInfrastructureProvider"/>.
        /// </summary>
        /// <typeparam name="TInfrastructureProvider">
        /// Текущая реализация <see cref="IInfrastructureProvider"/>.
        /// </typeparam>
        /// <param name="source">
        /// Модель представления, в которой произошла ошибка.
        /// </param>
        /// <param name="logger">
        /// Логгер.
        /// </param>
        /// <param name="aggregateException">
        /// Экземпляр <see cref="AggregateException"/>, логирование которого нужно выполнить.
        /// </param>
        /// <param name="context">
        /// Наименование контекста ошибки.
        /// </param>
        /// <param name="message">
        /// Сообщение об ошибке. Если этот параметр равен <see langword="null"/> или <see cref="String.Empty"/>,
        /// то в качестве сообщения об ошибке используется сообщение первого исключения из коллекции <see cref="AggregateException.InnerExceptions"/>.
        /// </param>
        public static void LogAggregateException<TInfrastructureProvider>(this ViewModel source, Logger logger,
            AggregateException aggregateException, string context, string message = null)
            where TInfrastructureProvider : IInfrastructureProvider
        {
            Contract.Requires<ArgumentNullException>(source != null);
            Contract.Requires<ArgumentNullException>(logger != null);
            Contract.Requires<ArgumentNullException>(aggregateException != null);
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(context));

            var provider = source.PartsContainer.GetExportedValue<TInfrastructureProvider>();

            source.LogAggregateException(provider.StateMessagesContainer, logger, aggregateException, context, message);
        }

        /// <summary>
        /// Выполняет логирование <see cref="AggregateException"/> и добавляет ошибку в коллекцию
        /// ошибок модели представления.
        /// </summary>
        /// <param name="source">
        /// Модель представления, в которой произошла ошибка.
        /// </param>
        /// <param name="logger">
        /// Логгер.
        /// </param>
        /// <param name="aggregateException">
        /// Экземпляр <see cref="AggregateException"/>, логирование которого нужно выполнить.
        /// </param>
        /// <param name="context">
        /// Наименование контекста ошибки.
        /// </param>
        /// <param name="message">
        /// Сообщение об ошибке. Если этот параметр равен <see langword="null"/> или <see cref="String.Empty"/>,
        /// то в качестве сообщения об ошибке используется сообщение первого исключения из коллекции <see cref="AggregateException.InnerExceptions"/>.
        /// </param>
        public static void LogAggregateException(this ViewModel source, Logger logger,
            AggregateException aggregateException, string context, string message = null)
        {
            Contract.Requires<ArgumentNullException>(source != null);
            Contract.Requires<ArgumentNullException>(logger != null);
            Contract.Requires<ArgumentNullException>(aggregateException != null);
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(context));

            source.LogAggregateException(source.StateMessagesContainer, logger, aggregateException, context, message);
        }

        #endregion
    }
}
