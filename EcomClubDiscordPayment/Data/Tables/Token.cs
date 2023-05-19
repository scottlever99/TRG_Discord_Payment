using System.ComponentModel.DataAnnotations;

namespace EcomClubDiscordPayment.Data.Tables
{
    public class Token
    {
        [Key]
        public string token { get; set; }
    }
}
