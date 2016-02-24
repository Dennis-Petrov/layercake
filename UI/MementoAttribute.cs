using System;

namespace LayerCake.UI
{
    /// <summary>
    /// Определяет свойство, которое изменяет значение в рамках транзакции.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class MementoAttribute : Attribute
    {
        /// <summary>
        /// Инициализирует экземпляр <see cref="MementoAttribute"/>.
        /// </summary>
        public MementoAttribute()
        {
        }
    }
}
