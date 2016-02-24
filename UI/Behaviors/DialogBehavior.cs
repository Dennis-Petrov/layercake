using System;
using System.Diagnostics.Contracts;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using LayerCake.UI.Commands;
using LayerCake.UI.Helpers;

namespace LayerCake.UI.Behaviors
{
    /// <summary>
    /// Поведение, реализующее функциональность диалога в модели представления
    /// </summary>
    public static class DialogBehavior
    {
        #region Присоединенное свойство, позволяющее установить результат работы диалога из модели представления

        /// <summary>
        /// Get-метод для присоединенного свойства <see cref="DialogResultProperty"/>.
        /// </summary>
        /// <param name="obj">
        /// <see cref="DependencyObject"/>, к которому присоединено свойство.
        /// </param>
        /// <returns>
        /// Значение присоединенного свойства.
        /// </returns>
        public static Nullable<Boolean> GetDialogResult(DependencyObject obj)
        {
            Contract.Requires<ArgumentNullException>(obj != null);

            return (Nullable<Boolean>)obj.GetValue(DialogResultProperty);
        }

        /// <summary>
        /// Set-метод для присоединенного свойства <see cref="DialogResultProperty"/>.
        /// </summary>
        /// <param name="obj">
        /// <see cref="DependencyObject"/>, к которому присоединено свойство.
        /// </param>
        /// <param name="value">
        /// Значение присоединенного свойства.
        /// </param>
        public static void SetDialogResult(DependencyObject obj, Nullable<Boolean> value)
        {
            Contract.Requires<ArgumentNullException>(obj != null);

            obj.SetValue(DialogResultProperty, value);
        }

        /// <summary>
        /// Присоединенное свойство-результат работы диалога.
        /// </summary>
        public static readonly DependencyProperty DialogResultProperty = DependencyProperty.RegisterAttached(
            "DialogResult",
            typeof(Nullable<Boolean>),
            typeof(DialogBehavior),
            new PropertyMetadata(null, DialogResultPropertyChanged));

        private static void DialogResultPropertyChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var window = target as Window;
            if (window == null || !window.IsVisible)
                // свойство имеет смысл только для видимых окон
                return;

            var newValue = (Nullable<Boolean>)e.NewValue;
            if (newValue.HasValue)
            {
                // устанавливаем результат диалога
                window.DialogResult = newValue;
            }
        }

        #endregion

        #region Присоединенное свойство-команда, для выполнения которой требуется принудительная валидация данных

        /// <summary>
        /// Get-метод для присоединенного свойства <see cref="CommandProperty"/>.
        /// </summary>
        /// <param name="obj">
        /// <see cref="DependencyObject"/>, к которому присоединено свойство.
        /// </param>
        /// <returns>
        /// Значение присоединенного свойства.
        /// </returns>
        public static ICommand GetCommand(DependencyObject obj)
        {
            Contract.Requires<ArgumentNullException>(obj != null);

            return (ICommand)obj.GetValue(CommandProperty);
        }

        /// <summary>
        /// Set-метод для присоединенного свойства <see cref="CommandProperty"/>.
        /// </summary>
        /// <param name="obj">
        /// <see cref="DependencyObject"/>, к которому присоединено свойство.
        /// </param>
        /// <param name="value">
        /// Значение присоединенного свойства.
        /// </param>
        public static void SetCommand(DependencyObject obj, ICommand value)
        {
            Contract.Requires<ArgumentNullException>(obj != null);

            obj.SetValue(CommandProperty, value);
        }

        /// <summary>
        /// Присоединенное свойство-команда, для выполнения которой требуется предварительная валидация данных.
        /// </summary>
        public static readonly DependencyProperty CommandProperty = DependencyProperty.RegisterAttached(
            "Command",
            typeof(ICommand),
            typeof(DialogBehavior),
            new PropertyMetadata(CommandPropertyChanged));

        private static void CommandPropertyChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var button = target as ButtonBase;
            if (button != null)
            {
                if (e.NewValue != null)
                {
                    // создаем команду-прокси, в качестве параметра у нее указываем 
                    // тот же элемент, к которому присоединяем команду;
                    // команда-прокси нужна для того, чтобы корректно обновлять состояние 
                    // элемента после генерации события CommandManager.RequerySuggested
                    button.CommandParameter = button;
                    button.Command = CreateCommandProxy();
                }
                else
                {
                    button.Command = null;
                    button.CommandParameter = null;
                }
            }
            else
            {
                var menuItem = target as MenuItem;
                if (menuItem != null)
                {
                    if (e.NewValue != null)
                    {
                        menuItem.CommandParameter = menuItem;
                        menuItem.Command = CreateCommandProxy();
                    }
                    else
                    {
                        menuItem.Command = null;
                        menuItem.CommandParameter = null;
                    }
                }
            }
        }

        private static RelayCommand<DependencyObject> CreateCommandProxy()
        {
            return new RelayCommand<DependencyObject>(CommandProxyExecute, target => GetCommand(target).CanExecute(null));
        }

        private static void CommandProxyExecute(DependencyObject target)
        {
            // выполняем принудительную валидацию данных
            if (UpdateBindings(target.GetVisualRoot() as FrameworkElement))
                GetCommand(target).Execute(null);
        }

        private static Boolean UpdateBindings(FrameworkElement root)
        {
            var validationSucceess = true;

            // деревое элементов просматривается в реверсивном порядке,
            // это необходимо для корректного обновления источников данных в привязках в случае цепочек
            // вида "ViewModel.Name <-> PropertyGrid.Text <-> TexBoxProperty.Text <-> TextBox.Text"
            foreach (var frameworkElement in root.GetFrameworkElementsTree(true))
            {
                // достаем из кэша свойств-зависимостей свойства, определенные для типа текущего элемента
                foreach (var dependencyProperty in frameworkElement.GetRegisteredDependencyProperties())
                {
                    var bindingExpression = frameworkElement.GetBindingExpression(dependencyProperty);
                    if (bindingExpression != null)
                    {
                        // если текущее свойство привязано к модели представления, обновляем данные в модели представления;
                        // в результате принудительного обновления данных будет выполнена валидация значения свойства
                        bindingExpression.UpdateSource();

                        if (bindingExpression.HasError && validationSucceess)
                        {
                            validationSucceess = false;

                            // элемент, связанный с этим свойством, должен получить фокус ввода
                            frameworkElement.FocusElement(dependencyProperty);
                        }
                    }
                }
            }

            return validationSucceess;
        }

        #endregion
    }
}
