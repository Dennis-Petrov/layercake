using System;
using System.Diagnostics;
using System.Windows.Input;

namespace LayerCake.UI.Commands
{
    /// <summary>
    /// Представляет реализацию <see cref="ICommand"/> для использования в MVVM.
    /// </summary>
    /// <typeparam name="T">
    /// Тип параметра команды.
    /// </typeparam>
    public class RelayCommand<T> : ICommand
    {
        #region Поля

        private readonly Action<T> execute;
        private readonly Func<T, Boolean> canExecute;

        #endregion

        #region Конструкторы
        
        /// <summary>
        /// Инициализирует экзмеляр <see cref="RelayCommand{T}"/>.
        /// </summary>
        /// <param name="execute">
        /// Делегат, обеспечивающий выполнение команды.
        /// </param>
        public RelayCommand(Action<T> execute)
            : this(execute, null)
        {
        }

        /// <summary>
        /// Инициализирует экзмеляр <see cref="RelayCommand{T}"/>.
        /// </summary>
        /// <param name="execute">
        /// Делегат, обеспечивающий выполнение команды.
        /// </param>
        /// <param name="canExecute">
        /// Делегат, выполняющий проверку возможности выполнения команды.
        /// </param>
        public RelayCommand(Action<T> execute, Func<T, Boolean> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            this.execute = execute;
            this.canExecute = canExecute;
        }

        #endregion

        #region Реализация ICommand

        /// <summary>
        /// Определяет, может ли команда быть выполнена в ее текущем состоянии.
        /// </summary>
        /// <param name="parameter">
        /// Параметр, используемый командой.
        /// </param>
        /// <returns>
        /// <see langword="true"/>, если команда может быть выполнена;
        /// <see langword="false"/> в противном случае.
        /// </returns>
        [DebuggerStepThrough]
        public Boolean CanExecute(Object parameter)
        {
            return canExecute == null ? true : canExecute((T)parameter);
        }

        /// <summary>
        /// Событие, которое генерируется, если произошли изменения, влияющие на способность команды быть выполненной.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Выполняет команду.
        /// </summary>
        /// <param name="parameter">
        /// Параметр, используемый командой.
        /// </param>
        public void Execute(Object parameter)
        {
            execute((T)parameter);
        }

        #endregion
    }
}
