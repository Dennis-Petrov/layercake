using System.Diagnostics.Contracts;

namespace System
{
    /// <summary>
    /// Предоставляет набор методов-расширений для класса <see cref="Lazy{T}"/>.
    /// </summary>
    public static class LazyExtensions
    {
        /// <summary>
        /// Освобождает ресурсы, используемые значением с ленивой инициализацией, если тип значения реализует <see cref="IDisposable"/>.
        /// </summary>
        /// <typeparam name="T">
        /// Тип значения с ленивой инициализацией.
        /// </typeparam>
        /// <param name="lazy">
        /// Значение с ленивой инициализацией.
        /// </param>
        public static void DisposeValue<T>(this Lazy<T> lazy)
            where T : IDisposable
        {
            Contract.Requires<ArgumentNullException>(lazy != null);

            if (lazy.IsValueCreated)
                lazy.Value.Dispose();
        }
    }
}
