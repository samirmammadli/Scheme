using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scheme.Entities
{

    public class ProjectContext : DbContext
    {
        public DbSet<Backlog> Backlogs { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Column> Columns { get; set; }
        public DbSet<History> History { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Sprint> Sprints { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<VerificationCode> VerificationCodes {get;set;}

        public ProjectContext(DbContextOptions<ProjectContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(x => x.Email).IsUnique(true);
            modelBuilder.Entity<VerificationCode>().HasIndex(x => x.Code).IsUnique(true);
            modelBuilder.Entity<Column>().HasIndex(x => x.Name).IsUnique(true);

            modelBuilder.Entity<Column>().HasOne<Project>().WithMany(x => x.Columns).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Card>().HasOne<Column>().WithMany(x => x.Cards).OnDelete(DeleteBehavior.Cascade);


            #region
            //Default values
            modelBuilder.Entity<User>().Property(x => x.IsConfirmed).HasDefaultValue<bool>(false);
            #endregion

        }
    }
}
