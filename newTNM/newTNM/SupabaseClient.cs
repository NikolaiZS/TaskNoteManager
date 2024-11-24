using System.Windows;

namespace newTNM
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

            var user = new User
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
                var response = await client.From<User>().Insert(new List<User> { user });
                MessageBox.Show("Пользователь успешно добавлен.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления пользователя: {ex.Message}");
            }
        }

        //            var response = await client.From<User>().Insert(newUser);

        public async Task<List<User>> GetUsersAsync()
        {
            var client = GetClient();

            var response = await client.From<User>().Get();
            return response.Models.ToList();
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            var client = GetClient();

            var response = await client
                .From<User>()
                .Filter("username", Supabase.Postgrest.Constants.Operator.Equals, username)
                .Single();
            return response;
        }

        public async Task<bool> AuthenticateUserAsync(string username, string password)
        {
            // Ищем пользователя в таблице
            var response = await _client
                .From<User>()
                .Filter("username", Supabase.Postgrest.Constants.Operator.Equals, username)
                .Filter("password", Supabase.Postgrest.Constants.Operator.Equals, password)
                .Single();

            return response != null;
        }
    }
}