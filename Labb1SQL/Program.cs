using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
namespace Labb1SQL
{
    internal class Program
    {
        static void Main(string[] args)
        {

            {
                string ConString = @"Data Source=(localdb)\.;Initial Catalog=Labb1SchoolDB;Integrated Security=True";
                SqlConnection con = new SqlConnection(ConString);
                

                while (true)
                {
                    Console.WriteLine("1. Get all students");
                    Console.WriteLine("2. Get all students from a specifik class");
                    Console.WriteLine("3. Add in a new staff memeber");
                    Console.WriteLine("4. Get existing staff");
                    Console.WriteLine("5. Get all grades from this month");
                    Console.WriteLine("6. GPA per course");
                    Console.WriteLine("7. Add new students");
                    Console.WriteLine("8. Grade students");
                    Console.WriteLine("9. Close");

                    Console.Write("Please select a Number: ");
                    string choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            GetAllStudents();
                            break;

                        case "2":
                            GetStudentsInClass();
                            break;

                        case "3":
                            AddNewStaff();
                            break;

                        case "4":
                            GetStaff();
                            break;

                        case "5":
                            GetRecentMonthGrades();
                            break;

                        case "6":
                            GPAPerCourse();
                            break;

                        case "7":
                            AddNewStudent();
                            break;

                        case "8":
                            GradeStudents();
                            break;

                        case "9":
                            Environment.Exit(0);
                            break;

                        default:
                            Console.WriteLine("Invalid selection. Please try again.");
                            break;
                    }

                    Console.WriteLine("Press Enter to return to the main menu.");
                    Console.ReadLine();
                    Console.Clear();
                }

                static void GetAllStudents()
                {
                    string ConString = @"Data Source=(localdb)\.;Initial Catalog=Labb1SchoolDB;Integrated Security=True";

                    Console.WriteLine("Select sorting option:");
                    Console.WriteLine("1. Sort by first Name (Ascending)");
                    Console.WriteLine("2. Sort by first Name (Descending)");
                    Console.WriteLine("3. Sort by last Name (Ascending)");
                    Console.WriteLine("4. Sort by last Name (Descending)");

                    Console.Write("Enter your choice (1-4): ");
                    string sortChoice = Console.ReadLine();

                    string sortOrder = "ASC"; // Default är stigaade

                    if (sortChoice == "2" || sortChoice == "4")
                    {
                        sortOrder = "DESC"; // Om använder väljer fallande, uppdateras sortorder
                    }

                    string sortBy = "FirstName"; // default sorterar genom förnamn

                    if (sortChoice == "3" || sortChoice == "4")
                    {
                        sortBy = "LastName"; // om användaren väljer efternamn så uppdateras sortby
                    }

                    string query = $"SELECT FirstName, LastName FROM Students ORDER BY {sortBy} {sortOrder}";

                    using (SqlConnection con = new SqlConnection(ConString))
                    {
                        using (SqlCommand command = new SqlCommand(query, con))
                        {
                            con.Open();

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                Console.WriteLine("All Students:");
                                Console.WriteLine("--------------");

                                while (reader.Read())
                                {
                                    Console.WriteLine($"{reader["FirstName"]} {reader["LastName"]}");
                                }
                            }
                        }
                    }
                }

                static List<string> GetAllClasses()
                {
                    string ConString = @"Data Source=(localdb)\.;Initial Catalog=Labb1SchoolDB;Integrated Security=True";
                    SqlConnection con = new SqlConnection(ConString);
                    string query = "SELECT ClassName FROM Classes";

                    List<string> classes = new List<string>();

                    using (SqlConnection connection = new SqlConnection(ConString))
                    {
                        using (SqlCommand command = new SqlCommand(query, con))
                        {
                            con.Open();

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    classes.Add(reader["ClassName"].ToString());
                                }
                            }
                        }
                    }

                    return classes;
                }

                static void GetStudentsInClass()
                {
                    // Hämta alla klasser
                    List<string> classes = GetAllClasses();

                    // Skriv ut alla klasser och låt användaren välja en
                    Console.WriteLine("Choose a Class:");

                    for (int i = 0; i < classes.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {classes[i]}");
                    }

                    Console.Write("Whats the number for the class? (pick 1-3): ");
                    int classChoice = int.Parse(Console.ReadLine());

                    // bekräftar användarens val
                    if (classChoice >= 1 && classChoice <= classes.Count)
                    {
                        
                        string ConString = @"Data Source=(localdb)\.;Initial Catalog=Labb1SchoolDB;Integrated Security=True";
                      
                        // SQL-fråga för att hämta alla elever i en viss klass
                        string query = $"SELECT * FROM Students WHERE ClassID = {classChoice}";

                        
                        using (SqlConnection con = new SqlConnection(ConString))
                        {
                            using (SqlCommand command = new SqlCommand(query, con))
                            {
                                con.Open();
                                
                                using (SqlDataReader reader = command.ExecuteReader())
                                {
                                    Console.WriteLine("StudentID\tFirstName\tLastName\tClassID");
                                    Console.WriteLine("-------------------------------------------");

                                    
                                    while (reader.Read())
                                    {
                                        Console.WriteLine($"{reader["StudentID"]}\t\t{reader["FirstName"]}\t\t{reader["LastName"]}\t\t{reader["ClassID"]}");
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid, Class does not exist, try again.");
                    }
                }

                static void AddNewStaff()
                {
                    string ConString = @"Data Source=(localdb)\.;Initial Catalog=Labb1SchoolDB;Integrated Security=True";

                    Console.WriteLine("Enter firstname:");
                    string firstName = Console.ReadLine();

                    Console.WriteLine("Enter Lastname:");
                    string lastName = Console.ReadLine();

                    Console.WriteLine("Decide category (for example: Teacher, janitor etc):");
                    string category = Console.ReadLine();

                    // Generera en unik identifierare för StaffID (Exempelvis med Guid)
                    Guid staffId = Guid.NewGuid();

                    string query = "INSERT INTO Staff (FirstName, LastName, Category) " +
                                   $"VALUES ('{firstName}', '{lastName}', '{category}')";

                    using (SqlConnection con = new SqlConnection(ConString))
                    {
                        using (SqlCommand command = new SqlCommand(query, con))
                        {
                            con.Open();

                            int affectedRows = command.ExecuteNonQuery();

                            if (affectedRows > 0)
                            {
                                Console.WriteLine("New staff member added successfully.");
                            }
                            else
                            {
                                Console.WriteLine("Failed to add new staff member.");
                            }
                        }
                    }
                }

                static void GetStaff()
                {
                    Console.WriteLine("Do you want to retrieve all personnel (press enter) or filter by category?");
                    Console.Write("Enter category (leave empty to retrieve all): ");
                    string categoryFilter = Console.ReadLine();

                    
                    string ConString = @"Data Source=(localdb)\.;Initial Catalog=Labb1SchoolDB;Integrated Security=True";

                    string query;

                    if (string.IsNullOrEmpty(categoryFilter))
                    {
                        query = "SELECT * FROM Staff";
                    }
                    else
                    {
                        query = $"SELECT * FROM Staff WHERE Category = '{categoryFilter}'";
                    }

                    using (SqlConnection con = new SqlConnection(ConString))
                    {
                        using (SqlCommand command = new SqlCommand(query, con))
                        {
                            con.Open();

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                Console.WriteLine("StaffID\tFirstName\tLastName\tCategory");
                                Console.WriteLine("-------------------------------------------");

                                while (reader.Read())
                                {
                                    Console.WriteLine($"{reader["StaffID"]}\t\t{reader["FirstName"]}\t\t{reader["LastName"]}\t\t{reader["Category"]}");
                                }
                            }
                        }
                    }
                }

                static void GetRecentMonthGrades()
                {
                    string ConString = @"Data Source=(localdb)\.;Initial Catalog=Labb1SchoolDB;Integrated Security=True";

                    // SQL-fråga för att hämta alla betyg senaste månaden
                    string query = "SELECT Students.FirstName, Students.LastName, Courses.CourseName, Grades.Grade " +
                                   "FROM Students " +
                                   "JOIN Grades ON Students.StudentID = Grades.Student_ID " +
                                   "JOIN Courses ON Grades.Course_ID = Courses.CourseID " +
                                   "WHERE Grades.GradeDate >= DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE()) - 1, 0)";

                    using (SqlConnection con = new SqlConnection(ConString))
                    {
                        using (SqlCommand command = new SqlCommand(query, con))
                        {
                            con.Open();

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                Console.WriteLine("FirstName\tLastName\tCourseName\tGrade");
                                Console.WriteLine("-------------------------------------------");

                                while (reader.Read())
                                {
                                    // Använd Console.WriteLine() för varje rad
                                    Console.WriteLine($"{reader["FirstName"]}\t\t{reader["LastName"]}\t\t{reader["CourseName"]}\t\t{reader["Grade"]}");
                                }
                            }
                        }
                    }
                }

                static void GPAPerCourse()
                {
                    string ConString = @"Data Source=(localdb)\.;Initial Catalog=Labb1SchoolDB;Integrated Security=True";

                    // SQL-fråga för att hämta snittbetyg per kurs samt högsta och lägsta betyg
                    string query = "SELECT Courses.CourseName, " +
                                   "AVG(Grades.Grade) AS AverageGrade, " +
                                   "MAX(Grades.Grade) AS MaxGrade, " +
                                   "MIN(Grades.Grade) AS MinGrade " +
                                   "FROM Courses " +
                                   "LEFT JOIN Grades ON Courses.CourseID = Grades.Course_ID " +
                                   "GROUP BY Courses.CourseName";

                    using (SqlConnection con = new SqlConnection(ConString))
                    {
                        using (SqlCommand command = new SqlCommand(query, con))
                        {
                            con.Open();

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                Console.WriteLine("CourseName\tAverageGrade\tMaxGrade\tMinGrade");
                                Console.WriteLine("-------------------------------------------");

                                while (reader.Read())
                                {
                                    Console.WriteLine($"{reader["CourseName"]}\t\t{reader["AverageGrade"]}\t\t{reader["MaxGrade"]}\t\t{reader["MinGrade"]}");
                                }
                            }
                        }
                    }
                }

                static void AddNewStudent()
                {
                    Console.Write("Whats the students firstname?: ");
                    string firstName = Console.ReadLine();

                    Console.Write("Whats the students lastname?: ");
                    string lastName = Console.ReadLine();

                    Console.Write("Whats the ClassID: ");
                    int classID = int.Parse(Console.ReadLine());

                    string ConString = @"Data Source=(localdb)\.;Initial Catalog=Labb1SchoolDB;Integrated Security=True";

                    // SQL-fråga för att lägga till nya elever
                    string query = $"INSERT INTO Students (FirstName, LastName, ClassID) VALUES ('{firstName}', '{lastName}', {classID})";

                    using (SqlConnection con = new SqlConnection(ConString))
                    {
                        using (SqlCommand command = new SqlCommand(query, con))
                        {
                            con.Open();

                            command.ExecuteNonQuery();

                            Console.WriteLine("New student has attended!");
                        }
                    }
                }

                static void GradeStudents()
                {
                    List<string> courses = GetAllCourses();
                    List<string> students = StudentsForGrading();

                    foreach (var student in students)
                    {
                        foreach (var course in courses)
                        {
                            Console.Write($"Grading the student: {student} in Course: {course}: ");
                            int grade = int.Parse(Console.ReadLine());

                            string query = $"INSERT INTO Grades (Student_ID, Course_ID, Grade, GradeDate) " +
                                           $"VALUES ({GetStudentID(student)}, {GetCourseID(course)}, {grade}, GETDATE()); " +
                                           "SELECT SCOPE_IDENTITY();";
                            string ConString = @"Data Source=(localdb)\.;Initial Catalog=Labb1SchoolDB;Integrated Security=True";
                            using (SqlConnection con = new SqlConnection(ConString))
                            {
                                using (SqlCommand command = new SqlCommand(query, con))
                                {
                                    con.Open();

                                    try
                                    {
                                        int gradeID = Convert.ToInt32(command.ExecuteScalar());
                                        Console.WriteLine($"New grade with GradeID {gradeID} has been set.");
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine($"An error occurred: {ex.Message}");
                                    }
                                }
                            }
                        }
                    }
                }

                static List<string> GetAllCourses()
                {
                    string ConString = @"Data Source=(localdb)\.;Initial Catalog=Labb1SchoolDB;Integrated Security=True";
                    string query = "SELECT CourseName FROM Courses";

                    List<string> courses = new List<string>();

                    using (SqlConnection con = new SqlConnection(ConString))
                    {
                        using (SqlCommand command = new SqlCommand(query, con))
                        {
                            con.Open();

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    courses.Add(reader["CourseName"].ToString());
                                }
                            }
                        }
                    }

                    return courses;
                }
                
                static List<string> StudentsForGrading()
                {
                    try
                    {
                        string ConString = @"Data Source=(localdb)\.;Initial Catalog=Labb1SchoolDB;Integrated Security=True";
                        string query = "SELECT FirstName + ' ' + LastName AS FullName FROM Students";

                        List<string> students = new List<string>();

                        using (SqlConnection con = new SqlConnection(ConString))
                        {
                            using (SqlCommand command = new SqlCommand(query, con))
                            {
                                con.Open();

                                using (SqlDataReader reader = command.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        students.Add(reader["FullName"].ToString());
                                    }
                                }
                            }
                        }

                        return students;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"An error occurred: {ex.Message}");
                        return null;
                    }
                }

                static int GetCourseID(string courseName)
                {
                    string ConString = @"Data Source=(localdb)\.;Initial Catalog=Labb1SchoolDB;Integrated Security=True";
                    string query = $"SELECT CourseID FROM Courses WHERE CourseName = '{courseName}'";

                    using (SqlConnection con = new SqlConnection(ConString))
                    {
                        using (SqlCommand command = new SqlCommand(query, con))
                        {
                            con.Open();
                            return (int)command.ExecuteScalar();
                        }
                    }
                }

                static int GetStudentID(string studentName)
                {
                    string ConString = @"Data Source=(localdb)\.;Initial Catalog=Labb1SchoolDB;Integrated Security=True";
                    string query = $"SELECT StudentID FROM Students WHERE FirstName + ' ' + LastName = '{studentName}'";

                    using (SqlConnection con = new SqlConnection(ConString))
                    {
                        using (SqlCommand command = new SqlCommand(query, con))
                        {
                            con.Open();
                            return (int)command.ExecuteScalar();
                        }
                    }
                }


            }
        }

    } 
}