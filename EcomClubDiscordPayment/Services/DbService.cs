using EcomClubDiscordPayment.Data;
using EcomClubDiscordPayment.Data.Tables;

namespace EcomClubDiscordPayment.Services
{
    public class DbService
    {
        private readonly DatabaseContext _dbContext;

        public DbService(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public string CreateToken()
        {
            string gui = Guid.NewGuid().ToString();
            while (_dbContext.tokenHistory.Any(a => a.token.Contains(gui)))
            {
                gui = Guid.NewGuid().ToString();
            }
            _dbContext.token.Add(new Token() { token = gui });
            _dbContext.SaveChanges();

            return gui;
        }

        public bool Validate(string token)
        {
            Token? tokenCompare = _dbContext.token.Where(w => w.token == token).FirstOrDefault();
            if (tokenCompare == null) return false;
            if (tokenCompare.token == token) return true;
            return false;
        }

        public void RemoveToken(string token, string code, string checkoutId, string subId)
        {
            try
            {
                var removeToken = _dbContext.token.FirstOrDefault(w => w.token == token);
                if (removeToken == null) return;
                _dbContext.token.Remove(removeToken);
                _dbContext.SaveChanges();

                _dbContext.tokenHistory.Add(new TokenHistory() { token = token, used_at = DateTime.UtcNow, inv_code = code, checkout_session_id = checkoutId, disc_name = "", sub_id = subId });
                _dbContext.SaveChanges();
            }
            catch
            {
                return;
            }
        }

        public bool CheckHistoryToken(string token)
        {
            var histToken = _dbContext.tokenHistory.FirstOrDefault(w => w.token == token);
            if (histToken == null) return false;
            if (histToken.token == token) return true;
            return false;
        }
    }
}
