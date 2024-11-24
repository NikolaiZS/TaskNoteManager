using Microsoft.VisualBasic;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace TNM.Models
{
    [Table("attachments")]
    public class Attachments : BaseModel
    {
        [PrimaryKey("attachmentid")]
        public int AttachmentId { get; set; }

        [Column("taskid")]
        public int TaskId { get; set; }

        [Column("filepath")]
        public string FilePath { get; set; }

        [Column("filename")]
        public string FileName { get; set; }

        [Column("uploadedat")]
        public DateTime UploadedAt { get; set; }
    }
}