using Microsoft.EntityFrameworkCore;

namespace Friend_List.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Friend> Friends { get; set; }
    }
}
