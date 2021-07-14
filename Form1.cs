using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SuppliesApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void buttonSelectAll_Click(object sender, EventArgs e)
        {
            for(int i = 0; i < checkedListBox.Items.Count; i++)
            {
                checkedListBox.SetItemChecked(i, true);
            }
        }
        private void buttonRemoveSelection_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox.Items.Count; i++)
            {
                checkedListBox.SetItemChecked(i, false);
            }
        }
        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            RefreshTable();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            RefreshTable();
        }
        private void RefreshTable()
        {
            //получить имена выбранных колонок
            List<string> checkedColumns = new List<string>();
            foreach (var item in checkedListBox.CheckedItems)
            {
                checkedColumns.Add(item.ToString());
            }
            //обновить таблицу
            TableRefresher.RefreshDataGridView(dgv, checkedColumns);
        }
    }
}
