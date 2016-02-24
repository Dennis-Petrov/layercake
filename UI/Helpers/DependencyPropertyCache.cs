using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace LayerCake.UI.Helpers
{
    internal static class DependencyPropertyCache
    {
        #region Поля

        private static readonly ConcurrentDictionary<Type, ConcurrentDictionary<DependencyProperty, Object>> dependencyPropertiesCache;

        #endregion

        #region Статический конструктор

        static DependencyPropertyCache()
        {
            // в качестве значения всегда добавляется null;
            // вложенный словарь используется как потокобезопасный аналог HashSet
            dependencyPropertiesCache = new ConcurrentDictionary<Type, ConcurrentDictionary<DependencyProperty, Object>>();

            #region Заполнение кэша свойств-зависимостей для известных элементов управления

            RegisterDependencyProperty<ContentControl>(ContentControl.ContentProperty);
            RegisterDependencyProperty<TextBox>(TextBox.TextProperty);
            RegisterDependencyProperty<CheckBox>(CheckBox.IsCheckedProperty);
            RegisterDependencyProperty<RadioButton>(RadioButton.IsCheckedProperty);
            RegisterDependencyProperty<ComboBox>(ComboBox.TextProperty);
            RegisterDependencyProperty<ComboBox>(ComboBox.SelectedValueProperty);

            #endregion
        }

        #endregion

        #region Public-методы

        public static Boolean RegisterDependencyProperty<T>(DependencyProperty dependencyProperty)
            where T : DependencyObject
        {
            var dependencyProperties = dependencyPropertiesCache.GetOrAdd(typeof(T), 
                t => new ConcurrentDictionary<DependencyProperty, Object>());

            return dependencyProperties.TryAdd(dependencyProperty, null);
        }

        public static IEnumerable<DependencyProperty> GetRegisteredDependencyProperties(this DependencyObject dependencyObject)
        {
            ConcurrentDictionary<DependencyProperty, Object> registeredProperties;
            
            if (dependencyPropertiesCache.TryGetValue(dependencyObject.GetType(), out registeredProperties))
                return registeredProperties.Keys;

            return Enumerable.Empty<DependencyProperty>();
        }

        #endregion
    }
}
