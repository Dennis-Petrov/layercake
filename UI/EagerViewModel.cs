using System;
using System.Diagnostics.Contracts;

namespace LayerCake.UI
{
    /// <summary>
    /// Предоставляет базовую функциональность для создания моделей представления, 
    /// которые являются обертками над заданным экземпляром доменной модели.
    /// </summary>
    /// <typeparam name="T">
    /// Тип доменной модели.
    /// </typeparam>
    public class EagerViewModel<T> : ViewModel<T>
        where T : class
    {
        #region Поля

        private readonly T model;

        #endregion

        #region Конструктор

        /// <summary>
        /// Инициализирует экземпляр <see cref="EagerViewModel{T}"/>.
        /// </summary>
        /// <param name="parent">
        /// Родительская модель представления.
        /// </param>
        /// <param name="model">
        /// Экземпляр <typeparamref name="T"/>, который нужно обернуть в эту модель представления.
        /// </param>
        public EagerViewModel(ViewModel parent, T model)
            : base(parent)
        {
            Contract.Requires<ArgumentNullException>(model != null);

            this.model = model;
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Возвращает экземпляр <typeparamref name="T"/>, который нужно обернуть в эту модель представления.
        /// </summary>
        /// <returns>
        /// Экземпляр <typeparamref name="T"/>, который нужно обернуть в эту модель представления.
        /// </returns>
        protected override T GetModel()
        {
            return model;
        }

        #endregion
    }
}
