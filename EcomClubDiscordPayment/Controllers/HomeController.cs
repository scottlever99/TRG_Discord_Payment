using EcomClubDiscordPayment.Models;
using EcomClubDiscordPayment.Services;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using System.Diagnostics;

namespace EcomClubDiscordPayment.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DbService _dbService;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, DbService dbService, IConfiguration configuration)
        {
            _logger = logger;
            _dbService = dbService;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult StripeCreateLink()
        {
            string token = _dbService.CreateToken();
            return Redirect("https://buy.stripe.com/test_8wMdSD2P8dBYdMc4gi?client_reference_id=" + token);
        }

        public IActionResult StripeRedirect([FromQuery]string checkout_session_id)
        {
            try
            {
                if (string.IsNullOrEmpty(checkout_session_id)) return NotFound();
                string key = _configuration.GetValue<string>("Stripe");
                var service = new SessionService();
                var session = service.Get(checkout_session_id);
                var token = session.ClientReferenceId;
                if (!_dbService.Validate(token)) return BadRequest("Invalid Token");
                _dbService.RemoveToken(token);
                //generate discord link
                //redirect to page with link
                return Ok(token);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return BadRequest("Process Failed. Contact: ecomclub@gmail.com");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}