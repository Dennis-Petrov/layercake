using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Windows.Data;

namespace LayerCake.UI
{
    /// <summary>
    /// Представляет контейнер сообщений о состоянии моделей представления.
    /// </summary>
    public sealed class ViewModelStateMessagesContainer : NotificationObject
    {
        #region Поля

        private readonly ObservableCollection<ViewModelStateMessage> stateMessages;
        private readonly ICollectionView stateMessagesSource;

        #endregion

        #region Конструктор

        /// <summary>
        /// Инициализирует экземпляр <see cref="ViewModelStateMessagesContainer"/>.
        /// </summary>
        public ViewModelStateMessagesContainer()
        {
            this.stateMessages = new ObservableCollection<ViewModelStateMessage>();
            this.stateMessages.CollectionChanged += (sender, args) => 
            { 
                HasMessages = this.stateMessages.Any();
                HasInformation = this.stateMessages.Any(message => message.Type == ViewModelStateMessageType.Information);
                HasWarnings = this.stateMessages.Any(message => message.Type == ViewModelStateMessageType.Warining);
                HasErrors = this.stateMessages.Any(message => message.Type == ViewModelStateMessageType.Error);
            };
            this.stateMessagesSource = CollectionViewSource.GetDefaultView(this.stateMessages);
            this.stateMessagesSource.Filter = obj => IsMatchFilter((ViewModelStateMessage)obj);
            this.showInformation = true;
            this.showWarnings = true;
            this.showErrors = true;
        }

        #endregion

        #region Private-методы

        private bool IsMatchFilter(ViewModelStateMessage message)
        {
            var isMatch = false;

            switch (message.Type)
            {
                case ViewModelStateMessageType.Information:
                    isMatch = showInformation;
                    break;
                case ViewModelStateMessageType.Warining:
                    isMatch = showWarnings;
                    break;
                case ViewModelStateMessageType.Error:
                    isMatch = showErrors;
                    break;
            }

            return isMatch;
        }

        #endregion

        #region Public-свойства

        /// <summary>
        /// Возвращает представление коллекции сообщений.
        /// </summary>
        public ICollectionView StateMessages
        {
            get { return stateMessagesSource; }
        }

        /// <summary>
        /// Возвращает признак того, есть ли сообщения в коллекции <see cref="StateMessages"/>.
        /// </summary>
        public bool HasMessages
        {
            get { return hasMessages; }
            private set
            {
                if (hasMessages != value)
                {
                    hasMessages = value;
                    OnPropertyChanged(() => HasMessages);
                }
            }
        }
        private bool hasMessages;

        /// <summary>
        /// Возвращает признак того, есть ли информационные сообщения в коллекции <see cref="StateMessages"/>.
        /// </summary>
        public bool HasInformation
        {
            get { return hasInformation; }
            private set
            {
                if (hasInformation != value)
                {
                    hasInformation = value;
                    OnPropertyChanged(() => HasInformation);
                }
            }
        }
        private bool hasInformation;

        /// <summary>
        /// Возвращает признак того, есть ли предупреждения в коллекции <see cref="StateMessages"/>.
        /// </summary>
        public bool HasWarnings
        {
            get { return hasWarnings; }
            private set
            {
                if (hasWarnings != value)
                {
                    hasWarnings = value;
                    OnPropertyChanged(() => HasWarnings);
                }
            }
        }
        private bool hasWarnings;
        
        /// <summary>
        /// Возвращает признак того, есть ли ошибки в коллекции <see cref="StateMessages"/>.
        /// </summary>
        public bool HasErrors
        {
            get { return hasErrors; }
            private set
            {
                if (hasErrors != value)
                {
                    hasErrors = value;
                    OnPropertyChanged(() => HasErrors);
                }
            }
        }
        private bool hasErrors;

        /// <summary>
        /// Возвращает или задает признак того, нужно ли отображать информационные сообщения.
        /// </summary>
        public bool ShowInformation
        {
            get { return showInformation; }
            set
            {
                if (showInformation != value)
                {
                    showInformation = value;
                    OnPropertyChanged(() => ShowInformation);
                    stateMessagesSource.Refresh();
                }
            }
        }
        private bool showInformation;

        /// <summary>
        /// Возвращает или задает признак того, нужно ли отображать предупреждения.
        /// </summary>
        public bool ShowWarnings
        {
            get { return showWarnings; }
            set
            {
                if (showWarnings != value)
                {
                    showWarnings = value;
                    OnPropertyChanged(() => ShowWarnings);
                    stateMessagesSource.Refresh();
                }
            }
        }
        private bool showWarnings;

        /// <summary>
        /// Возвращает или задает признак того, нужно ли отображать сообщения об ошибках.
        /// </summary>
        public bool ShowErrors
        {
            get { return showErrors; }
            set
            {
                if (showErrors != value)
                {
                    showErrors = value;
                    OnPropertyChanged(() => ShowErrors);
                    stateMessagesSource.Refresh();
                }
            }
        }
        private bool showErrors;

        #endregion

        #region Public-методы

        /// <summary>
        /// Добавляет сообщение о состоянии модели представления в контейнер.
        /// </summary>
        /// <param name="type">
        /// Тип сообщения.
        /// </param>
        /// <param name="source">
        /// Источник сообщения.
        /// </param>
        /// <param name="context">
        /// Контекст сообщения.
        /// </param>
        /// <param name="message">
        /// Текст сообщения.
        /// </param>
        public void AddMessage(ViewModelStateMessageType type, ViewModel source, string context, string message)
        {
            Contract.Requires<ArgumentNullException>(source != null);
            Contract.Requires<ArgumentException>(!String.IsNullOrEmpty(context));
            Contract.Requires<ArgumentException>(!String.IsNullOrEmpty(message));

            // для одного источника разрешается хранить только одно сообщение с заданным наименованием контекста
            RemoveMessage(source, context);

            this.stateMessages.Add(new ViewModelStateMessage(type, source, context, message));
        }

        /// <summary>
        /// Удаляет сообщения с заданным идентификатором источника и наименованием контекста из этого контейнера.
        /// </summary>
        /// <param name="source">
        /// Источник сообщений.
        /// </param>
        /// <param name="context">
        /// Наименование контекста сообщения.
        /// </param>
        public void RemoveMessage(ViewModel source, string context)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(context));

            this.stateMessages.RemoveWhere(message => message.SourceId == source.ObjectId && message.Context == context);
        }

        /// <summary>
        /// Удаляет сообщения с заданным идентификатором источника из этого контейнера.
        /// </summary>
        /// <param name="source">
        /// Источник сообщений.
        /// </param>
        public void RemoveMessages(ViewModel source)
        {
            this.stateMessages.RemoveWhere(message => message.SourceId == source.ObjectId);
        }

        #endregion
    }
}
