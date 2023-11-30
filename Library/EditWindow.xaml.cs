using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
        private ComboBoxHelper comboBoxHelper = new ComboBoxHelper();

        public EditWindow(DataRow row, bool isNewRecord)
        {
            InitializeComponent();
            this.row = row;
            TextBox textBox;

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
                    switch (column.ColumnName)
                    {
                        case "LibrarianID":
                            if (row.Table.TableName == "Librarians")
                            {
                                textBox = CreateTextBox(row, column);
                            }
                            else
                            {
                                ComboBox comboBoxLibrarians = CreateComboBox("comboBoxLibrarians", row, column);
                                comboBoxHelper.FillLibrariansComboBox(comboBoxLibrarians);
                            }
                            break;
                        case "LibraryRoomID":
                            if (row.Table.TableName == "LibraryRooms")
                            {
                                textBox = CreateTextBox(row, column);
                            }
                            else
                            {
                                ComboBox comboBoxLibraryRooms = CreateComboBox("comboBoxLibraryRooms", row, column);
                                comboBoxHelper.FillLibraryRoomsComboBox(comboBoxLibraryRooms);
                            }
                            break;
                        case "WorkID":
                            if (row.Table.TableName == "Works")
                            {
                                textBox = CreateTextBox(row, column);
                            }
                            else
                            {
                                ComboBox comboBoxWorks = CreateComboBox("comboBoxWorks", row, column);
                                comboBoxHelper.FillWorksComboBox(comboBoxWorks);
                            }
                            break;
                        case "BookID":
                            if (row.Table.TableName == "Books")
                            {
                                textBox = CreateTextBox(row, column);
                            }
                            else
                            {
                                ComboBox comboBoxBooks = CreateComboBox("comboBoxBooks", row, column);
                                comboBoxHelper.FillBooksComboBox(comboBoxBooks);
                            }
                            break;
                        case "AuthorID":
                            if (row.Table.TableName == "Authors")
                            {
                                textBox = CreateTextBox(row, column);
                            }
                            else
                            {
                                ComboBox comboBoxAuthors = CreateComboBox("comboBoxAuthors", row, column);
                                comboBoxHelper.FillAuthorsComboBox(comboBoxAuthors);
                            }
                            break;
                        case "GenreID":
                            if (row.Table.TableName == "Genres")
                            {
                                textBox = CreateTextBox(row, column);
                            }
                            else
                            {
                                ComboBox comboBoxGenres = CreateComboBox("comboBoxGenres", row, column);
                                comboBoxHelper.FillGenresComboBox(comboBoxGenres);
                            }
                            break;
                        case "PublisherID":
                            if (row.Table.TableName == "Publishers")
                            {
                                textBox = CreateTextBox(row, column);
                            }
                            else
                            {
                                ComboBox comboBoxPublishers = CreateComboBox("comboBoxPublishers", row, column);
                                comboBoxHelper.FillPublishersComboBox(comboBoxPublishers);
                            }
                            break;
                        case "ActID":
                            if (row.Table.TableName == "Acts")
                            {
                                textBox = CreateTextBox(row, column);
                            }
                            else
                            {
                                ComboBox comboBoxActs = CreateComboBox("comboBoxActs", row, column);
                                comboBoxHelper.FillActsComboBox(comboBoxActs);
                            }
                            break;
                        case "ShelfID":
                            if (row.Table.TableName == "Shelves")
                            {
                                textBox = CreateTextBox(row, column);
                            }
                            else
                            {
                                ComboBox comboBoxShelves = CreateComboBox("comboBoxShelves", row, column);
                                comboBoxHelper.FillShelvesComboBox(comboBoxShelves);
                            }
                            break;
                        case "SectionID":
                            if (row.Table.TableName == "Sections")
                            {
                                textBox = CreateTextBox(row, column);
                            }
                            else
                            {
                                ComboBox comboBoxSections = CreateComboBox("comboBoxSections", row, column);
                                comboBoxHelper.FillSectionsComboBox(comboBoxSections);
                            }
                            break;
                        case "SubscriptionID":
                            if (row.Table.TableName == "Subscriptions")
                            {
                                textBox = CreateTextBox(row, column);
                            }
                            else
                            {
                                ComboBox comboBoxSubscriptions = CreateComboBox("comboBoxSubscriptions", row, column);
                                comboBoxHelper.FillSubscriptionsComboBox(comboBoxSubscriptions);
                            }
                            break;
                        case "IsAvailable":
                            
                                ComboBox comboBoxAvailability = CreateComboBox("comboBoxAvailability", row, column);
                                comboBoxHelper.FillAvailabilityComboBox(comboBoxAvailability);
                            
                            break;
                        case "BookInventoryID":
                            if (row.Table.TableName == "BooksInventorisation")
                            {
                                textBox = CreateTextBox(row, column);
                            }
                            else
                            {
                                ComboBox comboBoxBookInventory = CreateComboBox("comboBoxBookInventory", row, column);
                                comboBoxHelper.FillBookInventoryComboBox(comboBoxBookInventory);
                            }
                            break;
                        case "ActionType":
                            
                                ComboBox comboBoxActionType = CreateComboBox("comboBoxActionType", row, column);
                                comboBoxHelper.FillActionTypeComboBox(comboBoxActionType);
                            
                            break;


                        default:
                            textBox = CreateTextBox(row, column);
                            break;
                    }
                }
            }
        }

        private ComboBox CreateComboBox(string name, DataRow row, DataColumn column)
        {
            ComboBox comboBox = new ComboBox
            {
                Name = name,
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(10, 0, 0, 10),
                VerticalAlignment = VerticalAlignment.Top,
                Width = 200
            };
            stackPanel.Children.Insert(stackPanel.Children.Count - 2, comboBox);
            controls.Add(column.ColumnName, comboBox);

            if (int.TryParse(row[column].ToString(), out int id))
            {
                comboBox.SelectedValue = id;
            }
            else
            {
                comboBox.SelectedValue = null;
            }

            return comboBox;
        }

        private TextBox CreateTextBox(DataRow row, DataColumn column)
        {
            TextBox textBox = new TextBox { Text = row[column].ToString() };
            textBox.ToolTip = column.ColumnName;
            if (column == row.Table.Columns[0])
            {
                textBox.IsEnabled = false;
            }
            stackPanel.Children.Insert(stackPanel.Children.Count - 2, textBox);
            controls.Add(column.ColumnName, textBox);

            return textBox;
        }


        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            try
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
                            MessageBox.Show($"Поле {column.ColumnName} не может быть пустым.");
                            return;
                        }
                    }
                    else
                    {
                        if (controls[column.ColumnName] is TextBox textBox)
                        {
                            if (string.IsNullOrWhiteSpace(textBox.Text))
                            {
                                MessageBox.Show($"Поле {column.ColumnName} не может быть пустым.");
                                return;
                            }
                            row[column] = textBox.Text;
                        }
                        else if (controls[column.ColumnName] is ComboBox comboBox)
                        {
                            if (comboBox.SelectedValue != null)
                            {
                                KeyValuePair<int, string> selectedPair = (KeyValuePair<int, string>)comboBox.SelectedValue;
                                row[column] = selectedPair.Key;
                            }
                            else
                            {
                                MessageBox.Show($"Поле {column.ColumnName} не может быть пустым.");
                                return;
                            }
                        }
                    }
                }
                this.DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message} Пожалуйста, заполните все данные.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}