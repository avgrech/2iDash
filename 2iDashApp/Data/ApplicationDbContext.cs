using Microsoft.EntityFrameworkCore;
using _2iDashApp.Model;

namespace _2iDashApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Organisation> Organisations => Set<Organisation>();
        public DbSet<SystemModel> Systems => Set<SystemModel>();
        public DbSet<Site> Sites => Set<Site>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Organisation>()
                .HasMany(o => o.Systems)
                .WithOne(s => s.Organisation)
                .HasForeignKey(s => s.OrganisationId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SystemModel>()
                .HasMany(s => s.Sites)
                .WithOne(si => si.SystemModel)
                .HasForeignKey(si => si.SystemModelId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Site>()
                .Property(s => s.Environment)
                .HasConversion<string>();
        }
    }
}
