using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using LayerCake.UI.Helpers;

namespace LayerCake.UI
{
    internal sealed class ObjectMemento<T> : Disposable, IMemento
        where T : INotifyPropertyChanged
    {
        #region Статические члены

        private static readonly Lazy<Dictionary<string, PropertyMapHelper>> propertyMap =
            new Lazy<Dictionary<string, PropertyMapHelper>>(CreatePropertyMap, true);

        private static IEnumerable<PropertyInfo> GetMementoProperties()
        {
            return typeof(T)
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => p.CanRead && 
                    p.GetCustomAttributes(typeof(MementoAttribute), true).Length > 0 &&
                    p.GetIndexParameters().Length == 0);
        }

        private static Type FindGenericListTypeArgument(PropertyInfo property)
        {
            var genericArguments = property.PropertyType.GetGenericArguments();

            foreach (var genericArgument in genericArguments)
            {
                var genericIList = typeof(IList<>).MakeGenericType(genericArgument);
                if (genericIList.IsAssignableFrom(property.PropertyType))
                {
                    return genericArgument;
                }
            }

            return null;
        }

        private static Dictionary<string, PropertyMapHelper> CreatePropertyMap()
        {
            var propertyMap = new Dictionary<string, PropertyMapHelper>();

            foreach (var property in GetMementoProperties())
            {
                Type mementoType = null;

                if (typeof(INotifyCollectionChanged).IsAssignableFrom(property.PropertyType) && property.PropertyType.IsGenericType)
                {
                    var collectionItemType = FindGenericListTypeArgument(property);
                    if (collectionItemType != null)
                    {
                        // свойство-коллекция, тип свойства реализует IList<T> и INotifyCollectionChanged
                        mementoType = typeof(CollectionMemento<,>)
                            .MakeGenericType(property.PropertyType, collectionItemType);
                    }
                }
                else if (typeof(INotifyPropertyChanged).IsAssignableFrom(property.PropertyType))
                {
                    // комплексное свойство, тип свойства реализует INotifyPropertyChanged
                    mementoType = typeof(ObjectMemento<>)
                        .MakeGenericType(property.PropertyType);
                }

                // конструируем делегат, который будет возвращать нужную реализацию
                // IMemento, в зависимости от типа свойства
                var mementoFactory = mementoType != null ?
                    (Func<object, IMemento>)(propertyValue => (IMemento)Activator.CreateInstance(mementoType, propertyValue)) : null;

                propertyMap.Add(property.Name, new PropertyMapHelper(property, mementoFactory));
            }

            return propertyMap;
        }

        #endregion

        #region Поля

        private readonly T originator;
        private readonly HashSet<string> modifiedProperties;
        private readonly Dictionary<PropertyMapHelper, OriginalValueHelper> originalValues;

        #endregion

        #region Конструктор

        public ObjectMemento(T originator)
        {
            this.originator = originator;
            this.originator.PropertyChanged += OnPropertyChanged;
            this.modifiedProperties = new HashSet<string>();
            this.originalValues = new Dictionary<PropertyMapHelper, OriginalValueHelper>();

            TakeSnapshot();
        }

        #endregion

        #region Private-методы

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            modifiedProperties.Add(e.PropertyName);
        }

        private void TakeSnapshot()
        {
            foreach (var mapHelper in propertyMap.Value.Values)
            {
                var propertyValue = mapHelper.Property.GetValue(originator, null);
                var memento = propertyValue != null && mapHelper.MementoFactory != null ?
                    mapHelper.MementoFactory(propertyValue) : null;

                originalValues.Add(mapHelper, new OriginalValueHelper(propertyValue, memento));
            }
        }

        #endregion

        #region Overrides

        protected override void OnDispose()
        {
            originator.PropertyChanged -= OnPropertyChanged;
            foreach (var valueHelper in originalValues.Values)
            {
                valueHelper.Dispose();
            }
        }

        #endregion

        #region Реализация IMemento

        public void Restore()
        {
            originator.PropertyChanged -= OnPropertyChanged;
            try
            {
                // восстанавливаем значения свойств
                foreach (var modifiedProperty in modifiedProperties)
                {
                    PropertyMapHelper mapHelper;

                    if (propertyMap.Value.TryGetValue(modifiedProperty, out mapHelper))
                    {
                        var valueHelper = originalValues[mapHelper];

                        mapHelper.Property.SetValue(originator, valueHelper.Value, null);
                    }
                }

                modifiedProperties.Clear();

                // откатываем изменения в значениях свойств, если это возможно
                var complexProperties = originalValues
                    .Where(kvp => kvp.Value.Memento != null);

                foreach (var kvp in complexProperties)
                {
                    kvp.Value.Memento.Restore();
                }
            }
            finally
            {
                originator.PropertyChanged += OnPropertyChanged;
            }
        }
    
        #endregion
    }
}
