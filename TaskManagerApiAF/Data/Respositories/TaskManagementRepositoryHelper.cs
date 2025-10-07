using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerApiAF.Data.Context;
using TaskManagerApiAF.Functions;
using TaskManagerApiAF.Interfaces.IRepositories;
using TaskManagerApiAF.Models;

namespace TaskManagerApiAF.Data.Respositories
{
    public class TaskManagementRepositoryHelper : ITaskManagementRepository
    {
        private readonly DbHelper _dbHelper;
        private readonly ILogger _logger;
        public TaskManagementRepositoryHelper(DbHelper dbHelper, ILoggerFactory loggerFactory)
        {
            _dbHelper = dbHelper;
            _logger = loggerFactory.CreateLogger<TaskFunctions>();
        }

        public Task DeleteSpecificTask(int taskId)
        {
            throw new NotImplementedException();
        }

        public Task<Tasks> GetSpecificTask(int taskId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Tasks>> GetTasks()
        {
            throw new NotImplementedException();
        }

        public async Task<Tasks> CreateTask(Tasks taskData)
        {
            taskData.TaskId = Guid.NewGuid();
            taskData.CreatedAt = DateTime.UtcNow;

            try
            {
                using var conn = _dbHelper.GetConnection();
                await conn.OpenAsync();

                var cmd = new SqlCommand("INSERT INTO Tasks (TaskId, Title, Description, IsCompleted, CreatedAt) VALUES (@Id, @Title, @Description, @IsCompleted, @CreatedAt)", conn);
                cmd.Parameters.AddWithValue("@Id", taskData.TaskId);
                cmd.Parameters.AddWithValue("@Title", taskData.Title);
                cmd.Parameters.AddWithValue("@Description", taskData.Description);
                cmd.Parameters.AddWithValue("@CreatedAt", taskData.CreatedAt);

                await cmd.ExecuteNonQueryAsync();

                return taskData;
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("Generated Error During database update process...", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception("Error Exception while creating Task.", ex);
            }
        }

        public Task UpdateTask(Tasks taskMgmt)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ValidateExistingTask(Tasks taskMgmt)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteTask(int taskId)
        {
            const string storedProcedureName = "sp_DeleteTask";
            try
            {
                using var conn = _dbHelper.GetConnection();
                await conn.OpenAsync();

                var cmd = new SqlCommand(storedProcedureName, conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Opcion", 1);
                cmd.Parameters.AddWithValue("@UserReg", "");
                cmd.Parameters.AddWithValue("@TaskId", taskId);

                int rowsAffected = await cmd.ExecuteNonQueryAsync();
                return rowsAffected > 0;

            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("Generated Error During database update process...", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception("Error Exception while creating Task.", ex);
            }
        }

        Task<bool> ITaskManagementRepository.UpdateTask(Tasks taskData)
        {
            throw new NotImplementedException();
        }

        public Task DeleteSpecificTask(Guid taskId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteTask(Guid taskId)
        {
            throw new NotImplementedException();
        }

        public Task<Tasks> GetSpecificTask(Guid taskId)
        {
            throw new NotImplementedException();
        }
    }
}
