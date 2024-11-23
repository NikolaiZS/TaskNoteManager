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

namespace TNM.Pages
{
    /// <summary>
    /// Логика взаимодействия для EditProject.xaml
    /// </summary>
    public partial class EditProject : Page
    {
        //public static Projects SelectedProject { get; set; }

        //private Projects _project;

        // Конструктор для передачи данных
        public EditProject()
        {
            InitializeComponent();

            // Используем статическое свойство SelectedProject для получения переданных данных
            //var project = SelectedProject;
            //if (project != null)
            //{
            //    // Здесь вы можете привязать данные или обновить UI
            //    ProjectNameTextBox.Text = project.ProjectName;
            //    ProjectDescriptionTextBox.Text = project.Description;
            //}
        }


        private void SaveChangesButton_Click(object sender, RoutedEventArgs e)
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

            // Логика сохранения изменений (РАБОТАТЬ ФЫР)
            MessageBox.Show($"Изменения для проекта '{projectName}' успешно сохранены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

            // Вернуться на страницу списка проектов (пример)
            NavigationService?.GoBack();
        }
    }
}
