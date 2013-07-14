using System.Windows;

namespace LayerCake.UI.Behaviors
{
    /// <summary>
    /// Поведение, позволяющее устанавливать фокус на элемент шаблона данных.
    /// </summary>
    public static class FocusBehavior
    {
        /// <summary>
        /// Возвращает значение <see cref="IsFocusedProperty"/>.
        /// </summary>
        /// <param name="obj">
        /// Объект, для которого нужно вернуть значение свойства.
        /// </param>
        /// <returns>
        /// Значение <see cref="IsFocusedProperty"/>.
        /// </returns>
        public static bool GetIsFocused(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsFocusedProperty);
        }

        /// <summary>
        /// Задает значение <see cref="IsFocusedProperty"/>.
        /// </summary>
        /// <param name="obj">
        /// Объект, для которого нужно задать значение свойства.
        /// </param>
        /// <param name="value">
        /// Значение <see cref="IsFocusedProperty"/>.
        /// </param>
        public static void SetIsFocused(DependencyObject obj, bool value)
        {
            obj.SetValue(IsFocusedProperty, value);
        }

        /// <summary>
        /// Позволяет устанавливать фокус на элементе шаблона данных.
        /// </summary>
        public static readonly DependencyProperty IsFocusedProperty = DependencyProperty.RegisterAttached(
            "IsFocused", 
            typeof(bool), 
            typeof(FocusBehavior),
            new UIPropertyMetadata(false, OnIsFocusedPropertyChanged));

        private static void OnIsFocusedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = d as UIElement;

            if (element != null && (bool)e.NewValue)
            {
                element.Focus();
            }
        }
    }
}
