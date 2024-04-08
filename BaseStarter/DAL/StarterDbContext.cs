using BaseStarter.Models;
using Microsoft.EntityFrameworkCore;


namespace BaseStarter.DAL
{
    
    /// <summary>
    /// Database context for Starter classes
    /// </summary>
    public class StarterDbContext : DbContext
    {
        public StarterDbContext(DbContextOptions<StarterDbContext> options) : base(options) { }

        public DbSet<Client> Clients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new ClientConfig());
            

        }
    }
}
