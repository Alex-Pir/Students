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

        protected override Model CreateModelObject(SqlDataReader reader)
        {
            return new Group()
            {
                Id = Convert.ToInt32(reader["ID"]),
                Name = Convert.ToString(reader["Name"]),
            };
        }

        protected override string GetTableName()
        {
            return "Groups";
        }

        protected override bool IsValidModel(Model model)
        {
            bool result = base.IsValidModel(model);

            return result && (model is Group);
        }

        protected override int AddInTable(Model model, SqlCommand command)
        {
            command.CommandText =
                        $@"insert into [{GetTableName()}]
                            ([Name])
                        values
                            (@name)
                        select SCOPE_IDENTITY()";

            command.Parameters.Add("@name", SqlDbType.NVarChar).Value = model.Name;

            return Convert.ToInt32(command.ExecuteScalar());
        }

        protected override void UpdateInTable(Model model, SqlCommand command)
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
