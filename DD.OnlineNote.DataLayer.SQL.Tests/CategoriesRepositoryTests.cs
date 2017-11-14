using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using DD.OnlineNote.Model;

namespace DD.OnlineNote.DataLayer.SQL.Tests
{
    /// <summary>
    /// Summary description for CategoriesRepositoryTests
    /// </summary>
    [TestClass]
    public class CategoriesRepositoryTests
    {
        private const string _connectionString = @"Server=tcp:srv-onlinenote.database.windows.net,1433;Initial Catalog=onlinenoteDB;Persist Security Info=False;User ID=WebAccess;Password=ApiReader2017;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        private readonly List<Guid> _userIdForDelete = new List<Guid>();

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void CreateCategoriesTest()
        {
            User _user = new User
            {
                Name = "BigTestUser",
            };

            var categoriesRepos = new CategoriesRepository(_connectionString);
            var userRepos = new UsersRepository(_connectionString, categoriesRepos);

            User createdUser = userRepos.Create(_user);
            var categorForTest = categoriesRepos.Create(createdUser.Id, _user.Name + " categories");

            //_userIdForDelete.Add(createdUser.Id);
           
           
            var DBCategoriesrId = categoriesRepos.Get(categorForTest.Id);

            Assert.AreEqual(categorForTest.Id, DBCategoriesrId.Id);
            

        }
        [TestCleanup()]
        public void CleanUp()
        {
            foreach (var Id in _userIdForDelete)
            {
                new UsersRepository(_connectionString, new CategoriesRepository(_connectionString)).Delete(Id);
                var catRepo = new CategoriesRepository(_connectionString);
                catRepo.Delete(catRepo.GetUserCategories(Id).Single().Id);
            }
              
        }
    }
}
