using System;
using System.Reflection;

namespace LayerCake.UI.Helpers
{
    internal sealed class PropertyMapHelper
    {
        #region Конструктор
        
        public PropertyMapHelper(PropertyInfo property, Func<object, IMemento> mementoFactory)
        {
            this.Property = property;
            this.MementoFactory = mementoFactory;
        }

        #endregion

        #region Public-свойства

        public PropertyInfo Property { get; private set; }
        public Func<object, IMemento> MementoFactory { get; private set; }

        #endregion
    }
}
