using System.Windows;

namespace TNM.Menu
{
    /// <summary>
    /// Логика взаимодействия для MainMenu.xaml
    /// </summary>
    public partial class MainMenu
    {
        public MainMenu()
        {
            InitializeComponent();

            Loaded += (sender, args) =>
            {
                Wpf.Ui.Appearance.SystemThemeWatcher.Watch(
                    this,                                    // Window class
                    Wpf.Ui.Controls.WindowBackdropType.Mica, // Background type
                    true                                     // Whether to change accents automatically
                );
            };
        }
    }
}