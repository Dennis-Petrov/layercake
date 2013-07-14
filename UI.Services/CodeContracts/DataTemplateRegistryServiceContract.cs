using System;
using System.Diagnostics.Contracts;
using System.Windows;

namespace LayerCake.UI.Services.CodeContracts
{
    [ContractClassFor(typeof(IDataTemplateRegistryService))]
    internal abstract class DataTemplateRegistryServiceContract : IDataTemplateRegistryService
    {
        #region Реализация IDataTemplateRegistryService

        public DataTemplate FindDataTemplate<T>(string id)
            where T : ViewModel
        {
            Contract.Requires<ArgumentException>(!String.IsNullOrEmpty(id));
            Contract.Ensures(Contract.Result<DataTemplate>() != null);

            throw new NotImplementedException();
        }

        #endregion
    }
}
