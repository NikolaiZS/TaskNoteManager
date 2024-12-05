using System.Windows.Controls;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;
using System.Configuration;

namespace TNM.Pages
{
    /// <summary>
    /// Логика взаимодействия для Settings.xaml
    /// </summary>
    public partial class Settings : Page
    {
        public Settings()
        {
            InitializeComponent();
        }

        private void ThemeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }
    }
}