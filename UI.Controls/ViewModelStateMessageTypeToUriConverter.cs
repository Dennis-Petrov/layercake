using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LayerCake.UI.Controls
{
    /// <summary>
    /// Представляет реализацию <see cref="IValueConverter"/> для конвертации <see cref="ViewModelStateMessageType"/> в <see cref="ImageSource"/>.
    /// </summary>
    [ValueConversion(typeof(ViewModelStateMessageType), typeof(ImageSource))]
    public sealed class ViewModelStateMessageTypeToUriConverter : IValueConverter
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
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null || value.GetType() != typeof(ViewModelStateMessageType) || targetType != typeof(ImageSource))
                return DependencyProperty.UnsetValue;

            BitmapImage imageSource = null;

            switch ((ViewModelStateMessageType)value)
            {
                case ViewModelStateMessageType.Information:
                    imageSource = new BitmapImage(new Uri("pack://application:,,,/LayerCake.UI.Controls;component/Images/Information.png"));
                    break;
                case ViewModelStateMessageType.Warining:
                    imageSource = new BitmapImage(new Uri("pack://application:,,,/LayerCake.UI.Controls;component/Images/Warning.png"));
                    break;
                case ViewModelStateMessageType.Error:
                    imageSource = new BitmapImage(new Uri("pack://application:,,,/LayerCake.UI.Controls;component/Images/Error.png"));
                    break;
            }

            return imageSource;
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
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }

        #endregion
    }
}
