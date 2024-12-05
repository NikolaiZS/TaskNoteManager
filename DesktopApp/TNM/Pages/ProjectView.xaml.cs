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
        private SupabaseClient _client = new SupabaseClient();
        public ObservableCollection<Projects> Projects { get; set; }

        public ProjectView()
        {
            InitializeComponent();
            Projects = new ObservableCollection<Projects>();
            DataContext = this;
            LoadProjects();
        }

        private void CreateNewProject_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Test");
        }

        private async void LoadProjects()
        {
            try
            {
                var client = App.SupabaseService.GetClient();
                var response = await client.From<Projects>().Get();

                Projects.Clear();
                foreach (var project in response.Models)
                {
                    Projects.Add(project);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки проектов: {ex.Message}");
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
                    var ProjectPage = new ProjectPage();
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