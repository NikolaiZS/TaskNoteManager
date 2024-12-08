using System.Windows.Controls;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;
using System.Configuration;
using System.Windows;
using Wpf.Ui.Input;
using System.Windows.Controls.Primitives;

namespace TNM.Pages
{
    /// <summary>
    /// Логика взаимодействия для Settings.xaml
    /// </summary>
    public partial class Settings : Page
    {
        public Settings()
        {
            InitializeComponent();
            var themeHelper = new ThemeBackgroundHelper();
            string currentTheme = themeHelper.GetCurrentTheme();
            foreach (ComboBoxItem item in ThemeComboBox.Items)
            {
                if (item.Tag.ToString() == currentTheme)
                {
                    ThemeComboBox.SelectedItem = item;
                    break;
                }
            }
        }

        private void ThemeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox && comboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                string selectedTheme = selectedItem.Tag.ToString();
                var themeHelper = new ThemeBackgroundHelper();
                var currentTheme = themeHelper.GetCurrentTheme();
                if (selectedTheme != currentTheme)
                {
                    themeHelper.UpdateThemeInConfig(selectedTheme);
                    ShowRestartSnackbar();
                }
            }
        }

        public class RelayCommand : System.Windows.Input.ICommand
        {
            private readonly Action<object> _execute;
            private readonly Func<object, bool> _canExecute;

            public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
            {
                _execute = execute;
                _canExecute = canExecute;
            }

            public bool CanExecute(object parameter) => _canExecute?.Invoke(parameter) ?? true;

            public void Execute(object parameter) => _execute(parameter);

            public event EventHandler CanExecuteChanged;
        }

        private void ShowRestartSnackbar()
        {
            var restartButton = new Wpf.Ui.Controls.Button
            {
                Content = "Применить",
                Margin = new Thickness(10, 0, 0, 0)
            };
            restartButton.Click += (s, e) => RestartApplication();
            var snackbarContent = new StackPanel
            {
                Orientation = Orientation.Horizontal
            };
            snackbarContent.Children.Add(new Wpf.Ui.Controls.TextBlock
            {
                Text = "Тема изменена. Применить изменения?",
                FontSize = 14,
                VerticalAlignment = VerticalAlignment.Center
            });

            snackbarContent.Children.Add(restartButton);
            var snackbar = new Wpf.Ui.Controls.Snackbar(SnackbarPresenter)
            {
                Content = snackbarContent,
                Timeout = TimeSpan.FromSeconds(10)
            };
            snackbar.Show();
        }

        private void RestartApplication()
        {
            var currentWindow = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive);
            if (currentWindow != null)
            {
                var themeHelper = new ThemeBackgroundHelper();
                var currentTheme = themeHelper.GetCurrentTheme();
                themeHelper.ApplyThemeFromConfig();
                themeHelper.UpdateThemeResources(currentTheme);
                var newWindow = (Window)Activator.CreateInstance(currentWindow.GetType());
                newWindow.Show();
                currentWindow.Close();
            }
        }
    }
}