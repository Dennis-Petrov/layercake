using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics.Contracts;
using System.Linq;
using LayerCake.UI.WeakEvents;

namespace LayerCake.UI
{
    /// <summary>
    /// Описывает коллекцию моделей представления, которая может синхронизировать свое содержимое
    /// с коллекцией доменных моделей.
    /// </summary>
    /// <typeparam name="TModel">
    /// Тип доменной модели.
    /// </typeparam>
    /// <typeparam name="TViewModel">
    /// Тип модели представления.
    /// </typeparam>
    public class ViewModelCollection<TViewModel, TModel> : ObservableCollection<TViewModel>
        where TViewModel : ViewModel<TModel>
        where TModel : class
    {
        #region Поля

        private readonly Boolean autoDisposeItemsOnRemove;
        private readonly ObservableCollection<TModel> models;
        private readonly Func<TModel, TViewModel> factoryMethod;
        private Boolean blockReentrancy;

        #endregion

        #region Конструктор

        /// <summary>
        /// Инициализирует экземпляр <see cref="ViewModelCollection{TViewModel, TModel}"/>.
        /// </summary>
        /// <param name="models">
        /// Исходный экземпляр <see cref="ObservableCollection{TModel}"/>, с которым будет синхронизироваться эта коллекция.
        /// </param>
        /// <param name="factoryMethod">
        /// Фабричный метод для получения <typeparamref name="TViewModel"/> из <typeparamref name="TModel"/>.
        /// </param>
        /// <param name="autoDisposeItemsOnRemove">
        /// <see langword="true"/> для автоматического вызова <see cref="IDisposable.Dispose"/>, 
        /// если <typeparamref name="TViewModel"/> реализует <see cref="IDisposable"/>; 
        /// <see langword="false"/> в противном случае.
        /// </param>
        public ViewModelCollection(ObservableCollection<TModel> models, Func<TModel, TViewModel> factoryMethod, 
            Boolean autoDisposeItemsOnRemove = false)
            : base(models.Select(model => factoryMethod(model)))
        {
            Contract.Requires<ArgumentNullException>(models != null);
            Contract.Requires<ArgumentNullException>(factoryMethod != null);

            this.models = models;
            this.models.CollectionChanged +=
                // подписка на изменение коллеции моделей осуществлена через слабую ссылку,
                // т.к. коллекция моделей - долгоживущий объект, по сравнению с коллекциями моделей представления
                new WeakNotifyCollectionChangedEventHandler(OnModelsCollectionChanged, handler => this.models.CollectionChanged -= handler);
            this.factoryMethod = factoryMethod;
            this.autoDisposeItemsOnRemove = autoDisposeItemsOnRemove && typeof(IDisposable).IsAssignableFrom(typeof(TViewModel));
        }

        #endregion

        #region Private-методы

        private void OnModelsCollectionChanged(Object sender, NotifyCollectionChangedEventArgs e)
        {
            if (blockReentrancy)
                // идет изменение коллекции моделей представления
                return;

            blockReentrancy = true;
            try
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        Insert(e.NewStartingIndex, factoryMethod((TModel)e.NewItems[0]));
                        break;
                    case NotifyCollectionChangedAction.Move:
                        Move(e.OldStartingIndex, e.NewStartingIndex);
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        RemoveAt(e.OldStartingIndex);
                        break;
                    case NotifyCollectionChangedAction.Replace:
                        this[e.NewStartingIndex] = factoryMethod((TModel)e.NewItems[0]);
                        break;
                    case NotifyCollectionChangedAction.Reset:
                        Clear();
                        break;
                }
            }
            finally
            {
                blockReentrancy = false;
            }
        }

        private void OnViewModelsCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (blockReentrancy)
                // идет изменение коллекции моделей
                return;

            blockReentrancy = true;
            try
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        models.Insert(e.NewStartingIndex, ((TViewModel)e.NewItems[0]).Model);
                        break;
                    case NotifyCollectionChangedAction.Move:
                        models.Move(e.OldStartingIndex, e.NewStartingIndex);
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        models.RemoveAt(e.OldStartingIndex);
                        break;
                    case NotifyCollectionChangedAction.Replace:
                        models[e.NewStartingIndex] = ((TViewModel)e.NewItems[0]).Model;
                        break;
                    case NotifyCollectionChangedAction.Reset:
                        models.Clear();
                        break;
                }
            }
            finally
            {
                blockReentrancy = false;
            }
        }

        private void DisposeItem(Int32 index)
        {
            ((IDisposable)Items[index]).Dispose();
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Выполняет очистку содержимого коллекции.
        /// </summary>
        protected override void ClearItems()
        {
            new List<TViewModel>(this).ForEach(t => Remove(t));
        }

        /// <summary>
        /// Генерирует событие <see cref="INotifyCollectionChanged.CollectionChanged"/>.
        /// </summary>
        /// <param name="e">
        /// Аргументы события.
        /// </param>
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            OnViewModelsCollectionChanged(e);
            base.OnCollectionChanged(e);
        }

        /// <summary>
        /// Удаляет элемент по заданному индексу.
        /// </summary>
        /// <param name="index">
        /// Индекс элемента.
        /// </param>
        protected override void RemoveItem(Int32 index)
        {
            if (autoDisposeItemsOnRemove)
                DisposeItem(index);

            base.RemoveItem(index);
        }

        /// <summary>
        /// Заменяет элемент по заданному индексу.
        /// </summary>
        /// <param name="index">
        /// Индекс элемента.
        /// </param>
        /// <param name="item">
        /// Новый элемент.
        /// </param>
        protected override void SetItem(Int32 index, TViewModel item)
        {
            if (autoDisposeItemsOnRemove)
                DisposeItem(index);

            base.SetItem(index, item);
        }

        #endregion
    }
}
