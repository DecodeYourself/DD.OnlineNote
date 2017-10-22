using System;
using System.Collections.Generic;
using DD.OnlineNote.Model;
using System.Data.SqlClient;

namespace DD.OnlineNote.DataLayer.SQL
{
    public class CategoriesRepository : ICategoriesRepository
    {
        private readonly string _connectionString;

        public CategoriesRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Category Create(Guid userId, string name)
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                using (var command = sqlConnection.CreateCommand())
                {
                    command.CommandText = "insert into Category (id, Name) values (@id, @Name)";

                    var category = new Category
                    {
                        Id = Guid.NewGuid(),
                        Name = name
                    };
                    command.Parameters.AddWithValue("@id", category.Id);
                    command.Parameters.AddWithValue("@Name", category.Name);
                    command.ExecuteNonQuery();

                    return category;
                }
            }
        }

        public IEnumerable<Category> GetUserCategories(Guid userId)
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                using (var command = sqlConnection.CreateCommand())
                {
                    command.CommandText = "select id, name from categories where userId = @userId";
                    command.Parameters.AddWithValue("@userId", userId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            yield return new Category
                            {
                                Name = reader.GetString(reader.GetOrdinal("name")),
                                Id = reader.GetGuid(reader.GetOrdinal("id"))
                            };
                        }
                    }
                }
            }
        }
    }
}
