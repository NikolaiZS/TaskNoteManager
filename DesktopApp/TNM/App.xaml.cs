using System.Configuration;
using System.Windows;
using System.Windows.Controls.Primitives;
using Wpf.Ui.Appearance;

namespace TNM
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static SupabaseClient _supabaseService;

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            await InitializeSupabase();
            // Чтение сохранённой темы
            string savedTheme = ConfigurationManager.AppSettings["ApplicationTheme"];
            ApplicationTheme initialTheme = savedTheme == "Dark" ? ApplicationTheme.Dark : ApplicationTheme.Light;
            ApplicationThemeManager.Apply(initialTheme);

            // Проверка текущей темы приложения
            var currentTheme = Wpf.Ui.Appearance.ApplicationThemeManager.GetAppTheme();
            MessageBox.Show($"Current application theme: {currentTheme}", "Application Theme");
        }

        private async Task InitializeSupabase()
        {
            _supabaseService = new SupabaseClient(); // Создаём экземпляр
            await _supabaseService.InitializeAsync(); // Инициализируем клиент
        }

        public static SupabaseClient SupabaseService => _supabaseService;
    }
}