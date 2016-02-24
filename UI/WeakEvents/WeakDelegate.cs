using System;
using System.Diagnostics.Contracts;

namespace LayerCake.UI.WeakEvents
{
    /// <summary>
    /// Базовый класс для создания слабых ссылок на делегаты
    /// </summary>
    /// <typeparam name="TDelegate">Тип делегата</typeparam>
    [ContractClass(typeof(WeakDelegateContract<>))]
    public abstract class WeakDelegate<TDelegate> 
        where TDelegate : class
    {
        #region Поля

        private WeakReference<TDelegate> weakDelegate;
        private Action<TDelegate> removeDelegateCode;

        #endregion

        #region Конструктор

        /// <summary>
        /// Инициализирует экземпляр <see cref="WeakDelegate{TDelegate}"/>.
        /// </summary>
        /// <param name="delegate">
        /// Делегат, на который нужно создать слабую ссылку.
        /// </param>
        /// <param name="removeDelegateCode">
        /// Действие, которое нужно выполнить для удаления слабой ссылки на делегат из цепочки вызовов.
        /// </param>
        protected WeakDelegate(TDelegate @delegate, Action<TDelegate> removeDelegateCode)
        {
            Contract.Requires<ArgumentException>(((MulticastDelegate)(Object)@delegate).Target != null);
            
            // сохраняем слабую ссылку на делегат
            this.weakDelegate = new WeakReference<TDelegate>(@delegate);
            this.removeDelegateCode = removeDelegateCode;
        }
        
        #endregion

        #region Protected-методы

        /// <summary>
        /// Возвращает ссылку на делегат, выполняющий полезную работу.
        /// </summary>
        /// <returns>
        /// Ссылка на делегат, выполняющий полезную работу, если объект, частью которого являлся делегат, еще не собран сборщиком мусора;
        /// <see langword="null"/> в противном случае.
        /// </returns>
        protected TDelegate GetRealDelegate()
        {
            // если объект, частью которого является делегат, выполняющий полезную работу, 
            // еще не собран сборщиком мусора, просто возвращаем его
            TDelegate realDelegate = weakDelegate.Target;
            if (realDelegate != null) 
                return realDelegate;

            // объект уже собран сборщиком мусора;
            // можно выбросить слабую ссылку на делегат:
            weakDelegate.Dispose();

            // и удалить себя из цепочки вызовов (если способ удаления задан пользователем):
            if (removeDelegateCode != null)
            {
                removeDelegateCode(GetDelegate());
                removeDelegateCode = null;
            }

            return null;
        }

        /// <summary>
        /// Возвращает делегат, который добавляется в цепочку вызовов.
        /// </summary>
        /// <returns>
        /// Делегат, который добавляется в цепочку вызовов.
        /// </returns>
        /// <remarks>
        /// Класс-наследник должен вернуть делегат, ссылающийся на private-метод, 
        /// сигнатура которого совпадает с <typeparamref name="TDelegate"/>.
        /// </remarks>
        protected abstract TDelegate GetDelegate();

        #endregion

        #region Операторы

        /// <summary>
        /// Выполняет неявное преобразование экземпляра <see cref="WeakDelegate{TDelegate}"/> в экземпляр <typeparamref name="TDelegate"/>.
        /// </summary>
        /// <param name="delegate">
        /// Экземпляр <see cref="WeakDelegate{TDelegate}"/>.
        /// </param>
        /// <returns>
        /// Зкземпляр <typeparamref name="TDelegate"/>.
        /// </returns>
        public static implicit operator TDelegate(WeakDelegate<TDelegate> @delegate)
        {
            return @delegate.GetDelegate();
        }

        #endregion
    }
}
