
using Students.Models;
using System.Data;
using System.Data.SqlClient;

namespace Students.Repositories
{
    public abstract class BaseSQLRepository : ISQLRepository
    {
        private readonly string _connectionString;

        public BaseSQLRepository(string connectionString)
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
                    model.Id = InsertIntoTable(model);
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
                    UpdateInTable(model);
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

        public abstract int InsertIntoTable(Model model);
        public abstract int UpdateInTable(Model model);
    }
}
