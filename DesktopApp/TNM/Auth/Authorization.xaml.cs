using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using TNM.Menu;

namespace TNM.Auth
{
    /// <summary>
    /// Логика взаимодействия для Authorization.xaml
    /// </summary>
    public partial class Authorization
    {
        private SupabaseClient _authClient;

        public Authorization()
        {
            InitializeComponent();
            var backgroundTheme = new ThemeBackgroundHelper();
            MainGrid.Background = backgroundTheme.GetBackgroundForCurrentTheme();
            _authClient = new SupabaseClient();
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Скрываем TextBlock и показываем ProgressRing
            LoginTextBlock.Visibility = Visibility.Hidden;
            LoginProgressRing.Visibility = Visibility.Visible;

            // Деактивируем кнопку, чтобы предотвратить повторный клик
            LoginButton.IsEnabled = false;

            try
            {
                var username = loginBox.Text;
                var password = passwordBox.Password;
                string hashedpassword = HashPassword(password);

                // Выполняем аутентификацию
                var user = await _authClient.AuthenticateUserAsync(username, hashedpassword);
                if (user != null)
                {
                    int userId = user.UserId;
                    CurrentUser.CurrentUserId = userId;

                    // Переход к следующему окну
                    MainMenu mainMenu = new MainMenu();
                    mainMenu.Show();
                    this.Close();
                }
                else
                {
                    assistBlock.Text = "Неправильный логин или пароль";
                    assistBlock.Foreground = new SolidColorBrush(Colors.Red);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                // Возвращаем исходный вид
                LoginButton.IsEnabled = true;
                LoginTextBlock.Visibility = Visibility.Visible;
                LoginProgressRing.Visibility = Visibility.Hidden;
            }
        }

        private void RegistrationButton_Click(object sender, RoutedEventArgs e)
        {
            Registration reg = new Registration();
            reg.Show();
            this.Close();
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (var b in bytes)
                    builder.Append(b.ToString("x2"));
                return builder.ToString();
            }
        }
    }
}