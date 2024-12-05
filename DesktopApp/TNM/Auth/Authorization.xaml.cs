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
            var username = loginBox.Text;
            var password = passwordBox.Password;
            string hashedpassword = HashPassword(password);

            var user = await _authClient.AuthenticateUserAsync(username, hashedpassword);
            if (user != null)
            {
                int userId = user.UserId;
                CurrentUser.CurrentUserId = userId;
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