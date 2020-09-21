using System.ComponentModel.DataAnnotations;

namespace Organizer.Models
{
    public class Task
    {
        [Key]
        public int Id { get; set; }
        public int DateId { get; set; }
        public virtual Date Date { get; set; }
        public string Title { get; set; }
    }
}
