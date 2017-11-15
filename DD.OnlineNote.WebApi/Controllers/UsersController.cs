using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DD.OnlineNote.DataLayer;
using DD.OnlineNote.DataLayer.SQL;
using DD.OnlineNote.Model;

namespace DD.OnlineNote.WebApi.Controllers
{
    public class UsersController : ApiController
    {
#if DEBUG
        private const string _connectionString = @"Data Source=localhost\SQLFORCODING;Initial Catalog=OnlineNote;Integrated Security=true";
#else
        private const string _connectionString = @"Server=tcp:srv-onlinenote.database.windows.net,1433;Initial Catalog=onlinenoteDB;Persist Security Info=False;User ID=WebAccess;Password=ApiReader2017;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
#endif
        private readonly IUserRepository _usersRepository;
        public UsersController()
        {
            _usersRepository = new UsersRepository(_connectionString, new CategoriesRepository(_connectionString));
        }
        [WebExceptionFilters]
        [HttpGet]
        [Route("api/Users/{id}")]
        public User Get(Guid id)
        {
            Logger.Log.Instance.Trace("Запрос пользователя с id: {0}", id);
            return _usersRepository.Get(id);
        }

        /// <summary>
        /// Создание пользователя
        /// </summary>
        /// <param name="user">Пользователь</param>
        /// <returns></returns>
        [WebExceptionFilters]
        [HttpPost]
        [Route("api/users/")]
        public User Post([FromBody] User user)
        {
            Logger.Log.Instance.Trace("Создание пользователя с именем: {0}", user.Name);
            return _usersRepository.Create(user);
        }
        [WebExceptionFilters]
        [HttpDelete]
        [Route("api/users/{id}")]
        public void Delete(Guid id)
        {
            Logger.Log.Instance.Trace("Удаление пользователя с id: {0}", id);
            _usersRepository.Delete(id);
        }
        [WebExceptionFilters]
        [HttpGet]
        [Route("api/users/{id}/categories")]
        public IEnumerable<Category> GetUserCategories(Guid id)
        {
            Logger.Log.Instance.Trace("Получение категорий пользователя с id: {0}", id);
            return _usersRepository.Get(id).Categories;
        }
        [WebExceptionFilters]
        [HttpPost]
        [Route("api/users/check")]
        public bool CheckUserByName([FromBody]string name)
        {
            Logger.Log.Instance.Trace("Поиск пользователя с именем: {0}", name);
            return _usersRepository.CheckUserByName(name);
        }
    }
}
