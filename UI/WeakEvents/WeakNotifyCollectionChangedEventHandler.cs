using System;
using System.Collections.Specialized;

namespace LayerCake.UI.WeakEvents
{
    internal sealed class WeakNotifyCollectionChangedEventHandler : WeakDelegate<NotifyCollectionChangedEventHandler>
    {
        #region Конструктор

        public WeakNotifyCollectionChangedEventHandler(NotifyCollectionChangedEventHandler @delegate,
            Action<NotifyCollectionChangedEventHandler> removeDelegateCode = null) 
            : base(@delegate, removeDelegateCode) 
        { 
        }
        
        #endregion

        #region Реализация WeakDelegate<NotifyCollectionChangedEventHandler>

        private void Callback(Object sender, NotifyCollectionChangedEventArgs e)
        {
            var eventHandler = base.GetRealDelegate();
            if (eventHandler != null)
            {
                eventHandler(sender, e);
            }
        }

        protected override NotifyCollectionChangedEventHandler GetDelegate()
        {
            return Callback;
        }

        #endregion
    }
}
