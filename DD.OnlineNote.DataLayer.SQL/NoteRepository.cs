using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DD.OnlineNote.Model;
using System.Data.SqlClient;

namespace DD.OnlineNote.DataLayer.SQL
{
    public class NoteRepository : INoteRepository
    {
        private readonly string _connectionString;
        public readonly IUserRepository _userRepository;

        public NoteRepository(string ConnectionString)
        {
            if (ConnectionString == null)
                throw new ArgumentException(nameof(ConnectionString));
           
            _connectionString = ConnectionString;
        }
        public Note Create(Note note)
        {
            if (note == null)
                return null;
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                using (var command = sqlConnection.CreateCommand())
                {
                    note.Id = Guid.NewGuid();

                    command.CommandText = "insert into Note ([id],[Title],[Content],[Owner],[DateCreated],[DateChanged],[CategoryName]) values(@id,@Title,@Content,@Owner,@DateCreated,@DateChanged,@CategoryName)";

                    command.Parameters.AddWithValue("@id", note.Id);
                    command.Parameters.AddWithValue("@Title", note.Title);
                    command.Parameters.AddWithValue("@Content", note.Content);
                    command.Parameters.AddWithValue("@Owner", note.Owner.Id);
                    command.Parameters.AddWithValue("@DateCreated", note.DateCreated);
                    command.Parameters.AddWithValue("@DateChanged", note.DateChanged);
                    command.Parameters.AddWithValue("@CategoryName", note.Categories.Single().Id);
                    command.ExecuteNonQuery();

                    return note;
                }
            }
        }

        public bool Delete(Guid NoteId)
        {
           using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                using (var command = sqlConnection.CreateCommand())
                {
                    command.CommandText = "delete from Note where id = @id";
                    command.Parameters.AddWithValue("@id", NoteId);
                    command.ExecuteNonQuery();
                }
            }
            return Get(NoteId) == null ? true : false;
        }
        
        public IEnumerable<Note> GetUserNotes(Guid userId)
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                using (var command = sqlConnection.CreateCommand())
                {
                    command.CommandText = "select id,Title,[Content],Owner,DateCreated,DateChanged,CategoryName from Note where Owner = @userId";
                    command.Parameters.AddWithValue("@userId", userId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Guid _userGuid = reader.GetGuid(reader.GetOrdinal("id"));
                            yield return new Note
                            {
                                Id = _userGuid,
                                Title = reader.GetString(reader.GetOrdinal("Title")),
                                Content = reader.GetString(reader.GetOrdinal("Content")),
                                Owner = new UsersRepository(_connectionString, new CategoriesRepository(_connectionString)).Get(reader.GetGuid(reader.GetOrdinal("Owner"))),
                                DateCreated = reader.GetDateTime(reader.GetOrdinal("DateCreated")),
                                DateChanged = reader.GetDateTime(reader.GetOrdinal("DateChanged")),
                                Categories = new CategoriesRepository(_connectionString).GetUserCategories(reader.GetGuid(reader.GetOrdinal("Owner"))),
                                SharedNote = new NoteRepository(_connectionString).GetSharedUsers(_userGuid)
                            };
                        }
                    }
                }
            }
        }

        public Note UpdateNote(Note note)
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                using (var command = sqlConnection.CreateCommand())
                {
                    command.CommandText = "UPDATE Note SET Title = @Title, Content = @Content, DateChanged = @DateChanged, CategoryName = @CategoryName WHERE id = @id";
                    command.Parameters.AddWithValue("@id", note.Id);
                    command.Parameters.AddWithValue("@Title", note.Title);
                    command.Parameters.AddWithValue("@Content", note.Content);
                    command.Parameters.AddWithValue("@DateChanged", note.DateChanged);
                    command.Parameters.AddWithValue("@CategoryName", note.Categories?.Single()?.Id);
                    command.ExecuteNonQuery();
                }
            }
            return Get(note.Id);
        }

        public IEnumerable<User> GetSharedUsers(Guid noteId)
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                using (var command = sqlConnection.CreateCommand())
                {
                    command.CommandText = "select id,Nickname,Email from Users where id in (select UserId from SharedNote where NoteId = @noteId)";
                    command.Parameters.AddWithValue("@noteId", noteId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Guid _userGuid = reader.GetGuid(reader.GetOrdinal("id"));
                            yield return new User
                            {
                                Id = _userGuid,
                                Name = reader.GetString(reader.GetOrdinal("Nickname")),
                                Categories = new CategoriesRepository(_connectionString).GetUserCategories(_userGuid)
                            };
                        }
                    }
                }
            }
        }
        private Note Get(Guid NoteId)
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                using (var command = sqlConnection.CreateCommand())
                {
                    command.CommandText = "select id,Title,[Content],Owner,DateCreated,DateChanged,CategoryName from Note where id = @noteid";
                    command.Parameters.AddWithValue("@noteid", NoteId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Guid _userGuid = reader.GetGuid(reader.GetOrdinal("id"));
                            return new Note
                            {
                                Title = reader.GetString(reader.GetOrdinal("Title")),
                                Content = reader.GetString(reader.GetOrdinal("[Content]")),
                                Owner = new UsersRepository(_connectionString, new CategoriesRepository(_connectionString)).Get(reader.GetGuid(reader.GetOrdinal("Owner"))),
                                DateCreated = reader.GetDateTime(reader.GetOrdinal("DateCreated")),
                                DateChanged = reader.GetDateTime(reader.GetOrdinal("DateChanged")),
                                Categories = new CategoriesRepository(_connectionString).GetUserCategories(reader.GetGuid(reader.GetOrdinal("Owner"))),
                                SharedNote = new NoteRepository(_connectionString).GetSharedUsers(_userGuid)
                            };
                        }
                        return null;
                    }
                }
            }
           
        }
    }
}
