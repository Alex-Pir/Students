using Students.Models;
using System.Data;
using System.Data.SqlClient;

namespace Students.Repositories
{
    public class StudentRepository : BaseRepository
    {
        public StudentRepository(string connectionString) : base(connectionString)
        {}

        public override Model CreateModelObject(SqlDataReader reader)
        {
            return new Student()
            {
                Id = Convert.ToInt32(reader["ID"]),
                Name = Convert.ToString(reader["Name"]),
                GroupId = Convert.ToInt32(reader["GroupId"])
            };
        }

        public override string GetTableName()
        {
            return "Student";
        }

        public void Update(Student model)
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
    }
}
