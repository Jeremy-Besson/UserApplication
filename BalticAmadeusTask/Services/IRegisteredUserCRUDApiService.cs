using BalticAmadeusTask.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace BalticAmadeusTask.Services
{
    public interface IRegisteredUserCRUDApiService
    {
        void SetAPIBaseURL(string apiURL);
        Task<HttpResponseMessage> Get(Guid id);
        Task<HttpResponseMessage> GetAll(string searchName, string searchEmail, int? maxNumbers);
        Task<HttpResponseMessage> Create(RegisteredUser registeredUser);
        Task<HttpResponseMessage> Edit(Guid id, RegisteredUser registeredUser);
        Task<HttpResponseMessage> Delete(Guid id);
    }
}
