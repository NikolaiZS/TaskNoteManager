using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace TNM.Models
{
    [Table("tags")]
    public class Tags : BaseModel
    {
        [PrimaryKey("tagid")]
        public int TagId { get; set; }

        [Column("tagname")]
        public string TagName { get; set; }
    }
}