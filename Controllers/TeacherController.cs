using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

namespace CollegeWebApplication.Controllers
{
    public class TeacherController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Welcome(string name, int numtimes = 1)
        {
            ViewData["Message"] = "Hello " + name;
            ViewData["NumTimes"] = numtimes;
            return View();
        }
    }
}
