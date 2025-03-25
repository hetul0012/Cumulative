
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
        public IActionResult List()
        {
            // get this information from the API
            List<Teacher> TeacherList = _api.ListTeacherID(); // Assuming ListTeacherNames method returns a list of Teacher objects

            // Return the list of teachers to the View
            return View(TeacherList);
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
    }
}
