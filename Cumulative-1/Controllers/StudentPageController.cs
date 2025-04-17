using Cumulative_1.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cumulative_1.Controllers
{
    public class StudentPageController : Controller
    {
        // get access to the component which retrieves data
        // the component is the StudentAPIController
        private readonly StudentAPIController _api;

        public StudentPageController(StudentAPIController api)
        {
            _api = api;
        }

        // GET: /StudentPage/List -> A webpage which shows all Student in the system
        [HttpGet]
        public IActionResult List()
        {
            // get this information from the API
            List<Student> Students = _api.ListStudent(); // Assuming List Student method returns a list of Student objects

            // Return the list of Student to the View
            return View(Students);
        }

        // GET: /StudentPage/Show/{id} -> A webpage which shows one specific Student by its id
        [HttpGet]
        public IActionResult Show(int StudentId)
        {
            // Get specific Student details
            Student SelectedStudent = _api.FindStudent(StudentId);

            // Return the Course details to the View
            return View(SelectedStudent);
        }

        // GET : StudentPage/New
        [HttpGet]
        public IActionResult New(int id)
        {
            return View();
        }

        // POST: StudentPage/Create
        [HttpPost]
        public IActionResult Create(Student NewStudent)
        {
            int StudentId = _api.AddStudent(NewStudent);

            // redirects to "Show" action on "Student" cotroller with id parameter supplied
            return RedirectToAction("Show", new { id = StudentId });
        }

        // GET : StudentPage/DeleteConfirm/{id}
        [HttpGet]
        public IActionResult DeleteConfirm(int id)
        {
            Student SelectedStudent = _api.FindStudent(id);
            return View(SelectedStudent);
        }

        // POST: StudentPage/Delete/{id}
        [HttpPost]
        public IActionResult Delete(int id)
        {
            int StudentId = _api.DeleteStudent(id);
            // redirects to list action
            return RedirectToAction("List");
        }

        // GET : StudentPage/Edit/{id}
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Student SelectedStudent = _api.FindStudent(id);
            return View(SelectedStudent);
        }


        // POST: StudentPage/Update/{id}
        [HttpPost]
        public IActionResult Update(int id, string StudentFName, string StudentLName, string StudentNumber, DateTime EnrolDate)
        {
            Student UpdatedStudent = new Student();
            UpdatedStudent.StudentFName = StudentFName;
            UpdatedStudent.StudentLName = StudentLName;
            UpdatedStudent.StudentNumber = StudentNumber;
            UpdatedStudent.EnrolDate = EnrolDate;
        

            // not doing anything with the response
            _api.UpdateStudent(id, UpdatedStudent);
            // redirects to show Student
            return RedirectToAction("Show", new { id = id });
        }

    }
}
