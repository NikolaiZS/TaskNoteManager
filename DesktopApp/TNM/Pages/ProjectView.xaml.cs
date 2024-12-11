using Supabase.Postgrest.Interfaces;
using Supabase.Postgrest;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using TNM.Models;
using Wpf.Ui;

namespace TNM.Pages
{
    /// <summary>
    // Логика взаимодействия для ProjectView.xaml
    /// </summary>
    public partial class ProjectView : Page
    {
        public ObservableCollection<Projects> Projects { get; set; }
        private readonly int _userId;

        public ProjectView()
        {
            InitializeComponent();
            Projects = new ObservableCollection<Projects>();
            DataContext = this;
            _userId = CurrentUser.CurrentUserId;
            LoadProjects();
        }

        private void CreateNewProject_Click(object sender, RoutedEventArgs e)
        {
        }

        private async void LoadProjects()
        {
            try
            {
                var client = App.SupabaseService.GetClient();
                await client.InitializeAsync();

                // Получаем проекты, где пользователь является владельцем
                var ownerProjects = await client.From<Projects>()
                    .Filter("ownerid", Op.Eq, _userId)
                    .Get();

                // Получаем projectid, где пользователь является работником
                var projectWorkers = await client.From<ProjectWorker>()
                    .Filter("workerid", Op.Eq, _userId)
                    .Get();

                var projectIds = projectWorkers.Models.Select(pw => pw.projectid).ToList();

                // Добавляем проекты, где пользователь является работником
                var workerProjects = await client.From<Projects>()
                    .Filter("projectid", Op.In, projectIds)
                    .Get();

                // Объединяем результаты
                var finalProjects = ownerProjects.Models.Union(workerProjects.Models);
                Projects.Clear();

                // Добавляем проекты в коллекцию
                foreach (var project in finalProjects)
                {
                    Projects.Add(project);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CardAction_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Wpf.Ui.Controls.CardAction cardAction)
            {
                // Получите данные проекта из DataContext
                var project = cardAction.DataContext as Projects;

                if (project != null)
                {
                    var ProjectPage = new ProjectPage(project);
                    NavigationService?.Navigate(ProjectPage);
                }
                else
                {
                    MessageBox.Show("Не удалось получить данные проекта.");
                }
            }
        }
    }
}