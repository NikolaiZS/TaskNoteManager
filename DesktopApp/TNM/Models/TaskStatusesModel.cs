using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace TNM.Models
{
    [Table("taskstatuses")]
    public class TaskStatuses : BaseModel
    {
        [PrimaryKey("statusid")]
        public int StatusId { get; set; }

        [Column("statusname")]
        public string StatusName { get; set; }
    }
}