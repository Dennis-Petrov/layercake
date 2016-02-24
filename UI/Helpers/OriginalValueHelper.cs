using System;

namespace LayerCake.UI.Helpers
{
    internal sealed class OriginalValueHelper : Disposable
    {
        #region Конструктор

        public OriginalValueHelper(object value, IMemento memento)
        {
            this.Value = value;
            this.Memento = memento;
        }

        #endregion

        #region Overrides

        protected override void OnDispose()
        {
            if (Memento != null)
                Memento.Dispose();
        }

        #endregion

        #region Public-свойства

        public object Value { get; private set; }
        public IMemento Memento { get; private set; }

        #endregion
    }
}
