using System.Linq;
using System.Diagnostics.Contracts;

namespace System.Collections.Generic
{
    /// <summary>
    /// Предоставляет методы-расширения для коллекций.
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// Добавляет диапазон элементов в коллекцию.
        /// </summary>
        /// <typeparam name="T">
        /// Тип элементов коллекции.
        /// </typeparam>
        /// <param name="collection">
        /// Коллекция, в которую нужно добавить диапазон элементов.
        /// </param>
        /// <param name="range">
        /// Диапазон элементов, которые нужно добавить в коллекцию.
        /// </param>
        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> range)
        {
            Contract.Requires<ArgumentNullException>(collection != null);
            Contract.Requires<ArgumentNullException>(range != null);

            foreach (var item in range)
            {
                collection.Add(item);
            }
        }

        /// <summary>
        /// Заменяет содержимое коллекции на новое, копируя элементы из заданной последовательности.
        /// </summary>
        /// <typeparam name="T">
        /// Тип элементов коллекции.
        /// </typeparam>
        /// <param name="collection">
        /// Коллекция, содержимое которой нужно заменить.
        /// </param>
        /// <param name="newContent">
        /// Последовательность <typeparamref name="T"/>, представляющая новое содержимое коллекции.
        /// </param>
        public static void RelpaceContent<T>(this ICollection<T> collection, IEnumerable<T> newContent)
        {
            Contract.Requires<ArgumentNullException>(collection != null);
            Contract.Requires<ArgumentNullException>(newContent != null);

            collection.Clear();
            collection.AddRange(newContent);
        }

        /// <summary>
        /// Выполняет удаление элементов из коллекции по заданному условию.
        /// </summary>
        /// <typeparam name="T">
        /// Тип элементов коллекции.
        /// </typeparam>
        /// <param name="collection">
        /// Коллекция, из которой нужно удалить элементы.
        /// </param>
        /// <param name="condition">
        /// Условие для удаления элементов.
        /// </param>
        public static void RemoveWhere<T>(this ICollection<T> collection, Func<T, Boolean> condition)
        {
            Contract.Requires<ArgumentNullException>(collection != null);
            Contract.Requires<ArgumentNullException>(condition != null);

            var itemsToRemove = collection.Where(condition).ToArray();

            foreach (var item in itemsToRemove)
            {
                collection.Remove(item);
            }
        }
    }
}
