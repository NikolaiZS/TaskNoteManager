using System.Windows;
using System.Windows.Controls;

namespace TNM.Pages
{
    /// <summary>
    /// Логика взаимодействия для ProjectView.xaml
    /// </summary>
    public partial class ProjectView : Page
    {
        public ProjectView()
        {
            InitializeComponent();
            //LoadProject();
        }

        private void CreateNewProject_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Test");
        }

        //private void LoadProject()
        //{
        //    using (var context = new TaskNoteManagementDBEntities())
        //    {
        //        var projects = context.Projects
        //            .Select(p => new { p.ProjectName, p.Description })
        //            .ToList();

        //        // Установите результат как ItemsSource для элемента UI
        //        ProjectList.ItemsSource = projects;
        //    }

        //}
        private void EditProject_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            //var project = button?.DataContext as Projects;

            //if (project != null)
            //{
            //    var mainWindow = Application.Current.MainWindow as MainMenu;
            //    if (mainWindow != null)
            //    {
            //        // Добавим отладочный вывод
            //        MessageBox.Show("Передача проекта в EditProject");

            //        // Передаем проект в статическое свойство SelectedProject
            //        EditProject.SelectedProject = project;

            //        // Навигация на страницу EditProject
            //        mainWindow.NavigationMenu.Navigate(typeof(EditProject));
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("Проект не найден!");
            //}
        }
    }
}