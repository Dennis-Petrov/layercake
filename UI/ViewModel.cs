using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Input;
using LayerCake.UI.Commands;
using LayerCake.UI.Helpers;

namespace LayerCake.UI
{
    /// <summary>
    /// Предоставляет базовую функциональность для создания моделей представления.
    /// </summary>
    public abstract class ViewModel : NotificationObject, IInfrastructureProvider, IDataErrorInfo
    {
        #region Конструктор

        /// <summary>
        /// Инициализирует экземпляр <see cref="ViewModel"/>.
        /// </summary>
        /// <param name="parent">
        /// Родительская модель представления.
        /// </param>
        protected ViewModel(ViewModel parent)
        {
            this.ObjectId = Guid.NewGuid();
            this.Parent = parent;
            this.partsContainer = new Lazy<ExportProvider>(GetPartsContainer, true);
            this.stateMessagesContainer = new Lazy<ViewModelStateMessagesContainer>(GetStateMessagesContainer);
            this.acceptCommand = new Lazy<RelayCommand>(() => new RelayCommand(Accept, CanAccept));
            this.cancelCommand = new Lazy<RelayCommand>(() => new RelayCommand(Cancel, CanCancel));
            this.closingCommand = new Lazy<RelayCommand>(() => new RelayCommand(Closing, CanClose));
        }

        #endregion

        #region Private-методы

        private string ValidateProperty(string propertyName)
        {
            // определяем метаданные свойства
            var propertyMetadata = ReflectedProperty.GetProperty(GetType(), propertyName);
            var displayAttribute = propertyMetadata.GetCustomAttributes<DisplayAttribute>().FirstOrDefault();
            var displayPropertyName = displayAttribute != null ? displayAttribute.GetName() : propertyName;

            // убираем ошибки валидации, связанные с этим свойством
            StateMessagesContainer.RemoveMessage(this, displayPropertyName);

            // пытаемся выполнить валидацию свойства
            var validationContext = new ValidationContext(this, null, null) { MemberName = propertyName };
            var validationResults = new Collection<ValidationResult>();

            if (!Validator.TryValidateProperty(propertyMetadata.Get(this), validationContext, validationResults))
            {
                // добавляем ошибку валидации в контейнер сообщений
                StateMessagesContainer.AddMessage(ViewModelStateMessageType.Error, this, displayPropertyName, 
                    validationResults[0].ErrorMessage);

                return validationResults[0].ErrorMessage;
            }

            return string.Empty;
        }

        #endregion

        #region Protected-методы

        /// <summary>
        /// Инициализирует и возвращает контейнер составных частей.
        /// </summary>
        /// <returns>
        /// Экземпляр <see cref="ExportProvider"/>, который является контейнером составных частей.
        /// </returns>
        protected virtual ExportProvider GetPartsContainer()
        {
            Contract.Ensures(Contract.Result<ExportProvider>() != null);

            return Parent != null ? Parent.PartsContainer : null;
        }

        /// <summary>
        /// Инициализирует и возвращает контейнер сообщений о состоянии модели представления.
        /// </summary>
        /// <returns>
        /// Экземпляр <see cref="ViewModelStateMessagesContainer"/>, который является контейнером сообщений 
        /// о состоянии модели представления.
        /// </returns>
        protected virtual ViewModelStateMessagesContainer GetStateMessagesContainer()
        {
            return new ViewModelStateMessagesContainer();
        }

        /// <summary>
        /// Определяет, можно ли закрыть диалог по кнопке 'OK', если эта модель представления отображается в диалоге.
        /// </summary>
        /// <returns>
        /// <see langword="true"/>, если диалог можно закрыть по кнопке 'OK';
        /// <see langword="false"/> в противном случае.
        /// </returns>
        protected virtual bool CanAccept()
        {
            return !StateMessagesContainer.HasErrors;
        }

        /// <summary>
        /// Выполняет действия при закрытии диалога по кнопке 'OK', если эта модель представления отображается в диалоге.
        /// </summary>
        protected virtual void Accept()
        {
            DialogResult = true;
        }

        /// <summary>
        /// Определяет, можно ли закрыть диалог по кнопке 'Отмена', если эта модель представления отображается в диалоге.
        /// </summary>
        /// <returns>
        /// <see langword="true"/>, если диалог можно закрыть по кнопке 'Отмена';
        /// <see langword="false"/> в противном случае.
        /// </returns>
        protected virtual bool CanCancel()
        {
            return true;
        }

        /// <summary>
        /// Выполняет действия при закрытии диалога по кнопке 'Отмена', если эта модель представления отображается в диалоге.
        /// </summary>
        protected virtual void Cancel()
        {
            DialogResult = false;
        }

        /// <summary>
        /// Определяет, можно ли закрыть окно, если эта модель представления отображается в немодальном окне.
        /// </summary>
        /// <returns>
        /// <see langword="true"/>, если окно можно закрыть;
        /// <see langword="false"/> в противном случае.
        /// </returns>
        protected virtual bool CanClose()
        {
            return !IsBusy;
        }

        /// <summary>
        /// Выполняет действия при закрытии окна, если эта модель представления отображается в немодальном окне.
        /// </summary>
        protected virtual void Closing()
        {
        }

        /// <summary>
        /// Определяет, можно ли развернуть родительскую модель представления.
        /// </summary>
        /// <returns>
        /// <see langword="true"/>, если можно развернуть родительскую модель представления;
        /// <see langword="false"/> в противном случае.
        /// </returns>
        protected virtual bool CanExpandParent()
        {
            return Parent != null;
        }

        #endregion

        #region Public-свойства

        /// <summary>
        /// Возвращает идентификатор этой модели представления.
        /// </summary>
        internal Guid ObjectId { get; private set; }

        /// <summary>
        /// Возвращает родительскую модель представления.
        /// </summary>
        public ViewModel Parent { get; private set; }

        /// <summary>
        /// Возвращает экземпляр <see cref="ExportProvider"/>, представляющий контейнер составных частей.
        /// </summary>
        public ExportProvider PartsContainer
        {
            get { return partsContainer.Value; }
        }
        private readonly Lazy<ExportProvider> partsContainer;

        /// <summary>
        /// Возвращает экземпляр <see cref="ViewModelStateMessagesContainer"/>, представляющий контейнер сообщений 
        /// о состоянии моделей представления.
        /// </summary>
        public ViewModelStateMessagesContainer StateMessagesContainer
        {
            get { return stateMessagesContainer.Value; }
        }
        private readonly Lazy<ViewModelStateMessagesContainer> stateMessagesContainer;

        /// <summary>
        /// Возвращает или задает признак того, является ли этот объект развернутым.
        /// </summary>
        public bool IsExpanded
        {
            get { return isExpanded; }
            set
            {
                if (isExpanded != value)
                {
                    isExpanded = value;

                    OnPropertyChanged(() => IsExpanded);

                    if (isExpanded && CanExpandParent())
                        Parent.IsExpanded = true;
                }
            }
        }
        private bool isExpanded;

        /// <summary>
        /// Возвращает или задает признак того, является ли этот объект выбранным.
        /// </summary>
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                if (isSelected != value)
                {
                    isSelected = value;
                    OnPropertyChanged(() => IsSelected);
                }
            }
        }
        private bool isSelected;

        /// <summary>
        /// Возвращает или задает признак того, является ли этот объект видимым.
        /// </summary>
        public bool IsVisible
        {
            get { return isVisible; }
            set
            {
                if (isVisible != value)
                {
                    isVisible = value;
                    OnPropertyChanged(() => IsVisible);
                }
            }
        }
        private bool isVisible;

        /// <summary>
        /// Возвращает или задает признак того, является ли этот объект занятым в настоящий момент времени.
        /// </summary>
        public bool IsBusy
        {
            get { return isBusy > 0; }
            set
            {
                var oldValue = isBusy > 0;

                if (value)
                    isBusy++;
                else
                    isBusy--;

                if (oldValue != (isBusy > 0))
                    OnPropertyChanged(() => IsBusy);
            }
        }
        private int isBusy;

        /// <summary>
        /// Возвращает или задает признак того, инициировано ли закрытие окна, 
        /// если эта модель представления отображается в окне.
        /// </summary>
        public bool IsClosingInitiated
        {
            get { return isClosingInitiated; }
            set
            {
                if (isClosingInitiated != value)
                {
                    isClosingInitiated = value;
                    OnPropertyChanged(() => IsClosingInitiated);
                }
            }
        }
        private bool isClosingInitiated;

        /// <summary>
        /// Возвращает или задает результат отображения модели представления в диалоге.
        /// </summary>
        public bool? DialogResult
        {
            get { return dialogResult; }
            set
            {
                if (dialogResult != value)
                {
                    dialogResult = value;
                    OnPropertyChanged(() => DialogResult);
                }
            }
        }
        private bool? dialogResult;

        #region Команды для взаимодействия с окнами

        /// <summary>
        /// Возвращает команду, которая выполняется при закрытии диалога по кнопке 'OK',
        /// если эта модель представления отображалась в диалоге.
        /// </summary>
        public RelayCommand AcceptCommand
        {
            get { return acceptCommand.Value; }
        }
        private readonly Lazy<RelayCommand> acceptCommand;

        /// <summary>
        /// Возвращает команду, которая выполняется при закрытии диалога по кнопке 'Отмена',
        /// если эта модель представления отображалась в диалоге.
        /// </summary>
        public RelayCommand CancelCommand
        {
            get { return cancelCommand.Value; } 
        }
        private readonly Lazy<RelayCommand> cancelCommand;

        /// <summary>
        /// Возвращает команду, которая выполняется при закрытии окна,
        /// если эта модель представления отображалась в окне.
        /// </summary>
        public RelayCommand ClosingCommand
        {
            get { return closingCommand.Value; }
        }
        private readonly Lazy<RelayCommand> closingCommand;

        #endregion

        #endregion

        #region Реализация IDataErrorInfo

        /// <summary>
        /// An error message indicating what is wrong with this object. The default is an empty string ("")
        /// </summary>
        string IDataErrorInfo.Error
        {
            get { return string.Empty; }
        }

        /// <summary>
        /// Gets the error message for the property with the given name
        /// </summary>
        /// <param name="columnName">The name of the property whose error message to get</param>
        /// <returns>
        /// The error message for the property. The default is an empty string ("")
        /// </returns>
        string IDataErrorInfo.this[string columnName]
        {
            get { return ValidateProperty(columnName); }
        }

        /// <summary>
        /// Проверяет наличие ошибок, связанных с конкретным свойством модели представления.
        /// </summary>
        /// <typeparam name="TProperty">
        /// Тип свойства.
        /// </typeparam>
        /// <param name="property">
        /// <see cref="LambdaExpression"/> для доступа к свойству.
        /// </param>
        /// <returns>
        /// <see langword="true"/>, если эта модель представления содержит ошибки, связанные со свойством <paramref name="property"/>;
        /// <see langword="false"/> в противном случае.
        /// </returns>
        public bool HasError<TProperty>(Expression<Func<TProperty>> property)
        {
            return !string.IsNullOrEmpty(((IDataErrorInfo)this)[property.GetMemberPathString()]);
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Генерирует событие <see cref="INotifyPropertyChanged.PropertyChanged"/>.
        /// </summary>
        /// <param name="propertyName">
        /// Наименование свойства объекта, значение которого изменилось.
        /// </param>
        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);

            CommandManager.InvalidateRequerySuggested();
        }

        #endregion
    }
}
