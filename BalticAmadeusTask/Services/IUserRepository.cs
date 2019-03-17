using BalticAmadeusTask.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BalticAmadeusTask.Services
{
    public interface IUserRepository
    {
        Task Create(RegisteredUser registeredUser);
        Task<IEnumerable<RegisteredUser>> GetAll();
        Task<RegisteredUser> GetById(Guid id);
        Task Update(RegisteredUser registeredUser);
        Task Delete(RegisteredUser registeredUser);
        Task<bool> IsEmailAlreadyExist(string userEmail, Guid exceptForUserId = new Guid());
    }
}
