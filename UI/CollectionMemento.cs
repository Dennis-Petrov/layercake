using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace LayerCake.UI
{
    internal sealed class CollectionMemento<TCollection, TItem> : Disposable, IMemento
        where TCollection : class, INotifyCollectionChanged, IList<TItem>
        where TItem : class, INotifyPropertyChanged
    {
        #region Поля

        private readonly TCollection originator;
        private readonly List<ObjectMemento<TItem>> snapshot;
        private readonly List<TItem> insertedItems;
        private readonly List<TItem> deletedItems;

        #endregion

        #region Конструктор

        public CollectionMemento(TCollection originator)
        {
            this.originator = originator;
            this.snapshot = new List<ObjectMemento<TItem>>(originator.Count);
            this.originator.CollectionChanged += OnCollectionChanged;
            this.insertedItems = new List<TItem>();
            this.deletedItems = new List<TItem>();

            TakeSnapshot();
        }

        #endregion

        #region Private-методы

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    insertedItems.Add((TItem)e.NewItems[0]);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    deletedItems.Add((TItem)e.OldItems[0]);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    insertedItems.Add((TItem)e.NewItems[0]);
                    deletedItems.Add((TItem)e.OldItems[0]);
                    break;
            }
        }

        private void TakeSnapshot()
        {
            foreach (var item in originator)
            {
                snapshot.Add(item != null ? new ObjectMemento<TItem>(item) : null);
            }
        }

        private void DeleteInsertedItems()
        {
            foreach (var item in insertedItems)
            {
                originator.Remove(item);
            }

            insertedItems.Clear();
        }

        private void InsertDeletedItems()
        {
            foreach (var item in deletedItems)
            {
                originator.Add(item);
            }

            deletedItems.Clear();
        }

        #endregion

        #region Overrides

        protected override void OnDispose()
        {
            originator.CollectionChanged -= OnCollectionChanged;

            foreach (var item in snapshot)
            {
                if (item != null)
                    item.Dispose();
            }
        }

        #endregion

        #region Реализация IMemento

        public void Restore()
        {
            originator.CollectionChanged -= OnCollectionChanged;
            try
            {
                // удаляем добавленные элементы
                DeleteInsertedItems();

                // восстанавливаем удаленные элементы
                InsertDeletedItems();

                // откатываем изменения в элементах
                foreach (var item in snapshot)
                {
                    if (item != null)
                        item.Restore();
                }
            }
            finally
            {
                originator.CollectionChanged += OnCollectionChanged;
            }
        }

        #endregion
    }
}
