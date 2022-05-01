using Microsoft.EntityFrameworkCore;
using SeqDbPrototypeWeb.Models;

namespace SeqDbPrototypeWeb.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Project> Project { get; set; }

        public DbSet<SeqDbPrototypeWeb.Models.Experiment> Experiment { get; set; }
    }
}
