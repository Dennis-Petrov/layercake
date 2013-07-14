using System;

namespace LayerCake.UI.WeakEvents
{
    /// <summary>
    /// Реализация слабых ссылок на делегаты <see cref="EventHandler{TEventArgs}"/>.
    /// </summary>
    /// <typeparam name="TEventArgs">
    /// Тип аргументов события.
    /// </typeparam>
    public sealed class WeakEventHandler<TEventArgs> : WeakDelegate<EventHandler<TEventArgs>> 
        where TEventArgs : EventArgs
    {
        #region Конструктор

        /// <summary>
        /// Создает экземпляр <see cref="WeakEventHandler{TEventArgs}"/>.
        /// </summary>
        /// <param name="delegate">
        /// Делегат, на который нужно создать слабую ссылку.
        /// </param>
        /// <param name="removeDelegateCode">
        /// Действие, которое нужно выполнить для удаления слабой ссылки на делегат из цепочки вызовов.
        /// </param>
        public WeakEventHandler(EventHandler<TEventArgs> @delegate, Action<EventHandler<TEventArgs>> removeDelegateCode = null) 
            : base(@delegate, removeDelegateCode) 
        { 
        }
        
        #endregion

        #region Реализация WeakDelegate<EventHandler<TEventArgs>>

        private void Callback(Object sender, TEventArgs e)
        {
            // если целевой объект не был собран сборщиком мусора, вызываем делегат:
            var eventHandler = base.GetRealDelegate();
            if (eventHandler != null)
            {
                eventHandler(sender, e);
            }
        }

        /// <summary>
        /// Возвращает делегат, который добавляется в цепочку вызовов.
        /// </summary>
        /// <returns>
        /// Делегат, который добавляется в цепочку вызовов.
        /// </returns>
        protected override EventHandler<TEventArgs> GetDelegate() 
        { 
            return Callback; 
        }

        #endregion
    }
}
