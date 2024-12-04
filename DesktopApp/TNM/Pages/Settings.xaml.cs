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
            if (ThemeComboBox.SelectedItem is ComboBoxItem selectedItem && selectedItem.Tag != null)
            {
                Wpf.Ui.Appearance.ApplicationThemeManager.Apply(Wpf.Ui.Appearance.ApplicationTheme.Light, Wpf.Ui.Controls.WindowBackdropType.Mica, true);
                //string themeTag = selectedItem.Tag.ToString();
                //ApplicationTheme selectedTheme = themeTag == "Dark" ? ApplicationTheme.Dark : ApplicationTheme.Light;

                //// Применение темы
                //ApplicationThemeManager.Apply(selectedTheme);

                //// Проверка текущей темы
                //var currentTheme = ApplicationThemeManager.GetAppTheme();

                //// Сохранение выбранной темы
                //Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                //if (config.AppSettings.Settings["ApplicationTheme"] != null)
                //{
                //    config.AppSettings.Settings["ApplicationTheme"].Value = themeTag;
                //}
                //else
                //{
                //    config.AppSettings.Settings.Add("ApplicationTheme", themeTag);
                //}
                //config.Save(ConfigurationSaveMode.Modified);
            }
        }
    }
}