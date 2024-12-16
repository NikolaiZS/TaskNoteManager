using System.Diagnostics;
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
        private List<string> selectedTags = new List<string>(); // Выбранные теги
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
            LoadStatuses();
            InitializeStatus();
            LoadAvailableTags();
        }

        // Инициализация статуса задачи
        private void InitializeStatus()
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

        private void StatusComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateStatusBackground();
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

        //private async void LoadTaskStatus()
        //{
        //    if (_SelectedTask != null)
        //    {
        //        try
        //        {
        //            var client = App.SupabaseService.GetClient();
        //            var response = await client.From<Tasks>()
        //                .Where(x => x.TaskId == _SelectedTask.TaskId)
        //                .Single();

        //            if (response != null && response.Model != null)
        //            {
        //                StatusComboBox.SelectedValue = response.Model.TaskStatusId;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            var snackbar = new Snackbar(SnackbarPresenter)
        //            {
        //                Title = $"Ошибка при загрузке статуса задачи: {ex.Message}",
        //                Timeout = TimeSpan.FromSeconds(3)
        //            };
        //            snackbar.Show();
        //        }
        //    }
        //}

        private async void CreateTaskButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var client = App.SupabaseService.GetClient();

                // Вставка задачи
                var task = new Tasks
                {
                    ProjectId = _SelectedProject.ProjectId,
                    Title = TaskTitleTextBox.Text,
                    Description = TaskDescriptionTextBlock.Text,
                    CreateduserId = CurrentUser.CurrentUserId,
                    TaskStatusId = 1,
                    CreateDate = DateTime.Now
                };

                var taskResponse = await client.From<Tasks>().Insert(task);
                if (taskResponse != null && taskResponse.Models.Any())
                {
                    int taskId = taskResponse.Models[0].TaskId; // Получаем ID созданной задачи

                    // Вставка тегов задачи в TaskTag таблицу
                    foreach (var tagName in selectedTags)
                    {
                        // Получаем все теги с заданным именем
                        var tagResponse = await client.From<Tags>()
                            .Filter("tagname", Supabase.Postgrest.Constants.Operator.Equals, tagName)
                            .Get();

                        if (tagResponse.Models != null && tagResponse.Models.Any())
                        {
                            foreach (var tag in tagResponse.Models)
                            {
                                // Создаём запись TaskTag
                                var taskTag = new TaskTag
                                {
                                    Taskid = taskId,
                                    Tagid = tag.TagId
                                };

                                // Вставка в таблицу TaskTag
                                await client.From<TaskTag>().Insert(taskTag);
                                Debug.WriteLine($"Связка задачи с ID {taskId} и тега с ID {tag.TagId} добавлена.");
                            }
                        }
                        else
                        {
                            Debug.WriteLine($"Тег '{tagName}' не найден в базе данных.");
                        }
                    }

                    var snackbar = new Snackbar(SnackbarPresenter)
                    {
                        Title = $"Задача успешно создана!",
                        Timeout = TimeSpan.FromSeconds(3)
                    };
                    snackbar.Show();
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

        private async void LoadAvailableTags()
        {
            availableTags = await LoadTagsFromDatabaseAsync();
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
            selectedTags.Add(tagName);
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
                Debug.WriteLine($"Выбранный тег: {selectedTag}");
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