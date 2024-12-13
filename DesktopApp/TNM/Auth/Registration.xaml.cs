using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace TNM.Auth
{
    /// <summary>
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration
    {
        private SupabaseClient _authClient;

        public Registration()
        {
            InitializeComponent();
            var backgroundTheme = new ThemeBackgroundHelper();
            MainGrid.Background = backgroundTheme.GetBackgroundForCurrentTheme();
            _authClient = new SupabaseClient();
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

        public async Task RegisterUsers(string username, string email, string password)
        {
            await _authClient.AddUserAsync(username, email, password);
        }

        private async void RegistrationButton_Click(object sender, RoutedEventArgs e)
        {
            string username = loginBox.Text;
            string password = passwordBox.Password;
            string email = mailBox.Text;
            string hashedpassword = HashPassword(password);

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                assistBox.Text = "Введите имя пользователя и пароль.";
                assistBox.Foreground = new SolidColorBrush(Colors.Red);
                return;
            }

            await RegisterUsers(username, email, hashedpassword);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Authorization auth = new Authorization();
            auth.Show();
            this.Close();
        }
    }
}