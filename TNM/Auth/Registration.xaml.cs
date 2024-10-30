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
            MainGrid.Background = GeneratePerlinNoiseBackground(800, 800, 0.0005);
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
        private static ImageBrush GeneratePerlinNoiseBackground(int width, int height, double scale)
        {
            var bitmap = new WriteableBitmap(width, height, 96, 96, PixelFormats.Gray8, null);
            var noise = new byte[width * height];

            // Generate a new random seed for each run
            var randomSeed = new Random().Next();
            var perlin = new PerlinNoise(randomSeed);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    double perlinValue = perlin.Noise(x * scale, y * scale);

                    // Adjust to grey-to-black transition
                    byte colorValue = (byte)(50 * (perlinValue + 0.4));
                    noise[y * width + x] = colorValue;
                }
            }

            bitmap.WritePixels(new Int32Rect(0, 0, width, height), noise, width, 0);
            return new ImageBrush(bitmap);
        }
    }
}
