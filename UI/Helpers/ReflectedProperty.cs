using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LayerCake.UI.Properties;

namespace LayerCake.UI.Helpers
{
    internal sealed class ReflectedProperty
    {
        #region Статические члены

        private static readonly ConcurrentDictionary<Type, ConcurrentDictionary<String, ReflectedProperty>> propertiesCache =
            new ConcurrentDictionary<Type, ConcurrentDictionary<String, ReflectedProperty>>();
        private static readonly ConcurrentDictionary<Type, Type> getPropertiesCalledCache = 
            new ConcurrentDictionary<Type,Type>();

        public static ReflectedProperty GetProperty(Type type, String propertyName)
        {
            var typeProperties = propertiesCache.GetOrAdd(type, t => new ConcurrentDictionary<String, ReflectedProperty>());
            return typeProperties.GetOrAdd(propertyName, pn => new ReflectedProperty(type, pn));
        }

        public static IEnumerable<ReflectedProperty> GetEditableProperties(Type type)
        {
            return type
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => p.CanRead && p.CanWrite)
                .Select(p => new ReflectedProperty(p));
        }

        #endregion

        #region Поля

        private readonly PropertyInfo propertyInfo;
        private readonly Lazy<MethodInfo> getter;
        private readonly Lazy<MethodInfo> setter;
        private readonly Dictionary<Type, Object[]> customAttributesCache;

        #endregion

        #region Конструкторы

        private ReflectedProperty(PropertyInfo propertyInfo)
        {
            this.propertyInfo = propertyInfo;
            this.getter = new Lazy<MethodInfo>(() => propertyInfo.GetGetMethod());
            this.setter = new Lazy<MethodInfo>(() => propertyInfo.GetSetMethod());
            this.customAttributesCache = new Dictionary<Type, Object[]>();
        }

        private ReflectedProperty(Type targetType, String propertyName, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public)
            : this(targetType.GetProperty(propertyName, bindingFlags))
        {
        }

        #endregion

        #region Public-свойства

        public String Name
        {
            get { return propertyInfo.Name; }
        }

        #endregion

        #region Public-методы

        public IEnumerable<T> GetCustomAttributes<T>()
            where T : Attribute
        {
            var attributeType = typeof(T);

            Object[] customAttributes;

            if (!customAttributesCache.TryGetValue(attributeType, out customAttributes))
            {
                customAttributes = propertyInfo.GetCustomAttributes(attributeType, true);
                customAttributesCache.Add(attributeType, customAttributes);
            }

            return customAttributes
                .Cast<T>();
        }

        public Object Get(Object target)
        {
            if (getter.Value == null)
                throw new InvalidOperationException(String.Format(Resources.RP_PropertyCannotBeRead, propertyInfo.DeclaringType, propertyInfo.Name));

            return getter.Value.Invoke(target, null);
        }

        public void Set(Object target, Object value)
        {
            if (setter == null)
                throw new InvalidOperationException(String.Format(Resources.RP_PropertyCannotBeWritten, propertyInfo.DeclaringType, propertyInfo.Name));

            setter.Value.Invoke(target, new[] { value });
        }
        #endregion
    }
}
