using Microsoft.VisualBasic;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace TNM.Models
{
    [Table("comments")]
    public class Comments : BaseModel
    {
        [PrimaryKey("commentid")]
        public int CommentId { get; set; }

        [Column("taskid")]
        public int TaskId { get; set; }

        [Column("userid")]
        public int UserId { get; set; }

        [Column("content")]
        public string Content { get; set; }

        [Column("createdat")]
        public DateTime CreatedAt { get; set; }
    }
}