using System;
using System.Diagnostics.Contracts;

namespace LayerCake.UI.WeakEvents
{
    [ContractClassFor(typeof(WeakDelegate<>))]
    internal abstract class WeakDelegateContract<TDelegate> : WeakDelegate<TDelegate>
        where TDelegate : class
    {
        #region Конструктор

        protected WeakDelegateContract(TDelegate @delegate, Action<TDelegate> removeDelegateCode)
            : base(@delegate, removeDelegateCode)
        {
        }

        #endregion

        #region Реализация WeakDelegate<TDelegate>

        protected override TDelegate GetDelegate()
        {
            Contract.Ensures(Contract.Result<TDelegate>() != null);

            throw new NotImplementedException();
        }

        #endregion
    }
}
