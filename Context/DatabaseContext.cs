using Microsoft.EntityFrameworkCore;

namespace PostCodeWebApplication.Models
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {

        }
        public DbSet<Clients> Clients { get; set; }
    }
}
