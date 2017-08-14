using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BookStore.Models;

namespace BookStore.Controllers
{
    public class HomeController : Controller
    {
        readonly BookContext _db = new BookContext();

        public async Task<ActionResult> Index()
        {
            var responseCookie = HttpContext.Response.Cookies["id"];
            if (responseCookie != null) responseCookie.Value = "ca-4353w";

            var books =await  _db.Books.ToListAsync();
            ViewBag.Books = books;
            return View();
        }

        [HttpGet]
        public ActionResult Buy(int id)
        {
            ViewBag.BookId = id;
            return View();
        }

        [HttpPost]
        public string Buy(Purchase purchase)
        {
            var id = HttpContext.Request.Cookies["id"];

            purchase.Date = DateTime.Now;
            _db.Purchases.Add(purchase);
            _db.SaveChanges();
            return "Спасибо," + purchase.Person + ", за покупку!" + "id_cookies:=" + id?.Value ;
        }

        public string ContextData()
        {
            HttpContext.Response.Write("<h1>Hello World</h1>");

            string user_agent = HttpContext.Request.UserAgent;
            string url = HttpContext.Request.RawUrl;
            string ip = HttpContext.Request.UserHostAddress;
            string referrer = HttpContext.Request.UrlReferrer == null ? "" : HttpContext.Request.UrlReferrer.AbsoluteUri;
            return "<p>User-Agent: " + user_agent + "</p><p>Url запроса: " + url +
                   "</p><p>Реферер: " + referrer + "</p><p>IP-адрес: " + ip + "</p>";
        }

        public string Square()
        {
            int a = Int32.Parse(Request.Params["a"]);
            int b = Int32.Parse(Request.Params["b"]);
            var s = a * b / 2;
            return "<h2>Площадь треугольника равна " + s + "</h2>";
        }
    }
}