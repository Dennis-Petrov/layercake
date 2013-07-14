using System;
using System.Windows;
using System.Windows.Controls;

namespace LayerCake.UI.Controls
{
    /// <summary>
    /// Interaction logic for PropertyGroupHeader.xaml
    /// </summary>
    public partial class PropertyGroupHeader : UserControl
    {
        #region Конструктор

        /// <summary>
        /// Инициализирует экземпляр <see cref="PropertyGroupHeader"/>.
        /// </summary>
        public PropertyGroupHeader()
        {
            InitializeComponent();
        }

        #endregion

        #region Dependency-свойства

        /// <summary>
        /// CLR-обертка для <see cref="GroupNameProperty"/>.
        /// </summary>
        public String GroupName
        {
            get { return (String)GetValue(GroupNameProperty); }
            set { SetValue(GroupNameProperty, value); }
        }

        /// <summary>
        /// Хранит наименование группы свойств.
        /// </summary>
        public static readonly DependencyProperty GroupNameProperty = DependencyProperty.Register(
            "GroupName", 
            typeof(String), 
            typeof(PropertyGroupHeader), 
            new UIPropertyMetadata(String.Empty));

        #endregion
    }
}
