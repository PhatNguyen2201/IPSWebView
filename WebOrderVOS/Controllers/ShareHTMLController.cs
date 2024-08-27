using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace WebOrderVOS.Controllers
{
    public class ShareHTMLController : Controller
    {
        public IConfiguration configuration { get; set; }
        public ShareHTMLController(IConfiguration _config)
        {
            configuration = _config;
        }
        public IActionResult Index(int ID)
        {
            DatabaseFunctions functions = new(configuration);
            string query = "select HTMLfilename from ExocadWebviewShare where ID  = " + ID;
            DataTable data = functions.ExQuery(query);
            string filename = "";
            if (data.Rows.Count == 1)
            {
                filename = data.Rows[0][0].ToString();
            }
            if (string.IsNullOrEmpty(filename))
            {
                return Content("Webview not found!");
            }

            string dir = "/SharedHTML/" + filename;
            return File(dir, "text/html");
        }
    }
}
