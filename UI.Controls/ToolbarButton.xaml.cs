using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace LayerCake.UI.Controls
{
    /// <summary>
    /// Interaction logic for ToolbarButton.xaml
    /// </summary>
    public partial class ToolbarButton : UserControl
    {
        #region Конструктор

        /// <summary>
        /// Инициализирует экземпляр <see cref="ToolbarButton"/>.
        /// </summary>
        public ToolbarButton()
        {
            InitializeComponent();
        }

        #endregion

        #region Dependency-свойства

        /// <summary>
        /// CLR-обертка для <see cref="ImageSourceProperty"/>.
        /// </summary>
        public ImageSource ImageSource
        {
            get { return (ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        /// <summary>
        /// Хранит источник изображения для кнопки.
        /// </summary>
        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(ToolbarButton), new UIPropertyMetadata(null));

        /// <summary>
        /// CLR-обертка для <see cref="ButtonContentProperty"/>.
        /// </summary>
        public Object ButtonContent
        {
            get { return (Object)GetValue(ButtonContentProperty); }
            set { SetValue(ButtonContentProperty, value); }
        }

        /// <summary>
        /// Хранит содержимое кнопки.
        /// </summary>
        public static readonly DependencyProperty ButtonContentProperty =
            DependencyProperty.Register("ButtonContent", typeof(Object), typeof(ToolbarButton), new UIPropertyMetadata(null));

        /// <summary>
        /// CLR-обертка для <see cref="CommandProperty"/>.
        /// </summary>
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        /// <summary>
        /// Хранит команду, связанную с кнопкой.
        /// </summary>
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(ToolbarButton), new UIPropertyMetadata(null));

        #endregion
    }
}
