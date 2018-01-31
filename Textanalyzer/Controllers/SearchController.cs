using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Textanalyzer.Web.Controllers
{
    public class SearchController : Controller
    {
        public IActionResult Index()
        {
            //var value = this.Context.Connection.GetHttpContext().Request.Query["key"].SingleOrDefault();
            return View();
        }
    }
}