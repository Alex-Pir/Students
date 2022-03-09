using Students.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Students.Repositories
{
    public interface ISQLRepository
    {
        void Add(Model author);
        void DeleteById(int id);
        Model? GetById(int id);
        List<Model> GetAll();
        void Update(Model model);
    }
}
