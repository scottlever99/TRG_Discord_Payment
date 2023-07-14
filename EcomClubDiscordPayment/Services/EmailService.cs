using MimeKit;

namespace EcomClubDiscordPayment.Services
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;
        private string username;
        private string password;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
            username = _configuration.GetValue<string>("Gmail:Username");
            password = _configuration.GetValue<string>("Gmail:Password");
        }

        public bool SendInviteEmail(string to, string link)
        {
            try
            {
                var email = new MimeMessage();

                string from = "support@resellergroup.co.uk";
                string subject = "Invite Link";

                email.From.Add(new MailboxAddress("Reseller Group", from));
                email.To.Add(new MailboxAddress(to, to));

                email.Subject = subject;
                email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = "<!DOCTYPE html>\r\n<html>\r\n<head>\r\n</head>\r\n<body>\r\n<style type=\"text/css\">\r\n\r\n.container,\r\n.container-fluid,\r\n.container-xxl,\r\n.container-xl,\r\n.container-lg,\r\n.container-md,\r\n.container-sm {\r\n  width: 100%;\r\n  padding-right: var(1.5rem, 0.75rem);\r\n  padding-left: var(1.5rem, 0.75rem);\r\n  margin-right: auto;\r\n  margin-left: auto;\r\n  border: 1px solid;\r\n}\r\n\r\n@media (min-width: 576px) {\r\n  .container-sm, .container {\r\n    max-width: 540px;\r\n  }\r\n}\r\n@media (min-width: 768px) {\r\n  .container-md, .container-sm, .container {\r\n    max-width: 720px;\r\n  }\r\n}\r\n@media (min-width: 992px) {\r\n  .container-lg, .container-md, .container-sm, .container {\r\n    max-width: 960px;\r\n  }\r\n}\r\n@media (min-width: 1200px) {\r\n  .container-xl, .container-lg, .container-md, .container-sm, .container {\r\n    max-width: 1140px;\r\n  }\r\n}\r\n@media (min-width: 1400px) {\r\n  .container-xxl, .container-xl, .container-lg, .container-md, .container-sm, .container {\r\n    max-width: 1320px;\r\n  }\r\n}\r\n</style>\r\n\r\n<div class=\"container\" style=\"justify-content: centre;\">\r\n    <h1>Welcome to The Reseller Group Discord Server</h1>\r\n    <br/>\r\n    <h4>This is your invite link: "+link+"</h4>\r\n</div>\r\n\r\n</body>\r\n</html>"
                };

                using (var smtp = new MailKit.Net.Smtp.SmtpClient())
                {
                    smtp.Connect("smtp.gmail.com", 465, true);

                    smtp.Authenticate(username, password);

                    smtp.Send(email);
                    smtp.Disconnect(true);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
