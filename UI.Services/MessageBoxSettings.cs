using System;
using System.Windows;

namespace LayerCake.UI.Services
{
    /// <summary>
    /// Задает параметры отображения окна сообщений.
    /// </summary>
    public class MessageBoxSettings
    {
        /// <summary>
        /// Возвращает или задает заголовок окна сообщений.
        /// </summary>
        public String Title { get; set; }

        /// <summary>
        /// Возвращает или задает текст сообщения.
        /// </summary>
        public String Text { get; set; }

        /// <summary>
        /// Возвращает или задает набор кнопок в окне сообщений.
        /// </summary>
        public MessageBoxButton Button { get; set; }

        /// <summary>
        /// Возвращает или задает пиктограмму окна сообщений.
        /// </summary>
        public MessageBoxImage Image { get; set; }

        /// <summary>
        /// Возвращает или задает результат отображения окна сообщений по умолчанию.
        /// </summary>
        public MessageBoxResult DefaultResult { get; set; }
    }
}
