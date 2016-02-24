using System;
using System.ComponentModel;

namespace LayerCake.UI.WeakEvents
{
    internal sealed class WeakPropertyChangedEventHandler : WeakDelegate<PropertyChangedEventHandler>
    {
        #region Конструктор

        public WeakPropertyChangedEventHandler(PropertyChangedEventHandler @delegate,
            Action<PropertyChangedEventHandler> removeDelegateCode = null) 
            : base(@delegate, removeDelegateCode) 
        { 
        }
        
        #endregion

        #region Реализация WeakDelegate<PropertyChangedEventHandler>

        private void Callback(Object sender, PropertyChangedEventArgs e)
        {
            var eventHandler = base.GetRealDelegate();
            if (eventHandler != null)
            {
                eventHandler(sender, e);
            }
        }

        protected override PropertyChangedEventHandler GetDelegate()
        {
            return Callback;
        }

        #endregion
    }
}
