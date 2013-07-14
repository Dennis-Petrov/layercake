using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics.Contracts;
using LayerCake.UI.Services;

namespace LayerCake.UI
{
    /// <summary>
    /// Передставляет базовый фукнционал для создания моделей представления уровня приложения.
    /// </summary>
    /// <typeparam name="T">
    /// Тип доменной модели приложения.
    /// </typeparam>
    public abstract class ApplicationViewModel<T> : ViewModel<T>, IDisposable
        where T : class
    {
        #region Поля

        private readonly ComposablePartCatalog catalog;
        private CompositionContainer container;
        private bool isDisposed;

        #endregion

        #region Конструктор

        /// <summary>
        /// Инициализирует экземпляр <see cref="ApplicationViewModel{T}"/>.
        /// </summary>
        /// <param name="catalog">
        /// Каталог составных частей приложения.
        /// </param>
        protected ApplicationViewModel(ComposablePartCatalog catalog)
            : base(null)
        {
            Contract.Requires<ArgumentNullException>(catalog != null);

            this.catalog = catalog;
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Инициализирует и возвращает контейнер составных частей.
        /// </summary>
        /// <returns>
        /// Экземпляр <see cref="ExportProvider"/>, который является контейнером составных частей.
        /// </returns>
        protected override ExportProvider  GetPartsContainer()
        {
            container = new CompositionContainer(catalog);
            container.ComposeParts(this);

            return container;
        }

        /// <summary>
        /// Создает доменную модель приложения.
        /// </summary>
        /// <returns>
        /// Экземпляр <typeparamref name="T"/>, который будет обернут в эту модель представления.
        /// </returns>
        protected override T GetModel()
        {
            return PartsContainer
                .GetExportedValue<IApplicationDataService<T>>()
                .LoadApplicationModel();
        }

        #endregion

        #region Реализация IDisposable

        /// <summary>
        /// Освобождает управляемые ресурсы, используемые этим экземпляром <see cref="ApplicationViewModel{T}"/>.
        /// </summary>
        public void Dispose()
        {
            if (isDisposed)
                return;

            container.DisposeIfNotNull();
            isDisposed = true;
        }

        #endregion
    }
}
