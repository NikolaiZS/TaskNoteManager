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
            MainGrid.Background = GenerateAnimatedGradientBackground();
            _authClient = new SupabaseClient();
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var username = loginBox.Text;
            var password = passwordBox.Password;
            string hashedpassword = HashPassword(password);

            var authenticated = await _authClient.AuthenticateUserAsync(username, hashedpassword);
            if (authenticated)
            {
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
    }
}