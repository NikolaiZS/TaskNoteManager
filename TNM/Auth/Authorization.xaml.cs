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
        public Authorization()
        {
            InitializeComponent();
            MainGrid.Background = GenerateAnimatedGradientBackground();
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

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = loginBox.Text;
            string password = passwordBox.Password;

            bool isAuthenticated = AuthenticateUser(username, password);

            if (isAuthenticated)
            {
                
                MainMenu mainMenu = new MainMenu();
                mainMenu.Show();
                this.Close();
            }
            else
            {
                assistBlock.Text = "Неверное имя или пароль";
            }
            
        }

        private void RegistrationButton_Click(object sender, RoutedEventArgs e)
        {
            
            Registration reg = new Registration();
            reg.Show();
            this.Close();
        }
        public bool AuthenticateUser(string username, string password)
        {
            using (var context = new TaskNoteManagementDBEntities())
            {
                var user = context.Users.SingleOrDefault(u => u.Username == username);

                if (user == null)
                    return false;

                string hashedPassword = HashPassword(password);
                return hashedPassword == user.PasswordHash;
            }
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