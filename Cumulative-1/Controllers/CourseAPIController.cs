using Cumulative_1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Mysqlx.Resultset;

namespace Cumulative_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseAPIController : ControllerBase
    {
        private readonly SchoolDbContext _context;
        public CourseAPIController(SchoolDbContext context)
        {
            _context = context;
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
            // Create an empty list of Courses
            List<Course> Courses = new List<Course>();

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

                        //Access Column information by the DB column name as an index
                        int Id = Convert.ToInt32(ResultSet["courseid"]);
                        string Code = ResultSet["coursecode"].ToString();


                        DateTime StartDate = Convert.ToDateTime(ResultSet["startdate"]);
                        DateTime FinishDate = Convert.ToDateTime(ResultSet["finishdate"]);
                        string Name = ResultSet["coursename"].ToString();

                        //short form for setting all properties while creating the object
                        Course CurrentCourse = new Course()
                        {
                            CourseId = Id,
                            CourseCode = Code,
                            CourseStartDate = StartDate,
                            CourseFinishDate = FinishDate,
                            CourseName = Name
                        };

                        Courses.Add(CurrentCourse);
                    }
                }
            }

            // Return the final list of Course
            return Courses;
        }


        // <summary>
        // Output a course associated with the input course id.
        // </summary>
        // <param name="CourseId">The primary key of the course</param>
        // <returns>An object associated with the course</returns>
        // <example>
        // GET: api/course/FindCourse/4 -> {"CourseId":"4","CourseCode":"http5104","TeacherId":"7","StartDate":"2018-09-04","FinishDate":"2018-12-14","CourseName":"Digital Design"}
        // </example>
        [HttpGet]
        [Route(template: "FindCourse/{CourseId}")]
        public Course FindCourse(int CourseId)
        {
            Course SelectedCourse = new Course();

            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                // opening the connection
                Connection.Open();

                // setting up an sql query
                string query = "select * from courses where courseid=" + CourseId;

                // setting the command text to the query
                MySqlCommand Command = Connection.CreateCommand();
                Command.CommandText = query;

                // use the result set to get information about the Course
                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {

                    while (ResultSet.Read())
                    {


                        //get information about the Course
                        SelectedCourse.CourseId = Convert.ToInt32(ResultSet["courseid"]);
                        SelectedCourse.CourseCode = ResultSet["coursecode"].ToString();
                        SelectedCourse.CourseStartDate = Convert.ToDateTime(ResultSet["startdate"]);
                        SelectedCourse.CourseFinishDate = Convert.ToDateTime(ResultSet["finishdate"]);
                        SelectedCourse.CourseName = ResultSet["coursename"].ToString();


                    }

                }
            }

            return SelectedCourse;
        }

        // <summary>
        // Adds an Course to the database
        // </summary>
        // <param name="CourseData">Course Object</param>
        // <example>
        // POST: api/CourseData/AddCourse
        // Headers: Content-Type: application/json
        // Request Body:
        // {
        //      "CourseId":"101",
        //	    "CourseCode":"htpp5200",
        //	    "TeacherId":"11",
        //	    "StartDate":"2024-06-04",
        //	    "FinishDate":"2024-07-12",
        // }
        // </example>
        // <returns>
        // The inserted Course Id from the database if successful. 0 if Unsuccessful
        // </returns>
        [HttpPost(template: "AddCourse")]
        public int AddCourse([FromBody] Course CourseData)
        {
            // 'using' will close the connection after the code executes
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                //Establish a new command (query) for our database
                MySqlCommand Command = Connection.CreateCommand();


                Command.CommandText = "insert into courses (courseid, coursecode, teacherid, startdate, finishdate, coursename) values (@courseid, @coursecode, @teacherid, @startdate, @finishdate, @coursename)";
                Command.Parameters.AddWithValue("@courseid", CourseData.CourseId);
                Command.Parameters.AddWithValue("@coursecode", CourseData.CourseCode);
                Command.Parameters.AddWithValue("@teacherid", CourseData.TeacherId);
                Command.Parameters.AddWithValue("@startdate", CourseData.CourseStartDate);
                Command.Parameters.AddWithValue("@finishdate", CourseData.CourseFinishDate);
                Command.Parameters.AddWithValue("@coursename", CourseData.CourseName);

                Command.ExecuteNonQuery();

                return Convert.ToInt32(Command.LastInsertedId);

            }
            // if failure
            return 0;
        }


        // <summary>
        // Deletes an Course from the database
        // </summary>
        // <param name="CourseId">Primary key of the Course to delete</param>
        // <example>
        // DELETE: api/CourseData/DeleteCourse -> 1
        // </example
        // <returns>
        // Number of rows affected by delete operation.
        // </returns>
        [HttpDelete(template: "DeleteCourse/{CourseId}")]
        public int DeleteCourse(int CourseId)
        {
            // 'using' will close the connection after the code executes
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                //Establish a new command (query) for our database
                MySqlCommand Command = Connection.CreateCommand();


                Command.CommandText = "delete from courses where courseid=@id";
                Command.Parameters.AddWithValue("@id", CourseId);
                return Command.ExecuteNonQuery();

            }
            // if failure
            return 0;
        }


        // <summary>
        // Updates an Course in the database. Data is Course object, request query contains ID
        // </summary>
        // <param name="CourseData">Course Object</param>
        // <param name="CourseId">The Course ID primary key</param>
        // <example>
        // PUT: api/Course/UpdateCourse/4
        // Headers: Content-Type: application/json
        // Request Body:
        // {
        //      "CourseId":"112",
        //	    "CourseCode":"htpp5400",
        //	    "TeacherId":"15",
        //	    "StartDate":"2023-06-03",
        //	    "FinishDate":"2024-12-12",
        // } -> 

        // </example>
        // <returns>
        // The updated Course object
        // </returns>
        [HttpPut(template: "UpdateCourse/{CourseId}")]
        public Course UpdateCourse(int CourseId, [FromBody] Course CourseData)
        {
            // 'using' will close the connection after the code executes
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                //Establish a new command (query) for our database
                MySqlCommand Command = Connection.CreateCommand();

                // parameterize query

                Command.CommandText = "update courses set coursecode=@coursecode, teacherid=@teacherid, startdate=@startdate, finishdate=@finishdate , coursename=@coursename where courseid=@id";
            
                Command.Parameters.AddWithValue("@courseid", CourseData.CourseId);
                Command.Parameters.AddWithValue("@coursecode", CourseData.CourseCode);
                Command.Parameters.AddWithValue("@teacherid", CourseData.TeacherId);
                Command.Parameters.AddWithValue("@startdate", CourseData.CourseStartDate);
                Command.Parameters.AddWithValue("@finishdate", CourseData.CourseFinishDate);
                Command.Parameters.AddWithValue("@coursename", CourseData.CourseName);

                Command.Parameters.AddWithValue("@id", CourseId);

                Command.ExecuteNonQuery();

            }

            return FindCourse(CourseId);
        }


    }
}
