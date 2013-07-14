using System;

namespace LayerCake.UI.Services
{
    /// <summary>
    /// Задает параметры отображения файлового диалога.
    /// </summary>
    public sealed class FileDialogSettings
    {
        /// <summary>
        /// Возвращает или задает заголовок диалога.
        /// </summary>
        public String Title { get; set; }

        /// <summary>
        /// Возвращает или задает расширение по умолчанию.
        /// </summary>
        public String DefaultExtension { get; set; }

        /// <summary>
        /// Возвращает или задает фильтр расширений.
        /// </summary>
        public String Filter { get; set; }

        /// <summary>
        /// Возвращает или задает имя файла.
        /// </summary>
        public String FileName { get; set; }

        /// <summary>
        /// Возвращает или задает начальный каталог для работы диалога.
        /// </summary>
        public String InitialDirectory { get; set; }
    }
}
