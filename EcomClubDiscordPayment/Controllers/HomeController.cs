using EcomClubDiscordPayment.Models;
using EcomClubDiscordPayment.Services;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using System.Diagnostics;
using System.Net.Mail;
using System.Net;

namespace EcomClubDiscordPayment.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DbService _dbService;
        private readonly IConfiguration _configuration;
        private readonly DiscordService _discord;
        private readonly EmailService _emailService;

        public HomeController(ILogger<HomeController> logger, DbService dbService, IConfiguration configuration, DiscordService discord, EmailService emailService)
        {
            _logger = logger;
            _dbService = dbService;
            _configuration = configuration;
            _discord = discord;
            _emailService = emailService;
        }

        public IActionResult Index()
        {
            //_emailService.SendInviteEmail("leversl21@gmail.com", "https://discord.gg/aysHsx");

            return View();
        }

        public IActionResult Stripe()
        {
            string token = _dbService.CreateToken();
            //TEST //return Redirect("https://buy.stripe.com/test_8wMdSD2P8dBYdMc4gi?client_reference_id=" + token);


            //TEST Live = https://buy.stripe.com/test_fZedQScitaTj1fa5kk
            //return Redirect("https://buy.stripe.com/test_fZedQScitaTj1fa5kk?client_reference_id=" + token);

            //PROD = https://buy.stripe.com/dR6dUy2p8deWbao7ss

            //NEW PROD https://buy.stripe.com/28o03I7Jsgr8diw146
            return Redirect("https://buy.stripe.com/28o03I7Jsgr8diw146?client_reference_id=" + token);

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
                var subId = session.SubscriptionId;
                var email = session.CustomerDetails.Email;

                var code = await _discord.GetInviteCode();

                if (!_dbService.Validate(token)) return BadRequest("Invalid Token");
                _dbService.RemoveToken(token, code, checkout_session_id, subId);

                return RedirectToAction("Discord", new { code = code, email = email });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return BadRequest("Process Failed. Contact: support@resellergroup.co.uk");
            }
        }

        public async Task<IActionResult> StripeRedirectPDF([FromQuery] string checkout_session_id)
        {
            try
            {
                if (string.IsNullOrEmpty(checkout_session_id)) return NotFound();

                StripeConfiguration.ApiKey = _configuration.GetValue<string>("Stripe");
                var service = new SessionService();
                var session = service.Get(checkout_session_id);
                var email = session.CustomerDetails.Email;

                _dbService.InsertBookletEmail(email);
                var smtpClient = new SmtpClient("smtp.hostinger.com")
                {
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential("booklet@resellergroup.co.uk", "CupCatDogPen1$"),
                    Port = 587
                };

                var message = new MailMessage(new MailAddress("booklet@resellergroup.co.uk", "Reseller Group"), new MailAddress(email))
                {
                    Subject = "Booklet",
                    IsBodyHtml = true,
                    //You have the template saved on your laptop for this email
                    Body = "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"background-color: black; color: white;\">\r\n    <tbody>\r\n        <tr>\r\n            <td width=\"100%\" align=\"center\" valign=\"top\" bgcolor=\"#eeeeee\" height=\"20\">\r\n            </td>\r\n        </tr>\r\n        <tr>\r\n            <td bgcolor=\"#eeeeee\" align=\"center\" style=\"padding:0px 15px 0px 15px\" class=\"m_5201679202317375323section-padding\">\r\n                <table bgcolor=\"#282828\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"max-width:600px\">\r\n                    <tbody>\r\n                        <tr>\r\n                            <td>\r\n                                <table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">\r\n                                    <tbody>\r\n                                        <tr>\r\n                                            <td align=\"center\" style=\"padding:30px 40px 0px 40px\">\r\n                                                <a href=\"https://resellergroup.co.uk\">\r\n                                                    <img src=\"https://i.ibb.co/3NnCkfG/Reseller-Logo-crop.png\" width=\"100\" border=\"0\" style=\"vertical-align:middle\" class=\"CToWUd\" data-bit=\"iit\">\r\n                                                </a>\r\n                                            </td>\r\n                                        </tr>\r\n                                        <tr>\r\n                                            <td style=\"font:16px/22px 'Helvetica Neue',Arial,'sans-serif';text-align:left;color:#ffffff;padding:20px 40px 0 40px\">\r\n                                                <p>Dear Customer,</p>\r\n                                                <p>Thank you for choosing our booklet!<br>Please download it below!</p>\r\n                                                <p>Kind Regards,<br>The Reseller Group</p>\r\n                                            </td>\r\n                                        </tr>\r\n                                        <tr>\r\n                                            <td>\r\n                                                <table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" style=\"margin: 30px 0px 0px 0px\">\r\n                                                    <tbody>\r\n                                                        <tr>\r\n                                                            <td align=\"center\" style=\"text-align:center\">\r\n                                                                <a name=\"m_5201679202317375323_CTA\" bgcolor=\"#000\" style=\"color:#282828;background-color:#ffffff;display:inline-block;font-family:'Helvetica Neue',Arial,'sans-serif';font-size:16px;line-height:30px;text-align:center;font-weight:bold;text-decoration:none;padding:5px 20px;border-radius:1px;text-transform:none\" href=\"https://resellergroup.azurewebsites.net/Booklet/Download\" target=\"_blank\">DOWNLOAD</a>\r\n                                                            </td>\r\n                                                        </tr>\r\n                                                    </tbody>\r\n                                                </table>\r\n                                            </td>\r\n                                        </tr>\r\n                                    </tbody>\r\n                                </table>\r\n                            </td>\r\n                        </tr>\r\n                        <tr>\r\n                            <td width=\"100%\" align=\"center\" valign=\"top\" bgcolor=\"#282828\" height=\"45\">\r\n                            </td>\r\n                        </tr>\r\n                    </tbody>\r\n                </table>\r\n            </td>\r\n        </tr>\r\n        <tr>\r\n            <td bgcolor=\"#eeeeee\" align=\"center\" style=\"padding:20px 0px\">\r\n                <table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" align=\"center\" style=\"max-width:600px\" class=\"m_5201679202317375323responsive-table\">\r\n                    <tbody>\r\n                        <tr></tr>\r\n                        <tr> \r\n                            <td bgcolor=\"#eeeeee\" align=\"center\">\r\n                                <table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" align=\"center\" style=\"max-width:600px\" class=\"m_5201679202317375323responsive-table\">\r\n                                    <tbody>\r\n                                        <tr>\r\n                                            <td align=\"center\" style=\"text-align:center;padding:10px 10px 10px 10px\">\r\n                                            </td>\r\n                                        </tr>\r\n                                    </tbody>\r\n                                </table>\r\n                            </td>\r\n                        </tr>\r\n                    </tbody>\r\n                </table>\r\n            </td>\r\n        </tr>\r\n    </tbody>\r\n</table>",
                };

                smtpClient.Send(message);
                return Ok("YOU HAVE BEEN SENT AN EMAIL WITH MORE DETAILS. THANKS, TRG");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return BadRequest("Process Failed. Contact: support@resellergroup.co.uk");
            }
        }

        public async Task<IActionResult> StripeRedirectHardCopy([FromQuery] string checkout_session_id)
        {
            try
            {
                if (string.IsNullOrEmpty(checkout_session_id)) return NotFound();

                StripeConfiguration.ApiKey = _configuration.GetValue<string>("Stripe");
                var service = new SessionService();
                var session = service.Get(checkout_session_id);
                var email = session.CustomerDetails.Email;

                _dbService.InsertBookletEmail(email);
                var smtpClient = new SmtpClient("smtp.hostinger.com")
                {
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential("booklet@resellergroup.co.uk", "CupCatDogPen1$"),
                    Port = 587
                };

                var message = new MailMessage(new MailAddress("booklet@resellergroup.co.uk", "Reseller Group"), new MailAddress(email))
                {
                    Subject = "Booklet",
                    IsBodyHtml = true,
                    //You have the template saved on your laptop for this email
                    Body = "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"background-color: black; color: white;\">\r\n    <tbody>\r\n        <tr>\r\n            <td width=\"100%\" align=\"center\" valign=\"top\" bgcolor=\"#eeeeee\" height=\"20\">\r\n            </td>\r\n        </tr>\r\n        <tr>\r\n            <td bgcolor=\"#eeeeee\" align=\"center\" style=\"padding:0px 15px 0px 15px\" class=\"m_5201679202317375323section-padding\">\r\n                <table bgcolor=\"#282828\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"max-width:600px\">\r\n                    <tbody>\r\n                        <tr>\r\n                            <td>\r\n                                <table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">\r\n                                    <tbody>\r\n                                        <tr>\r\n                                            <td align=\"center\" style=\"padding:30px 40px 0px 40px\">\r\n                                                <a href=\"https://resellergroup.co.uk\">\r\n                                                    <img src=\"https://i.ibb.co/3NnCkfG/Reseller-Logo-crop.png\" width=\"100\" border=\"0\" style=\"vertical-align:middle\" class=\"CToWUd\" data-bit=\"iit\">\r\n                                                </a>\r\n                                            </td>\r\n                                        </tr>\r\n                                        <tr>\r\n                                            <td style=\"font:16px/22px 'Helvetica Neue',Arial,'sans-serif';text-align:left;color:#ffffff;padding:20px 40px 0 40px\">\r\n                                                <p>Dear Customer,</p>\r\n                                                <p>Thank you for choosing our booklet!<br>Please download it below!</p>\r\n                                                <p>Kind Regards,<br>The Reseller Group</p>\r\n                                            </td>\r\n                                        </tr>\r\n                                        <tr>\r\n                                            <td>\r\n                                                <table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" style=\"margin: 30px 0px 0px 0px\">\r\n                                                    <tbody>\r\n                                                        <tr>\r\n                                                            <td align=\"center\" style=\"text-align:center\">\r\n                                                                <a name=\"m_5201679202317375323_CTA\" bgcolor=\"#000\" style=\"color:#282828;background-color:#ffffff;display:inline-block;font-family:'Helvetica Neue',Arial,'sans-serif';font-size:16px;line-height:30px;text-align:center;font-weight:bold;text-decoration:none;padding:5px 20px;border-radius:1px;text-transform:none\" href=\"https://resellergroup.azurewebsites.net/Booklet/Download\" target=\"_blank\">DOWNLOAD</a>\r\n                                                            </td>\r\n                                                        </tr>\r\n                                                    </tbody>\r\n                                                </table>\r\n                                            </td>\r\n                                        </tr>\r\n                                    </tbody>\r\n                                </table>\r\n                            </td>\r\n                        </tr>\r\n                        <tr>\r\n                            <td width=\"100%\" align=\"center\" valign=\"top\" bgcolor=\"#282828\" height=\"45\">\r\n                            </td>\r\n                        </tr>\r\n                    </tbody>\r\n                </table>\r\n            </td>\r\n        </tr>\r\n        <tr>\r\n            <td bgcolor=\"#eeeeee\" align=\"center\" style=\"padding:20px 0px\">\r\n                <table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" align=\"center\" style=\"max-width:600px\" class=\"m_5201679202317375323responsive-table\">\r\n                    <tbody>\r\n                        <tr></tr>\r\n                        <tr> \r\n                            <td bgcolor=\"#eeeeee\" align=\"center\">\r\n                                <table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" align=\"center\" style=\"max-width:600px\" class=\"m_5201679202317375323responsive-table\">\r\n                                    <tbody>\r\n                                        <tr>\r\n                                            <td align=\"center\" style=\"text-align:center;padding:10px 10px 10px 10px\">\r\n                                            </td>\r\n                                        </tr>\r\n                                    </tbody>\r\n                                </table>\r\n                            </td>\r\n                        </tr>\r\n                    </tbody>\r\n                </table>\r\n            </td>\r\n        </tr>\r\n    </tbody>\r\n</table>",
                };

                smtpClient.Send(message);
                return Ok("YOU HAVE BEEN SENT AN EMAIL WITH MORE DETAILS. THANKS, TRG");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return BadRequest("Process Failed. Contact: support@resellergroup.co.uk");
            }
        }

        public IActionResult Discord([FromQuery] string code, [FromQuery] string email)
        {
            string link = "https://discord.gg/" + code;
           
            _emailService.SendInviteEmail(email, link);
            return Redirect(link);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return BadRequest("ERROR: Try Again!");
            //return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}