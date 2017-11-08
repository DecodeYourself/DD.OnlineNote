using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DD.OnlineNote.Model;

namespace DD.OnlineNote.DataLayer.SQL.Tests
{
    /// <summary>
    /// Summary description for NoteRepositoryTests
    /// </summary>
    [TestClass]
    public class NoteRepositoryTests 
    {
        private const string _connectionString = @"Data Source=localhost\SQLFORCODING;Initial Catalog=OnlineNote;Integrated Security=true";
        private static List<Note> _noteIdTodelete = new List<Note>();

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
        public void Create()
        {
            User _user = new UsersRepository(_connectionString, new CategoriesRepository(_connectionString)).Create(new User { Name = "TestUser123" });
            Category _category = new CategoriesRepository(_connectionString).Create(_user.Id, $"Category123 {_user.Name}");

           
            Note testNote = new Note
            {
                Title = "TestTitle24345",
                Content = "TestContent24345",
                Owner = _user,
                DateCreated = DateTime.Now,
                DateChanged = DateTime.Now,
                Categories = new CategoriesRepository(_connectionString).GetUserCategories(_user.Id),
                SharedNote = null
            };
            var noteToSQL = new NoteRepository(_connectionString).Create(testNote);
            _noteIdTodelete.Add(noteToSQL);

            Assert.AreEqual(testNote.Owner, noteToSQL.Owner);
            
        }
        [TestMethod]
        public void Delete()
        {
            Assert.IsTrue(new NoteRepository(_connectionString).Delete(_noteIdTodelete[0].Id));
        }
        [TestMethod]
        public void GetUserNotes()
        {
            IEnumerable<Note> UserNotes = new NoteRepository(_connectionString).GetUserNotes(_noteIdTodelete[0].Owner.Id);
            foreach (Note item in UserNotes)
            {
                Assert.IsTrue(item.Owner.Id == _noteIdTodelete[0].Owner.Id);
            }
        }
        [TestMethod]
        public void UpdateNote()
        {
            User _user = new UsersRepository(_connectionString, new CategoriesRepository(_connectionString)).Create(new User { Name = "TestUserUpdater" });
            Category _category = new CategoriesRepository(_connectionString).Create(_user.Id, $"CategoryUpdater {_user.Name}");


            Note testNote = new Note
            {
                Title = "TestTitleUpdate",
                Content = "TestContentUpdate",
                Owner = _user,
                DateCreated = DateTime.Now,
                DateChanged = DateTime.Now,
                Categories = new CategoriesRepository(_connectionString).GetUserCategories(_user.Id),
                SharedNote = null
            };
            var noteToSQL = new NoteRepository(_connectionString).Create(testNote);

            noteToSQL.Title = "SuccessUpdate";
            noteToSQL.Content = "eeee";

            var noteAfterUpdate = new NoteRepository(_connectionString).UpdateNote(noteToSQL);

            Assert.AreEqual(noteToSQL.Content, noteAfterUpdate.Content);
        }
        //[TestCleanup]
        public void CleanData()
        {
            //foreach (var id in _noteIdTodelete)
            //    new UsersRepository(_connectionString, new CategoriesRepository(_connectionString)).Delete(id);

        }

        //public IEnumerable<User> GetSharedUsers(Guid noteId)
        //{
        //    return new NoteRepository().GetSharedUsers()
        //}
    }
}
