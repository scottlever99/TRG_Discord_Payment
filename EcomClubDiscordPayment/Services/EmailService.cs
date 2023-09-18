using MimeKit;
using System.Net.Mail;
using System.Net;

namespace EcomClubDiscordPayment.Services
{
    public class EmailService
    {
        private readonly DbService _dbService;

        public EmailService(DbService dbService)
        {
            _dbService = dbService;
        }

        public bool SendInviteEmail(string to, string link)
        {
            try
            {
                _dbService.SaveEmail(to);
                
                var smtpClient = new SmtpClient("smtp.hostinger.com")
                {
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential("subscriptions@resellergroup.co.uk", "TRGsubscriptionspassword1$"),
                    Port = 587
                };

                var message = new MailMessage(new MailAddress("subscriptions@resellergroup.co.uk", "Reseller Group"), new MailAddress(to))
                {
                    Subject = "Discord Community Invite",
                    IsBodyHtml = true,
                    Body = $"<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"background-color: black; color: white;\">\r\n    <tbody>\r\n        <tr>\r\n            <td width=\"100%\" align=\"center\" valign=\"top\" bgcolor=\"#eeeeee\" height=\"20\">\r\n            </td>\r\n        </tr>\r\n        <tr>\r\n            <td bgcolor=\"#eeeeee\" align=\"center\" style=\"padding:0px 15px 0px 15px\">\r\n                <table bgcolor=\"#282828\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"max-width:600px\">\r\n                    <tbody>\r\n                        <tr>\r\n                            <td>\r\n                                <table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">\r\n                                    <tbody>\r\n                                        <tr>\r\n                                            <td align=\"center\" style=\"padding:30px 40px 0px 40px\">\r\n                                                <a href=\"https://resellergroup.co.uk\">\r\n                                                    <img src=\"https://i.ibb.co/3NnCkfG/Reseller-Logo-crop.png\" width=\"100\" border=\"0\" style=\"vertical-align:middle\" class=\"CToWUd\" data-bit=\"iit\">\r\n                                                </a>\r\n                                            </td>\r\n                                        </tr>\r\n                                        <tr>\r\n                                            <td style=\"font:16px/22px 'Helvetica Neue',Arial,'sans-serif';text-align:left;color:#ffffff;padding:20px 40px 0 40px\">\r\n                                                <p>Dear Customer,</p>\r\n                                                <p>Thank you for joining our community, if you encounter any issues be sure to email us at: support@resellergroup.co.uk<br><br>If you haven't done so already, please join our discord community with the invite link below!</p>\r\n                                                <p>Kind Regards,<br>The Reseller Group</p>\r\n                                            </td>\r\n                                        </tr>\r\n                                        <tr>\r\n                                            <td>\r\n                                                <table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" style=\"margin: 30px 0px 0px 0px\">\r\n                                                    <tbody>\r\n                                                        <tr>\r\n                                                            <td align=\"center\" style=\"text-align:center\">\r\n                                                                <a name=\"m_5201679202317375323_CTA\" bgcolor=\"#000\" style=\"color:#282828;background-color:#ffffff;display:inline-block;font-family:'Helvetica Neue',Arial,'sans-serif';font-size:16px;line-height:30px;text-align:center;font-weight:bold;text-decoration:none;padding:5px 20px;border-radius:1px;text-transform:none\" href=\"{link}\" target=\"_blank\">DISCORD</a>\r\n                                                            </td>\r\n                                                        </tr>\r\n                                                    </tbody>\r\n                                                </table>\r\n                                            </td>\r\n                                        </tr>\r\n                                    </tbody>\r\n                                </table>\r\n                            </td>\r\n                        </tr>\r\n                        <tr>\r\n                            <td width=\"100%\" align=\"center\" valign=\"top\" bgcolor=\"#282828\" height=\"45\">\r\n                            </td>\r\n                        </tr>\r\n                    </tbody>\r\n                </table>\r\n            </td>\r\n        </tr>\r\n        <tr>\r\n            <td bgcolor=\"#eeeeee\" align=\"center\" style=\"padding:20px 0px\">\r\n                <table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" align=\"center\" style=\"max-width:600px\" class=\"m_5201679202317375323responsive-table\">\r\n                    <tbody>\r\n                        <tr></tr>\r\n                        <tr> \r\n                            <td bgcolor=\"#eeeeee\" align=\"center\">\r\n                                <table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" align=\"center\" style=\"max-width:600px\" class=\"m_5201679202317375323responsive-table\">\r\n                                    <tbody>\r\n                                        <tr>\r\n                                            <td align=\"center\" style=\"text-align:center;padding:10px 10px 10px 10px\">\r\n                                            </td>\r\n                                        </tr>\r\n                                    </tbody>\r\n                                </table>\r\n                            </td>\r\n                        </tr>\r\n                    </tbody>\r\n                </table>\r\n            </td>\r\n        </tr>\r\n    </tbody>\r\n</table>"
                };

                smtpClient.Send(message);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
