using Cumulative_1.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Cumulative_1.Controllers
{
    [Route("api/Teacher")]
    [ApiController]
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
        [Route(template: "ListTeachers")]
        public List<Teacher> ListTeachers(string SearchKey = null)
        {
            // Create an empty list of Teacher Names
            List<Teacher> Teachers = new List<Teacher>();


            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();

                MySqlCommand Command = Connection.CreateCommand();

                //SQL QUERY
                //Command.CommandText = "select * from ";

                string query = "select * from teachers";

                // search criteria, first, last or first + last
                if (SearchKey != null)
                {
                    query += " where lower(teacherfname) like lower(@key) or lower(teacherlname) like lower(@key) or lower(concat(teacherfname,' ',teacherlname)) like lower(@key)";
                    Command.Parameters.AddWithValue("@key", $"%{SearchKey}%");
                }
                //SQL QUERY
                Command.CommandText = query;
                Command.Prepare();


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

                        DateTime HireDate = Convert.ToDateTime(ResultSet["hiredate"]);
                        decimal Salary = Convert.ToDecimal(ResultSet["salary"]);

                        //short form for setting all properties while creating the object
                        Teacher CurrentTeacher = new Teacher()
                        {
                            TeacherId = Id,
                            TeacherFName = FirstName,
                            TeacherLName = LastName,
                            TeacherHireDate = HireDate,
                            TeacherSalary = Salary

                        };

                        Teachers.Add(CurrentTeacher);

                    }

                }
            }


            //Return the final list of Teacher names
            return Teachers;
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
                        string FirstName = ResultSet["teacherfname"]?.ToString() ?? "";
                        string LastName = ResultSet["teacherlname"]?.ToString() ?? "";

                        DateTime HireDate = Convert.ToDateTime(ResultSet["hiredate"]);
                        decimal Salary = Convert.ToDecimal(ResultSet["salary"]);


                        SelectedTeacher.TeacherId = Id;
                        SelectedTeacher.TeacherFName = FirstName;
                        SelectedTeacher.TeacherLName = LastName;
                        SelectedTeacher.TeacherHireDate = HireDate;
                        SelectedTeacher.TeacherSalary = Salary;

                    }
                }
            }


            //Return the final list of teacher names
            return SelectedTeacher;
        }


        // <summary>
        // Adds an Teacher to the database
        // </summary>
        // <param name="TeacherData">Teacher Object</param>
        // <example>
        // POST: api/TeacherData/AddTeacher
        // Headers: Content-Type: application/json
        // Request Body:
        // {
        //      "TeacherId":"11",
        //	    "TeacherFname":"Hetul",
        //	    "TeacherLname":"Suthar",
        //	    "EmployeeNumber":"T777",
        //	    "TeacherHireDate":"2024-10-12 00:00:00",
        // } -> 409
        // </example>
        // <returns>
        // The inserted Teacher Id from the database if successful. 0 if Unsuccessful
        // </returns>
        [HttpPost(template: "AddTeacher")]
        public int AddTeacher([FromBody] Teacher TeacherData)
        {
            // 'using' will close the connection after the code executes
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                //Establish a new command (query) for our database
                MySqlCommand Command = Connection.CreateCommand();

                
                Command.CommandText = "insert into teachers (teacherid, teacherfname, teacherlname, employeenumber, hiredate, salary) values (@teacherid, @teacherfname, @teacherlname, @employeenumber, CURRENT_DATE(), @salary)";
                Command.Parameters.AddWithValue("@teacherid", TeacherData.TeacherId);
                Command.Parameters.AddWithValue("@teacherfname", TeacherData.TeacherFName);
                Command.Parameters.AddWithValue("@teacherlname", TeacherData.TeacherLName);
                Command.Parameters.AddWithValue("@employeenumber", TeacherData.EmployeeNumber);
                Command.Parameters.AddWithValue("@hiredate", TeacherData.TeacherHireDate);
                Command.Parameters.AddWithValue("@salary", TeacherData.TeacherSalary);

                Command.ExecuteNonQuery();

                return Convert.ToInt32(Command.LastInsertedId);

            }
            // if failure
            return 0;
        }


        // <summary>
        // Deletes an Teacher from the database
        // </summary>
        // <param name="TeacherId">Primary key of the teacher to delete</param>
        // <example>
        // DELETE: api/TeacherData/DeleteTeacher -> 1
        // </example
        // <returns>
        // Number of rows affected by delete operation.
        // </returns>
        [HttpDelete(template: "DeleteTeacher/{TeacherId}")]
        public int DeleteTeacher(int TeacherId)
        {
            // 'using' will close the connection after the code executes
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                //Establish a new command (query) for our database
                MySqlCommand Command = Connection.CreateCommand();


                Command.CommandText = "delete from teachers where teacherid=@id";
                Command.Parameters.AddWithValue("@id", TeacherId);
                return Command.ExecuteNonQuery();

            }
            // if failure
            return 0;
        }

        // <summary>
        // Updates an Teacher in the database. Data is Teacher object, request query contains ID
        // </summary>
        // <param name="TeacherData">Teacher Object</param>
        // <param name="TeacherId">The Teacher ID primary key</param>
        // <example>
        // PUT: api/Teacher/UpdateTeacher/3
        // Headers: Content-Type: application/json
        // Request Body:
        // {
        //	    "TeacherFname":"Hetul",
        //	    "TeacherLname":"Suthar", 
        // } -> 
        // {
        //     "TeacherId":3,
        //	    "TeacherFname":"Hetul",
        //	    "TeacherLname":"Suthar",
        //	    "TeacherHireDate":"2025-06-06",
        //	    "TeacherSalary":"55"
        // }
        // </example>
        // <returns>
        // The updated Teacher object
        // </returns>
        [HttpPut(template: "UpdateTeacher/{TeacherId}")]
        public Teacher UpdateTeacher(int TeacherId, [FromBody] Teacher TeacherData)
        {
            // 'using' will close the connection after the code executes
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                //Establish a new command (query) for our database
                MySqlCommand Command = Connection.CreateCommand();

                // parameterize query
        
                Command.CommandText = "update teachers set teacherfname=@teacherfname, teacherlname=@teacherlname, employeenumber=@employeenumber, hiredate=@hiredate , salary=@salary where teacherid=@id";
                Command.Parameters.AddWithValue("@teacherfname", TeacherData.TeacherFName);
                Command.Parameters.AddWithValue("@teacherlname", TeacherData.TeacherLName);
                Command.Parameters.AddWithValue("@employeenumber", TeacherData.EmployeeNumber);
                Command.Parameters.AddWithValue("@hiredate", TeacherData.TeacherHireDate);
                Command.Parameters.AddWithValue("@salary", TeacherData.TeacherSalary);


                Command.Parameters.AddWithValue("@id", TeacherId);

                Command.ExecuteNonQuery();

            }

            return FindTeacher(TeacherId);
        }

    }
}

