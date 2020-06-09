using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Assessment7a.Models;
using System.Net.Http;

namespace Assessment7a.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Species(string SpeciesName)
        {
            Species animal = await AniDal.GetApiResponse<Species>(
               "api", "species", "https://gc-zoo.surge.sh/", SpeciesName);
        
            return View(animal);
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] string SpeciesName)
        {

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://gc-zoo.surge.sh/api/");

            var response = await client.GetAsync("animals.json");

            var results = await response.Content.ReadAsAsync<Animals>();

            return View(results);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
