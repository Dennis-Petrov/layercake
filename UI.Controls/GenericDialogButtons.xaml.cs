using System;
using System.Windows;
using System.Windows.Controls;

namespace LayerCake.UI.Controls
{
    /// <summary>
    /// Interaction logic for GenericDialogButtons.xaml
    /// </summary>
    public partial class GenericDialogButtons : UserControl
    {
        #region Конструктор

        /// <summary>
        /// Инициализирует экземпляр <see cref="GenericDialogButtons"/>.
        /// </summary>
        public GenericDialogButtons()
        {
            InitializeComponent();
        }

        #endregion

        #region Dependency-свойства

        /// <summary>
        /// CLR-обертка для <see cref="IsOkButtonDefaultProperty"/>.
        /// </summary>
        public Boolean IsOkButtonDefault
        {
            get { return (Boolean)GetValue(IsOkButtonDefaultProperty); }
            set { SetValue(IsOkButtonDefaultProperty, value); }
        }

        /// <summary>
        /// <see langword="true"/>, если кнопка "OK" является кнопкой по умолчанию в диалоге;
        /// <see langword="false"/> в противном случае.
        /// </summary>
        public static readonly DependencyProperty IsOkButtonDefaultProperty = DependencyProperty.Register(
            "IsOkButtonDefault", 
            typeof(Boolean), 
            typeof(GenericDialogButtons), 
            new UIPropertyMetadata(true));

        #endregion
    }
}
