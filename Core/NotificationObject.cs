using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Diagnostics.Contracts;

namespace LayerCake
{
    /// <summary>
    /// Предоставляет базовую реализацию <see cref="INotifyPropertyChanged"/>.
    /// </summary>
    public abstract class NotificationObject : INotifyPropertyChanged
    {
        /// <summary>
        /// Регистрирует обработчик события <see cref="INotifyPropertyChanged.PropertyChanged"/> для заданного свойства.
        /// </summary>
        /// <typeparam name="TProperty">
        /// Тип свойства.
        /// </typeparam>
        /// <param name="property">
        /// <see cref="LambdaExpression"/> для доступа к свойству.
        /// </param>
        /// <param name="handler">
        /// Обработчик события.
        /// </param>
        protected void RegisterPropertyChangeHandler<TProperty>(Expression<Func<TProperty>> property, Action handler)
        {
            Contract.Requires<ArgumentNullException>(property != null);
            Contract.Requires<ArgumentNullException>(handler != null);

            PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == property.GetMemberPathString())
                    handler();
            };
        }

        /// <summary>
        /// Генерирует событие <see cref="INotifyPropertyChanged.PropertyChanged"/>.
        /// </summary>
        /// <typeparam name="TProperty">
        /// Тип свойства.
        /// </typeparam>
        /// <param name="property">
        /// <see cref="LambdaExpression"/> для доступа к свойству, значение которого изменилось.
        /// </param>
        protected void OnPropertyChanged<TProperty>(Expression<Func<TProperty>> property)
        {
            OnPropertyChanged(property.GetMemberPathString());
        }

        /// <summary>
        /// Генерирует событие <see cref="INotifyPropertyChanged.PropertyChanged"/>.
        /// </summary>
        /// <param name="propertyName">
        /// Наименование свойства, значение которого изменилось.
        /// </param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(propertyName));

            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Возникает при изменении значения свойства.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
