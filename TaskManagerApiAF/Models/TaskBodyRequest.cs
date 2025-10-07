using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerApiAF.Models
{
    public class TaskBodyRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string TaskStatusCode { get; set; } = string.Empty;
        public int TaskPriorityId { get; set; } = 0;
        public Guid AssignedTo { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime DueDate { get; set; } = DateTime.UtcNow;
    }
}
