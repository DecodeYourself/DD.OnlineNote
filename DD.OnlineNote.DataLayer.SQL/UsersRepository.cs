using System;
using DD.OnlineNote.Model;
using System.Data.SqlClient;
using System.Net;

namespace DD.OnlineNote.DataLayer.SQL
{
    public class UsersRepository : IUserRepository
    {
        public readonly string _connectionString;
        public readonly ICategoriesRepository _categoriesRepository;

        public UsersRepository(string connectionString,ICategoriesRepository categoriesRepository)
        {
            if (connectionString == null)
                throw new ArgumentException(nameof(connectionString));
            if (categoriesRepository == null)
                throw new ArgumentException(nameof(categoriesRepository));

            _connectionString = connectionString;
            _categoriesRepository = categoriesRepository;
        }
        public User Create(User user)
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                using (var command = sqlConnection.CreateCommand())
                {
                    user.Id = Guid.NewGuid();
                    command.CommandText = "insert into Users (id, Nickname) values (@id, @name)";
                    command.Parameters.AddWithValue("@id", user.Id);
                    command.Parameters.AddWithValue("@name", user.Name);
                    command.ExecuteNonQuery();
                    return user;
                }
            }
        }

        public void Delete(Guid id)
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                using (var command = sqlConnection.CreateCommand())
                {
                    //SqlTransaction transaction;
                    //transaction = sqlConnection.BeginTransaction("Delete");

                    command.CommandText = "delete from Users where id = @id";
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
            }
        }

        public User Get(Guid id)
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                using (var command = sqlConnection.CreateCommand())
                {
                    command.CommandText = "select id, Nickname from users where id = @id";
                    command.Parameters.AddWithValue("@id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        //if (!reader.Read())
                        //    throw new ArgumentException($"Пользователь с id {id} не найден");
                        if (!reader.Read())
                            return null;
                        
                        var user = new User
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("id")),
                            Name = reader.GetString(reader.GetOrdinal("Nickname"))
                        };
                        user.Categories = _categoriesRepository.GetUserCategories(user.Id);
                        return user;
                    }
                }
            }
        }
    }
}
