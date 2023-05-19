using EcomClubDiscordPayment.Models;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto.Tls;
using System.Net.Http.Headers;
using System.Text;

namespace EcomClubDiscordPayment.Services
{
    public class DiscordService
    {
        private readonly IConfiguration _configuration;

        public DiscordService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> GetInviteCode()
        {
            try
            {
                string accessToken = _configuration.GetValue<string>("Discord");

                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("Accept", "*/*");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Authorization", "Bot " + accessToken);

                InviteRequest body = new()
                {
                    max_age = 86400,
                    max_uses = 1,
                    temporary = false,
                    unique = true
                };

                HttpRequestMessage message = new()
                {
                    RequestUri = new Uri("https://discord.com/api/channels/1108099341232652440/invites"),
                    Method = HttpMethod.Post,
                    Content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json")
                };

                var result = await client.SendAsync(message);
                var resSting = await result.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<InviteResponse>(resSting);
                return response.code;
            }
            catch
            {
                return "";
            }
        }
    }
}
