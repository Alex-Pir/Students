using Students.Models;

namespace Students.Repositories.Interfaces
{
    public interface IStudyRepository
    {
        public void AddStudentInGroup(int studentId, int groupId);
        public List<Student> GetAllStudentsGroup(int groupId);
        public List<string> GetCountStudentsInGroups();
    }
}
