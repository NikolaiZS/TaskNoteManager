using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TNM.Models;

namespace TNM.Pages
{
    /// <summary>
    /// Логика взаимодействия для TaskView.xaml
    /// </summary>
    public partial class TaskView : Page
    {
        // ctrl + c ctrl + v из TaskEdit

        private List<SolidColorBrush> tagColors;
        private List<SolidColorBrush> assignedColors;
        private int tagColorIndex = 0;
        private List<string> systemUsers; // ?

        public TaskView(/*Tasks Task*/)
        {
            InitializeComponent();
            InitializeTaskEdit();
        }

        private async Task<List<string>> LoadUsersFromDatabaseAsync() // я так понимаю это тут не нужно?
        {
            try
            {
                // Выполняем запрос к таблице "users"
                var client = App.SupabaseService.GetClient();
                var response = await client.From<Users>().Get();

                if (response.Models != null)
                {
                    // Возвращаем список имен пользователей
                    return response.Models
                        .Select(user => user.Username)
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке пользователей: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return new List<string>(); // Пустой список при ошибке
        }

        // Инициализация начальных данных
        private void InitializeTaskEdit()
        {
            // Инициализация цветов
            tagColors = new List<SolidColorBrush>
            {
                new SolidColorBrush(Color.FromRgb(0, 122, 204)), // Синий
                new SolidColorBrush(Color.FromRgb(255, 165, 0)), // Оранжевый
                new SolidColorBrush(Color.FromRgb(50, 205, 50)), // Зеленый
                new SolidColorBrush(Color.FromRgb(255, 99, 71)),  // Красный
            };

            assignedColors = new List<SolidColorBrush>
            {
                new SolidColorBrush(Color.FromRgb(255, 99, 71)),   // Красный
                new SolidColorBrush(Color.FromRgb(0, 122, 204)),  // Синий
                new SolidColorBrush(Color.FromRgb(255, 165, 0)),  // Оранжевый
                new SolidColorBrush(Color.FromRgb(50, 205, 50)),  // Зеленый
            };
            InitializeStatus();
            LoadInitialTags();
            LoadSystemUsers();
        }

        // Инициализация статуса задачи
        private void InitializeStatus()
        {
            StatusComboBox.SelectedIndex = 0;
            UpdateStatusBackground();
        }

        // Обновление цвета статуса
        private void StatusComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateStatusBackground();
        }

        private void UpdateStatusBackground()
        {
            if (StatusComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                var status = selectedItem.Content.ToString();
                Brush background = status switch
                {
                    "В процессе" => new SolidColorBrush(Color.FromRgb(255, 165, 0)), // Оранжевый
                    "Выполнено" => new SolidColorBrush(Color.FromRgb(50, 205, 50)),  // Зеленый
                    "Отменено" => new SolidColorBrush(Color.FromRgb(255, 99, 71)),   // Красный
                    _ => new SolidColorBrush(Color.FromRgb(51, 51, 51))             // По умолчанию
                };
                StatusComboBox.Background = background;
            }
        }

        // Загрузка начальных тегов
        private void LoadInitialTags()
        {
            var initialTags = new[] { "UI", "Backend", "Critical" };
            foreach (var tag in initialTags)
            {
                AddTag(tag);
            }
        }

        // Добавить новый тэг с уникальным цветом
        private void AddTag(string tagName)
        {
            var tag = new Border
            {
                Background = tagColors[tagColorIndex % tagColors.Count], // Используем цвет по циклу
                CornerRadius = new CornerRadius(10),
                Padding = new Thickness(10, 5, 10, 5),
                Margin = new Thickness(5),
                Child = new TextBlock
                {
                    Text = tagName,
                    Foreground = Brushes.White,
                    FontSize = 14
                }
            };
            TagsWrapPanel.Children.Add(tag);

            tagColorIndex++; // Переходим к следующему цвету для следующего тега
        }

        private void AddTag_Click(object sender, RoutedEventArgs e)
        {
            AddTag("Новый тег");
        }

        private async void LoadSystemUsers()
        {
            var users = await LoadUsersFromDatabaseAsync();
            systemUsers = users;
        }

        // Обработка кнопки "Редактировать"
        private void EditTaskButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Редактирование задачи!");
        }

        private void SubmitTaskButton_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}