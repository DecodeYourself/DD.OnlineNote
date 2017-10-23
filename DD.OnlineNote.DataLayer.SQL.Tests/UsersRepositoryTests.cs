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
        private const string ConnectionString = @"Data Source=localhost\SQLFORCODING;Initial Catalog=OnlineNote;Integrated Security=true";
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
            var repository = new UsersRepository(ConnectionString, new CategoriesRepository(ConnectionString));
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
            var categoriesRepository = new CategoriesRepository(ConnectionString);
            var usersRepository = new UsersRepository(ConnectionString, categoriesRepository);
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
                new UsersRepository(ConnectionString, new CategoriesRepository(ConnectionString)).Delete(id);

            //new UsersRepository(ConnectionString, new CategoriesRepository(ConnectionString)).Delete(Guid.Parse("150A896A-877B-4AFF-8FEF-A8F7879DB7F3"));
           
        }
    }
}
