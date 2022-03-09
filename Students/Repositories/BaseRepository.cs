
using Students.Models;
using System.Data;
using System.Data.SqlClient;

namespace Students.Repositories
{
    public abstract class BaseRepository : IRepository
    {
        protected readonly string _connectionString;

        public BaseRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Model> GetAll()
        {
            var result = new List<Model>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = $"select * from {GetTableName()}";

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(CreateModelObject(reader));
                        }
                    }
                }
            }

            return result;
        }

        public void Add(Model model)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText =
                        @"insert into [Author]
                            ([Name])
                        values
                            (@name)
                        select SCOPE_IDENTITY()";

                    command.Parameters.Add("@name", SqlDbType.NVarChar).Value = model.Name;

                    model.Id = Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }

        public Model? GetById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText =
                        $@"select *
                        from {GetTableName()}
                        where [Id] = @id";

                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return CreateModelObject(reader);
                        }
                    }
                }
            }

            return null;
        }

        public void Update(Model model)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText =
                        $@"update [{GetTableName()}]
                        set [Name] = @name
                        where [Id] = @id";

                    command.Parameters.Add("@name", SqlDbType.NVarChar).Value = model.Name;
                    command.Parameters.Add("@id", SqlDbType.Int).Value = model.Id;

                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText =
                        $"delete {GetTableName()} where [Id] = @id";

                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;

                    command.ExecuteNonQuery();
                }
            }
        }

        public abstract string GetTableName();
        public abstract Model CreateModelObject(SqlDataReader reader);
    }
}
