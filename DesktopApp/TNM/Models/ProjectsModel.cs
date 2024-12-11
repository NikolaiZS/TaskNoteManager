using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace TNM.Models
{
    [Table("projects")]
    public class Projects : BaseModel
    {
        [PrimaryKey("projectid", false)]
        public int ProjectId { get; set; }

        [Column("projectname")]
        public string Projectname { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("createdat")]
        public DateTime CreatedAt { get; set; }

        [Column("updatedat")]
        public DateTime? UpdatedAt { get; set; }

        [Column("ownerid")]
        public int OwnerId { get; set; }
    }
}