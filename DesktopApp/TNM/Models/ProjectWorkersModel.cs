using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNM.Models
{
    [Table("projectworkers")]
    public class ProjectWorker : BaseModel
    {
        [PrimaryKey("projectworkerid", false)]
        public int projectworkerid { get; set; }

        [Column("projectid")]
        public int projectid { get; set; }

        [Column("workerid")]
        public int workerid { get; set; }
    }
}