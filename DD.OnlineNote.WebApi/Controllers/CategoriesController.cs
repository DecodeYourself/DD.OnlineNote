﻿using System;
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
    public class CategoriesController : ApiController
    {
        private const string _connectionString = @"Data Source=localhost\SQLFORCODING;Initial Catalog=OnlineNote;Integrated Security=true";
        private readonly ICategoriesRepository _categoryRepository;

        public CategoriesController()
        {
            _categoryRepository = new CategoriesRepository(_connectionString);
        }
        [WebExceptionFilters]
        [HttpPost]
        [Route("api/Categories/{userId}")]
        public Category Create([FromBody]string name,Guid userId)
        {
            Logger.Log.Instance.Info("Создание категории с названием: {0}", name);
            return _categoryRepository.Create(userId, name);
        }

        [WebExceptionFilters]
        [HttpDelete]
        [Route("api/Categories/{categoryId}")]
        public void Delete(Guid categoryId)
        {
            Logger.Log.Instance.Info("Удаление категории с id: {0}", categoryId);
            _categoryRepository.Delete(categoryId);
        }

        [WebExceptionFilters]
        [HttpGet]
        [Route("api/Categories/GetUserCategories")]
        public IEnumerable<Category> GetUserCategories([FromBody]Guid userId)
        {
            Logger.Log.Instance.Info("Запрос категорий с  id: {0}", userId);
            return _categoryRepository.GetUserCategories(userId);
        }

        //[WebExceptionFilters]
        //[HttpGet]
        //[Route("api/Categories")]
        //public Category Get([FromBody]Guid categoryId)
        //{
        //    Logger.Log.Instance.Info("Получение категории с id", categoryId);
        //    return _categoryRepository.Get(categoryId);
        //}
    }
}
