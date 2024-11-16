using System;
using System.Collections.Generic;
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

            // Пример списка проектов
            var projects = new List<Project>
            {
                new Project { Title = "Проект 1", Description = "Описание проекта 1" },
                new Project { Title = "Проект 2", Description = "Описание проекта 2" },
                new Project { Title = "Проект 3", Description = "Описание проекта 3" },
                new Project { Title = "Проект 4", Description = "Описание проекта 4" },
                new Project { Title = "Проект 5", Description = "Описание проекта 5" },
            };

            ProjectList.ItemsSource = projects;
        }
        private void CreateNewProject_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Test");
        }
    }

    // Класс для хранения данных о проекте
    public class Project
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
