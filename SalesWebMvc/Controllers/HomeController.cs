using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Models.ViewModels;

namespace SalesWebMvc.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            Dictionary<string, object> viewData = new Dictionary<string, object>();
            viewData["name"] = "Bem-vindo ao Teste API REST JSON";
            viewData["author"] = "jean.barcellos@hevo.com.br";
            viewData["inteiros"] = 1;
            viewData["double"] = 2.98;

            return Json(viewData);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Salles Web MVC App from C# Course.";
            ViewData["Professor"] = "Nélio Alves";

            return Json(ViewData);
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return Json(ViewData);
        }

        public IActionResult Privacy()
        {
            ViewData["Message"] = "Use this page to detail your site's privacy policy.";

            return Json(ViewData);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return Json(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
