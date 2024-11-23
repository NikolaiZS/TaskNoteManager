using Newtonsoft.Json;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

[Table("users")] // Таблица в базе данных
public class User : BaseModel
{
    [PrimaryKey("userid", false)] // Поле userid как первичный ключ
    public int UserId { get; set; }

    [Column("username")]
    public string Username { get; set; }

    [Column("email")]
    public string Email { get; set; }

    [Column("password")] // Сопоставляем с новым названием столбца
    public string Password { get; set; }

    [Column("role")]
    public string Role { get; set; }

    [Column("createdat")]
    public DateTime CreatedAt { get; set; }

    [Column("updatedat")]
    public DateTime? UpdatedAt { get; set; }
}
