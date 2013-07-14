using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;

namespace LayerCake.UI
{
    /// <summary>
    /// Реализация <see cref="IEditableObject"/> для обеспечения транзакционного редактирования состояния объектов.
    /// </summary>
    /// <typeparam name="T">
    /// Тип объекта, состояние которого может быть отредактировано.
    /// </typeparam>
    public sealed class Caretaker<T> : Disposable, IEditableObject
        where T : class, INotifyPropertyChanged
    {
        #region Поля

        private readonly T originator;
        private ObjectMemento<T> memento;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует экземпляр <see cref="Caretaker{T}"/> и начинает редактирование состояния объекта.
        /// </summary>
        /// <param name="originator"></param>
        public Caretaker(T originator)
            : this(originator, true)
        {
        }

        /// <summary>
        /// Инициализирует экземпляр <see cref="Caretaker{T}"/>.
        /// </summary>
        /// <param name="originator">
        /// Целевой объект, состояние которого может быть отредактировано.
        /// </param>
        /// <param name="beginEdit">
        /// <see langword="true"/> для начала редактирования состояния объекта;
        /// <see langword="true"/>, если редактирование состояния будет начато вручную.
        /// </param>
        public Caretaker(T originator, Boolean beginEdit)
        {
            Contract.Requires<ArgumentNullException>(originator != null);

            this.originator = originator;

            if (beginEdit)
                BeginEdit();
        }

        #endregion

        #region Реализация IEditableObject

        /// <summary>
        /// Начинает редактирование состояния объекта.
        /// </summary>
        public void BeginEdit()
        {
            Contract.Assume(memento == null);

            memento = new ObjectMemento<T>(originator);
        }

        /// <summary>
        /// Отменяет редактирование состояния объекта и откатывает транзакцию.
        /// </summary>
        public void CancelEdit()
        {
            Contract.Assume(memento != null);

            memento.Restore();
            memento.Dispose();
            memento = null;
        }

        /// <summary>
        /// Завершает редактирование состояния объекта и фиксирует транзакцию.
        /// </summary>
        public void EndEdit()
        {
            Contract.Assume(memento != null);

            memento.Dispose();
            memento = null;
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Освобождает управляемые ресурсы, используемые этим <see cref="Caretaker{T}"/>.
        /// </summary>
        protected override void OnDispose()
        {
            if (memento != null)
                CancelEdit();
        }

        #endregion
    }
}
