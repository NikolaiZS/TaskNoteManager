using System.Windows;
using System.Windows.Controls;
using TNM.Models;

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
                MessageBox.Show("Введите название проекта и или описание. Название проекта не должно превышать 20 смиволов, описание не должно превышать 100 символов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Логика сохранения проекта (фир работать)(там бля этобля то бля уэуэуэуэуэаэуээв)
            try
            {
                var client = App.SupabaseService.GetClient();
                var project = new Projects
                {
                    projectname = projectName,
                    description = projectDescription,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    ownerid = CurrentUser.CurrentUserId
                };
                var response = await client.From<Projects>().Insert(project);
                MessageBox.Show($"{response.ResponseMessage}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}");
            }
            // Очистить поля после создания
            ProjectNameTextBox.Clear();
            ProjectDescriptionTextBox.Clear();
        }
    }
}