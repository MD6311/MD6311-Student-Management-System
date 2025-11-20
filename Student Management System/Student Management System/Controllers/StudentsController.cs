using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using StudentManagement.Models;
using System.Text;

namespace StudentManagement.Controllers
{
    [AuthFilter]
    public class StudentsController : Controller
    {
        private readonly MongoContext _context;

        public StudentsController(MongoContext context)
        {
            _context = context;
        }

        public IActionResult Index(string q)
        {
            var filter = Builders<Student>.Filter.Empty;
            if (!string.IsNullOrWhiteSpace(q))
            {
                var lower = q.ToLower();
                filter = Builders<Student>.Filter.Where(s =>
                    s.Name.ToLower().Contains(lower) ||
                    s.EnrollmentNo.ToLower().Contains(lower) ||
                    s.InstituteEmail.ToLower().Contains(lower) ||
                    s.PersonalEmail.ToLower().Contains(lower));
            }
            var students = _context.Students.Find(filter).SortByDescending(s => s.DOB).ToList();
            ViewBag.Query = q ?? string.Empty;
            return View(students);
        }

        public IActionResult Details(string id)
        {
            var student = _context.Students.Find(s => s.Id == id).FirstOrDefault();
            if (student == null) return NotFound();
            return View(student);
        }

        public IActionResult Create()
        {
            return View(new Student { DOB = DateTime.Now.AddYears(-18) });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Student student)
        {
            if (!ModelState.IsValid)
            {
                return View(student);
            }
            _context.Students.InsertOne(student);
            TempData["Success"] = "Student added successfully.";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(string id)
        {
            var student = _context.Students.Find(s => s.Id == id).FirstOrDefault();
            if (student == null) return NotFound();
            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, Student updated)
        {
            if (!ModelState.IsValid)
            {
                return View(updated);
            }
            updated.Id = id;
            _context.Students.ReplaceOne(s => s.Id == id, updated);
            TempData["Success"] = "Student updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(string id)
        {
            var student = _context.Students.Find(s => s.Id == id).FirstOrDefault();
            if (student == null) return NotFound();
            return View(student);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string id)
        {
            _context.Students.DeleteOne(s => s.Id == id);
            TempData["Success"] = "Student deleted.";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult ExportCsv()
        {
            var students = _context.Students.Find(_ => true).ToList();
            var sb = new StringBuilder();
            sb.AppendLine("Id,Name,EnrollmentNo,Semester,Field,GRNumber,Mobile,InstituteEmail,PersonalEmail,Address,DOB,Notes");
            foreach (var s in students)
            {
                sb.AppendLine($"\"{s.Id}\",\"{s.Name}\",\"{s.EnrollmentNo}\",\"{s.Semester}\",\"{s.Field}\",\"{s.GRNumber}\",\"{s.Mobile}\",\"{s.InstituteEmail}\",\"{s.PersonalEmail}\",\"{s.Address}\",\"{s.DOB:yyyy-MM-dd}\",\"{s.Notes}\"");
            }
            var bytes = Encoding.UTF8.GetBytes(sb.ToString());
            return File(bytes, "text/csv", "students_export.csv");
        }
    }
}