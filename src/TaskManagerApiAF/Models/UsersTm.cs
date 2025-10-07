using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerApiAF.Models
{
    public class UsersTm
    {
        [Key]
        public Guid UserId { get; set; }
        public string ExternalId { get; set; }
        public string Provider { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string Role { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
