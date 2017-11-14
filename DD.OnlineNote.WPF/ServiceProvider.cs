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
        private static ServiceProvider instance;
        private readonly HttpClient _client;

        private ServiceProvider(string ConnectionString)
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri(ConnectionString);
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public static ServiceProvider GetProvider(string ConnectionString)
        {
            if(instance == null)
                instance = new ServiceProvider(ConnectionString);
            

            return instance;
        }
    }
}
