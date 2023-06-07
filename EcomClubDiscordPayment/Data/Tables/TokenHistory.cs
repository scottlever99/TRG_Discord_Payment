using System.ComponentModel.DataAnnotations;

namespace EcomClubDiscordPayment.Data.Tables
{
    public class TokenHistory
    {
        [Key]
        public string token { get; set; }
        public DateTime used_at { get; set; }
        public string inv_code { get; set; }
        public string disc_name { get; set; }
        public string checkout_session_id { get; set; }
        public string sub_id { get; set; }
    }
}
