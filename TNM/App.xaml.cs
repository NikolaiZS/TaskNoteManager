using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TNM.Menu;

namespace TNM
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Убедитесь, что главное окно правильно установлено
            var mainMenu = new MainMenu();
            Application.Current.MainWindow = mainMenu;
        }

    }
}
