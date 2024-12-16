using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TNM.Models;
using Wpf.Ui.Controls;

namespace TNM.Pages
{
    /// <summary>
    /// Логика взаимодействия для TaskEdit.xaml
    /// </summary>
    public partial class TaskEdit : Page
    {
        private Dictionary<int, string> _tagsDictionary = new Dictionary<int, string>();
        private List<string> availableTags;
        private List<SolidColorBrush> tagColors;
        private int tagColorIndex = 0;
        private int assignedColorIndex = 0;
        private List<string> systemUsers;
        private Tasks _SelectedTask;
        private Projects _Projects;

        public TaskEdit(Tasks SelectedTask, Projects SelectedProject)
        {
            InitializeComponent();
            _SelectedTask = SelectedTask;
            _Projects = SelectedProject;
            InitializeTaskEdit();
        }

        // Инициализация начальных данных
        private async void InitializeTaskEdit()
        {
            TaskTitleTextBox.Text = _SelectedTask.Title;
            TaskDescriptionTextBlock.Text = _SelectedTask.Description;
            // Инициализация цветов
            tagColors = new List<SolidColorBrush>
            {
                new SolidColorBrush(Color.FromRgb(0, 122, 204)), // Синий
                new SolidColorBrush(Color.FromRgb(255, 165, 0)), // Оранжевый
                new SolidColorBrush(Color.FromRgb(50, 205, 50)), // Зеленый
                new SolidColorBrush(Color.FromRgb(255, 99, 71)),  // Красный
            };
            LoadStatuses();
            LoadTaskStatus();
            await LoadAllTagsAsync();
            await LoadAvailableTagsAsync();
            LoadInitialTags();
        }

        // Обновление цвета статуса
        private void StatusComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateStatusBackground();
        }

        private void UpdateStatusBackground()
        {
            // Проверяем, что SelectedItem не равен null
            if (StatusComboBox.SelectedItem != null)
            {
                // Используем dynamic для работы с анонимным типом
                dynamic selectedStatus = StatusComboBox.SelectedItem;
                string statusName = selectedStatus.StatusName; // Извлекаем StatusName

                // Определяем цвет фона на основе имени статуса
                Brush background = statusName switch
                {
                    "В работе" => new SolidColorBrush(Color.FromRgb(255, 165, 0)), // Оранжевый
                    "Завершено" => new SolidColorBrush(Color.FromRgb(50, 205, 50)),  // Зеленый
                    "Не завершено" => new SolidColorBrush(Color.FromRgb(255, 99, 71)),   // Красный
                    "Заморожен" => new SolidColorBrush(Color.FromRgb(10, 10, 205)),
                    _ => new SolidColorBrush(Color.FromRgb(51, 51, 51))             // По умолчанию
                };

                // Устанавливаем фон ComboBox
                StatusComboBox.Background = background;
            }
            else
            {
                Debug.WriteLine("SelectedItem is null.");
            }
        }

        private async void LoadStatuses()
        {
            try
            {
                var client = App.SupabaseService.GetClient();
                var response = await client.From<Statuses>().Get();

                if (response.Models != null)
                {
                    // Устанавливаем статусы как ItemsSource для StatusComboBox
                    StatusComboBox.ItemsSource = response.Models
                        .Select(status => new { status.StatusId, status.StatusName })
                        .ToList();

                    StatusComboBox.DisplayMemberPath = "StatusName";
                    StatusComboBox.SelectedValuePath = "StatusId";
                }
            }
            catch (Exception ex)
            {
                var snackbar = new Snackbar(SnackbarPresenter)
                {
                    Title = $"Ошибка при загрузке статусов: {ex.Message}",
                    Timeout = TimeSpan.FromSeconds(3)
                };
                snackbar.Show();
            }
        }

        private async void LoadTaskStatus()
        {
            if (_SelectedTask != null)
            {
                try
                {
                    var client = App.SupabaseService.GetClient();
                    var response = await client.From<Tasks>()
                        .Where(x => x.TaskId == _SelectedTask.TaskId)
                        .Single();

                    if (response != null && response != null)
                    {
                        StatusComboBox.SelectedValue = response.TaskStatusId;
                    }
                }
                catch (Exception ex)
                {
                    var snackbar = new Snackbar(SnackbarPresenter)
                    {
                        Title = $"Ошибка при загрузке статуса задачи: {ex.Message}",
                        Timeout = TimeSpan.FromSeconds(3)
                    };
                    snackbar.Show();
                }
            }
        }

        // Загрузка начальных тегов
        private async void LoadInitialTags()
        {
            await LoadAndAddTagsAsync();
        }

        private async Task LoadAndAddTagsAsync()
        {
            var tags = await LoadTagsForTaskAsync();
            foreach (var tag in tags)
            {
                AddTag(tag);
            }
        }

        private async Task LoadAvailableTagsAsync()
        {
            var client = App.SupabaseService.GetClient();

            try
            {
                var tagsResponse = await client
                    .From<Tags>()
                    .Select("*")
                    .Get();

                if (tagsResponse.Models != null)
                {
                    availableTags = tagsResponse.Models
                        .Select(tag => tag.TagName)
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                var snackbar = new Snackbar(SnackbarPresenter)
                {
                    Title = $"Ошибка загрузки доступных тегов: {ex.Message}",
                    Timeout = TimeSpan.FromSeconds(3)
                };
                snackbar.Show();
            }
        }

        private void AddTag(string tagName)
        {
            // Создание визуального элемента для WrapPanel
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

            // Обновляем отображение текущих тегов
            UpdateCurrentTagsDisplay();

            // Удаляем тег из доступных для добавления
            availableTags.Remove(tagName);
            TagSelectionListView.ItemsSource = availableTags.Where(tag => !IsTagAdded(tag)).ToList();
        }

        private void AddTag_Click(object sender, RoutedEventArgs e)
        {
            // Проверка на null и инициализация
            if (availableTags == null)
            {
                var snackbar = new Snackbar(SnackbarPresenter)
                {
                    Title = $"БЛЯТЬ",
                    Timeout = TimeSpan.FromSeconds(3)
                };
                snackbar.Show();
                availableTags = new List<string>();
            }

            var availableTagsToShow = availableTags
                .Where(tag => !IsTagAdded(tag))
                .ToList();

            if (!availableTagsToShow.Any())
            {
                var snackbar = new Snackbar(SnackbarPresenter)
                {
                    Title = "Все теги уже добавлены",
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

        // Обработка кнопки "Редактировать"
        private async void EditTaskButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var client = App.SupabaseService.GetClient();
                var response = await client.From<Tasks>()
                    .Filter("taskid", Op.Eq, _SelectedTask.TaskId)
                    .Set(x => x.Title, TaskTitleTextBox.Text)
                    .Set(x => x.Description, TaskDescriptionTextBlock.Text)
                    .Set(x => x.UpdatedDate, DateTime.Now)
                    .Set(x => x.TaskStatusId, StatusComboBox.SelectedValue)
                    .Update();
                if (response.ResponseMessage.IsSuccessStatusCode)
                {
                    await client.From<TaskTag>()
                        .Filter("taskid", Op.Eq, _SelectedTask.TaskId)
                        .Delete();
                }
            }
            catch (Exception ex)
            {
                var snackbar = new Snackbar(SnackbarPresenter)
                {
                    Title = $"Ошибка {ex.Message}",
                    Timeout = TimeSpan.FromSeconds(3)
                };
                snackbar.Show();
            }
        }

        private async Task LoadAllTagsAsync()
        {
            var client = App.SupabaseService.GetClient();

            try
            {
                // Запрашиваем все теги из таблицы Tags
                var tagsResponse = await client
                    .From<Tags>() // предполагается, что у вас есть класс Tag с полями TagId и Name
                    .Select("*")
                    .Get();

                if (tagsResponse.Models != null)
                {
                    // Заполняем словарь тегов
                    _tagsDictionary = tagsResponse.Models
                        .ToDictionary(tag => tag.TagId, tag => tag.TagName);
                }
            }
            catch (Exception ex)
            {
                var snackbar = new Snackbar(SnackbarPresenter)
                {
                    Title = $"Ошибка при загрузке всех тегов: {ex.Message}",
                    Timeout = TimeSpan.FromSeconds(3)
                };
                snackbar.Show();
            }
        }

        private async Task<List<string>> LoadTagsForTaskAsync()
        {
            var client = App.SupabaseService.GetClient();
            var tagNames = new List<string>();

            try
            {
                // Шаг 1: Получить tasktag по taskId
                var taskTagsResponse = await client
                    .From<TaskTag>()
                    .Select("*")
                    .Where(t => t.Taskid == _SelectedTask.TaskId)
                    .Get();

                if (taskTagsResponse.Models != null)
                {
                    foreach (var taskTag in taskTagsResponse.Models)
                    {
                        // Шаг 2: Сопоставить tagid с локальным словарём
                        if (_tagsDictionary.TryGetValue(taskTag.Tagid, out var tagName))
                        {
                            tagNames.Add(tagName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var snackbar = new Snackbar(SnackbarPresenter)
                {
                    Title = $"Ошибка при загрузке тегов задачи: {ex.Message}",
                    Timeout = TimeSpan.FromSeconds(3)
                };
                snackbar.Show();
            }

            return tagNames;
        }

        private void UpdateCurrentTagsDisplay()
        {
            var tagNames = TagsWrapPanel.Children
                .OfType<Border>()
                .Select(border => ((Wpf.Ui.Controls.TextBlock)border.Child).Text)
                .ToList();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            var taskView = new TaskView(_SelectedTask, _Projects);
            NavigationService?.Navigate(taskView);
        }
    }
}