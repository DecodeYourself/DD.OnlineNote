using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DD.OnlineNote.Model;

namespace DD.OnlineNote.DataLayer
{
    public interface INoteRepository
    {
        Note Create(Note note);
        void Delete(Guid id);
        IEnumerable<Note> GetUserNotes(Guid userId);
        Note UpdateNote(Note note);
    }
}
