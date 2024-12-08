using System.Windows;
using TNM.Models;

namespace TNM
{
    public class SupabaseClient
    {
        private static Supabase.Client _client;
        private static bool _isInitialized;

        public async Task InitializeAsync()
        {
            if (_isInitialized)
                return;

            const string supabaseUrl = "https://ninnomkcctiulknjoxut.supabase.co";
            const string supabaseKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6Im5pbm5vbWtjY3RpdWxrbmpveHV0Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3MzIzMTY5MzcsImV4cCI6MjA0Nzg5MjkzN30.BYW8RoqthyNXhWOmUESlWPC603PCsQOsPE0gWbBUHEU";

            if (string.IsNullOrWhiteSpace(supabaseUrl) || string.IsNullOrWhiteSpace(supabaseKey))
            {
                throw new InvalidOperationException("Supabase URL или API ключ не заданы.");
            }

            _client = new Supabase.Client(supabaseUrl, supabaseKey);
            await _client.InitializeAsync();

            _isInitialized = true; // Отметить как инициализированный
        }

        public Supabase.Client GetClient()
        {
            if (!_isInitialized || _client == null)
            {
                throw new InvalidOperationException("Supabase клиент не инициализирован.");
            }

            return _client;
        }

        public async Task AddUserAsync(string username, string email, string password)
        {
            var client = App.SupabaseService.GetClient();

            var user = new Users
            {
                Username = username,
                Email = email,
                Password = password, // Теперь используем обновлённое имя свойства
                Role = "user", // Присваиваем роль "user" по умолчанию
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            try
            {
                // Вставка объекта пользователя в базу
                var response = await client.From<Users>().Insert(new List<Users> { user });
                MessageBox.Show("Пользователь успешно добавлен.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления пользователя: {ex.Message}");
            }
        }

        //            var response = await client.From<User>().Insert(newUser);

        public async Task<List<Users>> GetUsersAsync()
        {
            var client = GetClient();

            var response = await client.From<Users>().Get();
            return response.Models.ToList();
        }

        public async Task<Users?> GetUserByUsernameAsync(string username)
        {
            var client = GetClient();

            var response = await client
                .From<Users>()
                .Filter("username", Op.Eq, username)
                .Single();
            return response;
        }

        public async Task<Users> AuthenticateUserAsync(string username, string password)
        {
            var response = await _client
            .From<Users>()
            .Filter("username", Op.Eq, username)
            .Filter("password", Op.Eq, password)
            .Single();

            return response;
        }

        public async Task<List<Projects>> GetAllProjectsAsync()
        {
            var client = App.SupabaseService.GetClient();

            try
            {
                // Получаем все проекты из таблицы "projects"
                var response = await client.From<Projects>().Get();
                return response.Models;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при получении проектов: {ex.Message}");
                return new List<Projects>();
            }
        }
    }
}