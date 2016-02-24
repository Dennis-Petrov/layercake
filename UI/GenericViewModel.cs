using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Windows.Input;
using LayerCake.UI.WeakEvents;

namespace LayerCake.UI
{
    /// <summary>
    /// Предоставляет базовую функциональность для создания моделей представления, 
    /// которые являются обертками над экземпляром доменной модели.
    /// </summary>
    /// <typeparam name="T">
    /// Тип доменной модели.
    /// </typeparam>
    [ContractClass(typeof(ViewModelContract<>))]
    public abstract class ViewModel<T> : ViewModel
        where T : class
    {
        #region Конструктор

        /// <summary>
        /// Инициализирует экземпляр <see cref="ViewModel{T}"/>.
        /// </summary>
        /// <param name="parent">
        /// Родительская модель представления.
        /// </param>
        protected ViewModel(ViewModel parent)
            : base(parent)
        {
            this.model = new Lazy<T>(InitializeModel);
        }

        #endregion

        #region Private-методы

        private T InitializeModel()
        {
            var instance = GetModel();

            var notificationObject = instance as INotifyPropertyChanged;
            if (notificationObject != null)
            {
                notificationObject.PropertyChanged += new WeakPropertyChangedEventHandler(OnModelPropertyChanged,
                    handler => notificationObject.PropertyChanged -= handler);
            }

            return instance;
        }

        private void OnModelPropertyChanged(Object sender, PropertyChangedEventArgs args)
        {
            CommandManager.InvalidateRequerySuggested();
        }

        #endregion

        #region Protected-методы

        /// <summary>
        /// Возвращает экземпляр <typeparamref name="T"/>, который нужно обернуть в эту модель представления.
        /// </summary>
        /// <returns>
        /// Экземпляр <typeparamref name="T"/>, который нужно обернуть в эту модель представления.
        /// </returns>
        protected abstract T GetModel();

        #endregion

        #region Public-свойства

        /// <summary>
        /// Возвращает экземпляр <typeparamref name="T"/>, который обернут в эту модель представления.
        /// </summary>
        public T Model
        {
            get { return model.Value; }
        }
        private readonly Lazy<T> model;

        #endregion
    }
}
