using System;
using System.Xml.Serialization;

namespace LayerCake.UI.Services
{
    /// <summary>
    /// Задает параметры отображения окна.
    /// </summary>
    public sealed class WindowSettings : NotificationObject
    {
        #region Конструктор

        /// <summary>
        /// Инициализирует экземпляр <see cref="WindowSettings"/>.
        /// </summary>
        public WindowSettings()
        {
            left = Double.NaN;
            top = Double.NaN;
            width = Double.NaN;
            height = Double.NaN;
            title = String.Empty;
        }

        #endregion

        #region Public-свойства

        /// <summary>
        /// Возвращает или задает заголовок окна.
        /// </summary>
        [XmlIgnore]
        public String Title
        {
            get { return title; }
            set
            {
                if (title != value)
                {
                    title = value;
                    OnPropertyChanged(() => Title);
                }
            }
        }
        private String title;

        /// <summary>
        /// Возвращает или задает признак, можно ли изменять размеры окна.
        /// </summary>
        [XmlIgnore]
        public Boolean CanResize
        {
            get { return canResize; }
            set
            {
                if (canResize != value)
                {
                    canResize = value;
                    OnPropertyChanged(() => CanResize);
                }
            }
        }
        private Boolean canResize;

        /// <summary>
        /// Возвращает или задает абсциссу левого верхнего угла.
        /// </summary>
        public Double Left
        {
            get { return left; }
            set
            {
                if (left != value)
                {
                    left = value;
                    OnPropertyChanged(() => Left);
                }
            }
        }
        private Double left;

        /// <summary>
        /// Возвращает или задает ординату левого верхнего угла.
        /// </summary>
        public Double Top
        {
            get { return top; }
            set
            {
                if (top != value)
                {
                    top = value;
                    OnPropertyChanged(() => Top);
                }
            }
        }
        private Double top;

        /// <summary>
        /// Возвращает или задает ширину окна.
        /// </summary>
        public Double Width
        {
            get { return width; }
            set
            {
                if (width != value)
                {
                    width = value;
                    OnPropertyChanged(() => Width);
                }
            }
        }
        private Double width;

        /// <summary>
        /// Возвращает или задает высоту окна.
        /// </summary>
        public Double Height
        {
            get { return height; }
            set
            {
                if (height != value)
                {
                    height = value;
                    OnPropertyChanged(() => Height);
                }
            }
        }
        private Double height;

        #endregion
    }
}
