
namespace System
{
    /// <summary>
    /// Данные события, содержащие единственное значение, доступное только для чтения.
    /// </summary>
    /// <typeparam name="T">
    /// Тип значения данных события.
    /// </typeparam>
    public class ReadOnlyEventArgs<T> : EventArgs
    {
        /// <summary>
        /// Инициализирует экземпляр <see cref="ReadOnlyEventArgs{T}"/>.
        /// </summary>
        /// <param name="value">
        /// Значение данных события.
        /// </param>
        public ReadOnlyEventArgs(T value)
        {
            Value = value;
        }

        /// <summary>
        /// Возвращает значение данных события.
        /// </summary>
        public T Value { get; private set; }
    }
}
