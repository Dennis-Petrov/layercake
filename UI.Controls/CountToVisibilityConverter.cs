using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace LayerCake.UI.Controls
{
    /// <summary>
    /// Представляет реализацию <see cref="IValueConverter"/> для конвертации <see cref="Int32"/>, 
    /// представляющего число элементов в коллекции, в <see cref="Visibility"/> и обратно.
    /// </summary>
    [ValueConversion(typeof(Int32), typeof(Visibility))]
    public sealed class CountToVisibilityConverter : IValueConverter
    {
        #region Реализация IValueConverter

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">
        /// The value produced by the binding source.
        /// </param>
        /// <param name="targetType">
        /// The type of the binding target property.
        /// </param>
        /// <param name="parameter">
        /// The converter parameter to use.
        /// </param>
        /// <param name="culture">
        /// The culture to use in the converter.
        /// </param>
        /// <returns>
        /// A converted value. If the method returns <see langword="null"/>, the valid <see langword="null"/> value is used.
        /// </returns>
        public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            if (value == null || value.GetType() != typeof(Int32) || targetType != typeof(Visibility))
                // целевой тип не поддерживается, либо значение не является Int32
                return DependencyProperty.UnsetValue;

            return (Int32)value > 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">
        /// The value that is produced by the binding target.
        /// </param>
        /// <param name="targetType">
        /// The type to convert to.
        /// </param>
        /// <param name="parameter">
        /// The converter parameter to use.
        /// </param>
        /// <param name="culture">
        /// The culture to use in the converter.
        /// </param>
        /// <returns>
        /// A converted value. If the method returns <see langword="null"/>, the valid <see langword="null"/> value is used.
        /// </returns>
        public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            // обратная конвертация не поддерживается
            return DependencyProperty.UnsetValue;
        }

        #endregion    
    }
}
