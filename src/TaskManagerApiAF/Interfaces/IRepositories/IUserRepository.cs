using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerApiAF.Models;

namespace TaskManagerApiAF.Interfaces.IRepositories
{
    public interface IUserRepository
    {
        Task<List<UsersTm>> GetUsers();
        Task<bool> ValidateExistingUser(UsersTm userData);
        Task<UsersTm> CreateUser(UsersTm userData);
        Task<bool> UpdateUser(UsersTm userData);
        Task DeleteSpecificUser(Guid userId);
        Task<bool> DeleteUser(Guid userId);
        Task<UsersTm> GetSpecificUser(Guid userId);
    }
}
