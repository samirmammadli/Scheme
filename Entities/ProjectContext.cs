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
            //modelBuilder.Entity<UserCard>()
            //    .HasKey(t => new { t.CardId, t.UserId });

            modelBuilder.Entity<User>().HasIndex(x => x.Email).IsUnique(true);
            modelBuilder.Entity<VerificationCode>().HasIndex(x => x.Code).IsUnique(true);
            modelBuilder.Entity<Column>().HasIndex(x => x.Name).IsUnique(true);


            #region
            ////Default values
            modelBuilder.Entity<User>().Property(x => x.IsConfirmed).HasDefaultValue<bool>(false);
            //modelBuilder.Entity<Card>().Property(x => x.Color).HasDefaultValue("#FFFFFF");
            //modelBuilder.Entity<Card>().Property(x => x.Score).HasDefaultValue(0);
            #endregion


            ////Associations
            //modelBuilder.Entity<UserCard>()
            //    .HasOne(sc => sc.User)
            //    .WithMany(s => s.UserCards)
            //    .HasForeignKey(sc => sc.UserId);

        }
    }
}
