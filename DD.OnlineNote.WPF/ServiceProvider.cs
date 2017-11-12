using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using DD.OnlineNote.Model;

namespace DD.OnlineNote.WPF
{
    public class ServiceProvider
    {
        private readonly HttpClient _client;
        public ServiceProvider(string connectionString)
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri(connectionString);
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
