using Students.Models;
using Students.Repositories;
using System.Text.RegularExpressions;

namespace Students
{
    public class Program
    {
        private const string DB_CONNECTION = @"Data Source=LAPTOP-H58NE3UN;Initial Catalog=test_database;Pooling=true;Integrated Security=SSPI";

        private const string STRING_REGEX = @"\s+";

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
            Console.WriteLine("report - отчет о количестве студентов в группах");
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

                        if (students.Count == 0)
                        {
                            Console.WriteLine("Студенты не найдены");
                        }
                    }
                    else if (command == "add-student")
                    {
                        Console.WriteLine("Введите имя студента");

                        string name = ReadStringByConsole();

                        int age;

                        Console.WriteLine("Введите возраст студента");

                        if (!int.TryParse(Console.ReadLine(), out age) || age <= 0)
                        {
                            throw new Exception("Возраст должен быть положительным числом");
                        }

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

                        foreach (Models.Group group in groups)
                        {
                            Console.WriteLine($"Id: {group.Id}, Name: {group.Name}");
                        }

                        if (groups.Count == 0)
                        {
                            Console.WriteLine("Группы не найдены");
                        }
                    }
                    else if (command == "add-group")
                    {
                        Console.WriteLine("Введите название группы");
                        string name = ReadStringByConsole();

                        groupRepository.Add(new Models.Group
                        {
                            Name = name
                        });

                        Console.WriteLine("Успешно добавлено");
                    }
                    else if (command == "get-group-students")
                    {
                        Console.WriteLine("Введите ID группы");

                        int id;

                        if (!int.TryParse(Console.ReadLine(), out id))
                        {
                            throw new Exception("ID должен быть числом");
                        }

                        CheckId(id);

                        List<Student> students = studyRepository.GetAllStudentsGroup(id);

                        foreach (Student student in students)
                        {
                            Console.WriteLine($"Id: {student.Id}, Name: {student.Name}");
                        }

                        if (students.Count == 0)
                        {
                            Console.WriteLine("В группе нет студентов");
                        }
                    }
                    else if (command == "add-to-group")
                    {
                        Console.WriteLine("Введите ID группы");
                        
                        int groupId;
                        int studentId;

                        if (!int.TryParse(Console.ReadLine(), out groupId))
                        {
                            throw new Exception("ID группы должен быть числом");
                        }

                        CheckId(groupId);

                        if (!groupRepository.ElementExists(groupId))
                        {
                            throw new Exception("Группы с таким идентификатором не существует");
                        }

                        Console.WriteLine("Введите ID студента");

                        if (!int.TryParse(Console.ReadLine(), out studentId))
                        {
                            throw new Exception("ID студента должен быть числом");
                        }

                        CheckId(studentId);

                        if (!studentRepository.ElementExists(studentId))
                        {
                            throw new Exception("Студента с таким идентификатором не существует");
                        }

                        studyRepository.AddStudentInGroup(
                            new Study()
                            {
                                StudentId = studentId,
                                GroupId = groupId
                            }
                        );
                        Console.WriteLine("Успешно добавлено");
                    }
                    else if (command == "report")
                    {
                        
                        List<Models.Group> groups = studyRepository.GetCountStudentsInGroups();

                        foreach (Models.Group group in groups)
                        {
                            Console.WriteLine($"{group.Name} | {group.StudentsCount}");
                        }

                        if (groups.Count == 0)
                        {
                            Console.WriteLine("В группах нет студентов");
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
                    Console.WriteLine(ex.Message);
                }
            }
        }

        protected static void CheckId(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID должен быть положительным целым числом");
            }
        }

        protected static string ReadStringByConsole()
        {
            return new Regex(STRING_REGEX)
                .Replace(Console.ReadLine(), @" ")
                .Trim();
        }
    }
}