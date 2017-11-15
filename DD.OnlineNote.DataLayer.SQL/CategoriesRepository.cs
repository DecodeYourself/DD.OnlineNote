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
                    command.CommandText = "insert into Category (id, UserId, Name) values (@id, @UserId, @Name)";

                    var category = new Category
                    {
                        Id = Guid.NewGuid(),
                        Name = name
                    };
                    command.Parameters.AddWithValue("@id", category.Id);
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.AddWithValue("@Name", category.Name);
                    command.ExecuteNonQuery();

                    return category;
                }
            }
        }

        public void Delete(Guid categoriesId)
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                using (var command = sqlConnection.CreateCommand())
                {
                    command.CommandText = "delete from Category where id = @id";
                    command.Parameters.AddWithValue("@id", categoriesId);
                    command.ExecuteNonQuery();
                }
            }
        }

        //[Obsolete]
        //public Category Get(Guid categoriesId)
        //{
        //    using (var sqlConnection = new SqlConnection(_connectionString))
        //    {
        //        sqlConnection.Open();
        //        using (var command = sqlConnection.CreateCommand())
        //        {
        //            command.CommandText = "select * from Category where id = @id";
        //            command.Parameters.AddWithValue("@id", categoriesId);
        //            using (var reader = command.ExecuteReader())
        //            {
        //                if (reader.Read())
        //                {
        //                    return new Category
        //                    {
        //                        Name = reader.GetString(reader.GetOrdinal("name")),
        //                        Id = reader.GetGuid(reader.GetOrdinal("id"))
        //                    };
        //                }
        //                else
        //                    return null;

        //            }
        //        }
        //    }
        //}

        public IEnumerable<Category> GetUserCategories(Guid userId)
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                using (var command = sqlConnection.CreateCommand())
                {
                    command.CommandText = "select id, name from Category where userId = @userId";
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
