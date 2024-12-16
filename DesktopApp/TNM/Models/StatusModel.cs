using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace TNM.Models
{
    [Table("statuses")]
    public class Statuses : BaseModel
    {
        [PrimaryKey("statusid")]
        public int StatusId { get; set; }

        [Column("statusname")]
        public string StatusName { get; set; }
    }
}