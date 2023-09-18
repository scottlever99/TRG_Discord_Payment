using System.ComponentModel.DataAnnotations;

namespace EcomClubDiscordPayment.Data.Tables
{
    public class Subscription_Emails
    {
        [Key]
        public int id { get; set; }
        public string email { get; set; }
        public DateTime created { get; set; }
    }
}
