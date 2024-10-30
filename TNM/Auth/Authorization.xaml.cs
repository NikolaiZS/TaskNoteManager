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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Wpf.Ui;

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
            string login = loginBox.Text;
            string password = passwordBox.Text;
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

        


        private void enterLogin_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void goToReg_Click(object sender, RoutedEventArgs e)
        {
            var reg = new Registration();
            reg.Show();
            this.Close();
        }
    }
    public class PerlinNoise
    {
        private int[] permutation;

        public PerlinNoise(int seed = 0)
        {
            var random = new Random(seed);
            permutation = new int[512];
            int[] p = new int[256];

            for (int i = 0; i < 256; i++)
                p[i] = i;

            for (int i = 255; i >= 0; i--)
            {
                int swapIndex = random.Next(i + 1);
                int temp = p[i];
                p[i] = p[swapIndex];
                p[swapIndex] = temp;
            }

            for (int i = 0; i < 512; i++)
                permutation[i] = p[i % 256];
        }

        private double Fade(double t) => t * t * t * (t * (t * 6 - 15) + 10);

        private double Lerp(double a, double b, double t) => a + t * (b - a);

        private double Grad(int hash, double x, double y)
        {
            int h = hash & 15;
            double u = h < 8 ? x : y;
            double v = h < 4 ? y : h == 12 || h == 14 ? x : 0;
            return ((h & 1) == 0 ? u : -u) + ((h & 2) == 0 ? v : -v);
        }

        public double Noise(double x, double y)
        {
            int X = (int)Math.Floor(x) & 255;
            int Y = (int)Math.Floor(y) & 255;

            x -= Math.Floor(x);
            y -= Math.Floor(y);

            double u = Fade(x);
            double v = Fade(y);

            int a = permutation[X] + Y;
            int aa = permutation[a];
            int ab = permutation[a + 1];
            int b = permutation[X + 1] + Y;
            int ba = permutation[b];
            int bb = permutation[b + 1];

            return Lerp(v, Lerp(u, Grad(permutation[aa], x, y), Grad(permutation[ba], x - 1, y)),
                        Lerp(u, Grad(permutation[ab], x, y - 1), Grad(permutation[bb], x - 1, y - 1)));
        }
    }
}
