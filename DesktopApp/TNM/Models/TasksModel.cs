using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace TNM.Models
{
    [Table("tasks")]
    public class TasksModel : BaseModel
    {
        [PrimaryKey("taskid")]
        public int taskid { get; set; }

        [Column("projectid")]
        public int projectid { get; set; }

        [Column("title")]
        public string title { get; set; }

        [Column("description")]
        public string description { get; set; }

        [Column("assigneduserid")]
        public int assigneduserid { get; set; }

        [Column("createduserid")]
        public int createduserid { get; set; }

        [Column("status")]
        public string status { get; set; }

        [Column("priority")]
        public string priority { get; set; }

        [Column("duedate")]
        public DateTime duedate { get; set; }

        [Column("createdate")]
        public DateTime createdate { get; set; }

        [Column("updatedate")]
        public DateTime updateddate { get; set; }

        [Column("tagsid")]
        public int tagsid { get; set; }
    }
}