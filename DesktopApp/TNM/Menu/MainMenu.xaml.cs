using System.Windows;
using TNM.Pages;

namespace TNM.Menu
{
    /// <summary>
    /// Логика взаимодействия для MainMenu.xaml
    /// </summary>
    public partial class MainMenu
    {
        private readonly Stack<Type> _navigationHistory = new Stack<Type>();

        public MainMenu()
        {
            InitializeComponent();
            Loaded += (_, _) => NavigationMenu.Navigate(typeof(ProjectView));
        }
    }
}