using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Windows;
using LayerCake.UI.Services.Properties;

namespace LayerCake.UI.Services
{
    /// <summary>
    /// Реализация <see cref="IDataTemplateRegistryService"/> по умолчанию.
    /// </summary>
    public abstract class DataTemplateRegistryService : IDataTemplateRegistryService
    {
        #region Поля

        private readonly Dictionary<Type, Dictionary<string, DataTemplateContainer>> dataTemplates;

        #endregion

        #region Конструктор

        /// <summary>
        /// Инициализирует экземпляр <see cref="DataTemplateRegistryService"/>.
        /// </summary>
        protected DataTemplateRegistryService()
        {
            this.dataTemplates = new Dictionary<Type, Dictionary<string, DataTemplateContainer>>();
        }

        #endregion

        #region Protected-методы

        /// <summary>
        /// Регистрирует сопоставление шаблона данных типу модели представления.
        /// </summary>
        /// <typeparam name="T">
        /// Тип модели представления.
        /// </typeparam>
        /// <param name="id">
        /// Идентификатор шаблона данных.
        /// </param>
        /// <param name="uri">
        /// Экземпляр <see cref="Uri"/>, задающий расположение шаблона данных.
        /// </param>
        protected void RegisterDataTemplate<T>(string id, Uri uri)
            where T : ViewModel
        {
            Contract.Requires<ArgumentException>(!String.IsNullOrEmpty(id));
            Contract.Requires<ArgumentNullException>(uri != null);

            Dictionary<string, DataTemplateContainer> dataTemplatesPerType;

            if (!dataTemplates.TryGetValue(typeof(T), out dataTemplatesPerType))
            {
                dataTemplatesPerType = new Dictionary<string, DataTemplateContainer>();
                dataTemplates.Add(typeof(T), dataTemplatesPerType);
            }

            dataTemplatesPerType.Add(id, new DataTemplateContainer(typeof(T), uri));
        }

        /// <summary>
        /// Регистрирует сопоставление шаблона данных типу модели представления, используя полное имя
        /// <typeparamref name="T"/> в качестве идентификатора шаблона.
        /// </summary>
        /// <typeparam name="T">
        /// Тип модели представления.
        /// </typeparam>
        /// <param name="uri">
        /// Экземпляр <see cref="Uri"/>, задающий расположение шаблона данных.
        /// </param>
        protected void RegisterDataTemplate<T>(Uri uri)
            where T : ViewModel
        {
            RegisterDataTemplate<T>(typeof(T).FullName, uri);
        }

        #endregion

        #region Реализация IDataTemplateRegistryService

        /// <summary>
        /// Ищет шаблон данных для заданного типа модели предстваления по идентификатору шаблона.
        /// </summary>
        /// <typeparam name="T">
        /// Тип модели представления.
        /// </typeparam>
        /// <param name="id">
        /// Идентификатор шаблона данных.
        /// </param>
        /// <returns>
        /// Экземпляр <see cref="DataTemplate"/>, представляющий шаблон данных для заданного типа.
        /// </returns>
        public DataTemplate FindDataTemplate<T>(string id) 
            where T : ViewModel
        {
            Dictionary<string, DataTemplateContainer> dataTemplatesForType;

            if (dataTemplates.TryGetValue(typeof(T), out dataTemplatesForType))
            {
                DataTemplateContainer container;

                if (dataTemplatesForType.TryGetValue(id, out container))
                {
                    return container.DataTemplate;
                }

                throw new InvalidOperationException(string.Format(Resources.DTRS_DataTemplateNotFound, id, typeof(T)));
            }

            throw new InvalidOperationException(string.Format(Resources.DTRS_DataTemplatesForTypeNotFound, typeof(T)));
        }

        #endregion
    }
}
