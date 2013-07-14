using System;
using System.Diagnostics.Contracts;

namespace LayerCake.UI.Services.CodeContracts
{
    [ContractClassFor(typeof(IApplicationDataService<>))]
    internal abstract class ApplicationDataServiceContract<TModel> : IApplicationDataService<TModel>
        where TModel : class
    {
        #region Реализация IApplicationDataService<TModel>

        public TModel LoadApplicationModel()
        {
            Contract.Ensures(Contract.Result<TModel>() != null);

            throw new NotImplementedException();
        }

        public WindowSettings LoadMainWindowSettings()
        {
            Contract.Ensures(Contract.Result<WindowSettings>() != null);

            throw new NotImplementedException();
        }

        public void SaveApplicationModel(TModel instance)
        {
            Contract.Requires<ArgumentNullException>(instance != null);

            throw new NotImplementedException();
        }

        public void SaveMainWindowSettings(WindowSettings instance)
        {
            Contract.Requires<ArgumentNullException>(instance != null);

            throw new NotImplementedException();
        }

        #endregion
    }
}
