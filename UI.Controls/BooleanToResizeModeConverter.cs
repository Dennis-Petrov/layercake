using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace LayerCake.UI.Controls
{
    /// <summary>
    /// Представляет реализацию <see cref="IValueConverter"/> для конвертации <see cref="Boolean"/> в <see cref="ResizeMode"/> и обратно.
    /// </summary>
    [ValueConversion(typeof(Boolean), typeof(Visibility))]
    public class BooleanToResizeModeConverter : IValueConverter
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
            if (value == null || value.GetType() != typeof(Boolean) || targetType != typeof(ResizeMode))
                return DependencyProperty.UnsetValue;

            return (Boolean)value ? ResizeMode.CanResize : ResizeMode.NoResize;
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
            if (value == null || value.GetType() != typeof(ResizeMode) || targetType != typeof(Boolean))
                return DependencyProperty.UnsetValue;

            return (ResizeMode)value == ResizeMode.CanResize;
        }

        #endregion
    }
}
