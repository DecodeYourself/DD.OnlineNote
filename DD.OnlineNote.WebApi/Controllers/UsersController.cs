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
        private const string _connectionString = @"Data Source=localhost\SQLFORCODING;Initial Catalog=OnlineNote;Integrated Security=true";
        private readonly IUserRepository _usersRepository;
        public UsersController()
        {
            _usersRepository = new UsersRepository(_connectionString, new CategoriesRepository(_connectionString));
        }

        [HttpGet]
        [Route("api/users/{id}")]
        public User Get(Guid id)
        {
            Logger.Log.Instance.Debug("Запрос пользователя с id: {0}", id);
            return _usersRepository.Get(id);
        }

        /// <summary>
        /// Создание пользователя
        /// </summary>
        /// <param name="user">Пользователь</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/users")]
        public User Post([FromBody] User user)
        {
            Logger.Log.Instance.Debug("Создание пользователя с именем: {0}", user.Name);
            return _usersRepository.Create(user);
        }

        [HttpDelete]
        [Route("api/users/{id}")]
        public void Delete(Guid id)
        {
            Logger.Log.Instance.Debug("Удаление пользователя с id: {0}", id);
            _usersRepository.Delete(id);
        }

        [HttpGet]
        [Route("api/users/{id}/categories")]
        public IEnumerable<Category> GetUserCategories(Guid id)
        {
            return _usersRepository.Get(id).Categories;
        }
    }
}
