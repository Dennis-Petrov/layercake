using System;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics.Contracts;
using System.Windows;
using LayerCake.UI.Properties;
using LayerCake.UI.Services;
using NLog;

namespace LayerCake.UI
{
    /// <summary>
    /// Предоставляет базовый функционал для загрузчика LayerCake-приложения.
    /// </summary>
    /// <typeparam name="TViewModel">
    /// Тип модели представления уровня приложения.
    /// </typeparam>
    /// <typeparam name="T">
    /// Тип доменной модели приложения.
    /// </typeparam>
    [ContractClass(typeof(ApplicationBootstrapperContract<,>))]
    public abstract class ApplicationBootstrapper<TViewModel, T>
        where TViewModel : ApplicationViewModel<T>
        where T : class
    {
        #region Статические члены

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Виртуальные методы

		protected void AddMergedResourceDictionary(Application application, string uri)
		{
			application.Resources.MergedDictionaries.Add(new ResourceDictionary
			{
				Source = new Uri(uri)
			});
		}

		protected virtual void InitializeApplicationResources(Application application)
		{
			AddMergedResourceDictionary(application,
				"pack://application:,,,/PresentationFramework.Aero, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35;component/themes/Aero.NormalColor.xaml");
			AddMergedResourceDictionary(application,
				"pack://application:,,,/LayerCake.UI.Controls;component/MessageBoxStyle.xaml");
			AddMergedResourceDictionary(application,
				"pack://application:,,,/LayerCake.UI.Controls;component/Styles/ListBox.xaml");
			AddMergedResourceDictionary(application,
				"pack://application:,,,/LayerCake.UI.Controls;component/Styles/TreeView.xaml");
			AddMergedResourceDictionary(application,
				"pack://application:,,,/LayerCake.UI.Controls;component/Styles/ComboBox.xaml");
			AddMergedResourceDictionary(application,
				"pack://application:,,,/LayerCake.UI.Controls;component/Styles/DataGrid.xaml");
		}

        #endregion

        #region Абстрактные методы

        /// <summary>
        /// Создает каталог составных частей уровня приложения.
        /// </summary>
        /// <returns>
        /// Экземпляр <see cref="ComposablePartCatalog"/>, представляющий каталог составных частей уровня приложения.
        /// </returns>
        protected abstract ComposablePartCatalog CreateApplicationCatalog();

        /// <summary>
        /// Создает модель представления уровня приложения.
        /// </summary>
        /// <param name="applicationCatalog">
        /// Экземпляр <see cref="ComposablePartCatalog"/>, представляющий каталог составных частей уровня приложения.
        /// </param>
        /// <returns>
        /// Экземпляр <typeparamref name="TViewModel"/>, представляющий модель представления уровня приложения.
        /// </returns>
        protected abstract TViewModel CreateApplicationViewModel(ComposablePartCatalog applicationCatalog);

        /// <summary>
        /// Выполняет конфигурирование параметров отображения главного окна.
        /// </summary>
        /// <param name="settings">
        /// Параметры отображения главного окна.
        /// </param>
        protected abstract void ConfigureWindowSettings(WindowSettings settings);




        #endregion

        #region Public-методы

        /// <summary>
        /// Выполняет LayerCake-приложение.
        /// </summary>
        public void Run()
        {
            logger.Debug(Resources.AB_LayerCakeHello);
            logger.Debug(Resources.AB_Starting);
            try
            {
                logger.Debug(Resources.AB_CreatingApplicationCatalog);

                using (var catalog = CreateApplicationCatalog())
                {
                    logger.Debug(Resources.AB_CreatingApplicationViewModel);
                    var viewModel = CreateApplicationViewModel(catalog);

                    logger.Debug(Resources.AB_LoadingMainWindowsSettings);

                    var service = viewModel
                        .PartsContainer
                        .GetExportedValue<IApplicationDataService<T>>();

                    var settings = service.LoadMainWindowSettings();

                    ConfigureWindowSettings(settings);

                    logger.Debug(Resources.AB_CreatingApplication);
                    
                    var application = new Application();

                    InitializeApplicationResources(application);

                    logger.Debug(Resources.AB_ShowingMainWindow);
                    viewModel
                        .PartsContainer
                        .GetExportedValue<IPresentationService>()
                        .ShowInMainWindow(viewModel, settings);

                    logger.Debug(Resources.AB_Running);
                    application.Run();

                    logger.Debug(Resources.AB_SavingWindowSettings);
                    service.SaveMainWindowSettings(settings);
                }
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw;
            }
            logger.Debug(Resources.AB_Stopping);
        }

        #endregion
    }
}
