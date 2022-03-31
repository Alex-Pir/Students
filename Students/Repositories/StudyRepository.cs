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

        public void AddStudentInGroup(Study study)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText =
                        @"select * from [Studies]
                            where StudentId = @studentId AND GroupId = @groupId";

                    command.Parameters.Add("@studentId", SqlDbType.NVarChar).Value = study.StudentId;
                    command.Parameters.Add("@groupId", SqlDbType.NVarChar).Value = study.GroupId;

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            throw new Exception("Студент уже в группе");
                        }
                    }

                    command.CommandText =
                        @"insert into [Studies]
                            ([StudentId], [GroupId])
                        values
                            (@studentId, @groupId)";

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
                        @"select Student.Name as sName, Student.Id as sId, Student.Age as sAge from [Studies]
                            join [Student] on StudentId = Student.Id
                            where GroupId = @groupId";

                    command.Parameters.Add("@groupId", SqlDbType.Int).Value = groupId;

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new Student()
                            {
                                Id = Convert.ToInt32(reader["sId"]),
                                Name = Convert.ToString(reader["sName"]),
                                Age = Convert.ToInt32(reader["sAge"])
                            });
                        }
                    }
                }
            }

            return result;
        }

        public List<string> GetCountStudentsInGroups()
        {
            List<string> result = new List<string>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText =
                        @"select Groups.Name as gName, COUNT(StudentId) as countId from [Studies]
                            join [Groups] on GroupId = Groups.Id
                            where GroupId in (select Id from [Groups])
                            group by Groups.Name";

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add($"{reader["gName"]} | {reader["countId"]}");
                        }
                    }
                }
            }

            return result;
        }
    }
}
