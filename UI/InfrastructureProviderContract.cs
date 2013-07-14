using System;
using System.Diagnostics.Contracts;

namespace LayerCake.UI
{
    [ContractClassFor(typeof(IInfrastructureProvider))]
    internal abstract class InfrastructureProviderContract : IInfrastructureProvider
    {
        #region Реализация IInfrastructureProvider

        public abstract bool IsBusy { get; set; }

        public ViewModelStateMessagesContainer StateMessagesContainer
        {
            get 
            {
                Contract.Ensures(Contract.Result<ViewModelStateMessagesContainer>() != null);

                throw new NotImplementedException(); 
            }
        }

        #endregion
    }
}
