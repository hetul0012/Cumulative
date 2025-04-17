
using Microsoft.AspNetCore.Mvc;
using Cumulative_1.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Cumulative_1.Controllers
{
    public class CoursePageController : Controller
    {
        // get access to the component which retrieves data
        // the component is the CourseAPIController
        private readonly CourseAPIController _api;

        public CoursePageController(CourseAPIController api)
        {
            _api = api;
        }

        // GET: /CoursePage/List -> A webpage which shows all Course in the system
        [HttpGet]
        public IActionResult List()
        {
            // get this information from the API
            List<Course> Courses = _api.ListCourse(); // Assuming ListCourse method returns a list of Course objects

            // Return the list of Course to the View
            return View(Courses);
        }

        // GET: /CoursePage/Show/{id} -> A webpage which shows one specific Course by its id
        [HttpGet]
        public IActionResult Show(int CourseId)
        {
            // Get specific Course details
            Course SelectedCourse = _api.FindCourse(CourseId);

            // Return the Course details to the View
            return View(SelectedCourse);
        }

        // GET : CoursePage/New
        [HttpGet]
        public IActionResult New(int id)
        {
            return View();
        }

        // POST: CoursePage/Create
        [HttpPost]
        public IActionResult Create(Course NewCourse)
        {
            int CourseId = _api.AddCourse(NewCourse);

            // redirects to "Show" action on "Course" cotroller with id parameter supplied
            return RedirectToAction("Show", new { id = CourseId });
        }

        // GET : CoursePage/DeleteConfirm/{id}
        [HttpGet]
        public IActionResult DeleteConfirm(int id)
        {
            Course SelectedCourse = _api.FindCourse(id);
            return View(SelectedCourse);
        }

        // POST: CoursePage/Delete/{id}
        [HttpPost]
        public IActionResult Delete(int id)
        {
            int CourseId = _api.DeleteCourse(id);
            // redirects to list action
            return RedirectToAction("List");
        }


        // GET : CoursePage/Edit/{id}
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Course SelectedCourse = _api.FindCourse(id);
            return View(SelectedCourse);
        }


        // POST: CoursePage/Update/{id}
        [HttpPost]
        public IActionResult Update(int id, string CourseCode, int TeacherId, DateTime CourseStartDate, DateTime CourseFinishDate, string CourseName)
        {
            Course UpdatedCourse = new Course();
            UpdatedCourse.CourseCode = CourseCode;
            UpdatedCourse.TeacherId = TeacherId;
            UpdatedCourse.CourseStartDate = CourseStartDate;
            UpdatedCourse.CourseFinishDate = CourseFinishDate;
            UpdatedCourse.CourseName = CourseName;


       


            // not doing anything with the response
            _api.UpdateCourse(id, UpdatedCourse);
            // redirects to show Course
            return RedirectToAction("Show", new { id = id });
        }

    }
}
