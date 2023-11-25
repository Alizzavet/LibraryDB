using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Library
{
    /// <summary>
    /// Логика взаимодействия для EditWindow.xaml
    /// </summary>
    public partial class EditWindow : Window
    {
        private DataRow row;
        private Dictionary<string, Control> controls = new Dictionary<string, Control>();

        public EditWindow(DataRow row)
        {
            InitializeComponent();
            this.row = row;

            foreach (DataColumn column in row.Table.Columns)
            {
                if (column.DataType == typeof(DateTime))
                {
                    DatePicker datePicker = new DatePicker();
                    if (row[column] != DBNull.Value)
                    {
                        datePicker.SelectedDate = (DateTime)row[column];
                    }
                    stackPanel.Children.Insert(stackPanel.Children.Count - 2, datePicker);
                    controls.Add(column.ColumnName, datePicker);
                }
                else
                {
                    TextBox textBox = new TextBox { Text = row[column].ToString() };
                    textBox.ToolTip = column.ColumnName;
                    if (column == row.Table.Columns[0])
                    {
                        textBox.IsEnabled = false;
                    }
                    stackPanel.Children.Insert(stackPanel.Children.Count - 2, textBox);
                    controls.Add(column.ColumnName, textBox);
                }
            }
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            foreach (DataColumn column in row.Table.Columns)
            {
                if (column.DataType == typeof(DateTime))
                {
                    DatePicker datePicker = (DatePicker)controls[column.ColumnName];
                    if (datePicker.SelectedDate.HasValue)
                    {
                        row[column] = datePicker.SelectedDate.Value;
                    }
                    else
                    {
                        row[column] = DBNull.Value;
                    }
                }
                else
                {
                    TextBox textBox = (TextBox)controls[column.ColumnName];
                    if (string.IsNullOrWhiteSpace(textBox.Text))
                    {
                        MessageBox.Show($"Поле {column.ColumnName} не может быть пустым.");
                        return;
                    }
                    row[column] = textBox.Text;
                }
            }
            this.DialogResult = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
