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
    public class UserRepository : IUserRepository
    {
        private readonly TaskDbContext _context;
        public UserRepository(TaskDbContext context)
        {
            _context = context;
        }
        public async Task<UsersTm> CreateUser(UsersTm userData)
        {
            try
            {
                _context.UsersTm.Add(userData);
                await _context.SaveChangesAsync();
                return userData;
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("Generated Error During database creating process...", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception("Error Exception while creating User.", ex);
            }
        }

        public async Task DeleteSpecificUser(Guid userId)
        {
            try
            {
                var specificUser = await _context.UsersTm.FindAsync(userId);
                if (specificUser != null)
                {
                    _context.UsersTm.Remove(specificUser);
                    await _context.SaveChangesAsync();
                }
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("Generated Error During database deleting process...", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception("Error Exception while deleting User.", ex);
            }
        }

        public async Task<bool> DeleteUser(Guid userId)
        {
            const string storedProcedureName = "sp_DeleteUser";

            try
            {
                int rowsAffected = await _context.Database.ExecuteSqlRawAsync(
                    $"EXEC {storedProcedureName} @UserId = {0}",
                    userId
                );

                return rowsAffected > 0 ? true : false;
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("Generated Error During database deleting process...", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception("Error Exception while deleting User.", ex);
            }
        }

        public async Task<UsersTm> GetSpecificUser(Guid userId)
        {
            try
            {
                return await _context.UsersTm.FindAsync(userId);
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("Generated Error During database selecting process...", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception("Error Exception while selecting User.", ex);
            }
        }

        public async Task<List<UsersTm>> GetUsers()
        {
            try
            {
                return await _context.UsersTm.ToListAsync();
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("Generated Error During database selecting process...", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception("Error Exception while selecting User.", ex);
            }
        }

        public async Task UpdateUser(UsersTm userData)
        {
            try
            {
                _context.Entry(userData).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("Generated Error During database update process...", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception("Error Exception while updating User.", ex);
            }
        }

        public async Task<bool> ValidateExistingUser(UsersTm userData)
        {
            try
            {
                return await _context.UsersTm.AnyAsync(t => t.UserId == userData.UserId);
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("Generated Error During database finding process...", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception("Error Exception while finding User.", ex);
            }
        }

        Task<bool> IUserRepository.UpdateUser(UsersTm userData)
        {
            throw new NotImplementedException();
        }
    }
}
