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
    public class NotesController : ApiController
    {
#if !DEBUG
        //private const string _connectionString = @"Data Source=localhost\SQLFORCODING;Initial Catalog=OnlineNote;Integrated Security=true";
        private const string _connectionString = @"Server=tcp:srv-onlinenote.database.windows.net,1433;Initial Catalog=onlinenoteDB;Persist Security Info=False;User ID=********;Password=*********;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

#else
        private const string _connectionString = @"Server=tcp:srv-onlinenote.database.windows.net,1433;Initial Catalog=onlinenoteDB;Persist Security Info=False;User ID=********;Password=*********;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
#endif
        private readonly INoteRepository _noteRepository;

        public NotesController()
        {
            _noteRepository = new NoteRepository(_connectionString);
        }

        [WebExceptionFilters]
        [HttpPost]
        [Route("api/note/Create")]
        public Note Create([FromBody]Note note)
        {
            Logger.Log.Instance.Trace("Создание заметки с названием: {0}, владелец: {1}", note.Title , note.Owner);
            return _noteRepository.Create(note);
        }

        [WebExceptionFilters]
        [HttpDelete]
        [Route("api/note/{noteId}")]
        public bool Delete(Guid noteId)
        {
            
            Logger.Log.Instance.Trace("Удаление заметки с id: {0}", noteId);
            return _noteRepository.Delete(noteId);
        }

        [WebExceptionFilters]
        [HttpGet]
        [Route("api/note/getusernotes/{userId}")]
        public IEnumerable<Note> GetUserNotes(Guid userId)
        {
            Logger.Log.Instance.Trace("Запрос заметок пользователя с id: {0}", userId);
            return _noteRepository.GetUserNotes(userId);
        }
        [WebExceptionFilters]
        [HttpPost]
        [Route("api/note")]
        public Note UpdateNote([FromBody]Note note)
        {
            Logger.Log.Instance.Trace("Обновление заметки с id", note.Id);
            return _noteRepository.UpdateNote(note);         
        }
        [WebExceptionFilters]
        [HttpGet]
        [Route("api/note/GetSharedUsers/")]
        public IEnumerable<User> GetSharedUsers([FromBody]Guid noteId)
        {
            Logger.Log.Instance.Trace("Запрос пользователей с доступом к заметке с id: {0}", noteId);
            return _noteRepository.GetSharedUsers(noteId);
        }
        [WebExceptionFilters]
        [HttpGet]
        [Route("api/note/GetCategoryName/{noteId}")]
        public Guid GetCategoryNameByNoteId(Guid noteId)
        {
            Logger.Log.Instance.Trace("Запрос имени категории от заметки с id: {0}", noteId);
            return _noteRepository.GetCategoryNameByNoteId(noteId);
        }
    }
}
