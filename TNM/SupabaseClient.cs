using System;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.Http.Headers;
using Windows.System;

public class SupabaseClient
{
    private static readonly HttpClient client = new HttpClient();
    private readonly string _apiUrl = "https://ninnomkcctiulknjoxut.supabase.co";
    string _apiKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6Im5pbm5vbWtjY3RpdWxrbmpveHV0Iiwicm9sZSI6InNlcnZpY2Vfcm9sZSIsImlhdCI6MTczMjMxNjkzNywiZXhwIjoyMDQ3ODkyOTM3fQ.wv5mxSpXKn-h4tWKFt63hXRDR5vuTXaPQKTD-2r_boI";
                     
    public SupabaseClient()
    {
        client.DefaultRequestHeaders.Add("apikey", _apiKey);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

    }

    public async Task<bool> AuthenticateUser(string username, string password)
    {
        try
        {
            // Фильтр по имени пользователя и паролю
            var requestUri = $"{_apiUrl}/rest/v1/users?username=eq.{username}&password=eq.{password}";
            client.DefaultRequestHeaders.Add("apikey", _apiKey);

            var response = await client.GetAsync(requestUri);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var users = JsonConvert.DeserializeObject<List<User>>(content);

                if (users.Count > 0)
                {
                    return true; // Пользователь найден
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль.");
                    return false;
                }
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                MessageBox.Show($"Ошибка авторизации: {errorContent}");
                return false;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> RegisterUser(string username, string password, string email)
    {
        var data = new
        {
            username = username,
            password = password,
            email = email
        };

        var jsonContent = JsonConvert.SerializeObject(data);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        try
        {
            var response = await client.PostAsync($"{_apiUrl}/rest/v1/users", content);

            if (response.IsSuccessStatusCode)
            {
                return true; // Пользователь успешно зарегистрирован
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                MessageBox.Show($"Ошибка регистрации: {errorContent}");
                return false;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка: {ex.Message}");
            return false;
        }
    }


    // Пример получения всех записей из таблицы 'users'
    public async Task GetUsers()
    {
        try
        {
            var response = await client.GetAsync($"{_apiUrl}/rest/v1/users");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject(content);
            Console.WriteLine(users);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    // Пример добавления новой записи в таблицу 'users'
    public async Task AddUser(string name, string email)
    {
        var user = new
        {
            name = name,
            email = email
        };

        var jsonContent = JsonConvert.SerializeObject(user);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        try
        {
            var response = await client.PostAsync($"{_apiUrl}/rest/v1/users", content);
            response.EnsureSuccessStatusCode();
            Console.WriteLine("User added successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
