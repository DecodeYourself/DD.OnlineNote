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
    public class NoteRepositoryTests : INoteRepository
    {
        private const string _connectionString = @"Data Source=localhost\SQLFORCODING;Initial Catalog=OnlineNote;Integrated Security=true";
        private readonly List<Guid> _noteIdTodelete = new List<Guid>();

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

       
        public void TestMethod1()
        {
            //
            // TODO: Add test logic here
            //
        }

        [TestMethod]
        public Note Create(Note note)
        {
            throw new NotImplementedException();
        }
        [TestMethod]
        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }
        [TestMethod]
        public IEnumerable<Note> GetUserNotes(Guid userId)
        {
            throw new NotImplementedException();
        }
        [TestMethod]
        public Note UpdateNote(Note note)
        {
            throw new NotImplementedException();
        }
    }
}
