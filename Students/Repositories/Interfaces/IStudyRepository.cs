using Students.Models;

namespace Students.Repositories.Interfaces
{
    public interface IStudyRepository
    {
        public void AddStudentInGroup(Study study);
        public List<Student> GetAllStudentsGroup(int groupId);
        public List<Group> GetCountStudentsInGroups();
    }
}
