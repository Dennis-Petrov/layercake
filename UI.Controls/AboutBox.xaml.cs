using System.Windows;

namespace LayerCake.UI.Controls
{
    /// <summary>
    /// Представляет окно "О программе".
    /// </summary>
    public partial class AboutBox : Window
    {
        /// <summary>
        /// Инициализирует экземпляр <see cref="AboutBox"/>.
        /// </summary>
        public AboutBox()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
