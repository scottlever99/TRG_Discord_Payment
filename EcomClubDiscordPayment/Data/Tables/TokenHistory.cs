using System.ComponentModel.DataAnnotations;

namespace EcomClubDiscordPayment.Data.Tables
{
    public class TokenHistory
    {
        [Key]
        public string token { get; set; }
        public DateTime used_at { get; set; }
    }
}
