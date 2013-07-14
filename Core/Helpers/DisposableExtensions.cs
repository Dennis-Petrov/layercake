
namespace System
{
    /// <summary>
    /// Предоставляет набор методов-расширений для <see cref="IDisposable"/>.
    /// </summary>
    public static class DisposableExtensions
    {
        /// <summary>
        /// Выполняет вызов <see cref="IDisposable.Dispose"/> у аргумента <paramref name="disposable"/>, 
        /// если он не равен <see langword="null"/>.
        /// </summary>
        /// <param name="disposable"></param>
        public static void DisposeIfNotNull(this IDisposable disposable)
        {
            if (disposable != null)
                disposable.Dispose();
        }
    }
}
