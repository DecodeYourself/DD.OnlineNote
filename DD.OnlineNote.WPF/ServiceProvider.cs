using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using DD.OnlineNote.Model;

namespace DD.OnlineNote.WPF
{
    public class ServiceProvider
    {
        private static ServiceProvider instance;
        public readonly HttpClient Client;


        private ServiceProvider(string ConnectionString)
        {
            Client = new HttpClient();
            Client.BaseAddress = new Uri(ConnectionString);
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public static ServiceProvider GetProvider(string ConnectionString)
        {
            if(instance == null)
                instance = new ServiceProvider(ConnectionString);

            return instance;
        }

        public async Task<bool> CheckLogin(string loginName)
        {
            HttpResponseMessage Response = await Client.PostAsJsonAsync("api/users/check", loginName);
            if(Response.StatusCode == HttpStatusCode.OK)
            {
                return await Response.Content.ReadAsAsync<bool>();
            }
            else
            {
                //ошибка с соединением
                return false;
            }
        }

        public Note CreateNote(Note note)
        {
            throw new NotImplementedException();
        }

        public async Task<User> CreateUser(User user)
        {
            HttpResponseMessage Response = await Client.PostAsJsonAsync("api/users/", user);
            if (Response.StatusCode == HttpStatusCode.OK)
            {
                return await Response.Content.ReadAsAsync<User>();
            }
            else
            {
                //ошибка с соединением
                return null;
            }
        }

        public Category CreateCategory(Guid userId, string name)
        {
            throw new NotImplementedException();
        }

        public void DeleteCategory(Guid categoriesId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetSharedUsers(Guid noteId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Category> GetUserCategories(Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Note>> GetUserNotes(Guid userId)
        {
            HttpResponseMessage Response = await Client.GetAsync("api/users/check/userId");
            if (Response.StatusCode == HttpStatusCode.OK)
            {
                return await Response.Content.ReadAsAsync<IEnumerable<Note>>();
            }
            else
            {
                //ошибка с соединением
                return null;
            }
        }

        public Note UpdateNote(Note note)
        {
            throw new NotImplementedException();
        }

        bool DeleteUser(Guid Noteid)
        {
            throw new NotImplementedException();
        }

        User GetUser(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
