using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows;
using LayerCake.UI.Controls;

namespace LayerCake.UI.Services
{
    /// <summary>
    /// Реализация <see cref="IPresentationService"/> по умолчанию.
    /// </summary>
    [Export(typeof(IPresentationService))]
    public sealed class PresentationService : IPresentationService
    {
        #region Private-типы

        private struct SizeToContentKey
        {
            public SizeToContentKey(Double width, Double height)
            {
                this.IsWidhtNaN = Double.IsNaN(width);
                this.IsHeightNaN = Double.IsNaN(height);
            }

            public readonly Boolean IsWidhtNaN;
            public readonly Boolean IsHeightNaN;
        }

        #endregion

        #region Поля

        [Import]
        private IDataTemplateRegistryService dataTemplateRegistryService = null;
        private readonly Dictionary<SizeToContentKey, SizeToContent> sizeToContentMap;
        private readonly Stack<Window> windows;

        #endregion

        #region Конструктор

        /// <summary>
        /// Инициализирует экземпляр <see cref="PresentationService"/>.
        /// </summary>
        public PresentationService()
        {
            this.sizeToContentMap = new Dictionary<SizeToContentKey, SizeToContent>
            {
                { new SizeToContentKey(Double.NaN, Double.NaN), SizeToContent.WidthAndHeight },
                { new SizeToContentKey(Double.NaN, 0.0), SizeToContent.Width },
                { new SizeToContentKey(0.0, Double.NaN), SizeToContent.Height },
                { new SizeToContentKey(0.0, 0.0), SizeToContent.Manual }
            };

            this.windows = new Stack<Window>();
        }

        #endregion

        #region Private-методы

        private Window CreateWindow<T>(string templateId, T viewModel, WindowSettings windowSettings)
            where T : ViewModel
        {
            var window = new GenericWindow
            {
                DataContext = new WindowDataContext
                {
                    ViewModel = viewModel,
                    WindowSettings = windowSettings
                }
            };

            window.ContentPlaceholder.Content = viewModel;
            window.ContentPlaceholder.ContentTemplate = dataTemplateRegistryService.FindDataTemplate<T>(templateId);
            
            return window;
        }

        private Double CalcCoordinate(Double parentCoordinate, Double parentSize, Double thisSize)
        {
            return parentCoordinate + (parentSize - thisSize) / 2;
        }

        #endregion

        #region Реализация IPresentationService

        /// <summary>
        /// Возвращает <see langword="true"/>, если главное окно было показано;
        /// <see langword="false"/> в противном случае.
        /// </summary>
        public bool IsMainWindowShown
        {
            get { return windows.Count > 0; }
        }

        /// <summary>
        /// Возвращает окно верхнего уровня.
        /// </summary>
        public Window TopmostWindow
        {
            get { return windows.Count > 0 ? windows.Peek() : null; }
        }

        /// <summary>
        /// Отображает модель представления в главном окне.
        /// </summary>
        /// <typeparam name="T">
        /// Тип модели представления.
        /// </typeparam>
        /// <param name="templateId">
        /// Идентификатор шаблона данных.
        /// </param>
        /// <param name="viewModel">
        /// Модель представления, которую нужно отобразить в главном окне.
        /// </param>
        /// <param name="windowSettings">
        /// Параметры отображения главного окна.
        /// </param>
        public void ShowInMainWindow<T>(string templateId, T viewModel, WindowSettings windowSettings)
            where T : ViewModel
        {
            Application.Current.MainWindow = CreateWindow<T>(templateId, viewModel, windowSettings);

            windows.Push(Application.Current.MainWindow);
            
            Application.Current.MainWindow.Show();
        }

        /// <summary>
        /// Отображает модель представления в главном окне, используя шаблон данных по умолчанию.
        /// </summary>
        /// <typeparam name="T">
        /// Тип модели представления.
        /// </typeparam>
        /// <param name="viewModel">
        /// Модель представления, которую нужно отобразить в главном окне.
        /// </param>
        /// <param name="windowSettings">
        /// Параметры отображения главного окна.
        /// </param>
        public void ShowInMainWindow<T>(T viewModel, WindowSettings windowSettings) 
            where T : ViewModel
        {
            ShowInMainWindow<T>(typeof(T).FullName, viewModel, windowSettings);
        }

        /// <summary>
        /// Отображает модель представления в диалоге.
        /// </summary>
        /// <typeparam name="T">
        /// Тип модели представления.
        /// </typeparam>
        /// <param name="templateId">
        /// Идентификатор шаблона данных.
        /// </param>
        /// <param name="viewModel">
        /// Модель представления, которую нужно отобразить в диалоге.
        /// </param>
        /// <param name="windowSettings">
        /// Параметры отображения диалога.
        /// </param>
        /// <returns>
        /// <see langword="true"/>, если диалог закрыт по кнопке подтверждения;
        /// <see langword="false"/> в противном случае.
        /// </returns>
        public bool ShowInDialog<T>(string templateId, T viewModel, WindowSettings windowSettings)
            where T : ViewModel
        {
            var window = CreateWindow<T>(templateId, viewModel, windowSettings);

            window.SizeToContent = sizeToContentMap[new SizeToContentKey(windowSettings.Width, windowSettings.Height)];
            window.ShowInTaskbar = false;
            window.Owner = windows.Peek();

            if (window.SizeToContent == SizeToContent.Manual)
            {
                // обходим ситуацию, когда диалог не центрируется по владельцу из-за 
                // задания координат окна через Binging
                window.Left = CalcCoordinate(window.Owner.Left, window.Owner.Width, windowSettings.Width);
                window.Top = CalcCoordinate(window.Owner.Top, window.Owner.Height, windowSettings.Height);
            }
            else
                window.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            viewModel.DialogResult = null;

            windows.Push(window);

            var dialogResult = window.ShowDialog();

            windows.Pop();

            return dialogResult.HasValue && dialogResult.Value;
        }

        /// <summary>
        /// Отображает модель представления в диалоге, используя шаблон данных по умолчанию.
        /// </summary>
        /// <typeparam name="T">
        /// Тип модели представления.
        /// </typeparam>
        /// <param name="viewModel">
        /// Модель представления, которую нужно отобразить в диалоге.
        /// </param>
        /// <param name="windowSettings">
        /// Параметры отображения диалога.
        /// </param>
        /// <returns>
        /// <see langword="true"/>, если диалог закрыт по кнопке подтверждения;
        /// <see langword="false"/> в противном случае.
        /// </returns>
        public bool ShowInDialog<T>(T viewModel, WindowSettings windowSettings) 
            where T : ViewModel
        {
            return ShowInDialog<T>(typeof(T).FullName, viewModel, windowSettings);
        }

        #endregion
    }
}
