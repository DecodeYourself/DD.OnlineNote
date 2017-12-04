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
#if DEBUG
        const string connectionString = "http://localhost:62140/";
#else
        const string connectionString = "http://onlinenote.azurewebsites.net/";
#endif
        private static ServiceProvider instance;
        public readonly HttpClient Client;

        //static ServiceProvider()
        //{
        //    instance = new ServiceProvider(connectionString);
        //}

        private ServiceProvider(string ConnectionString)
        {
            Client = new HttpClient();
            Client.BaseAddress = new Uri(ConnectionString);
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public static ServiceProvider GetProvider()
        {
            if (instance == null)
                instance = new ServiceProvider(connectionString);

            return instance;
        }

        public async Task<bool> CheckLogin(string loginUsrName)
        {
            HttpResponseMessage Response = await Client.PostAsJsonAsync("api/users/check", loginUsrName);
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
        public async Task<User> Login(User loginUsr)
        {
            HttpResponseMessage Response = await Client.PostAsJsonAsync("api/users/login", loginUsr);
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
        public async Task<Note> CreateNote(Note note)
        {
            HttpResponseMessage Response = await Client.PostAsJsonAsync("api/note/Create", note);
            if (Response.StatusCode == HttpStatusCode.OK)
            {
                return await Response.Content.ReadAsAsync<Note>();
            }
            else
            {
                //ошибка с соединением
                return null;
            }
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

        public async Task<Category> CreateCategory(Guid userId, string name)
        {
            HttpResponseMessage Response = await Client.PostAsJsonAsync($"api/Categories/{userId}", name);
            if (Response.StatusCode == HttpStatusCode.OK)
            {
                return await Response.Content.ReadAsAsync<Category>();
            }
            else
            {
                //ошибка с соединением
                return null;
            }
        }

        public async Task DeleteCategory(Guid categoriesId)
        {
            HttpResponseMessage Response = await Client.DeleteAsync($"api/Categories/{categoriesId}");
            if (Response.StatusCode == HttpStatusCode.OK)
            {
                await Response.Content.ReadAsAsync<User>();
            }
            else
            {
                //ошибка с соединением
                
            }
        }

        public IEnumerable<User> GetSharedUsers(Guid noteId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Category>> GetUserCategories(Guid userId)
        {
            HttpResponseMessage Response = await Client.GetAsync($"api/users/{userId}/categories");
            if (Response.StatusCode == HttpStatusCode.OK)
            {
                return await Response.Content.ReadAsAsync<IEnumerable<Category>>();
            }
            else
            {
                //ошибка с соединением
                return null;
            }
        }

        public async Task<IEnumerable<Note>> GetUserNotes(Guid userId)
        {
            HttpResponseMessage Response = await Client.GetAsync($"api/note/getusernotes/{userId}");
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

        public async Task<Note> UpdateNote(Note note)
        {
            HttpResponseMessage Response = await Client.PostAsJsonAsync("api/note", note);
            if (Response.StatusCode == HttpStatusCode.OK)
            {
                return await Response.Content.ReadAsAsync<Note>();
            }
            else
            {
                //ошибка с соединением
                return null;
            }
        }
        public async Task<Category> UpdateCategory(Category updateCategory)
        {
            
            HttpResponseMessage Response = await Client.PostAsJsonAsync($"api/Categories/", updateCategory);
            if (Response.StatusCode == HttpStatusCode.OK)
            {
                return await Response.Content.ReadAsAsync<Category>();
            }
            else
            {
                //ошибка с соединением
                return null;
            }
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
