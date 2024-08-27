using System.Data;
using Microsoft.AspNetCore.Mvc;

namespace WebOrderVOS.Controllers
{
    public class ExocadHomeController : Controller
    {
        public IConfiguration Config { get; set; }
        public ExocadHomeController(IConfiguration _config)
        {
            Config = _config;
        }

        public IActionResult Index()
        {
            List<Models.HomeModels> list = new();
            DatabaseFunctions functions = new DatabaseFunctions(Config);
            try
            {
                DataTable tableOrder = functions.ExQuery(functions.querySQL);
                foreach (DataRow row in tableOrder.Rows)
                {
                    list.Add(new Models.HomeModels()
                    {
                        Status = row["ds_status"].ToString(),
                        Paients = row["Patient First Name"].ToString() + " " + row["Patient last Name"].ToString(),
                        DateIn = row["Date"].ToString(),
                        DateDue = row["t_duedate"].ToString(),
                        Technician = row["fname"].ToString() + " " + row["lname"].ToString(),
                        Treatment = row["Type of Treatment"].ToString(),
                    });
                }
                return View(list);
            }
            catch
            {
                list.Add(new Models.HomeModels()
                {
                    Status = "SQLError",
                    Paients = "SQLError",
                    DateIn = "SQLError",
                    DateDue = "SQLError",
                    Technician = "SQLError",
                    Treatment = "SQLError",
                });
            return View(list);
            }
        }

        [HttpPost]
        public IActionResult Index(IFormCollection filter)
        {
            DatabaseFunctions functions = new DatabaseFunctions(Config);
            string getvalue = filter.ToList()[0].Value;
            string queryDate;
            string datefrom;
            string dateto;
            switch (getvalue)
            {
                case "0":
                    datefrom = DateTime.Today.ToString("yyyy-MM-dd");
                    dateto = DateTime.Today.AddDays(1).ToString("yyyy-MM-dd");
                    break;
                case "1":
                    dateto = DateTime.Today.AddDays(1).ToString("yyyy-MM-dd");
                    datefrom = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");
                    break;
                case "2":
                    dateto = DateTime.Today.AddDays(1).ToString("yyyy-MM-dd");
                    datefrom = DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd");
                    break;
                case "3":
                    dateto = DateTime.Today.AddDays(1).ToString("yyyy-MM-dd");
                    datefrom = DateTime.Today.ToString("yyyy-MM-01");
                    break;
                default:
                    datefrom = dateto = "";
                    break;
            }
            if (getvalue != "3")
            {
                queryDate = " AND trm.t_date BETWEEN '" + datefrom + "' AND '" + dateto + "'";
            }
            else
            {
                queryDate = "";
            }
            DataTable tableOrder = functions.ExQuery(functions.querySQL + queryDate);
            List<Models.HomeModels> list = new();
            foreach (DataRow row in tableOrder.Rows)
            {
                list.Add(new Models.HomeModels()
                {
                    Status = row["ds_status"].ToString(),
                    Paients = row["Patient First Name"].ToString() + " " + row["Patient last Name"].ToString(),
                    DateIn = row["Date"].ToString(),
                    DateDue = row["t_duedate"].ToString(),
                    Technician = row["fname"].ToString() + " " + row["lname"].ToString(),
                    Treatment = row["Type of Treatment"].ToString(),
                });
            }
            return View(list);
        }
    }
}