using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace TNM.Models
{
    [Table("tasks")]
    public class Tasks : BaseModel
    {
        [PrimaryKey("taskid")]
        public int TaskId { get; set; }

        [Column("projectid")]
        public int ProjectId { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("createduserid")]
        public int CreateduserId { get; set; }

        [Column("statusid")]
        public int TaskStatusId { get; set; }

        [Column("duedate")]
        public DateTime Duedate { get; set; }

        [Column("createdate")]
        public DateTime CreateDate { get; set; }

        [Column("updatedate")]
        public DateTime UpdatedDate { get; set; }

        [Column("iscompleted")]
        public bool isCompleted { get; set; }
    }
}