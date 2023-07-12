using Microsoft.EntityFrameworkCore;
using NISA.Model;

namespace NISA.DataAccessLayer
{
    public class DBContext:DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options) { }
        public DbSet<TicketDetails> ticketDetails { get; set; }
        public DbSet<UserDetails> userDetails { get; set; }
        public DbSet<TicketHandlingDetails> ticketHandlingDetails { get; set; }
    }
}