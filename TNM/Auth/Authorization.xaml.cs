using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Wpf.Ui;
using System.Windows.Media.Animation;
using TNM.Menu;
using System.Security.Cryptography;

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
        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Maximize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = this.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void TitleBar_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == System.Windows.Input.MouseButton.Left)
                this.DragMove();
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var username = loginBox.Text;
            var password = passwordBox.Password;
            string hashedpassword = HashPassword(password);

            var authenticated = await _authClient.AuthenticateUser(username, hashedpassword);
            if (authenticated)
            {
                MessageBox.Show("Login successful!");
            }
            else
            {
                MessageBox.Show("Invalid credentials.");
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
        public static LinearGradientBrush GenerateAnimatedGradientBackground()
        {
            var gradientBrush = new LinearGradientBrush
            {
                StartPoint = new Point(0, 0),
                EndPoint = new Point(1, 1),
                GradientStops = new GradientStopCollection
        {
            new GradientStop(Color.FromRgb(15, 15, 25), 0.0),
            new GradientStop(Color.FromRgb(20, 20, 30), 0.5),
            new GradientStop(Color.FromRgb(10, 10, 20), 1.0)
        }
            };
            var startPointAnimation = new PointAnimation
            {
                From = new Point(0, 0),
                To = new Point(1, 1),
                Duration = new Duration(TimeSpan.FromSeconds(8)),
                RepeatBehavior = RepeatBehavior.Forever,
                AutoReverse = true,
                EasingFunction = new SineEase { EasingMode = EasingMode.EaseInOut }
            };

            var endPointAnimation = new PointAnimation
            {
                From = new Point(1, 1),
                To = new Point(0, 0),
                Duration = new Duration(TimeSpan.FromSeconds(8)),
                RepeatBehavior = RepeatBehavior.Forever,
                AutoReverse = true,
                EasingFunction = new SineEase { EasingMode = EasingMode.EaseInOut }
            };

            gradientBrush.BeginAnimation(LinearGradientBrush.StartPointProperty, startPointAnimation);
            gradientBrush.BeginAnimation(LinearGradientBrush.EndPointProperty, endPointAnimation);

            return gradientBrush;
        }

        
    }
}