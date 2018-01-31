using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Textanalyzer.Web.Controllers
{
    public class SearchController : Controller
    {
        private static readonly HttpClient client = new HttpClient();

        [HttpGet]
        public IActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Search(string searchString)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

            if (string.IsNullOrWhiteSpace(searchString))
            {
                return View();
            }

            string[] split = searchString.Split(' ');
            List<string> temp = new List<string>();

            if (split.Length > 3)
            {
                temp.Add(split[0]);
                temp.Add(split[1]);
                temp.Add(split[2]);
            }
            else
            {
                foreach (string s in split)
                {
                    temp.Add(s);
                }
            }


            string urlHelp = "";

            foreach (string s in temp)
            {
                urlHelp += s + " ";
            }

            urlHelp = urlHelp.Trim();

            string url = HttpContext.Request.Host + "/api/searchapi/" + urlHelp;

            var stringTask = client.GetStringAsync("https://api.github.com/orgs/dotnet/repos");

            string msg = await stringTask;

            return Json(msg);
        }
    }
}