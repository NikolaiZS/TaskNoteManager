using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TNM.Models;

namespace TNM.Pages
{
    /// <summary>
    /// Логика взаимодействия для ProjectPage.xaml
    /// </summary>
    public partial class ProjectPage : Page
    {
        public ObservableCollection<Tasks> _Tasks { get; set; }
        public int projectId = 1;
        private readonly Projects _project;

        public ProjectPage(Projects project)
        {
            InitializeComponent();
            _Tasks = new ObservableCollection<Tasks>();
            DataContext = this;
            _project = project;
            LoadTasks();
        }

        private async void CreateNewTask_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    var client = App.SupabaseService.GetClient();
            //    var response = await client.
            //}
        }

        private async void LoadTasks()
        {
            try
            {
                var client = App.SupabaseService.GetClient();
                var response = await client.From<Tasks>()
                                    .Filter("projectid", Supabase.Postgrest.Constants.Operator.Equals, _project.projectid)
                                    .Select("taskid, title, description, createduserid, assigneduserid, tagsid, createdate, updatedate, status, priority")
                                    .Get();

                _Tasks.Clear();
                if (response.Models.Count == 0)
                {
                    MessageBox.Show("None selected");
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
                MessageBox.Show($"Ошибка загрузки задач: {ex.Message}");
            }
        }

        private void CardAction_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Wpf.Ui.Controls.CardAction cardAction)
            {
                var task = cardAction.DataContext as Tasks;

                if (task != null)
                {
                    var ViewTaskPage = new TaskEdit(/*task*/);
                    NavigationService?.Navigate(ViewTaskPage);
                }
                else
                {
                    MessageBox.Show("Не удалось получить данные задачи.");
                }
            }
        }
    }
}