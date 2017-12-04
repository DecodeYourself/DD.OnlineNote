using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DD.OnlineNote.Model;


namespace DD.OnlineNote.DataLayer.SQL.Tests
{
    [TestClass]
    public class UsersRepositoryTests
    {
#if !DEBUG
        //private const string _connectionString = @"Data Source=localhost\SQLFORCODING;Initial Catalog=OnlineNote;Integrated Security=true";
        private const string _connectionString = @"Server=tcp:srv-onlinenote.database.windows.net,1433;Initial Catalog=onlinenoteDB;Persist Security Info=False;User ID=WebAccess;Password=ApiReader2017;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

#else
        private const string _connectionString = @"Server=tcp:srv-onlinenote.database.windows.net,1433;Initial Catalog=onlinenoteDB;Persist Security Info=False;User ID=WebAccess;Password=ApiReader2017;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
#endif
        private readonly List<Guid> _tempUsers = new List<Guid>();

        [TestMethod]
        public void ShouldCreateUser()
        {
            //arrange
            var user = new User
            {
                Name = "test"
            };

            //act
            var repository = new UsersRepository(_connectionString, new CategoriesRepository(_connectionString));
            var result = repository.Create(user);

            _tempUsers.Add(user.Id);

            var userFromDb = repository.Get(result.Id);

            //asserts
            Assert.AreEqual(user.Name, userFromDb.Name);
        }

        [TestMethod]
        public void ShouldCreateUserAndAddCategory()
        {
            

            //arrange
            var user = new User
            {
                Name = "test"
            };
            const string category = "testCategory";

            //act
            var categoriesRepository = new CategoriesRepository(_connectionString);
            var usersRepository = new UsersRepository(_connectionString, categoriesRepository);
            user = usersRepository.Create(user);

            _tempUsers.Add(user.Id);
            

            categoriesRepository.Create(user.Id, category);
            user = usersRepository.Get(user.Id);

            //asserts
            Assert.AreEqual(category, user.Categories.Single().Name);
        }

        [TestCleanup]
        public void CleanData()
        {
            foreach (var id in _tempUsers)
                new UsersRepository(_connectionString, new CategoriesRepository(_connectionString)).Delete(id);

        }
    }
}
