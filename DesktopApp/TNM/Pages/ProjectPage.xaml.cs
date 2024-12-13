using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using TNM.Models;

namespace TNM.Pages
{
    /// <summary>
    /// Логика взаимодействия для ProjectPage.xaml
    /// </summary>
    public partial class ProjectPage : Page
    {
        public ObservableCollection<Tasks> _Tasks { get; set; }
        public Projects SelectedProject { get; private set; }

        public ProjectPage(Projects project)
        {
            InitializeComponent();
            _Tasks = new ObservableCollection<Tasks>();
            DataContext = this;
            SelectedProject = project;
            LoadTasks();
        }

        private void CreateNewTask_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Wpf.Ui.Controls.Button)
            {
                var createTask = new CreateTask(SelectedProject);
                NavigationService?.Navigate(createTask);
            }
        }

        private async void LoadTasks()
        {
            try
            {
                var client = App.SupabaseService.GetClient();
                var response = await client.From<Tasks>()
                                    .Filter("projectid", Op.Eq, SelectedProject.ProjectId)
                                    .Select("taskid, title, description, createduserid, createdate, updatedate, taskstatusid, priority")
                                    .Get();

                _Tasks.Clear();
                if (response.Models.Count == 0)
                {
                    MessageBox.Show("Задач нет.");
                }
                else
                {
                    foreach (var task in response.Models)
                    {
                        _Tasks.Add(task);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading tasks: {ex.Message}");
            }
        }

        private void CardAction_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Wpf.Ui.Controls.CardAction cardAction)
            {
                var Selectedtask = cardAction.DataContext as Tasks;

                if (Selectedtask != null)
                {
                    var viewTaskPage = new TaskView(Selectedtask, SelectedProject);
                    NavigationService?.Navigate(viewTaskPage);
                }
                else
                {
                    MessageBox.Show("Не удалось получить данные задачи.");
                }
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            var projectView = new ProjectView();
            NavigationService?.Navigate(projectView);
        }

        private void EditProjectButton_Click(object sender, RoutedEventArgs e)
        {
            var editProject = new EditProject(SelectedProject);
            NavigationService?.Navigate(editProject);
        }
    }
}