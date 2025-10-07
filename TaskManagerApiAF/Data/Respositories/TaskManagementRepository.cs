using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerApiAF.Data.Context;
using TaskManagerApiAF.Interfaces.IRepositories;
using TaskManagerApiAF.Models;

namespace TaskManagerApiAF.Data.Respositories
{
    public class TaskManagementRepository : ITaskManagementRepository
    {
        private readonly TaskDbContext _context;
        public TaskManagementRepository(TaskDbContext context)
        {
            _context = context;
        }

        public async Task DeleteSpecificTask(Guid taskId)
        {
            try
            {
                var specificTask = await _context.Tasks.FindAsync(taskId);
                if (specificTask != null)
                {
                    _context.Tasks.Remove(specificTask);
                    await _context.SaveChangesAsync();
                }
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("Generated Error During database deleting process...", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception("Error Exception while deleting Task.", ex);
            }
        }

        public async Task<Tasks> GetSpecificTask(Guid taskId)
        {
            try
            {
                return await _context.Tasks.FindAsync(taskId);
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("Generated Error During database selecting process...", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception("Error Exception while selecting Task.", ex);
            }
        }

        public async Task<List<Tasks>> GetTasks()
        {            
            try
            {
                return await _context.Tasks.ToListAsync();
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("Generated Error During database selecting process...", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception("Error Exception while selecting Task.", ex);
            }
        }

        public async Task<Tasks> CreateTask(Tasks taskData)
        {
            try
            {
                _context.Tasks.Add(taskData);
                await _context.SaveChangesAsync();
                return taskData;
            }
            catch (DbUpdateException dbEx) 
            {
                throw new Exception("Generated Error During database creating process...", dbEx);
            }
            catch (Exception ex) 
            {
                throw new Exception("Error Exception while creating Task.", ex);
            }
        }

        public async Task<bool> UpdateTask(Tasks taskData)
        {
            try
            {
                _context.Entry(taskData).State = EntityState.Modified;
                _context.Entry(taskData).Property(x => x.TaskSecuence).IsModified = false;
                var rows = await _context.SaveChangesAsync();
                return rows > 0;
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("Generated Error During database update process...", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception("Error Exception while updating Task.", ex);
            }
        }

        public async Task<bool> ValidateExistingTask(Tasks taskData)
        {
            try
            {
                return await _context.Tasks.AnyAsync(t => t.TaskId == taskData.TaskId);
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("Generated Error During database finding process...", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception("Error Exception while finding Task.", ex);
            }
        }

        public async Task<bool> DeleteTask(Guid taskId)
        {
            const string storedProcedureName = "sp_DeleteTask";

            try {
                var taskIdParam = new SqlParameter("@TaskId", taskId);

                int rowsAffected = await _context.Database.ExecuteSqlRawAsync(
                    $"EXEC {storedProcedureName} @TaskId", taskIdParam
                );

                return rowsAffected > 0 ? true : false;
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("Generated Error During database deleting process...", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception("Error Exception while deleting Task.", ex);
            }

        }
    }
}
