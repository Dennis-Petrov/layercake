using System;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics.Contracts;

namespace LayerCake.Helpers
{
    /// <summary>
    /// Предоставляет набор вспомогательных методов для выполнения асинхронных операций.
    /// </summary>
    public static class AsyncOperation
    {
        /// <summary>
        /// Выполняет асинхронную операцию, не возвращающую результат.
        /// </summary>
        /// <param name="content">
        /// Делегат <see cref="Action"/>, определяющий суть асинхронной операции.
        /// </param>
        /// <param name="faulted">
        /// Делегат <see cref="Action{AggregateException}"/>, вызываемый в случае, если асинхронная операция завершилась с ошибкой.
        /// </param>
        /// <param name="useCurrentSynchronizationContext">
        /// <see langword="true"/>, если для вызова <paramref name="faulted"/> используется текущий контекст синхронизации;
        /// <see langword="false"/> в противном случае.
        /// </param>
        public static void Start(Action content, Action<AggregateException> faulted, bool useCurrentSynchronizationContext = true)
        {
            Start(content, null, faulted, useCurrentSynchronizationContext);
        }

        /// <summary>
        /// Выполняет асинхронную операцию, не возвращающую результат.
        /// </summary>
        /// <param name="content">
        /// Делегат <see cref="Action"/>, определяющий суть асинхронной операции.
        /// </param>
        /// <param name="completed">
        /// Делегат <see cref="Action"/>, вызываемый в случае, если асинхронная операция завершилась успешно.
        /// </param>
        /// <param name="faulted">
        /// Делегат <see cref="Action{AggregateException}"/>, вызываемый в случае, если асинхронная операция завершилась с ошибкой.
        /// </param>
        /// <param name="useCurrentSynchronizationContext">
        /// <see langword="true"/>, если для вызова <paramref name="completed"/> и <paramref name="faulted"/> используется 
        /// текущий контекст синхронизации; <see langword="false"/> в противном случае.
        /// </param>
        public static void Start(Action content, Action completed, Action<AggregateException> faulted, 
            bool useCurrentSynchronizationContext = true)
        {
            Contract.Requires<ArgumentNullException>(content != null);
            Contract.Requires<ArgumentNullException>(faulted != null);

            var task = Task
                .Factory
                .StartNew(() => content());

            var scheduler = useCurrentSynchronizationContext ?
                TaskScheduler.FromCurrentSynchronizationContext() : TaskScheduler.Default;

            task
                .ContinueWith(t => faulted(t.Exception), CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, scheduler);

            if (completed != null)
            {
                task
                    .ContinueWith(t => completed(), CancellationToken.None, TaskContinuationOptions.OnlyOnRanToCompletion, scheduler);
            }
        }

        /// <summary>
        /// Выполняет асинхронную операцию, возвращающую результат.
        /// </summary>
        /// <typeparam name="TResult">
        /// Тип результата асинхронной операции.
        /// </typeparam>
        /// <param name="content">
        /// Делегат <see cref="Func{TResult}"/>, определяющий суть асинхронной операции.
        /// </param>
        /// <param name="completed">
        /// Делегат <see cref="Action{TResult}"/>, вызываемый в случае, если асинхронная операция завершилась успешно.
        /// </param>
        /// <param name="faulted">
        /// Делегат <see cref="Action{AggregateException}"/>, вызываемый в случае, если асинхронная операция завершилась с ошибкой.
        /// </param>
        /// <param name="useCurrentSynchronizationContext">
        /// <see langword="true"/>, если для вызова <paramref name="completed"/> и <paramref name="faulted"/> используется 
        /// текущий контекст синхронизации; <see langword="false"/> в противном случае.
        /// </param>
        public static void Start<TResult>(Func<TResult> content, Action<TResult> completed, Action<AggregateException> faulted,
            bool useCurrentSynchronizationContext = true)
        {
            Contract.Requires<ArgumentNullException>(content != null);
            Contract.Requires<ArgumentNullException>(faulted != null);

            var task = Task
                .Factory
                .StartNew(() => content());

            var scheduler = useCurrentSynchronizationContext ?
                TaskScheduler.FromCurrentSynchronizationContext() : TaskScheduler.Default;

            task
                .ContinueWith(t => faulted(t.Exception), CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, scheduler);

            if (completed != null)
            {
                task
                    .ContinueWith(t => completed(t.Result), CancellationToken.None, TaskContinuationOptions.OnlyOnRanToCompletion, scheduler);
            }
        }
    }
}
