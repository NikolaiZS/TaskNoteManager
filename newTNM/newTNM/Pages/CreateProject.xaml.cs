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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace newTNM.Pages
{
    /// <summary>
    /// Логика взаимодействия для CreateProject.xaml
    /// </summary>
    public partial class CreateProject : Page
    {
        public CreateProject()
        {
            InitializeComponent();
        }

        private void CreateProjectButton_Click(object sender, RoutedEventArgs e)
        {
            string projectName = ProjectNameTextBox.Text.Trim();
            string projectDescription = ProjectDescriptionTextBox.Text.Trim();

            // Проверка на пустое название
            if (string.IsNullOrWhiteSpace(projectName))
            {
                MessageBox.Show("Введите название проекта.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Проверка длины названия
            if (projectName.Length > 20)
            {
                MessageBox.Show("Название проекта не должно превышать 20 символов.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Проверка длины описания
            if (projectDescription.Length > 100)
            {
                MessageBox.Show("Описание проекта не должно превышать 100 символов.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Логика сохранения проекта (фир работать)
            MessageBox.Show($"Проект '{projectName}' успешно создан!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

            // Очистить поля после создания
            ProjectNameTextBox.Clear();
            ProjectDescriptionTextBox.Clear();
        }
    }
}
