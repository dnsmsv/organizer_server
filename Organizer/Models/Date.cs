using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;

namespace Organizer.Models
{
    public class Date
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime TimeStamp { get; set; }
        public virtual List<Task> Tasks { get; set; }
    }
}
