using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using System.Windows.Media;
using System.Windows;
using Wpf.Ui.Appearance;

namespace TNM
{
    public class ThemeBackgroundHelper
    {
        /// <summary>
        /// Возвращает текущую тему приложения из конфигурации.
        /// </summary>
        public string GetCurrentTheme()
        {
            return ConfigurationManager.AppSettings["ApplicationTheme"] ?? "Dark";
        }

        /// <summary>
        /// Динамически обновляет цвет brush в зависимости от темы
        /// </summary>
        public void UpdateThemeResources(string theme)
        {
            var resources = Application.Current.Resources;

            if (theme == "Dark")
            {
                resources["TextPrimaryBrush"] = new SolidColorBrush(Colors.White);
            }
            else
            {
                resources["TextPrimaryBrush"] = new SolidColorBrush(Colors.Black);
            }
        }

        /// <summary>
        /// Генерирует фон на основе текущей темы.
        /// </summary>
        public Brush GetBackgroundForCurrentTheme()
        {
            string currentTheme = GetCurrentTheme();

            return currentTheme switch
            {
                "Dark" => GenerateDarkThemeGradient(),
                "Light" or "HighContrast" => GenerateLightThemeGradient(),
                _ => GenerateDarkThemeGradient() // Значение по умолчанию
            };
        }

        /// <summary>
        /// Генерирует градиент для темной темы.
        /// </summary>
        private RadialGradientBrush GenerateDarkThemeGradient()
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

            StartGradientAnimation(gradientBrush);
            return gradientBrush;
        }

        /// <summary>
        /// Генерирует градиент для светлой темы.
        /// </summary>
        private RadialGradientBrush GenerateLightThemeGradient()
        {
            var gradientBrush = new RadialGradientBrush
            {
                GradientOrigin = new Point(0.5, 0.5),
                Center = new Point(0.5, 0.5),
                RadiusX = 0.5,
                RadiusY = 0.5,
                GradientStops = new GradientStopCollection
            {
                new GradientStop(Color.FromRgb(255, 255, 255), 0.0),
                new GradientStop(Color.FromRgb(240, 240, 240), 0.5),
                new GradientStop(Color.FromRgb(220, 220, 220), 1.0)
            }
            };

            StartGradientAnimation(gradientBrush);
            return gradientBrush;
        }

        /// <summary>
        /// Добавляет анимацию к градиенту.
        /// </summary>
        private void StartGradientAnimation(RadialGradientBrush gradientBrush)
        {
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
        }

        /// <summary>
        /// Обновляет фон и другие визуальные элементы из конфига
        /// </summary>
        public void ApplyThemeFromConfig()
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
                    ApplicationTheme.Unknown,
                    Wpf.Ui.Controls.WindowBackdropType.None,
                    false
                );
            }
        }

        /// <summary>
        /// Обновляет значение ApplicationTheme в App.config.
        /// </summary>
        public void UpdateThemeInConfig(string newTheme)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["ApplicationTheme"].Value = newTheme;
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}