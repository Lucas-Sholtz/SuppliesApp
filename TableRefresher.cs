using System.Collections.Generic;
using System.Windows.Forms;

namespace SuppliesApp
{
    static class TableRefresher
    {
        //в массиве указаны имена колонок по порядку
        private static readonly string[] ColumnOrder = { "ShippingDate", "Organisation", "City", "Country", "Manager", "Count", "Sum" };
        public static void RefreshDataGridView(DataGridView dgv, List<string> checkedColumns) //обновляет таблицу
        {
            //изменить содержимое
            dgv.DataSource = QuerySender.SendQuery(checkedColumns);
            //и порядок колонок
            ReorderDataGridViewColumns(dgv);
        }
        private static void ReorderDataGridViewColumns(DataGridView dgv) //изменяет порядок колонок
        {
            //получаем пары имя-номер
            Dictionary<string, int> columnIndexPair = AssignColumnIndexes(dgv);
            //для каждой колонки меняем порядок отображения
            foreach (DataGridViewColumn column in dgv.Columns)
            {
                column.DisplayIndex = columnIndexPair[column.Name];
            }
        }
        private static Dictionary<string, int> AssignColumnIndexes(DataGridView dgv) //генерирует пары имя-индекс
        {
            Dictionary<string, int> columnIndexPairs = new Dictionary<string, int>();
            //получаем названия колонок в таблице
            List<string> dgvColumnNames = new List<string>();
            foreach (DataGridViewColumn column in dgv.Columns)
            {
                dgvColumnNames.Add(column.HeaderText);
            }
            //если имя колонки есть в списке
            int index = 0;
            foreach (var name in ColumnOrder)
            {
                if (dgvColumnNames.Contains(name))
                {
                    //присваеваем ей номер
                    columnIndexPairs[name] = index;
                    //увеличиваем номер на 1
                    index++;
                }
            }

            return columnIndexPairs;
        }
    }
}

