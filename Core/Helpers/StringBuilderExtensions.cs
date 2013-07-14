using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace System.Text
{
    /// <summary>
    /// Предоставляет набор методов-расишрений для класса <see cref="StringBuilder"/>.
    /// </summary>
    public static class StringBuilderExtensions
    {
        /// <summary>
        /// Объединяет элементы последовательности <see cref="IEnumerable{T}"/> в строку, 
        /// используя заданную строку-разделитель между каждым элементом последовательности, и добавляет полученную строку к 
        /// экземпляру <see cref="StringBuilder"/>.
        /// </summary>
        /// <typeparam name="T">
        /// Тип элементов в последовательности.
        /// </typeparam>
        /// <param name="value">
        /// Экземпляр <see cref="StringBuilder"/>.
        /// </param>
        /// <param name="separator">
        /// Строка-разделитель.
        /// </param>
        /// <param name="sequence">
        /// Последовательность <see cref="IEnumerable{T}"/>.
        /// </param>
        /// <returns>
        /// Экземпляр <see cref="StringBuilder"/>, к которому добавлена полученная строка.
        /// </returns>
        public static StringBuilder AppendJoined<T>(this StringBuilder value, IEnumerable<T> sequence, string separator = ",")
        {
            Contract.Requires<ArgumentNullException>(value != null);
            Contract.Requires<ArgumentNullException>(sequence != null);

            return value.Append(String.Join(separator, sequence));
        }
    }
}
