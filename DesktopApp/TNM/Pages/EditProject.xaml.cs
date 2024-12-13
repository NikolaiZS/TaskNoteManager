using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TNM.Models;
using Wpf.Ui.Controls;

namespace TNM.Pages
{
    /// <summary>
    /// Логика взаимодействия для EditProject.xaml
    /// </summary>
    public partial class EditProject : Page
    {
        private List<SolidColorBrush> assignedColors;
        private int assignedColorIndex = 0;
        private Projects _Selectedproject;

        // Конструктор для передачи данных
        public EditProject(Projects SelectedProject)
        {
            InitializeComponent();
            InitializeTaskView();
            _Selectedproject = SelectedProject;
        }

        private void InitializeTaskView()
        {
            // Инициализация цветов
            assignedColors = new List<SolidColorBrush>
            {
                new SolidColorBrush(Color.FromRgb(255, 99, 71)),   // Красный
                new SolidColorBrush(Color.FromRgb(0, 122, 204)),  // Синий
                new SolidColorBrush(Color.FromRgb(255, 165, 0)),  // Оранжевый
                new SolidColorBrush(Color.FromRgb(50, 205, 50)),  // Зеленый
            };
            LoadInitialAssigned();
        }

        // Загрузка начальных назначенных
        private void LoadInitialAssigned()
        {
            var initialAssigned = new[] { "Борис Петрович", "Аркадий Паровозов" };
            foreach (var person in initialAssigned)
            {
                AddAssigned(person);
            }
        }

        private void SaveChangesButton_Click(object sender, RoutedEventArgs e)
        {
            string projectName = ProjectNameTextBox.Text.Trim();
            string projectDescription = ProjectDescriptionTextBox.Text.Trim();

            // Проверка на пустое название
            StringBuilder errorMessages = new StringBuilder();

            if (string.IsNullOrWhiteSpace(projectName) || projectName.Length > 20 || projectDescription.Length > 100)
            {
                if (string.IsNullOrWhiteSpace(projectName))
                {
                    errorMessages.AppendLine("Введите название проекта.");
                }

                if (projectName.Length > 20)
                {
                    errorMessages.AppendLine("Название проекта не должно превышать 20 символов.");
                }

                if (projectDescription.Length > 100)
                {
                    errorMessages.AppendLine("Описание проекта не должно превышать 100 символов.");
                }
                var snackbarErr = new Snackbar(SnackbarPresenter)
                {
                    Title = $"Ошибка при сохранении",
                    Timeout = TimeSpan.FromSeconds(3)
                };
                snackbarErr.Show();
                return;
            }

            // Логика сохранения изменений (РАБОТАТЬ ФЫР)
            var snackbar = new Snackbar(SnackbarPresenter)
            {
                Title = $"Изменения для проекта {projectName} успешно сохранены!",
                Timeout = TimeSpan.FromSeconds(3)
            };
            snackbar.Show();

            // Вернуться на страницу списка проектов (пример)
            NavigationService?.GoBack();
        }

        // Добавить нового назначенного с уникальным цветом
        private void AddAssigned(string name)
        {
            var assigned = new Border
            {
                Background = assignedColors[assignedColorIndex % assignedColors.Count], // Используем цвет по циклу
                CornerRadius = new CornerRadius(10),
                Padding = new Thickness(10, 5, 10, 5),
                Margin = new Thickness(5),
                Child = new Wpf.Ui.Controls.TextBlock
                {
                    Text = name,
                    Foreground = Brushes.White,
                    FontSize = 14
                }
            };
            AssignedWrapPanel.Children.Add(assigned);
            assignedColorIndex++; // Переходим к следующему цвету для следующего назначенного
        }

        private void AddAssigned_Click(object sender, RoutedEventArgs e)
        {
            AddAssigned("Новый человек");
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            var projectPage = new ProjectPage(_Selectedproject);
            NavigationService?.Navigate(projectPage);
        }
    }
}