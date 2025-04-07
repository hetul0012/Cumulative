
using Microsoft.AspNetCore.Mvc;
using Cumulative_1.Models;

namespace Cumulative_1.Controllers
{
    public class TeacherPageController : Controller
    {
        // get access to the component which retrieves data
        // the component is the TeacherAPIController
        private readonly TeacherAPIController _api;

        public TeacherPageController(TeacherAPIController api)
        {
            _api = api;
        }

        // GET: /TeacherPage/List -> A webpage which shows all teachers in the system
        [HttpGet]
        public IActionResult List(string SearchKey)
        {
            // get this information from the API
            List<Teacher> Teachers = _api.ListTeachers(SearchKey); // Assuming ListTeacherNames method returns a list of Teacher objects

            // Return the list of teachers to the View
            return View(Teachers);
        }


        // GET: /TeacherPage/Show/{id} -> A webpage which shows one specific teacher by its id
        [HttpGet]
        public IActionResult Show(int id)
        {
            // Get specific teacher details
            Teacher SelectedTeacher = _api.FindTeacher(id);

            // Return the teacher details to the View
            return View(SelectedTeacher);
        }


        // GET : TeacherPage/New
        [HttpGet]
        public IActionResult New(int id)
        {
            return View();
        }

        // POST: TeacherPage/Create
        [HttpPost]
        public IActionResult Create(Teacher NewTeacher)
        {
            int TeacherId = _api.AddTeacher(NewTeacher);

            // redirects to "Show" action on "Teacher" cotroller with id parameter supplied
            return RedirectToAction("Show", new { id = TeacherId });
        }

        // GET : TeacherPage/DeleteConfirm/{id}
        [HttpGet]
        public IActionResult DeleteConfirm(int id)
        {
            Teacher SelectedTeacher = _api.FindTeacher(id);
            return View(SelectedTeacher);
        }

        // POST: TeacherPage/Delete/{id}
        [HttpPost]
        public IActionResult Delete(int id)
        {
            int TeacherId = _api.DeleteTeacher(id);
            // redirects to list action
            return RedirectToAction("List");
        }


    }
}
