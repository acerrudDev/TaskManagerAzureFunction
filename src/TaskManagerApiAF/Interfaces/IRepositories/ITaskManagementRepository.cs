using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerApiAF.Models;

namespace TaskManagerApiAF.Interfaces.IRepositories
{
    public interface ITaskManagementRepository
    {
        Task<List<Tasks>> GetTasks();
        Task<bool> ValidateExistingTask(Tasks taskData);
        Task<Tasks> CreateTask(Tasks taskData);
        Task<bool> UpdateTask(Tasks taskData);
        Task DeleteSpecificTask(Guid taskId);
        Task<bool> DeleteTask(Guid taskId);
        Task<Tasks> GetSpecificTask(Guid taskId);
    }
}
