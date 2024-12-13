using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace TNM.Models
{
    [Table("projectworkers")]
    public class ProjectWorker : BaseModel
    {
        [PrimaryKey("projectworkerid", false)]
        public int ProjectworkerId { get; set; }

        [Column("projectid")]
        public int ProjectId { get; set; }

        [Column("workerid")]
        public int WorkerId { get; set; }
    }
}