using System;
using System.Collections.ObjectModel;
using LayerCake.UI.Services.Properties;

namespace LayerCake.UI.Services
{
    /// <summary>
    /// Задает параметры отображения окна "О программе".
    /// </summary>
    public sealed class AboutBoxSettings
    {
        #region Конструктор

        /// <summary>
        /// Инициализирует экземпляр <see cref="AboutBoxSettings"/>.
        /// </summary>
        public AboutBoxSettings()
        {
            this.Version = Resources.ABS_DefaultVersion;
            this.Copyright = Resources.ABS_DefaultCopyrigt;
            this.LogoSource = "pack://application:,,,/LayerCake.UI.Services;component/Resources/DefaultLogo.jpg";
        }

        #endregion

        #region Public-свойства

        /// <summary>
        /// Возвращает или задает наименование программного продукта.
        /// </summary>
        public String ProductName { get; set; }

        /// <summary>
        /// Возвращает или задает наименование приложения.
        /// </summary>
        public String ApplicationName { get; set; }

        /// <summary>
        /// Возвращает или задает версию приложения.
        /// </summary>
        public String Version { get; set; }

        /// <summary>
        /// Возвращает или задает текст, декларирующий авторские права на приложение.
        /// </summary>
        public String Copyright { get; set; }

        /// <summary>
        /// Возвращает или задает источник для загрузки логотипа приложения.
        /// </summary>
        public String LogoSource { get; set; }

        /// <summary>
        /// Возвращает коллекцию <see cref="String"/>, каждый элемент которой описывает дополнительные сведения о приложении.
        /// </summary>
        public Collection<String> AdditionalInfo
        {
            get
            {
                return additionalInfo ?? (additionalInfo = new Collection<String>());
            }
        }
        private Collection<String> additionalInfo;

        #endregion
    }
}
