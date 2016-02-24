using System;
using System.Windows;
using System.Windows.Controls;

namespace LayerCake.UI.Behaviors
{
    /// <summary>
    /// Поведение, позволяющее связать <see cref="PasswordBox.Password"/> с источником данных.
    /// </summary>
    public static class PasswordBindingBehavior
    {
        #region Присоединенное свойство, позволяющее избежать рекурсивного обновления

        private static Boolean GetUpdatingPassword(DependencyObject dp)
        {
            return (Boolean)dp.GetValue(UpdatingPasswordProperty);
        }

        private static void SetUpdatingPassword(DependencyObject dp, Boolean value)
        {
            dp.SetValue(UpdatingPasswordProperty, value);
        }

        private static readonly DependencyProperty UpdatingPasswordProperty = DependencyProperty.RegisterAttached(
            "UpdatingPassword",
            typeof(Boolean),
            typeof(PasswordBindingBehavior),
            new PropertyMetadata(false));

        #endregion

        /// <summary>
        /// Get-метод для присоединенного свойства <see cref="PasswordProperty"/>.
        /// </summary>
        /// <param name="obj">
        /// <see cref="DependencyObject"/>, к которому присоединено свойство.
        /// </param>
        /// <returns>
        /// Значение присоединенного свойства.
        /// </returns>
        public static string GetPassword(DependencyObject obj)
        {
            return (string)obj.GetValue(PasswordProperty);
        }

        /// <summary>
        /// Set-метод для присоединенного свойства <see cref="PasswordProperty"/>.
        /// </summary>
        /// <param name="obj">
        /// <see cref="DependencyObject"/>, к которому присоединено свойство.
        /// </param>
        /// <param name="value">
        /// Значение присоединенного свойства.
        /// </param>
        public static void SetPassword(DependencyObject obj, String value)
        {
            obj.SetValue(PasswordProperty, value);
        }

        /// <summary>
        /// Присоединенное свойство, связывающее <see cref="PasswordBox.Password"/> с источником данных.
        /// </summary>
        public static readonly DependencyProperty PasswordProperty = DependencyProperty.RegisterAttached(
            "Password", 
            typeof(String),
            typeof(PasswordBindingBehavior), 
            new PropertyMetadata(String.Empty, OnPasswordPropertyChanged));

        private static void OnPasswordPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var passwordBox = d as PasswordBox;
            if (passwordBox != null)
            {
                passwordBox.PasswordChanged -= OnPasswordBoxPasswordChanged;
                try
                {
                    if (!GetUpdatingPassword(passwordBox))
                    {
                        passwordBox.Password = (String)e.NewValue;
                    }
                }
                finally
                {
                    passwordBox.PasswordChanged += OnPasswordBoxPasswordChanged;
                }
            }
        }

        private static void OnPasswordBoxPasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = (PasswordBox)sender;

            SetUpdatingPassword(passwordBox, true);
            try
            {
                SetPassword(passwordBox, passwordBox.Password);
            }
            finally
            {
                SetUpdatingPassword(passwordBox, false);
            }
        }
    }
}
