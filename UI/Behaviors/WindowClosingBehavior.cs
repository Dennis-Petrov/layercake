using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Windows;
using System.Windows.Input;

namespace LayerCake.UI.Behaviors
{
    /// <summary>
    /// Определяет поведение для связывания закрытия окна с командами модели представления.
    /// </summary>
    public static class WindowClosingBehavior
    {
        #region Команда, выполняемая при закрытии окна

        /// <summary>
        /// Get-метод для присоединенного свойства <see cref="ClosedProperty"/>.
        /// </summary>
        /// <param name="obj">
        /// <see cref="DependencyObject"/>, к которому присоединено свойство.
        /// </param>
        /// <returns>Значение присоединенного свойства</returns>
        public static ICommand GetClosed(DependencyObject obj)
        {
            Contract.Requires<ArgumentNullException>(obj != null);

            return (ICommand)obj.GetValue(ClosedProperty);
        }

        /// <summary>
        /// Set-метод для присоединенного свойства <see cref="ClosedProperty"/>.
        /// </summary>
        /// <param name="obj">
        /// <see cref="DependencyObject"/>, к которому присоединено свойство.
        /// </param>
        /// <param name="value">
        /// Значение присоединенного свойства.
        /// </param>
        public static void SetClosed(DependencyObject obj, ICommand value)
        {
            Contract.Requires<ArgumentNullException>(obj != null);

            obj.SetValue(ClosedProperty, value);
        }

        /// <summary>
        /// Присоединенное свойство-команда, выполняемая при закрытии окна.
        /// </summary>
        public static readonly DependencyProperty ClosedProperty = DependencyProperty.RegisterAttached(
            "Closed", 
            typeof(ICommand), 
            typeof(WindowClosingBehavior),
            new UIPropertyMetadata(new PropertyChangedCallback(ClosedChanged)));

        private static void ClosedChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            Window window = target as Window;
            
            if (window != null)
            {
                if (e.NewValue != null)
                {
                    window.Closed += Window_Closed;
                }
                else
                {
                    window.Closed -= Window_Closed;
                }
            }
        }

        private static void Window_Closed(object sender, EventArgs e)
        {
            ICommand closed = GetClosed(sender as Window);
            if (closed != null)
            {
                closed.Execute(null);
            }
        }

        #endregion

        #region Команда, выполняемая перед закрытием окна

        /// <summary>
        /// Get-метод для присоединенного свойства <see cref="ClosingProperty"/>.
        /// </summary>
        /// <param name="obj">
        /// <see cref="DependencyObject"/>, к которому присоединено свойство.
        /// </param>
        /// <returns>
        /// Значение присоединенного свойства.
        /// </returns>
        public static ICommand GetClosing(DependencyObject obj)
        {
            Contract.Requires<ArgumentNullException>(obj != null);

            return (ICommand)obj.GetValue(ClosingProperty);
        }

        /// <summary>
        /// Set-метод для присоединенного свойства <see cref="ClosingProperty"/>.
        /// </summary>
        /// <param name="obj">
        /// <see cref="DependencyObject"/>, к которому присоединено свойство.
        /// </param>
        /// <param name="value">
        /// Значение присоединенного свойства.
        /// </param>
        public static void SetClosing(DependencyObject obj, ICommand value)
        {
            Contract.Requires<ArgumentNullException>(obj != null);

            obj.SetValue(ClosingProperty, value);
        }

        /// <summary>
        /// Присоединенное свойство-команда, выполняемая перед закрытием окна.
        /// </summary>
        public static readonly DependencyProperty ClosingProperty = DependencyProperty.RegisterAttached(
            "Closing", 
            typeof(ICommand), 
            typeof(WindowClosingBehavior),
            new UIPropertyMetadata(new PropertyChangedCallback(ClosingChanged)));

        private static void ClosingChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            Window window = target as Window;

            if (window != null)
            {
                if (e.NewValue != null)
                {
                    window.Closing += Window_Closing;
                }
                else
                {
                    window.Closing -= Window_Closing;
                }
            }
        }

        private static void Window_Closing(object sender, CancelEventArgs e)
        {
            ICommand closing = GetClosing(sender as Window);
            if (closing != null)
            {
                if (closing.CanExecute(null))
                {
                    closing.Execute(null);
                }
                else
                {
                    ICommand cancelClosing = GetCancelClosing(sender as Window);
                    if (cancelClosing != null)
                    {
                        cancelClosing.Execute(null);
                    }

                    e.Cancel = true;
                }
            }
        }

        #endregion

        #region Команда, выполняемая в случае отмены закрытия окна

        /// <summary>
        /// Get-метод для присоединенного свойства <see cref="CancelClosingProperty"/>.
        /// </summary>
        /// <param name="obj">
        /// <see cref="DependencyObject"/>, к которому присоединено свойство.
        /// </param>
        /// <returns>
        /// Значение присоединенного свойства.
        /// </returns>
        public static ICommand GetCancelClosing(DependencyObject obj)
        {
            Contract.Requires<ArgumentNullException>(obj != null);

            return (ICommand)obj.GetValue(CancelClosingProperty);
        }

        /// <summary>
        /// Set-метод для присоединенного свойства <see cref="CancelClosingProperty"/>.
        /// </summary>
        /// <param name="obj">
        /// <see cref="DependencyObject"/>, к которому присоединено свойство.
        /// </param>
        /// <param name="value">
        /// Значение присоединенного свойства.
        /// </param>
        public static void SetCancelClosing(DependencyObject obj, ICommand value)
        {
            Contract.Requires<ArgumentNullException>(obj != null);

            obj.SetValue(CancelClosingProperty, value);
        }

        /// <summary>
        /// Присоединенное свойство-команда, выполняемая в случае отмены закрытия окна.
        /// </summary>
        public static readonly DependencyProperty CancelClosingProperty = DependencyProperty.RegisterAttached(
            "CancelClosing", 
            typeof(ICommand), 
            typeof(WindowClosingBehavior));

        #endregion

        #region Присоединенное свойство, позволяющее инициировать закрытие окна из модели представления

        /// <summary>
        /// Возвращает значение <see cref="WindowClosingBehavior.IsClosingInitiatedProperty"/>.
        /// </summary>
        /// <param name="obj">
        /// <see cref="DependencyObject"/>, к которому присоединено свойство.
        /// </param>
        /// <returns>
        /// <see langword="true"/>, если инициировано закрытие окна;
        /// <see langword="false"/> в противном случае.
        /// </returns>
        public static Boolean GetIsClosingInitiated(DependencyObject obj)
        {
            Contract.Requires<ArgumentNullException>(obj != null);

            return (Boolean)obj.GetValue(IsClosingInitiatedProperty);
        }

        /// <summary>
        /// Устанавливает значение <see cref="WindowClosingBehavior.IsClosingInitiatedProperty"/>.
        /// </summary>
        /// <param name="obj">
        /// <see cref="DependencyObject"/>, к которому присоединено свойство.
        /// </param>
        /// <param name="value">
        /// Значение <see cref="WindowClosingBehavior.IsClosingInitiatedProperty"/>, присоединенное к заданному объекту.
        /// </param>
        public static void SetIsClosingInitiated(DependencyObject obj, Boolean value)
        {
            Contract.Requires<ArgumentNullException>(obj != null);

            obj.SetValue(IsClosingInitiatedProperty, value);
        }

        /// <summary>
        /// Присоединенное свойство, позволяющее инициировать закрытие окна из модели представления.
        /// </summary>
        public static readonly DependencyProperty IsClosingInitiatedProperty = DependencyProperty.RegisterAttached(
            "IsClosingInitiated", 
            typeof(Boolean), 
            typeof(WindowClosingBehavior),
            new UIPropertyMetadata(false, IsClosingInitiatedChanged));

        private static void IsClosingInitiatedChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            Window window = target as Window;

            if (window != null && (Boolean)e.NewValue)
            {
                window.Close();
            }
        }

        #endregion
    }
}
