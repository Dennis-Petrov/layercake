using System;

namespace LayerCake.UI
{
    /// <summary>
    /// Описывает сообщение о состоянии модели представления.
    /// </summary>
    public sealed class ViewModelStateMessage
    {
        #region Конструктор

        internal ViewModelStateMessage(ViewModelStateMessageType type, ViewModel source, string context, string message)
        {
            this.Type = type;
            this.SourceId = source.ObjectId;
            this.Source = source.ToString();
            this.Context = context;
            this.Message = message;
        }

        #endregion

        #region Public-свойства

        /// <summary>
        /// Возвращает тип сообщения.
        /// </summary>
        public ViewModelStateMessageType Type { get; private set; }

        /// <summary>
        /// Возвращает идентификатор источника сообщения.
        /// </summary>
        public Guid SourceId { get; private set; }

        /// <summary>
        /// Возвращает источник сообщения.
        /// </summary>
        /// <remarks>
        /// Источник сообщения - это читаемое, локализованное название модели представления, которая создала это сообщение.
        /// </remarks>
        public String Source { get; private set; }

        /// <summary>
        /// Возвращает контекст сообщения.
        /// </summary>
        /// <remarks>
        /// Контекст сообщения - это читаемое, локализованное название свойства-данного или свойства команды модели представления.
        /// </remarks>
        public string Context { get; private set; }

        /// <summary>
        /// Возвращает текст сообщения.
        /// </summary>
        public string Message { get; private set; }

        #endregion
    }
}
