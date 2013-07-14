using System;
using System.Windows;
using System.Windows.Controls;

namespace LayerCake.UI.Controls
{
    /// <summary>
    /// Interaction logic for PropertyNameBlock.xaml
    /// </summary>
    public partial class PropertyNameBlock : UserControl
    {
        #region Конструктор

        /// <summary>
        /// Инициализирует экземпляр <see cref="PropertyNameBlock"/>.
        /// </summary>
        public PropertyNameBlock()
        {
            InitializeComponent();
        }

        #endregion

        #region Dependency-свойства

        /// <summary>
        /// Хранит наименование свойства модели.
        /// </summary>
        public static readonly DependencyProperty PropertyNameProperty =
           DependencyProperty.Register(
              "PropertyName",
              typeof(String),
              typeof(PropertyNameBlock),
              new FrameworkPropertyMetadata(null));

        /// <summary>
        /// CLR-обертка для <see cref="PropertyNameProperty"/>.
        /// </summary>
        public String PropertyName
        {
            get { return (String)GetValue(PropertyNameProperty); }
            set { SetValue(PropertyNameProperty, value); }
        }

        #endregion
    }
}
