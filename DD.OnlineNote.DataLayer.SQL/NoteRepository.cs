using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DD.OnlineNote.Model;

namespace DD.OnlineNote.DataLayer.SQL
{
    class NoteRepository : INoteRepository
    {
        public Note Create(Note note)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Note> GetUserNotes(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Note UpdateNote(Note note)
        {
            throw new NotImplementedException();
        }
    }
}
