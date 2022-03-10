
using Students.Models;
using Students.Repositories.Interfaces;
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
            if (!IsValidModel(model))
            {
                throw new Exception("Model is not valid!");
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    model.Id = AddInTable(model, command);
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

        public virtual void Update(Model model)
        {
            if (!IsValidModel(model))
            {
                throw new Exception("Model is not valid!");
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    UpdateInTable(model, command);
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

        protected virtual bool IsValidModel(Model model)
        {
            return model.Name != null && model.Name.Trim().Length != 0;
        }


        protected abstract int AddInTable(Model model, SqlCommand command);
        protected abstract void UpdateInTable(Model model, SqlCommand command);
        protected abstract string GetTableName();
        protected abstract Model CreateModelObject(SqlDataReader reader);
    }
}
