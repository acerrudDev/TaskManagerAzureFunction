using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerApiAF.Interfaces.IRepositories;
using TaskManagerApiAF.Interfaces.IServices;
using TaskManagerApiAF.Models;

namespace TaskManagerApiAF.Services
{
    public class TaskManagementService : ITaskManagementService
    {
        private readonly ITaskManagementRepository _taskManagementRepository;

        public TaskManagementService(ITaskManagementRepository taskManagementRepository)
        {
            _taskManagementRepository = taskManagementRepository;
        }

        public async Task<Tasks> CreateTask(TaskBodyRequest taskDataRequest)
        {
            var taskData = new Tasks
            {
                Title = taskDataRequest.Title,
                Description = taskDataRequest.Description,
                TaskStatusCode = taskDataRequest.TaskStatusCode,
                TaskPriorityId = taskDataRequest.TaskPriorityId,
                AssignedTo = taskDataRequest.AssignedTo,
                CreatedBy = taskDataRequest.CreatedBy,
                DueDate = taskDataRequest.DueDate,
                CreatedAt = DateTime.UtcNow 
            };

            return await _taskManagementRepository.CreateTask(taskData);
        }

        public async Task DeleteSpecificTask(Guid taskId)
        {
            await _taskManagementRepository.DeleteSpecificTask(taskId);
        }

        public async Task<bool> DeleteTask(Guid taskId)
        {
            return await _taskManagementRepository.DeleteTask(taskId);
        }

        public async Task<Tasks> GetSpecificTask(Guid taskId)
        {
            return await _taskManagementRepository.GetSpecificTask(taskId);
        }

        public async Task<List<Tasks>> GetTasks()
        {
            return await _taskManagementRepository.GetTasks();
        }

        public async Task<bool> UpdateTask(TaskBodyRequest taskDataRequest, Guid taskId)
        {
            var taskData = new Tasks
            {
                TaskId = taskId,
                Title = taskDataRequest.Title,
                Description = taskDataRequest.Description,
                TaskStatusCode = taskDataRequest.TaskStatusCode,
                TaskPriorityId = taskDataRequest.TaskPriorityId,
                AssignedTo = taskDataRequest.AssignedTo,
                CreatedBy = taskDataRequest.CreatedBy,
                DueDate = taskDataRequest.DueDate,
                CreatedAt = DateTime.UtcNow
            };

            return await _taskManagementRepository.UpdateTask(taskData);
        }

        public async Task<bool> ValidateExistingTask(Tasks taskData)
        {
            return await _taskManagementRepository.ValidateExistingTask(taskData);
        }
    }
}
