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
    /// Логика взаимодействия для EditUserDataWindow.xaml
    /// </summary>
    public partial class EditUserDataWindow : Window
    {
        public EditUserDataWindow()
        {
            InitializeComponent();
            rbnAdministrator.IsChecked = true;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message} Пожалуйста, введите все данные корректно", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

}
