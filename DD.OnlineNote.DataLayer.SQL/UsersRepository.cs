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

        public UsersRepository(string connectionString, ICategoriesRepository categoriesRepository)
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
                    command.CommandText = "insert into Users (id, Nickname,Password) values (@id, @name, @Password)";
                    command.Parameters.AddWithValue("@id", user.Id);
                    command.Parameters.AddWithValue("@name", user.Name);
                    command.Parameters.AddWithValue("@Password", user.Password);
                    command.ExecuteNonQuery();
                }
                _categoriesRepository.Create(user.Id, "Default");
                return user;

            }
        }

        public void Delete(Guid id)
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                using (var command = sqlConnection.CreateCommand())
                {
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
                    command.CommandText = "select id, Nickname,Password from users where id = @id";
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
                            Name = reader.GetString(reader.GetOrdinal("Nickname")),
                            Password = reader.GetString(reader.GetOrdinal("Password"))
                        };
                        user.Categories = _categoriesRepository.GetUserCategories(user.Id);
                        return user;
                    }
                }
            }
        }

        public bool CheckUserByName(string Name)
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                using (var command = sqlConnection.CreateCommand())
                {
                    command.CommandText = "select id, Nickname from users where Nickname = @Name";
                    command.Parameters.AddWithValue("@Name", Name);

                    using (var reader = command.ExecuteReader())
                    {
                        return reader.HasRows;
                    }
                }
            }
        }

        public User LoginUser(User LoginUser)
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                using (var command = sqlConnection.CreateCommand())
                {
                    command.CommandText = "select id, Nickname, Password from users where Nickname = @Name";
                    command.Parameters.AddWithValue("@Name", LoginUser.Name);

                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                            return null;

                        var user = new User
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("id")),
                            Name = reader.GetString(reader.GetOrdinal("Nickname")),
                            Password = reader.GetString(reader.GetOrdinal("Password"))
                        };

                        return LoginUser.Password == user.Password ? user : null;
                    }
                }
            }
        }
    }
}
