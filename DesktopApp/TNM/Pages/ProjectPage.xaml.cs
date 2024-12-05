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

        public ProjectPage(Tasks Tasks)
        {
            InitializeComponent();
            _Tasks = new ObservableCollection<Tasks>();
            DataContext = this;
            LoadTasks();
        }

        private void CreateNewTask_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Test");
        }

        private async void LoadTasks()
        {
            try
            {
                var _client = new SupabaseClient();
                _client = App.SupabaseService.GetClient();
                var response = await _client.From<Tasks>().Get();

                _Tasks.Clear();
                foreach (var task in response.Models)
                {
                    _Tasks.Add(task);
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