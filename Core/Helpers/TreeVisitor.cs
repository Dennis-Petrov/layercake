using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace LayerCake.Helpers
{
    /// <summary>
    /// Предоставляет набор вспомогательных методов для работы с деревьями объектов.
    /// </summary>
    public static class TreeVisitor
    {
        /// <summary>
        /// Выполняет обход дерева в ширину.
        /// </summary>
        /// <typeparam name="TNodeType">
        /// Тип узлов в дереве.
        /// </typeparam>
        /// <param name="root">
        /// Корневой узел дерева.
        /// </param>
        /// <param name="getChildNodesFunc">
        /// Делегат, возвращающий последовательность дочерних узлов для текущего узла дерева.
        /// </param>
        /// <returns>
        /// Узлы дерева в виде последовательности <see cref="IEnumerable{TNodeType}"/>.
        /// </returns>
        public static IEnumerable<TNodeType> WidthTraversal<TNodeType>(TNodeType root, Func<TNodeType,
            IEnumerable<TNodeType>> getChildNodesFunc)
            where TNodeType : class
        {
            Contract.Requires<ArgumentNullException>(root != null);
            Contract.Requires<ArgumentNullException>(getChildNodesFunc != null);

            var visited = new HashSet<TNodeType>();
            var queue = new Queue<TNodeType>();

            // посещаем первый узел
            yield return root;
            visited.Add(root);
            queue.Enqueue(root);

            // просматриваем очередь
            while (queue.Count > 0)
            {
                var parent = queue.Dequeue();
                foreach (var child in getChildNodesFunc(parent))
                {
                    if (child == default(TNodeType))
                        continue;

                    if (!visited.Contains(child))
                    {
                        yield return child;
                        visited.Add(child);
                        queue.Enqueue(child);
                    }
                }
            }
        }
    }
}
