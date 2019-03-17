using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BalticAmadeusTask.Helpers
{
    public static class HTTPHelper
    {
        public static async Task<T> Convert<T>(HttpResponseMessage httpResponseMessage)
        {
            return await httpResponseMessage.Content.ReadAsAsync<T>();
        }
    }
}
