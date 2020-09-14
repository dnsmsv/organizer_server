using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Organizer.Models
{
    public class Task
    {
        [Key]
        public int Id { get; set; }
        [JsonIgnore]
        public int DateId { get; set; }
        [JsonIgnore]
        public virtual Date Date { get; set; }
        public string Title { get; set; }
    }
}
