using System.Windows;

namespace TNM
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static SupabaseClient? _supabaseService;

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            await InitializeSupabase();
            var theme = new ThemeBackgroundHelper();
            var currentTheme = theme.GetCurrentTheme();
            theme.ApplyThemeFromConfig();
            theme.UpdateThemeResources(currentTheme);
        }

        private async Task InitializeSupabase()
        {
            _supabaseService = new SupabaseClient(); // Создаём экземпляр
            await _supabaseService.InitializeAsync(); // Инициализируем клиент
        }

        public static SupabaseClient SupabaseService => _supabaseService;
    }
}