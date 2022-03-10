using Students.Models;

namespace Students.Repositories.Interfaces
{
    public interface IRepository
    {
        void Add(Model author);
        void DeleteById(int id);
        Model? GetById(int id);
        List<Model> GetAll();
        void Update(Model model);
    }
}
