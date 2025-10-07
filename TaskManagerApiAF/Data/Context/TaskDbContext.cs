using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerApiAF.Models;

namespace TaskManagerApiAF.Data.Context
{
    public class TaskDbContext : DbContext
    {
        public TaskDbContext(DbContextOptions<TaskDbContext> options) : base(options) { }

        public DbSet<Tasks> Tasks { get; set; }
        public DbSet<UsersTm> UsersTm { get; set; }
    }
}
