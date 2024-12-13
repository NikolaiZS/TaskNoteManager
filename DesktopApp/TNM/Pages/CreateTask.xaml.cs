using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TNM.Models;
using Wpf.Ui.Controls;

namespace TNM.Pages
{
    /// <summary>
    /// Логика взаимодействия для CreateTask.xaml
    /// </summary>
    public partial class CreateTask : Page
    {
        private List<SolidColorBrush> tagColors;
        private List<string> availableTags;
        private int tagColorIndex = 0;
        private Projects _SelectedProject;

        public CreateTask(Projects SelectedProject)
        {
            InitializeComponent();
            InitializeTaskEdit();
            _SelectedProject = SelectedProject;
        }

        private async Task<List<string>> LoadTagsFromDatabaseAsync()
        {
            try
            {
                var client = App.SupabaseService.GetClient();
                var response = await client.From<Tags>().Get();

                if (response.Models != null)
                {
                    return response.Models
                        .Select(tag => tag.TagName)
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                var snackbar = new Snackbar(SnackbarPresenter)
                {
                    Title = $"Ошибка при загузке тегов: {ex.Message}",
                    Timeout = TimeSpan.FromSeconds(3)
                };
                snackbar.Show();
            }

            return new List<string>();
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

            InitializeStatus();
            LoadInitialTags();
            LoadAvailableTags();
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

        private void EditTaskButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private async void LoadAvailableTags()
        {
            availableTags = await LoadTagsFromDatabaseAsync();
        }

        private void LoadInitialTags()
        {
            var initialTags = new[] { "WIP" };
            foreach (var tag in initialTags)
            {
                AddTag(tag);
            }
        }

        private void AddTag(string tagName)
        {
            var tag = new Border
            {
                Background = tagColors[tagColorIndex % tagColors.Count],
                CornerRadius = new CornerRadius(10),
                Padding = new Thickness(10, 5, 10, 5),
                Margin = new Thickness(5),
                Child = new Wpf.Ui.Controls.TextBlock
                {
                    Text = tagName,
                    Foreground = Brushes.White,
                    FontSize = 14
                }
            };
            TagsWrapPanel.Children.Add(tag);
            tagColorIndex++;
        }

        private void AddTag_Click(object sender, RoutedEventArgs e)
        {
            if (availableTags == null || !availableTags.Any())
            {
                var snackbar = new Snackbar(SnackbarPresenter)
                {
                    Title = $"Нет доступных тегов для добавления",
                    Timeout = TimeSpan.FromSeconds(3)
                };
                snackbar.Show();
                return;
            }

            var availableTagsToShow = availableTags.Where(tag => !IsTagAdded(tag)).ToList();

            if (!availableTagsToShow.Any())
            {
                var snackbar = new Snackbar(SnackbarPresenter)
                {
                    Title = $"Все теги уже добавлены",
                    Timeout = TimeSpan.FromSeconds(3)
                };
                snackbar.Show();
                return;
            }

            TagSelectionListView.ItemsSource = availableTagsToShow;
            TagSelectionFlyout.Show();
        }

        private void ConfirmTagSelection_Click(object sender, RoutedEventArgs e)
        {
            if (TagSelectionListView.SelectedItem is string selectedTag)
            {
                AddTag(selectedTag);
                TagSelectionFlyout.Hide();
            }
            else
            {
                var snackbar = new Snackbar(SnackbarPresenter)
                {
                    Title = $"Пожалуйста выберите тег",
                    Timeout = TimeSpan.FromSeconds(3)
                };
                snackbar.Show();
            }
        }

        private bool IsTagAdded(string tagName)
        {
            return TagsWrapPanel.Children.OfType<Border>().Any(border =>
                border.Child is Wpf.Ui.Controls.TextBlock textBlock && textBlock.Text == tagName);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            var projectPage = new ProjectPage(_SelectedProject);
            NavigationService?.Navigate(projectPage);
        }
    }
}