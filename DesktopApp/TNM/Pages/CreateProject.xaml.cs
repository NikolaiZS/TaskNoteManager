using System.Windows;
using System.Windows.Controls;
using TNM.Models;
using Wpf.Ui.Controls;

namespace TNM.Pages
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

        private async void CreateProjectButton_Click(object sender, RoutedEventArgs e)
        {
            string projectName = ProjectNameTextBox.Text.Trim();
            string projectDescription = ProjectDescriptionTextBox.Text.Trim();

            // Проверка на пустое название
            if (string.IsNullOrWhiteSpace(projectName) ||
                string.IsNullOrWhiteSpace(projectDescription) ||
                projectName.Length > 20 ||
                projectDescription.Length > 100)
            {
                var snackbar = new Snackbar(SnackbarPresenter)
                {
                    Title = $"Введите название проекта и или описание. Название проекта не должно превышать 20 смиволов, описание не должно превышать 100 символов",
                    Timeout = TimeSpan.FromSeconds(3)
                };
                snackbar.Show();
                return;
            }

            // Логика сохранения проекта (фир работать)(там бля этобля то бля уэуэуэуэуэаэуээв)
            try
            {
                var client = App.SupabaseService.GetClient();
                var project = new Projects
                {
                    Projectname = projectName,
                    Description = projectDescription,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    OwnerId = CurrentUser.CurrentUserId
                };
                var response = await client.From<Projects>().Insert(project);
                var snackbar = new Snackbar(SnackbarPresenter)
                {
                    Title = $"Проект успешно создан",
                    Timeout = TimeSpan.FromSeconds(3)
                };
                snackbar.Show();
            }
            catch (Exception ex)
            {
                var snackbar = new Snackbar(SnackbarPresenter)
                {
                    Title = $"Ошибка: {ex.Message}",
                    Timeout = TimeSpan.FromSeconds(3)
                };
                snackbar.Show();
            }
            // Очистить поля после создания
            ProjectNameTextBox.Clear();
            ProjectDescriptionTextBox.Clear();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            var projectView = new ProjectView();
            NavigationService?.Navigate(projectView);
        }
    }
}