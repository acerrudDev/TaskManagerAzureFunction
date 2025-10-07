using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerApiAF.Models
{
    public class Tasks
    {
        [Key]
        public Guid TaskId { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TaskSecuence { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string TaskStatusCode { get; set; } = string.Empty;
        public int TaskPriorityId { get; set; } = 0;
        public Guid AssignedTo { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime DueDate { get; set; } = DateTime.UtcNow;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
