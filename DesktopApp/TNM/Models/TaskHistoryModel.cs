using Microsoft.VisualBasic;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace TNM.Models
{
    [Table("taskhistory")]
    public class TaskHistory : BaseModel
    {
        [PrimaryKey("historyid")]
        public int HistoryId { get; set; }

        [Column("taskid")]
        public int TaskId { get; set; }

        [Column("changedbyuserid")]
        public int ChangedByUserId { get; set; }

        [Column("oldstatus")]
        public string OldStatus { get; set; }

        [Column("newstatus")]
        public string NewStatus { get; set; }

        [Column("changedate")]
        public DateTime ChangedDate { get; set; }

        [Column("description")]
        public string Description { get; set; }
    }
}