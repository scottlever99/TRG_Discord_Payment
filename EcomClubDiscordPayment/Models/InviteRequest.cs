namespace EcomClubDiscordPayment.Models
{
    public class InviteRequest
    {
        public int max_age { get; set; }
        public int max_uses { get; set; }
        public bool temporary { get; set; }
        public bool unique { get; set; }
    }
}
