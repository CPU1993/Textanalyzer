using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Textanalyzer.Data.Util;

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
        public async Task<IActionResult> Search(Search search)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

            if (string.IsNullOrWhiteSpace(search.SearchString))
            {
                return View();
            }

            string[] split = search.SearchString.Split(' ');
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

            string url = "https://" + HttpContext.Request.Host + "/api/searchapi/" + urlHelp;

            var stringTask = client.GetStringAsync(url);

            var msg = await stringTask;
            
            return Json(msg);
        }
    }
}