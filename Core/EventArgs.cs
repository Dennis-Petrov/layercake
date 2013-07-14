
namespace System
{
    /// <summary>
    /// Данные события, содержащие единственное значение.
    /// </summary>
    /// <typeparam name="T">
    /// Тип значения данных события.
    /// </typeparam>
    public class EventArgs<T> : EventArgs
    {
        /// <summary>
        /// Инициализирует экземпляр <see cref="EventArgs{T}"/>.
        /// </summary>
        /// <param name="value">
        /// Значение данных события.
        /// </param>
        public EventArgs(T value = default(T))
        {
            Value = value;
        }

        /// <summary>
        /// Возвращает или задает значение данных события.
        /// </summary>
        public T Value { get; set; }
    }
}
