using System;

namespace LayerCake.UI.WeakEvents
{
    /// <summary>
    /// Реализация слабых ссылок на делегаты <see cref="EventHandler"/>.
    /// </summary>
    public sealed class WeakEventHandler : WeakDelegate<EventHandler>
    {
        #region Конструктор

        /// <summary>
        /// Инициализирует экземпляр <see cref="WeakEventHandler"/>.
        /// </summary>
        /// <param name="delegate">
        /// Делегат, на который нужно создать слабую ссылку.
        /// </param>
        /// <param name="removeDelegateCode">
        /// Действие, которое нужно выполнить для удаления слабой ссылки на делегат из цепочки вызовов.
        /// </param>
        public WeakEventHandler(EventHandler @delegate, Action<EventHandler> removeDelegateCode = null)
            : base(@delegate, removeDelegateCode) 
        { 
        }

        #endregion

        #region Реализация WeakDelegate<EventHandler>

        private void Callback(Object sender, EventArgs e)
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
        protected override EventHandler GetDelegate() 
        { 
            return Callback; 
        }

        #endregion
    }
}
