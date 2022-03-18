using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TestPaymob.Models;
using TestPaymob.Repos.Interface;
using Microsoft.Extensions.Configuration;

namespace TestPaymob.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPAyMobRepo _payMobRepo;

        public HomeController(ILogger<HomeController> logger, IPAyMobRepo PayMobRepo)
        {
            _logger = logger;
            _payMobRepo = PayMobRepo;
        }
        public IActionResult Index()
        {

           return View();
        }
        public async Task < IActionResult> Checkout()
        {

            Product product = new Product
            {

                CreatedAt = DateTime.UtcNow,
                Name = "Consultation",
                Description = "Desc",
                Price = 350
            };
            string paymentKey = await _payMobRepo.Purchase(product);
            string iframeUrl = "https://accept.paymob.com/api/acceptance/iframes/359176?payment_token=" + paymentKey;
            return Redirect(iframeUrl);
        }

        
        [HttpGet]
        public IActionResult Callback([FromQuery]bool Success)

        {
            if(Success)
            return View("Success");
            return View("Failed");

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