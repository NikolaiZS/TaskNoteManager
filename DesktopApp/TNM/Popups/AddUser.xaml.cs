using System;
using System.Collections.Generic;
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

namespace TNM.Popups
{
    /// <summary>
    /// Логика взаимодействия для AddUser.xaml
    /// </summary>
    public partial class AddUser
    {
        private readonly List<string> systemUsers; // Список всех пользователей
        private readonly Func<string, bool> IsUserAssigned; // Метод проверки добавленных пользователей
        private readonly Action<string> AddAssigned; // Метод добавления пользователя

        public AddUser(/*List<string> users, Func<string, bool> isUserAssigned, Action<string> addAssigned*/)
        {
            InitializeComponent();

            //systemUsers = users;
            //IsUserAssigned = isUserAssigned;
            //AddAssigned = addAssigned;

            // Заполнение ComboBox пользователями
            UserSelectionComboBox.ItemsSource = systemUsers.Where(user => !IsUserAssigned(user)).ToList();

            // Проверяем, есть ли доступные пользователи
            if (!UserSelectionComboBox.Items.OfType<string>().Any())
            {
                MessageBox.Show("Все пользователи уже добавлены.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (UserSelectionComboBox.SelectedItem is string selectedUser)
            {
                AddAssigned(selectedUser);
                MessageBox.Show($"Пользователь {selectedUser} добавлен.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите пользователя.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}