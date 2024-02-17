using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using practicesNet.Models;

namespace practicesNet.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Tckno> Values { get; set; }
        protected readonly IConfiguration Configuration;

        public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration configuration) : base(options)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(Configuration.GetConnectionString("WebApiDatabase"));
        }
    }
}