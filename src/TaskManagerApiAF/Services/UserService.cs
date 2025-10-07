using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerApiAF.Interfaces.IServices;
using TaskManagerApiAF.Models;

namespace TaskManagerApiAF.Services
{
    public class UserService : IUserService
    {
        public Task<UsersTm> CreateUser(UsersTm userData)
        {
            throw new NotImplementedException();
        }

        public Task DeleteSpecificUser(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteUser(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<UsersTm> GetSpecificUser(Guid taskId)
        {
            throw new NotImplementedException();
        }

        public Task<List<UsersTm>> GetUsers()
        {
            throw new NotImplementedException();
        }

        public Task UpdateUser(UsersTm userData)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ValidateExistingUser(UsersTm userData)
        {
            throw new NotImplementedException();
        }

        Task<bool> IUserService.UpdateUser(UsersTm userData)
        {
            throw new NotImplementedException();
        }
    }
}
