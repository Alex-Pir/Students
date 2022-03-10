using Students.Models;
using System.Data;
using System.Data.SqlClient;

namespace Students.Repositories
{
    public class StudentRepository : BaseRepository
    {
        public StudentRepository(string connectionString) : base(connectionString)
        {}

        protected override Model CreateModelObject(SqlDataReader reader)
        {
            return new Student()
            {
                Id = Convert.ToInt32(reader["Id"]),
                Name = Convert.ToString(reader["Name"]),
                Age = Convert.ToInt32(reader["Age"])
            };
        }

        protected override string GetTableName()
        {
            return "Student";
        }

        protected override bool IsValidModel(Model model)
        {
            bool result = base.IsValidModel(model);

            if (!result && !(model is Student))
            {
                return false;
            }

            Student student = (Student)model;

            return student.Age > 0;
        }

        protected override void UpdateInTable(Model model, SqlCommand command)
        {
            Student student = (Student)model;

            command.CommandText =
                        $@"update [{GetTableName()}]
                        set [Name] = @name, [Age] = @age
                        where [Id] = @id";

            command.Parameters.Add("@name", SqlDbType.NVarChar).Value = student.Name;
            command.Parameters.Add("@id", SqlDbType.Int).Value = student.Id;
            command.Parameters.Add("@age", SqlDbType.Int).Value = student.Age;

            command.ExecuteNonQuery();
        }

        protected override int AddInTable(Model model, SqlCommand command)
        {
            Student student = (Student)model;

            command.CommandText =
                        $@"insert into [{GetTableName()}]
                            ([Name], [Age])
                        values
                            (@name, @age)
                        select SCOPE_IDENTITY()";

            command.Parameters.Add("@name", SqlDbType.NVarChar).Value = student.Name;
            command.Parameters.Add("@age", SqlDbType.Int).Value = student.Age;

            return Convert.ToInt32(command.ExecuteScalar());
        }
    }
}
