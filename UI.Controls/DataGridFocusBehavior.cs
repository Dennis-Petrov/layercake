using System.Windows;
using System.Windows.Controls;

namespace LayerCake.UI.Controls
{
    /// <summary>
    /// Представляет присоединенное поведение для оповещения о потере фокуса экземпляром <see cref="System.Windows.Controls.DataGrid"/>.
    /// </summary>
    public static class DataGridFocusBehavior
    {
        #region Присоединенное свойство, определяющее наличие ячейки в фокусе

        /// <summary>
        /// Возвращает значение <see cref="HasFocusedCellProperty"/> для заданного объекта.
        /// </summary>
        /// <param name="obj">
        /// Объект, для которого нужно получить значение <see cref="HasFocusedCellProperty"/>.
        /// </param>
        /// <returns>
        /// Значение <see cref="HasFocusedCellProperty"/>.
        /// </returns>
        public static bool GetHasFocusedCell(DependencyObject obj)
        {
            return (bool)obj.GetValue(HasFocusedCellProperty);
        }

        /// <summary>
        /// Задает значение <see cref="HasFocusedCellProperty"/> для заданного объекта.
        /// </summary>
        /// <param name="obj">
        /// Объект, для которого нужно задать значение <see cref="HasFocusedCellProperty"/>.
        /// </param>
        /// <param name="value">
        /// Значение <see cref="HasFocusedCellProperty"/>.
        /// </param>
        public static void SetHasFocusedCell(DependencyObject obj, bool value)
        {
            obj.SetValue(HasFocusedCellProperty, value);
        }

        /// <summary>
        /// Присоединенное свойство, определяющее наличие у <see cref="System.Windows.Controls.DataGrid"/> ячейки в фокусе.
        /// </summary>
        public static readonly DependencyProperty HasFocusedCellProperty = DependencyProperty.RegisterAttached(
            "HasFocusedCell",
            typeof(bool),
            typeof(DataGridFocusBehavior),
            new UIPropertyMetadata(false));

        #endregion

        #region Присоединенное свойство, определяющее активность родительского окна

        /// <summary>
        /// Возвращает значение <see cref="IsParentWindowActiveProperty"/> для заданного объекта.
        /// </summary>
        /// <param name="obj">
        /// Объект, для которого нужно получить значение <see cref="IsParentWindowActiveProperty"/>.
        /// </param>
        /// <returns>
        /// Значение <see cref="IsParentWindowActiveProperty"/>.
        /// </returns>
        public static bool GetIsParentWindowActive(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsParentWindowActiveProperty);
        }

        /// <summary>
        /// Задает значение <see cref="IsParentWindowActiveProperty"/> для заданного объекта.
        /// </summary>
        /// <param name="obj">
        /// Объект, для которого нужно задать значение <see cref="IsParentWindowActiveProperty"/>.
        /// </param>
        /// <param name="value">
        /// Значение <see cref="IsParentWindowActiveProperty"/>.
        /// </param>
        public static void SetIsParentWindowActive(DependencyObject obj, bool value)
        {
            obj.SetValue(IsParentWindowActiveProperty, value);
        }

        /// <summary>
        /// Присоединенное свойство, определяющее активность родительского окна 
        /// для экземпляра <see cref="System.Windows.Controls.DataGrid"/>.
        /// </summary>
        public static readonly DependencyProperty IsParentWindowActiveProperty = DependencyProperty.RegisterAttached(
            "IsParentWindowActive",
            typeof(bool),
            typeof(DataGridFocusBehavior),
            new UIPropertyMetadata(false));

        #endregion

        #region Присоединенное свойство, устанавливающее стиль ячейки

        /// <summary>
        /// Возвращает значение <see cref="CellStyleProperty"/> для заданного объекта.
        /// </summary>
        /// <param name="obj">
        /// Объект, для которого нужно получить значение <see cref="CellStyleProperty"/>.
        /// </param>
        /// <returns>
        /// Значение <see cref="CellStyleProperty"/>.
        /// </returns>
        public static Style GetCellStyle(DependencyObject obj)
        {
            return (Style)obj.GetValue(CellStyleProperty);
        }

        /// <summary>
        /// Задает значение <see cref="CellStyleProperty"/> для заданного объекта.
        /// </summary>
        /// <param name="obj">
        /// Объект, для которого нужно задать значение <see cref="CellStyleProperty"/>.
        /// </param>
        /// <param name="value">
        /// Значение <see cref="CellStyleProperty"/>.
        /// </param>
        public static void SetCellStyle(DependencyObject obj, Style value)
        {
            obj.SetValue(CellStyleProperty, value);
        }

        /// <summary>
        /// Присоединенное свойство, устанавливающее <see cref="DataGrid.CellStyleProperty"/>.
        /// </summary>
        public static readonly DependencyProperty CellStyleProperty = DependencyProperty.RegisterAttached(
            "CellStyle", 
            typeof(Style), 
            typeof(DataGridFocusBehavior), 
            new UIPropertyMetadata(null, OnCellStyleChanged));

        private static void OnCellStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var grid = d as DataGrid;
            if (grid != null)
            {
                grid.CellStyle = (Style)e.NewValue;
            }
        }

        #endregion

        #region Присоединенное свойство, устанавливающее стиль строки

        /// <summary>
        /// Возвращает значение <see cref="RowStyleProperty"/> для заданного объекта.
        /// </summary>
        /// <param name="obj">
        /// Объект, для которого нужно получить значение <see cref="RowStyleProperty"/>.
        /// </param>
        /// <returns>
        /// Значение <see cref="RowStyleProperty"/>.
        /// </returns>
        public static Style GetRowStyle(DependencyObject obj)
        {
            return (Style)obj.GetValue(RowStyleProperty);
        }

        /// <summary>
        /// Задает значение <see cref="RowStyleProperty"/> для заданного объекта.
        /// </summary>
        /// <param name="obj">
        /// Объект, для которого нужно задать значение <see cref="RowStyleProperty"/>.
        /// </param>
        /// <param name="value">
        /// Значение <see cref="RowStyleProperty"/>.
        /// </param>
        public static void SetRowStyle(DependencyObject obj, Style value)
        {
            obj.SetValue(RowStyleProperty, value);
        }

        /// <summary>
        /// Присоединенное свойство, устанавливающее <see cref="DataGrid.RowStyle"/>.
        /// </summary>
        public static readonly DependencyProperty RowStyleProperty = DependencyProperty.RegisterAttached(
            "RowStyle", 
            typeof(Style), 
            typeof(DataGridFocusBehavior),
            new UIPropertyMetadata(null, OnRowStyleChanged));

        private static void OnRowStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var grid = d as DataGrid;
            if (grid != null)
            {
                grid.RowStyle = (Style)e.NewValue;
            }
        }

        #endregion
    }
}
