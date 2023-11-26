using System.Diagnostics;
using System.Threading.Tasks;
using GetWordBeforeWatchingMovie.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using GetWordBeforeWatchingMovie.Models;
using GetWordBeforeWatchingMovie.Services.Interfaces;

namespace GetWordBeforeWatchingMovie.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWordListServices _wordListServices;

        public HomeController(ILogger<HomeController> logger, IWordListServices wordListServices)
        {
            _logger = logger;
            _wordListServices = wordListServices;
        }

        [HttpPost]
        public async Task<IActionResult> ReceiveRequest(string englishLevel, string imdbIdTextbox)
        {
            _logger.LogDebug(englishLevel + imdbIdTextbox);
            if (string.IsNullOrWhiteSpace(englishLevel))
            {
                englishLevel = "A1";
            }
            
            // Get the words
            Level userLevel = englishLevel switch
            {
                "A1" => Level.A1,
                "A2" => Level.A2,
                "B1" => Level.B1,
                "B2" => Level.B2,
                "C1" => Level.C1
            };
            var wordList = await _wordListServices.GetWordLists(imdbIdTextbox, userLevel); 
            var model = new WordViewModel (wordList, englishLevel);
            return View("Index", model);
        }

        public IActionResult Index()
        {
            return View();
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
