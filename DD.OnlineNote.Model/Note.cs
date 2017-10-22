using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DD.OnlineNote.Model
{
    public class Note
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public User Owner { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateChanged { get; set; }

        public IEnumerable<Category> Categories { get; set; }

        public IEnumerable<User> SharedNote { get; set; }
    }
}
