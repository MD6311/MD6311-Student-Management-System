using Microsoft.AspNetCore.Mvc;

namespace StudentManagement.Controllers
{
    public class AccountController : Controller
    {
        private const string AdminUser = "admin@marwadiuniversity.ac.in";
        private const string AdminPass = "Admin@123";

        public IActionResult Login()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("LoggedUser")))
                return RedirectToAction("Index", "Students");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string username, string password)
        {
            if (string.Equals(username, AdminUser, System.StringComparison.OrdinalIgnoreCase) && password == AdminPass)
            {
                HttpContext.Session.SetString("LoggedUser", username);
                return RedirectToAction("Index", "Students");
            }
            ViewBag.Error = "Invalid credentials";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("LoggedUser");
            return RedirectToAction("Login");
        }
    }
}