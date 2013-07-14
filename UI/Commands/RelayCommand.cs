using System;

namespace LayerCake.UI.Commands
{
    /// <summary>
    /// Представляет реализацию <see cref="System.Windows.Input.ICommand"/> без параметров для использования в MVVM.
    /// </summary>
    public sealed class RelayCommand : RelayCommand<Object>
    {
        #region Конструкторы

        /// <summary>
        /// Инициализирует экзмеляр <see cref="RelayCommand"/>.
        /// </summary>
        /// <param name="execute">
        /// Делегат, обеспечивающий выполнение команды.
        /// </param>
        /// <param name="canExecute">
        /// Делегат, выполняющий проверку возможности выполнения команды.
        /// </param>
        public RelayCommand(Action execute, Func<Boolean> canExecute)
            : base(obj => execute(), obj => canExecute != null ? canExecute() : true)
        {
        }

        /// <summary>
        /// Инициализирует экзмеляр <see cref="RelayCommand"/>.
        /// </summary>
        /// <param name="execute">
        /// Делегат, обеспечивающий выполнение команды.
        /// </param>
        public RelayCommand(Action execute)
            : this(execute, null)
        {
        }

        #endregion
    }
}
