using Students.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Students.Repositories
{
    public class GroupRepository : BaseRepository
    {
        public GroupRepository(string connectionString) : base(connectionString)
        {
        }

        public override Model CreateModelObject(SqlDataReader reader)
        {
            return new Group()
            {
                Id = Convert.ToInt32(reader["ID"]),
                Name = Convert.ToString(reader["Name"]),
            };
        }

        public override string GetTableName()
        {
            return "Group";
        }

        public List<Student> GetGroupStudents(int groupId)
        {
            if (groupId <= 0)
            {
                throw new ArgumentException("Id can not be negative or 0");
            }

            var result = new List<Student>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = $"select * from student where [Group]=@id";
                    command.Parameters.Add("@id", SqlDbType.Int).Value = groupId;

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new Student()
                            {
                                Id = Convert.ToInt32(reader["ID"]),
                                Name = Convert.ToString(reader["Name"]),
                                GroupId = groupId
                            });
                        }
                    }
                }
            }

            return result;
        }
    }
}
