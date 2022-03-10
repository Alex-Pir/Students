using Students.Models;
using Students.Repositories;

namespace Students
{
    public class Program
    {
        private const string DB_CONNECTION = @"Data Source=LAPTOP-H58NE3UN;Initial Catalog=test_database;Pooling=true;Integrated Security=SSPI";

        public static void Main(string[] args)
        {
            StudentRepository studentRepository = new StudentRepository(DB_CONNECTION);
            GroupRepository groupRepository = new GroupRepository(DB_CONNECTION);
            StudyRepository studyRepository = new StudyRepository(DB_CONNECTION);

            Console.WriteLine("Доступные команды:");
            Console.WriteLine("get-students - показать список студентов");
            Console.WriteLine("add-student - добавить студента");
            Console.WriteLine("get-groups - показать список групп");
            Console.WriteLine("add-group - добавить группу");
            Console.WriteLine("get-group-students - вывести студентов по id группы");
            Console.WriteLine("add-to-group - добавить студента в группу");
            Console.WriteLine("exit - выйти из приложения");

            while (true)
            {
                try
                {
                    string command = Console.ReadLine();

                    if (command == "get-students")
                    {
                        List<Model> students = studentRepository.GetAll();

                        foreach (Student student in students)
                        {
                            Console.WriteLine($"Id: {student.Id}, Name: {student.Name}, Age: {student.Age}");
                        }
                    }
                    else if (command == "add-student")
                    {
                        Console.WriteLine("Введите имя студента");
                        string name = Console.ReadLine();

                        Console.WriteLine("Введите возраст студента");
                        int age = Convert.ToInt32(Console.ReadLine());

                        studentRepository.Add(new Student
                        {
                            Name = name,
                            Age = age
                        });
                        Console.WriteLine("Успешно добавлено");
                    }
                    else if (command == "get-groups")
                    {
                        List<Model> groups = groupRepository.GetAll();

                        foreach (Group group in groups)
                        {
                            Console.WriteLine($"Id: {group.Id}, Name: {group.Name}");
                        }
                    }
                    else if (command == "add-group")
                    {
                        Console.WriteLine("Введите название группы");
                        string name = Console.ReadLine();

                        groupRepository.Add(new Group
                        {
                            Name = name
                        });
                        Console.WriteLine("Успешно добавлено");
                    }
                    else if (command == "get-group-students")
                    {
                        Console.WriteLine("Введите ID группы");
                        int id = Convert.ToInt32(Console.ReadLine());

                        if (id <= 0)
                        {
                            throw new ArgumentException("ID can not negative or 0");
                        }

                        List<Student> students = studyRepository.GetAllStudentsGroup(id);

                        foreach (Student student in students)
                        {
                            Console.WriteLine($"Id: {student.Id}, Name: {student.Name}");
                        }
                    }
                    else if (command == "exit")
                    {
                        return;
                    }
                    else
                    {
                        Console.WriteLine("Команда не найдена");
                    }
                } catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }
    }
}