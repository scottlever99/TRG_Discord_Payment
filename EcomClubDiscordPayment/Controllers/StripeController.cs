using EcomClubDiscordPayment.Services;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace EcomClubDiscordPayment.Controllers
{
    public class StripeController : Controller
    {
        private readonly DbService _dbService;
        private readonly EmailService _emailService

        public StripeController(DbService dbService, EmailService emailService)
        {
            _dbService = dbService;
            _emailService = emailService;
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

            return Redirect("https://buy.stripe.com/28obMq9RAgr8fqE003?client_reference_id=" + token);
        }

        public IActionResult Yearly()
        {
            string token = _dbService.CreateToken();

            return Redirect("https://buy.stripe.com/9AQ9Ei1l4gr85Q47sw?client_reference_id=" + token);
        }

        [HttpPost]
        public async Task<IActionResult> Webhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            const string endpointSecret = "whsec_...";
            try
            {
                var stripeEvent = EventUtility.ParseEvent(json);
                var signatureHeader = Request.Headers["Stripe-Signature"];

                stripeEvent = EventUtility.ConstructEvent(json, signatureHeader, endpointSecret);

                if (stripeEvent.Type == Events.ChargeFailed)
                {
                    var charge = stripeEvent.Data.Object as Charge;
                    Console.WriteLine("A FAILED payment for user {0} was made.", charge.Amount);
                    _emailService.SendSubCancelledEmail(charge.Customer.Email);
                }
                else
                {
                    Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
                }
                return Ok();
            }
            catch (StripeException e)
            {
                Console.WriteLine("Error: {0}", e.Message);
                return BadRequest();
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
        }
    }
}
