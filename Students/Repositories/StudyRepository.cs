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
                        @"select * from [UniversityStudy]
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
                        @"insert into [UniversityStudy]
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
                        @"select UniversityStudent.Name as sName, UniversityStudent.Id as sId, UniversityStudent.Age as sAge from [UniversityStudy]
                            join [UniversityStudent] on StudentId = UniversityStudent.Id
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

        public List<Group> GetCountStudentsInGroups()
        {
            List<Group> result = new List<Group>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText =
                        @"select UniversityGroup.Name as gName, UniversityGroup.Id as gId, COUNT(StudentId) as countId from [UniversityStudy]
                            join [UniversityGroup] on GroupId = UniversityGroup.Id
                            where GroupId in (select Id from [UniversityGroup])
                            group by UniversityGroup.Id, UniversityGroup.Name";

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new Group()
                            {
                                Id = Convert.ToInt32(reader["gId"]),
                                Name = Convert.ToString(reader["gName"]),
                                StudentsCount = Convert.ToInt32(reader["countId"])
                            });
                        }
                    }
                }
            }

            return result;
        }
    }
}
