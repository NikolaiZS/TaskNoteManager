using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNM.Models
{
    [Table("tasktag")]
    public class TaskTag : BaseModel
    {
        [PrimaryKey("tasktagid", false)]
        public int Tasktagid { get; set; }

        [Column("taskid")]
        public int Taskid { get; set; }

        [Column("tagid")]
        public int Tagid { get; set; }
    }
}