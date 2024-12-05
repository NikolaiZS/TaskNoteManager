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
            ApplyThemeFromConfig();
        }

        private void ApplyThemeFromConfig()
        {
            var themeValue = ConfigurationManager.AppSettings["ApplicationTheme"];

            if (Enum.TryParse(themeValue, true, out ApplicationTheme theme))
            {
                ApplicationThemeManager.Apply(
                    theme,
                    Wpf.Ui.Controls.WindowBackdropType.None,
                    false
                );
            }
            else
            {
                // Тема по умолчанию
                ApplicationThemeManager.Apply(
                    ApplicationTheme.Dark,
                    Wpf.Ui.Controls.WindowBackdropType.None,
                    false
                );
            }
        }

        private async Task InitializeSupabase()
        {
            _supabaseService = new SupabaseClient(); // Создаём экземпляр
            await _supabaseService.InitializeAsync(); // Инициализируем клиент
        }

        public static SupabaseClient SupabaseService => _supabaseService;
    }
}