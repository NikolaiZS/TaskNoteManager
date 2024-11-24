using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using TNM.Models;

namespace TNM.Pages
{
    /// <summary>
    // Логика взаимодействия для ProjectView.xaml
    /// </summary>
    public partial class ProjectView : Page
    {
        SupabaseClient _client = new SupabaseClient();
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


        //private void EditProject_Click(object sender, RoutedEventArgs e)
        //{
        //    var button = sender as Button;
        //    var project = button?.DataContext as Projects;

        //    if (project != null)
        //    {
        //        var mainWindow = Application.Current.MainWindow as MainMenu;
        //        if (mainWindow != null)
        //        {
        //            // Добавим отладочный вывод
        //            MessageBox.Show("Передача проекта в EditProject");

        //            // Передаем проект в статическое свойство SelectedProject
        //            EditProject.SelectedProject = project;

        //            // Навигация на страницу EditProject
        //            mainWindow.NavigationMenu.Navigate(typeof(EditProject));
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show("Проект не найден!");
        //    }
        //}
    }
}