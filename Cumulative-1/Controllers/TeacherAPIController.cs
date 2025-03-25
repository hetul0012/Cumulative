using Cumulative_1.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace Cumulative_1.Controllers
{
    public class TeacherAPIController : Controller
    {
        private readonly SchoolDbContext _context;
        public TeacherAPIController(SchoolDbContext context)
        {
            _context = context;
        }
            
        /// <summary>
        /// Returns a list of Teachers in the system
        /// </summary>
        /// <example>
        /// GET /localhost:7044/ListTeacherNames -> ["Alexander Bennett","Caitlin Cummings", "Linda Chan"..]
        /// </example>
        /// <returns>
        /// A list of strings, formatted "{First Name} {Last Name}"
        /// </returns>
        [HttpGet]
        [Route(template: "ListTeacherNames")]
        public List<string> ListTeacherNames()
        {
            // Create an empty list of Teacher Names
            List<string> TeacherNames = new List<string>();


            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();

                MySqlCommand Command = Connection.CreateCommand();

                //SQL QUERY
                Command.CommandText = "select * from teachers";

                // Gather Result Set of Query into a variable
                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    //Loop Through Each Row the Result Set
                    while (ResultSet.Read())
                    {

                        string TeacherFName = ResultSet["teacherfname"].ToString();
                        string TeacherLName = ResultSet["teacherlname"].ToString();


                        //Access Column information by the DB column name as an index
                        string TeacherName = $"{TeacherFName} {TeacherLName}";
                        //Add the Teacher Name to the List
                        TeacherNames.Add(TeacherName);
                    }
                }
            }


            //Return the final list of Teacher names
            return TeacherNames;
        }


        /// <summary>
        /// Returns an Teacher in the database by their ID
        /// </summary>
        /// <example>
        /// GET api/Teacher/FindTeacher/4 -> {"TeacherId":4,"TeacherFname":"Linda","TeacherLName":"Chan"}
        /// </example>
        /// <returns>
        /// A matching Teacher object by its ID. Empty object if Teacher not found
        /// </returns>
        [HttpGet(template: "FindTeacher/{id}")]
        public Teacher FindTeacher(int id)
        {

            Teacher SelectedTeacher = new Teacher();

            // 'using' will close the connection after the code executes
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                //Establish a new command (query) for our database
                MySqlCommand Command = Connection.CreateCommand();

                // @id is replaced with a 'sanitized' id
                Command.CommandText = "select * from teachers where teacherid=@id";
                Command.Parameters.AddWithValue("@id", id);

                // Gather Result Set of Query into a variable
                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    //Loop Through Each Row the Result Set
                    while (ResultSet.Read())
                    {
                        //Access Column information by the DB column name as an index
                        int Id = Convert.ToInt32(ResultSet["teacherid"]);
                        string FirstName = ResultSet["teacherfname"].ToString();
                        string LastName = ResultSet["teacherlname"].ToString();
                        string Employeenumber = ResultSet["employeenumber"].ToString();
                        DateTime TeacherhireDate = Convert.ToDateTime(ResultSet["hiredate"]);
                        decimal TeacherSalary = Convert.ToDecimal(ResultSet["salary"]);


                        SelectedTeacher.TeacherId = Id;
                        SelectedTeacher.TeacherFName = FirstName;
                        SelectedTeacher.TeacherLName = LastName;
                        SelectedTeacher.EmployeeNumber = Employeenumber;
                        SelectedTeacher.HireDate = TeacherhireDate;
                        SelectedTeacher.Salary = TeacherSalary;

                    }
                }
            }


            //Return the final list of teacher names
            return SelectedTeacher;
        }

        /// <summary>
        /// Returns All information of Teacher in the system
        /// </summary>
        /// <example>
        /// GET /localhost:7044/ListTeacherAllData -> ["5 Jessica Morris T389 04-06-2012 12.00.00 AM 48.62"]
        /// </example>
        /// <returns>
        /// A list of strings, formatted "{TeacherId} {TeacherFName} {TeacherLName} {Employeenumber} {Hiredate} {Salary}"
        /// </returns>

        [HttpGet]
        [Route(template: "ListTeacherID")]
        public List<Teacher> ListTeacherID()
        {
            // Create an empty list of Teachers
            List<Teacher> TeacherList = new List<Teacher>();

            // 'using' will close the connection after the code executes
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                MySqlCommand Command = Connection.CreateCommand();

                // SQL QUERY
                Command.CommandText = "select * from teachers";

                // Gather Result Set of Query into a variable
                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    while (ResultSet.Read())
                    {
                        Teacher teacher = new Teacher
                        {
                            TeacherId = Convert.ToInt32(ResultSet["teacherid"]),
                            TeacherFName = ResultSet["teacherfname"].ToString(),
                            TeacherLName = ResultSet["teacherlname"].ToString(),
                            EmployeeNumber = ResultSet["employeenumber"].ToString(),
                            HireDate = Convert.ToDateTime(ResultSet["hiredate"]),
                            Salary = Convert.ToDecimal(ResultSet["salary"])
                        };

                        // Add the teacher object to the list
                        TeacherList.Add(teacher);
                    }
                }
            }

            // Return the final list of teachers
            return TeacherList;
        }


        /// <summary>
        /// Returns a list of Students in the system
        /// </summary>
        /// <example>
        /// GET /localhost:7044/ListStudent -> ["Sarah Sarah","Jennifer Faulkner",..]
        /// </example>
        /// <returns>
        /// A list of strings, formatted "{First Name} {Last Name}"
        /// </returns>
        [HttpGet]
        [Route(template: "ListStudents")]
        public List<string> ListStudents()
        {
            // Create an empty list of Student Names
            List<string> StudentNames = new List<string>();


            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();

                MySqlCommand Command = Connection.CreateCommand();

                //SQL QUERY
                Command.CommandText = "select * from students";

                // Gather Result Set of Query into a variable
                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    //Loop Through Each Row the Result Set
                    while (ResultSet.Read())
                    {

                        string StudentFName = ResultSet["studentfname"].ToString();
                        string StudentLName = ResultSet["studentlname"].ToString();


                        //Access Column information by the DB column name as an index
                        string StudentName = $"{StudentFName} {StudentLName}";
                        //Add the Student Name to the List
                        StudentNames.Add(StudentName);
                    }
                }
            }


            //Return the final list of Students names
            return StudentNames;
        }


        /// <summary>
        /// Returns a list of Courses in the system
        /// </summary>
        /// <example>
        /// GET /localhost:7044/ListCourses -> ["1","http5101","Web Application Development"]
        /// </example>
        /// <returns>
        /// A list of strings, formatted "{CourseId} {CourseCode} {CourseName}"
        /// </returns>


        [HttpGet]
        [Route(template: "ListCourse")]
        public List<Course> ListCourse()
        {
            // Create an empty list of Teachers
            List<Course> CourseList = new List<Course>();

            // 'using' will close the connection after the code executes
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                MySqlCommand Command = Connection.CreateCommand();

                // SQL QUERY
                Command.CommandText = "select * from courses";

                // Gather Result Set of Query into a variable
                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    while (ResultSet.Read())
                    {
                        Course course = new Course
                        {
                            CourseId = Convert.ToInt32(ResultSet["courseid"]),
                            CourseCode = ResultSet["coursecode"].ToString(),
                            CourseName = ResultSet["coursename"].ToString(),
   
                        };

                        // Add the Course object to the list
                        CourseList.Add(course);
                    }
                }
            }

            // Return the final list of Course
            return CourseList;
        }


    }
}

