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
    public class NotesController : ApiController
    {
        private const string _connectionString = @"Data Source=localhost\SQLFORCODING;Initial Catalog=OnlineNote;Integrated Security=true";
        private readonly INoteRepository _noteRepository;

        public NotesController()
        {
            _noteRepository = new NoteRepository(_connectionString);
        }

        [HttpPost]
        [Route("api/note/Create")]
        public Note Create(Note note)
        {
            Logger.Log.Instance.Info("Создание заметки с названием: {0}, владелец: {1}", note.Title , note.Owner);
            return _noteRepository.Create(note);
        }

        [HttpDelete]
        [Route("api/note/{noteId}")]
        public bool Delete(Guid noteId)
        {
            Logger.Log.Instance.Info("Удаление заметки с id: {0}", noteId);
            return _noteRepository.Delete(noteId);
        }

        [HttpGet]
        [Route("api/note/getusernotes")]
        public IEnumerable<Note> GetUserNotes(Guid userId)
        {
            Logger.Log.Instance.Info("Запрос заметок пользователя с  id: {0}", userId);
            return _noteRepository.GetUserNotes(userId);
        }

        [HttpPost]
        [Route("api/note")]
        public Note UpdateNote(Note note)
        {
            Logger.Log.Instance.Info("Обновление заметки с id", note.Id);
            return _noteRepository.UpdateNote(note);
        }
        [HttpGet]
        [Route("api/note/GetSharedUsers")]
        public IEnumerable<User> GetSharedUsers(Guid noteId)
        {
            Logger.Log.Instance.Info("Запрос пользователей с доступом к заметке с id: {0}", noteId);
            return _noteRepository.GetSharedUsers(noteId);
        }
    }
}
