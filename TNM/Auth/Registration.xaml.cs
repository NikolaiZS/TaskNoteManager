using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Wpf.Ui;

namespace TNM.Auth
{
    /// <summary>
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration
    {
        public Registration()
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

        public void RegisterUser(string username, string password, string email)
        {
            using (var context = new TaskNoteManagementDBEntities())
            {
                if (context.Users.Any(u => u.Username == username))
                {
                    assistBox.Text = "Пользователь с таким именем уже существует.";
                    return;
                }

                var newUser = new Users
                {
                    Username = username,
                    PasswordHash = HashPassword(password), 
                    Email = email,
                    Role = "User",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                try
                {
                    context.Users.Add(newUser);
                    context.SaveChanges();
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                {
                    foreach (var validationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            string errortext = ($"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}");
                        }
                    }
                    assistBox.Text = "Ошибка при валидации данных. Проверьте введенные значения.";
                }
                Authorization auth = new Authorization();
                auth.Show();
                this.Close();

            }
        }

        private void RegistrationButton_Click(object sender, RoutedEventArgs e)
        {
            string username = loginBox.Text;
            string password = passwordBox.Password;
            string email = mailBox.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                assistBox.Text = "Введите имя пользователя и пароль.";
                return;
            }

            RegisterUser(username, password, email);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Authorization auth = new Authorization();
            auth.Show();
            this.Close();
        }
    }
}
