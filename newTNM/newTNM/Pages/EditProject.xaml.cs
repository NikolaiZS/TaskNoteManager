using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace newTNM.Pages
{
    /// <summary>
    /// Логика взаимодействия для EditProject.xaml
    /// </summary>
    public partial class EditProject : Page
    {
        private List<SolidColorBrush> assignedColors;
        private int assignedColorIndex = 0;

        // Конструктор для передачи данных
        public EditProject()
        {
            InitializeComponent();
            InitializeTaskView();
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
            if (string.IsNullOrWhiteSpace(projectName))
            {
                MessageBox.Show("Введите название проекта", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Проверка длины названия
            if (projectName.Length > 20)
            {
                MessageBox.Show("Название проекта не должно превышать 20 символов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Проверка длины описания
            if (projectDescription.Length > 100)
            {
                MessageBox.Show("Описание проекта не должно превышать 100 символов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Логика сохранения изменений (РАБОТАТЬ ФЫР)
            MessageBox.Show($"Изменения для проекта '{projectName}' успешно сохранены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

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
                Child = new TextBlock
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
    }
}