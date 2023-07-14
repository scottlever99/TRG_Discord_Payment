using EcomClubDiscordPayment.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcomClubDiscordPayment.Controllers
{
    public class StripeController : Controller
    {
        private readonly DbService _dbService;

        public StripeController(DbService dbService)
        {
            _dbService = dbService;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Monthly()
        {
            string token = _dbService.CreateToken();

            return Redirect("https://buy.stripe.com/28o03I7Jsgr8diw146?client_reference_id=" + token);
        }

        public IActionResult Six()
        {
            string token = _dbService.CreateToken();

            return Redirect("https://buy.stripe.com/28o03I7Jsgr8diw146?client_reference_id=" + token);
        }

        public IActionResult Yearly()
        {
            string token = _dbService.CreateToken();

            return Redirect("https://buy.stripe.com/28o03I7Jsgr8diw146?client_reference_id=" + token);
        }
    }
}
