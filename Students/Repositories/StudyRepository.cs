using Students.Models;
using Students.Repositories.Interfaces;
using System.Data;
using System.Data.SqlClient;

namespace Students.Repositories
{
    public class StudyRepository : IStudyRepository
    {
        protected readonly string _connectionString;

        public StudyRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddStudentInGroup(int studentId, int groupId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText =
                        @"select * from [Study]
                            where StudentId = @studentId AND GroupId = @groupId";

                    command.Parameters.Add("@studentId", SqlDbType.NVarChar).Value = studentId;
                    command.Parameters.Add("@groupId", SqlDbType.NVarChar).Value = groupId;

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            throw new Exception("Student already in group");
                        }
                    }

                    command.CommandText =
                        @"insert into [Study]
                            ([StudentId], [GroupId])
                        values
                            (@name)";

                    command.ExecuteScalar();
                }
            }
        }

        public List<Student> GetAllStudentsGroup(int groupId)
        {
            var result = new List<Student>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText =
                        @"select Student.Name, Student.Id, Student.Age from [Student]
                            join [Study] on Study.GroupId = @groupId";

                    command.Parameters.Add("@groupId", SqlDbType.NVarChar).Value = groupId;

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new Student()
                            {
                                Id = Convert.ToInt32(reader["Student.Id"]),
                                Name = Convert.ToString(reader["Student.Name"]),
                                Age = Convert.ToInt32(reader["Student.Age"])
                            });
                        }
                    }
                }
            }

            return result;
        }
    }
}
