using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
                var createTask = new CreateTask();
                NavigationService?.Navigate(createTask);
            }
        }

        private async void LoadTasks()
        {
            try
            {
                var client = App.SupabaseService.GetClient();
                var response = await client.From<Tasks>()
                                    .Filter("projectid", Op.Eq, SelectedProject.projectid)
                                    .Select("taskid, title, description, createduserid, assigneduserid, tagsid, createdate, updatedate, status, priority")
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
                var task = cardAction.DataContext as Tasks;

                if (task != null)
                {
                    var viewTaskPage = new TaskEdit();
                    NavigationService?.Navigate(viewTaskPage);
                }
                else
                {
                    MessageBox.Show("Не удалось получить данные задачи.");
                }
            }
        }
    }
}