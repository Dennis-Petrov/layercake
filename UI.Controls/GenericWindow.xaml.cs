using System.Windows;
using System.Windows.Controls;

namespace LayerCake.UI.Controls
{
    /// <summary>
    /// Interaction logic for GenericWindow.xaml
    /// </summary>
    public partial class GenericWindow : Window
    {
        #region Конструктор

        /// <summary>
        /// Инициализирует экземпляр <see cref="GenericWindow"/>.
        /// </summary>
        public GenericWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Public-свойства

        /// <summary>
        /// Возвращает экземпляр <see cref="ContentControl"/>, предназначенный для размещения полезного содержимого окна.
        /// </summary>
        public ContentControl ContentPlaceholder
        {
            get { return contentPlaceholder; }
        }

        #endregion
    }
}
