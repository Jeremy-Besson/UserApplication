using BalticAmadeusTask.Helpers;
using BalticAmadeusTask.Models;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace BalticAmadeusTask.Services
{
    public class RegisteredUserCrudApiService : IRegisteredUserCRUDApiService
    {
        private readonly HttpClient _client;
        private readonly string _baseRoute = "api/users";

        public RegisteredUserCrudApiService(HttpClient httpClient)
        {
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Add("User-Agent", "HttpClientFactory-Sample");
            _client = httpClient;
        }

        public void SetAPIBaseURL(string apiURL)
        {
            _client.BaseAddress = new Uri(apiURL);
        }

        public async Task<HttpResponseMessage> Get(Guid id)
        {
            return await _client.GetAsync($"{_baseRoute}/{id}");
        }

        public async Task<HttpResponseMessage> GetAll(string searchName, string searchEmail, int? maxElements)
        {
            return await _client.GetAsync($"{_baseRoute}?name={searchName}&email={searchEmail}&maxElements={maxElements}");
        }

        public async Task<HttpResponseMessage> Create(RegisteredUser registeredUser)
        {
            return await _client.PostAsync($"{_baseRoute}", new JsonContent(registeredUser));
        }

        public async Task<HttpResponseMessage> Edit(Guid id, RegisteredUser registeredUser)
        {
            return await _client.PutAsync($"{_baseRoute}/{id}", new JsonContent(registeredUser));
        }

        public Task<HttpResponseMessage> Delete(Guid Id)
        {
            throw new NotImplementedException();
            //return await _client.DeleteAsync($"api/users/{id}");
        }
    }
}
