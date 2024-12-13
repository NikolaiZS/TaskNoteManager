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
            // видит изменения только внутри кнопок NavigationView
            // TODO: переделать
            NavigationMenu.Navigated += NavigationMenu_Navigated;
        }

        private void NavigationMenu_Navigated(object sender, RoutedEventArgs e)
        {
            if (sender is Wpf.Ui.Controls.NavigationView navigationView)
            {
                // navigationView.SelectedItem только если есть такая вкладка в NavigationView
                var selectedItem = navigationView.SelectedItem as Wpf.Ui.Controls.NavigationViewItem;
                if (selectedItem != null && selectedItem.TargetPageType != null)
                {
                    // + в историю
                    _navigationHistory.Push(selectedItem.TargetPageType);
                }
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (_navigationHistory.Count > 1)
            {
                // - из истории
                _navigationHistory.Pop();
                // вроде как get, но из стека
                var previousPageType = _navigationHistory.Peek();
                NavigationMenu.Navigate(previousPageType);
            }
            else
            {
                MessageBox.Show("Нет предыдущих страниц.");
            }
        }
    }
}