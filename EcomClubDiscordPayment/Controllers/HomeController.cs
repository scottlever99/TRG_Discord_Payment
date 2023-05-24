using EcomClubDiscordPayment.Models;
using EcomClubDiscordPayment.Services;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using System.Diagnostics;

namespace EcomClubDiscordPayment.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DbService _dbService;
        private readonly IConfiguration _configuration;
        private readonly DiscordService _discord;

        public HomeController(ILogger<HomeController> logger, DbService dbService, IConfiguration configuration, DiscordService discord)
        {
            _logger = logger;
            _dbService = dbService;
            _configuration = configuration;
            _discord = discord;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Stripe()
        {
            string token = _dbService.CreateToken();
            //TEST //return Redirect("https://buy.stripe.com/test_8wMdSD2P8dBYdMc4gi?client_reference_id=" + token);


            //TEST Live = https://buy.stripe.com/test_fZedQScitaTj1fa5kk
            //return Redirect("https://buy.stripe.com/test_fZedQScitaTj1fa5kk?client_reference_id=" + token);

            //PROD = https://buy.stripe.com/dR6dUy2p8deWbao7ss
            return Redirect("https://buy.stripe.com/dR6dUy2p8deWbao7ss?client_reference_id=" + token);

        }

        public async Task<IActionResult> StripeRedirect([FromQuery]string checkout_session_id)
        {
            try
            {
                if (string.IsNullOrEmpty(checkout_session_id)) return NotFound();
                
                StripeConfiguration.ApiKey = _configuration.GetValue<string>("Stripe");
                var service = new SessionService();
                var session = service.Get(checkout_session_id);
                var token = session.ClientReferenceId;

                var code = await _discord.GetInviteCode();

                if (!_dbService.Validate(token)) return BadRequest("Invalid Token");
                _dbService.RemoveToken(token, code, checkout_session_id);
                
                return RedirectToAction("Discord", new { code = code });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return BadRequest("Process Failed. Contact: theecomclubuk@gmail.com");
            }
        }

        public IActionResult Discord([FromQuery] string code)
        {
            return Redirect("https://discord.gg/" + code);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}