namespace EcomClubDiscordPayment.Models
{

    public class Channel
    {
        public string id { get; set; }
        public string name { get; set; }
        public int type { get; set; }
    }

    public class Guild
    {
        public string id { get; set; }
        public string name { get; set; }
        public object splash { get; set; }
        public object banner { get; set; }
        public object description { get; set; }
        public string icon { get; set; }
        public List<object> features { get; set; }
        public int verification_level { get; set; }
        public object vanity_url_code { get; set; }
        public int premium_subscription_count { get; set; }
        public bool nsfw { get; set; }
        public int nsfw_level { get; set; }
    }

    public class Inviter
    {
        public string id { get; set; }
        public string username { get; set; }
        public object global_name { get; set; }
        public string avatar { get; set; }
        public string discriminator { get; set; }
        public int public_flags { get; set; }
        public bool bot { get; set; }
        public object avatar_decoration { get; set; }
    }

    public class InviteResponse
    {
        public string code { get; set; }
        public int type { get; set; }
        public DateTime expires_at { get; set; }
        public Guild guild { get; set; }
        public Channel channel { get; set; }
        public Inviter inviter { get; set; }
        public int uses { get; set; }
        public int max_uses { get; set; }
        public int max_age { get; set; }
        public bool temporary { get; set; }
        public DateTime created_at { get; set; }
    }

}
