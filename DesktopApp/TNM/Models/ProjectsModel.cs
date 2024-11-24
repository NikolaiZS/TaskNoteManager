using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace TNM.Models
{
    public class Projects : BaseModel
    {
        [PrimaryKey("projectid")]
        public int projectid { get; set; }

        [Column("projectname")]
        public string projectname { get; set; }

        [Column("projectdescription")]
        public string projectdescription { get; set; }

        [Column("createdat")]
        public DateTime CreatedAt { get; set; }

        [Column("updatedat")]
        public DateTime? UpdatedAt { get; set; }
    }
}