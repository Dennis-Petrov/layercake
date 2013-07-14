using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using LayerCake.Helpers;

namespace LayerCake.UI.Helpers
{
    internal static class DependencyObjectExtensions
    {
        #region Private-методы

        private static IEnumerable<DependencyObject> GetVisualChildren(DependencyObject root)
        {
            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(root); i++)
            {
                yield return VisualTreeHelper.GetChild(root, i);
            }
        }

        private static IEnumerable<DependencyObject> GetLogicalChildren(DependencyObject root)
        {
            foreach (var child in LogicalTreeHelper.GetChildren(root))
            {
                var dependencyChild = child as DependencyObject;
                if (dependencyChild != null)
                    yield return dependencyChild;
            }
        }

        #endregion

        #region Public-методы

        public static DependencyObject GetVisualRoot(this DependencyObject current)
        {
            DependencyObject parent;
            do
            {
                parent = VisualTreeHelper.GetParent(current);
                if (parent != null)
                    current = parent;
            }
            while (parent != null);

            return current;
        }

        public static IEnumerable<DependencyObject> GetDependencyObjectsTree(this DependencyObject root,
            Boolean reversed, Boolean visualTree)
        {
            Func<DependencyObject, IEnumerable<DependencyObject>> getChildNodesFunc;

            if (visualTree)
                getChildNodesFunc = GetVisualChildren;
            else
                getChildNodesFunc = GetLogicalChildren;

            var dependencyObjects = TreeVisitor.WidthTraversal<DependencyObject>(root, getChildNodesFunc);

            return reversed ? dependencyObjects.Reverse() : dependencyObjects;
        }

        #endregion
    }
}
