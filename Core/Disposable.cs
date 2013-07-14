
namespace System
{
    /// <summary>
    /// Предоставляет базовую реализацию <see cref="IDisposable"/> для корректного освобождения управляемых ресурсов.
    /// </summary>
    public abstract class Disposable : IDisposable
    {
        /// <summary>
        /// Если перегружен, освобождает ресурсы, используемые объектом класса, унаследованного от <see cref="Disposable"/>.
        /// </summary>
        protected abstract void OnDispose();

        /// <summary>
        /// Возвращает <see langword="true"/>, если ресурсы, используемые этим объектом, уже освобождены, 
        /// в противном случае возвращает <see langword="false"/>.
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// Освобождает управляемые ресурсы, используемые этим экземпляром <see cref="Disposable"/>.
        /// </summary>
        public void Dispose()
        {
            if (IsDisposed)
                return;

            OnDispose();

            IsDisposed = true;
        }
    }
}
