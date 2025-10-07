using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerApiAF.Models;

namespace TaskManagerApiAF.Interfaces.IServices
{
    public interface ITaskManagementService
    {        
        Task<List<Tasks>> GetTasks();
        Task<bool> ValidateExistingTask(Tasks taskData);
        Task<Tasks> CreateTask(TaskBodyRequest taskDataRequest);
        Task<bool> UpdateTask(TaskBodyRequest taskDataRequest, Guid taskId);
        Task DeleteSpecificTask(Guid taskId);
        Task<bool> DeleteTask(Guid taskId);
        Task<Tasks> GetSpecificTask(Guid taskId);
    }
}
