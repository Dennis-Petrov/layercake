using System;

namespace LayerCake.UI.WeakEvents
{
    internal struct WeakReference<T> : IDisposable 
        where T : class
    {
        #region Поля

        private WeakReference weakReference;

        #endregion

        #region Конструктор
        
        public WeakReference(T target) 
        { 
            weakReference = new WeakReference(target); 
        }

        #endregion

        #region Public-свойства

        public T Target 
        { 
            get { return (T)weakReference.Target; } 
        }

        #endregion

        #region Реализация IDisposable

        public void Dispose() 
        { 
            weakReference = null; 
        }

        #endregion
    }
}
