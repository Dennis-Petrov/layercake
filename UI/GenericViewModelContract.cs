using System;
using System.Diagnostics.Contracts;

namespace LayerCake.UI
{
    [ContractClassFor(typeof(ViewModel<>))]
    internal abstract class ViewModelContract<T> : ViewModel<T>
        where T : class
    {
        protected ViewModelContract(ViewModel parent)
            : base(parent)
        {
        }

        protected override T GetModel()
        {
            Contract.Ensures(Contract.Result<T>() != null);

            throw new NotImplementedException();
        }
    }
}
