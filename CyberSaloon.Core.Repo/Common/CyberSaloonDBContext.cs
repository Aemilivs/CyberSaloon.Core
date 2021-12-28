using CyberSaloon.Core.Repo.Applicants;
using CyberSaloon.Core.Repo.Applications;
using CyberSaloon.Core.Repo.Artists;
using CyberSaloon.Core.Repo.Arts;
using Microsoft.EntityFrameworkCore;

namespace CyberSaloon.Core.Repo.Common
{
    public class CyberSaloonDBContext : DbContext
    {
        public CyberSaloonDBContext(DbContextOptions<CyberSaloonDBContext> options) : base(options) { }
        
        public virtual DbSet<Applicant> Applicants { get; set; } = null!;
        public virtual DbSet<Application> Applications { get; set; } = null!;
        public virtual DbSet<ApplicationIngestLog> ApplicationIngestLogs { get; set; } = null!;
        public virtual DbSet<Artist> Artists { get; set; } = null!;
        public virtual DbSet<Art> Arts { get; set; } = null!;

        override protected void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Application>()
                .HasOne(it => it.Applicant)
                .WithMany(it => it.Applications)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .Entity<Application>()
                .HasMany(it => it.Supporters)
                .WithMany(it => it.Supported);

            // modelBuilder
            //     .Entity<Application>()
            //     .HasOne(it => it.Art)
            //     .WithOne(it => it.Application)
            //     .HasForeignKey<Art>(it => it.ApplicationId);
            
            // modelBuilder
            //     .Entity<Application>()
            //     .HasOne(it => it.Art)
            //     .WithOne(it => it.Application)
            //     .HasForeignKey<Art>(it => it.ApplicationId)
            //     .OnDelete(DeleteBehavior.Restrict);
        }
    }
}