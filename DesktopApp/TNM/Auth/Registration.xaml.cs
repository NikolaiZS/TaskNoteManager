using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

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
            MainGrid.Background = GenerateAnimatedGradientBackground();
            _authClient = new SupabaseClient();
        }

        public static RadialGradientBrush GenerateAnimatedGradientBackground()
        {
            var gradientBrush = new RadialGradientBrush
            {
                GradientOrigin = new Point(0.5, 0.5),
                Center = new Point(0.5, 0.5),
                RadiusX = 0.5,
                RadiusY = 0.5,
                GradientStops = new GradientStopCollection
                {
                    new GradientStop(Color.FromRgb(10, 10, 20), 0.0),
                    new GradientStop(Color.FromRgb(20, 20, 30), 0.5),
                    new GradientStop(Color.FromRgb(15, 15, 25), 1.0)
                }
            };

            var radiusXAnimation = new DoubleAnimation
            {
                From = 0.4,
                To = 0.6,
                Duration = new Duration(TimeSpan.FromSeconds(6)),
                RepeatBehavior = RepeatBehavior.Forever,
                AutoReverse = true,
                EasingFunction = new SineEase { EasingMode = EasingMode.EaseInOut }
            };

            var radiusYAnimation = new DoubleAnimation
            {
                From = 0.4,
                To = 0.6,
                Duration = new Duration(TimeSpan.FromSeconds(6)),
                RepeatBehavior = RepeatBehavior.Forever,
                AutoReverse = true,
                EasingFunction = new SineEase { EasingMode = EasingMode.EaseInOut }
            };

            gradientBrush.BeginAnimation(RadialGradientBrush.RadiusXProperty, radiusXAnimation);
            gradientBrush.BeginAnimation(RadialGradientBrush.RadiusYProperty, radiusYAnimation);

            return gradientBrush;
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