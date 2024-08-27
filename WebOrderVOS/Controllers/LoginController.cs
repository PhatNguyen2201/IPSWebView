using Microsoft.AspNetCore.Mvc;

namespace WebOrderVOS.Controllers
{
    public class LoginController : Controller
    {
        public IConfiguration configuration { get; set; }
        public LoginController(IConfiguration _config)
        {
            configuration = _config;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(IFormCollection ts)
        {
            if (ts.ToList()[0].Value == configuration["Account:Username"] && ts.ToList()[1].Value == configuration["Account:password"])
            {
                return RedirectToAction("Index", "Home");
            }
            return Content("Sai tên đăng nhập hoặc mật khẩu");
        }
    }
}
