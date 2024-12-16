using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Linq;
using TNM.Models;
using Wpf.Ui.Controls;

namespace TNM.Pages
{
    /// <summary>
    /// Логика взаимодействия для TaskView.xaml
    /// </summary>
    public partial class TaskView : Page
    {
        private Dictionary<int, string> _tagsDictionary = new Dictionary<int, string>();
        private List<string> availableTags;
        private List<SolidColorBrush> tagColors;
        private int tagColorIndex = 0;
        private List<string> systemUsers;
        private Tasks _Selectedtask;
        private Projects _Selectedprojects;

        public TaskView(Tasks Selectedtask, Projects SelectedProject)
        {
            InitializeComponent();
            _Selectedtask = Selectedtask;
            _Selectedprojects = SelectedProject;
            InitializeTaskEdit();
        }

        // Инициализация начальных данных
        private async void InitializeTaskEdit()
        {
            TaskTitleTextBox.Text = _Selectedtask.Title;
            TaskDescriptionTextBlock.Text = _Selectedtask.Description;
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
            if (_Selectedtask != null)
            {
                try
                {
                    var client = App.SupabaseService.GetClient();
                    var response = await client.From<Tasks>()
                        .Where(x => x.TaskId == _Selectedtask.TaskId)
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

        // Загрузка начальных тегов
        private async void LoadInitialTags()
        {
            await LoadAndAddTagsAsync();
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

        private async Task LoadAndAddTagsAsync()
        {
            var tags = await LoadTagsForTaskAsync();
            foreach (var tag in tags)
            {
                AddTag(tag);
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
                    .Where(t => t.Taskid == _Selectedtask.TaskId)
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

        // Обработка кнопки "Редактировать"
        private void EditTaskButton_Click(object sender, RoutedEventArgs e)
        {
            var taskEdit = new TaskEdit(_Selectedtask, _Selectedprojects);
            NavigationService?.Navigate(taskEdit);
        }

        private async void SubmitTaskButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var client = App.SupabaseService.GetClient();

                // Обновление статуса задачи
                var updateResponse = await client
                    .From<Tasks>()
                    .Where(x => x.TaskId == _Selectedtask.TaskId)
                    .Set(x => x.isCompleted, true)
                    .Set(x => x.UpdatedDate, DateTime.Now)
                    .Set(x => x.TaskStatusId, StatusComboBox.SelectedValue)
                    .Update();

                if (updateResponse == null)
                {
                    throw new Exception("Не удалось обновить задачу.");
                }

                // Вставка комментария в таблицу Comment
                var comment = new Comments
                {
                    TaskId = _Selectedtask.TaskId,
                    UserId = CurrentUser.CurrentUserId,
                    Content = CommentTextBox.Text,
                    CreatedAt = DateTime.Now
                };

                var insertResponse = await client
                    .From<Comments>()
                    .Insert(comment);

                if (insertResponse == null)
                {
                    throw new Exception("Не удалось добавить комментарий.");
                }

                // Уведомление об успешной операции
                var snackbar = new Snackbar(SnackbarPresenter)
                {
                    Title = $"Задача выполнена и комментарий добавлен!",
                    Timeout = TimeSpan.FromSeconds(3)
                };
                snackbar.Show();
            }
            catch (Exception ex)
            {
                // Уведомление об ошибке
                var snackbar = new Snackbar(SnackbarPresenter)
                {
                    Title = $"Ошибка: {ex.Message}",
                    Timeout = TimeSpan.FromSeconds(3)
                };
                snackbar.Show();
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            var projectPage = new ProjectPage(_Selectedprojects);
            NavigationService?.Navigate(projectPage);
        }
    }
}