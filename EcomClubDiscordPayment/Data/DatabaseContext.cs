using EcomClubDiscordPayment.Data.Tables;
using Microsoft.EntityFrameworkCore;

namespace EcomClubDiscordPayment.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<Token> token { get; set; }
        public DbSet<TokenHistory> tokenHistory { get; set; }

    }
}
