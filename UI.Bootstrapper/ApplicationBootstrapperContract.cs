using System;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics.Contracts;
using LayerCake.UI.Services;

namespace LayerCake.UI
{
    [ContractClassFor(typeof(ApplicationBootstrapper<,>))]
    internal abstract class ApplicationBootstrapperContract<TViewModel, T> : ApplicationBootstrapper<TViewModel, T>
        where TViewModel : ApplicationViewModel<T>
        where T : class
    {
        protected override ComposablePartCatalog CreateApplicationCatalog()
        {
            Contract.Ensures(Contract.Result<ComposablePartCatalog>() != null);

            throw new NotImplementedException();
        }

        protected override TViewModel CreateApplicationViewModel(ComposablePartCatalog applicationCatalog)
        {
            Contract.Ensures(Contract.Result<TViewModel>() != null);

            throw new NotImplementedException();
        }

        protected override void ConfigureWindowSettings(WindowSettings settings)
        {
            Contract.Requires<ArgumentNullException>(settings != null);

            throw new NotImplementedException();
        }
    }
}
