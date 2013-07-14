using System;
using System.Diagnostics.Contracts;

namespace NLog
{
    /// <summary>
    /// Предоставляет набор методов-расширений для <see cref="Logger"/>.
    /// </summary>
    public static class LoggerExtensions
    {
        /// <summary>
        /// Выполняет метод, возвращающий результат, с использованием контекста логирования исключений.
        /// </summary>
        /// <typeparam name="T">
        /// Тип возвращаемого значения заданного метода.
        /// </typeparam>
        /// <param name="logger">
        /// Экземпляр <see cref="Logger"/>, представляющий контекст логирования исключений.
        /// </param>
        /// <param name="method">
        /// Метод, который нужно выполнить.
        /// </param>
        /// <returns>
        /// Результат, возвращемый методом <paramref name="method"/>.
        /// </returns>
        public static T ExecuteInContext<T>(this Logger logger, Func<T> method)
        {
            Contract.Requires<ArgumentNullException>(logger != null);
            Contract.Requires<ArgumentNullException>(method != null);

            try
            {
                return method();
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw;
            }
        }

        /// <summary>
        /// Выполняет метод, не возвращающий результат, с использованием контекста логирования исключений.
        /// </summary>
        /// <param name="logger">
        /// Экземпляр <see cref="Logger"/>, представляющий контекст логирования исключений.
        /// </param>
        /// <param name="method">
        /// Метод, который нужно выполнить.
        /// </param>
        public static void ExecuteInContext(this Logger logger, Action method)
        {
            Contract.Requires<ArgumentNullException>(method != null);

            logger.ExecuteInContext<object>(() =>
            {
                method();
                return null;
            });
        }
    }
}
