using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace LayerCake.UI.Helpers
{
    internal static class FrameworkElementExtensions
    {
        #region Private-методы

        public static void SelectElement(this FrameworkElement element)
        {
            var textBox = element as TextBoxBase;
            if (textBox != null)
                textBox.SelectAll();
            else
            {
                var listBoxItem = element as ListBoxItem;
                if (listBoxItem != null)
                    listBoxItem.IsSelected = true;
                else
                {
                    var treeViewItem = element as TreeViewItem;
                    if (treeViewItem != null)
                        treeViewItem.IsSelected = true;
                }
            }
        }

        #endregion

        #region Public-методы

        public static IEnumerable<FrameworkElement> GetFrameworkElementsTree(this FrameworkElement root,
            Boolean reversed, Boolean visualTree = true)
        {
            if (root == null)
                yield break;

            foreach (var dependencyObject in root.GetDependencyObjectsTree(reversed, visualTree))
            {
                var element = dependencyObject as FrameworkElement;
                if (element != null)
                    yield return element;
            }
        }

        public static Boolean FocusElement(this FrameworkElement element, DependencyProperty dp)
        {
            // если элемент является элементом списка, дерева либо является полем ввода текста,
            // выбираем его
            element.SelectElement();

            if (element.Focusable && element.IsVisible)
            {
                // фокус устанавливаем на текущем элементе
                return element.Focus();
            }

            // Следующий код нужен для работы с цепочками привязок вида:
            // ViewModel.Name <-> PropertyGrid.Text <-> TexBoxProperty.Text <-> TextBox.Text
            //
            // Когда ViewModel устанавливает фокус на свойстве Name, нужно пройти по цепочке привязок,
            // и в итоге установить фокус на TextBox.

            foreach (var frameworkElement in element.GetFrameworkElementsTree(false))
            {
                foreach (var dependencyProperty in frameworkElement.GetRegisteredDependencyProperties())
                {
                    // проверяем, привязано ли DP дочернего элемента к DP текущего элемента
                    var bindingExpression = frameworkElement.GetBindingExpression(dependencyProperty);

                    if (bindingExpression != null &&
                        bindingExpression.DataItem == element &&
                        bindingExpression.ParentBinding.Path.Path == dp.Name)
                    {
                        // да, привязано, вызываем метод рекурсивно
                        return frameworkElement.FocusElement(dependencyProperty);
                    }
                }
            }

            return false;
        }

        #endregion
    }
}
